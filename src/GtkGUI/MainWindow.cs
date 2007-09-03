/*
    MensajeroRemoting - Mensajero instantáneo hecho con .NET Remoting
    y otras tecnologías de .NET.
    Copyright (C) 2007  Luis Ignacio Larrateguy, Milton Pividori y César Sandrigo

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections;
using System.Collections.Generic;

using Gtk;
using Glade;

using log4net.Appender;
using log4net.Config;

namespace MensajeroRemoting
{
	public class MainWindow : ICliente
	{

		private string servidor;
		private string nick;
		private string ip;
		
		[Widget]
		private Gtk.Window mainWindow;
		
		[Widget]
		private Gtk.ComboBoxEntry cmbEstado;
		
		[Widget]
		private Gtk.VBox vbox1;
		
		private Gtk.TreeView tvContactos;
		private Gtk.ListStore contactos;
		private Dictionary<string, TreeIter> treeItersContactos;
		private Dictionary<string, VentanaChat> ventanasChat;
		
		private ControladorCliente controladorCliente;
		
		private log4net.ILog logger;
		
		public MainWindow()
		{
			this.logger = log4net.LogManager.GetLogger(this.GetType());
			System.IO.FileInfo fi = new System.IO.FileInfo("log4net.config.xml");
			this.logger.Debug(fi.Exists);
			XmlConfigurator.Configure(fi);
			
			Application.Init();
			
			this.treeItersContactos = new Dictionary<string, Gtk.TreeIter>();
			this.ventanasChat = new Dictionary<string, VentanaChat>();
			
			this.logger.Debug("Creando interfaz glade...");
			Glade.XML gxml = new XML("mainwindow.glade", "mainWindow", null);
			gxml.Autoconnect(this);
			
			this.mainWindow.DeleteEvent += new DeleteEventHandler(this.OnDeleteMainWindow);	
			
			this.cmbEstado.Entry.IsEditable = false;
			
			// tvContactos
			this.tvContactos = new TreeView();
			this.tvContactos.HeadersVisible = false;
			this.tvContactos.Selection.Mode = SelectionMode.Single;
			
			TreeViewColumn nick = new TreeViewColumn();
			CellRendererText nickText = new CellRendererText();
			nick.PackStart(nickText, true);
			
			this.tvContactos.AppendColumn(nick);
			
			nick.AddAttribute(nickText, "text", 0);
			
			// La posición 0 es el nick y la 1 la cadena de conexión
			this.contactos = new ListStore(typeof(string));
			this.tvContactos.Model = this.contactos;
			
			// Handler para cuando se hace doble click
			this.tvContactos.RowActivated += new RowActivatedHandler(this.OnContactosRowActivated);
			
			this.vbox1.PackStart(this.tvContactos);
			this.vbox1.ReorderChild(this.tvContactos, 1);
			
			this.mainWindow.ShowAll();
			
			Application.Run();
		}
		
		public ConexionClienteHandler MetodoContactoConectado
		{
			get { return (new ConexionClienteHandler(this.ContactoConectado)); }
		}
		
		public ConexionClienteHandler MetodoContactoDesconectado
		{
			get { return (new ConexionClienteHandler(this.ContactoDesconectado)); }
		}
		
		public MensajeRecibidoHandler MetodoMensajeRecibido
		{
			get { return (new MensajeRecibidoHandler(this.RecibirMensaje)); }
		}
		
		public ControladorCliente ControladorCliente
		{
			get { return this.controladorCliente; }
		}
		
		public string Nick
		{
			get { return this.nick; }
		}
		
		public string Servidor
		{
			get { return this.servidor; }
		}
		
		public void OnContactosRowActivated(object o, EventArgs args)
		{
			TreeIter filaSeleccionada;
			this.tvContactos.Selection.GetSelected(out filaSeleccionada);
			
			/* Obtengo la cadena de conexion que se almacena en la posición
			 * 1 del modelo del treeview */
			string nickSeleccionado = (string)this.contactos.GetValue(filaSeleccionada, 0);
			
			this.logger.Debug("Cadena seleccionada: " + nickSeleccionado);
			
			this.MostrarVentanaChat(nickSeleccionado);
		}
		
		public void VentanaChatCerrada(string nick)
		{
			// La cadena de conexion identifica a la ventana cerrada
			this.ventanasChat.Remove(nick);
		}
		
		public void OnCmbEstadoChanged(object o, EventArgs args)
		{
			this.logger.Debug("Ejecutando OnCmbEstadoChanged...");
			
			if (this.cmbEstado.Active < 0)
				return;
			
			if (this.cmbEstado.ActiveText.Equals("Conectado")) {
				this.Conectar();
			}
			else if (this.cmbEstado.ActiveText.Equals("Desconectado")) {
				this.Desconectar();
			}
		}
		
		private void Conectar()
		{
			this.logger.Debug("Ejecutando Conectar (MainWindow)");
			// Pido el nick
			
			// Deshabilito temporalmente el manejador de eventos del combobox
			this.cmbEstado.Changed -= new EventHandler(this.OnCmbEstadoChanged);
			this.cmbEstado.Entry.Text = "Conectando...";
			
			DataInput servidorInput = new DataInput(this.mainWindow,nick);
			ResponseType respuesta = (ResponseType)servidorInput.Run();
			if (respuesta == ResponseType.Cancel) {
				this.cmbEstado.Entry.Text = "Desconectado";
				this.cmbEstado.Changed += new EventHandler(this.OnCmbEstadoChanged);
			} else {
				this.nick = servidorInput.NickEscogido;
				this.servidor = servidorInput.getServidorEscogido();
				this.logger.Debug("En MainWindow el servidor resultó ser: " + this.servidor);
				
				//Deberia hacer lo mismo con la IP
				this.ip  = servidorInput.getIpEscogida();
				
				servidorInput.Destroy();
				
				// Creo la instancia
				this.controladorCliente = new ControladorCliente(this, this.ip, this.servidor, this.Nick);
				try {
				
					string[] clientesConectados = controladorCliente.Conectar(this.nick);
					
					this.cmbEstado.Entry.Text = "Conectado";
					
					this.mainWindow.Title = "IM - Nick: " + this.Nick;

					// Limpio, por las dudas, el ListStore y el Dictionary
					this.contactos.Clear();
					this.treeItersContactos.Clear();
					
					// Agrego los contactos al TreeView y al Dictionary
					TreeIter iter;
					foreach (string ci in clientesConectados) {
						iter = this.contactos.AppendValues(ci);
						this.treeItersContactos.Add(ci, iter);
					}
				}
				catch (NickOcupadoException) {
					Gtk.MessageDialog md = new Gtk.MessageDialog(this.mainWindow, DialogFlags.Modal,
	                           MessageType.Error, ButtonsType.Ok,
	                           "El nick elegido ya está siendo usado por otro usuario");
					md.Run();
					md.Destroy();
					this.cmbEstado.Entry.Text = "Desconectado";
				}
				finally {
					this.cmbEstado.Changed += new EventHandler(this.OnCmbEstadoChanged);
				}
				this.logger.Debug("Fin de Conectar (MainWindow)");
			}
		}
		
		private void Desconectar()
		{
			this.logger.Debug("Ejecutando Desconectar (MainWindow)");
			
			// Primero me desconecto del servidor
			if (controladorCliente != null)
				controladorCliente.Desconectar();
			
			this.mainWindow.Title = "IM - Instant Messenger ";
			/* Luego limpio la lista de contactos (GUI - TreeView) y luego
			 * limpio el diccionario <cadenaConexion,TreeIter> */
			this.contactos.Clear();
//			TreeIter iter;
//			foreach (string ci in this.treeItersContactos.Keys) {
//				iter = this.treeItersContactos[ci];
//				this.contactos.Remove(ref iter);
//			}
			
			this.treeItersContactos.Clear();
			
			this.logger.Debug("Fin de Desconectar (MainWindow)");
		}
		
		public void OnDeleteMainWindow(object o, DeleteEventArgs args)
		{
			this.logger.Debug("Ejecutando OnDeleteMainWindow...");
			
			this.Desconectar();
			
			Application.Quit();
		}
		
		public void ContactoConectado(string nickCliente)
		{
			//ClienteInfo unCliente = new ClienteInfo();
			//unCliente.cadenaConexion = unCliente2.cadenaConexion;
			//unCliente.nick = unCliente2.nick;
			
			this.logger.Debug("Notificación de contacto conectado!");

			if (this.nick.Equals(nickCliente)) {
				this.logger.Debug("  Pero soy yo mismo. No me agrego, porque no me gusta chatear conmigo, salvo cuando estoy solo, y no tengo ganas de estudiar o programar");
				return;
			}
			
			if (this.treeItersContactos.ContainsKey(nickCliente)) {
				this.logger.Debug("Ops, el cliente que se conecto no soy yo, pero ya lo tengo agregado :S");
				return;
			}
			
			TreeIter iter = this.contactos.AppendValues(nickCliente);
			this.treeItersContactos.Add(nickCliente, iter);
			
			//this.mainWindow.GdkWindow.ProcessUpdates(true);

			this.logger.Debug("  Listo, agregado");
		}
		
		public void ContactoDesconectado(string nickClienteDesconectado)
		{
			this.logger.Debug("Notificación de contacto desconectado!");
			
//			Console.WriteLine("Mi cadena es '" + this.cadenaConexion + "', y mi nick es '" +
//			                  this.nick);
//			Console.WriteLine("La cadena del otro es '" + unCliente + "', y su nick es '" +
//			                  unCliente);
			
			if (this.nick.Equals(nickClienteDesconectado)) {
				this.logger.Debug("  Pero soy yo mismo. Paro aca nomas.");
				return;
			}
			
			if (!this.treeItersContactos.ContainsKey(nickClienteDesconectado)) {
				this.logger.Debug("Se desconectó un cliente pero no lo tengo!");
				return;
			}
			
//			try {
			TreeIter iter = this.treeItersContactos[nickClienteDesconectado];
			this.contactos.Remove(ref iter);
			this.treeItersContactos.Remove(nickClienteDesconectado);
			
			//this.mainWindow.GdkWindow.ProcessUpdates(true);
			
			this.logger.Debug("  Listo, quitado");
//			}
//			catch (KeyNotFoundException) {
//				Console.WriteLine("  ERROR: Contacto no presente en mi lista de contactos");
//			}
		}
		
		public void RecibirMensaje(string nickOrigen, string mensaje)
		{
			this.logger.Debug("Mostrando mensaje recibido...");
			
			if (this.ventanasChat.ContainsKey(nickOrigen))
				this.ventanasChat[nickOrigen].MensajeRecibido(mensaje);
			else
				this.MostrarVentanaChat(nickOrigen, mensaje);
//			
//			ventanaChat.MensajeRecibido(mensaje);
		}
		
		public Gtk.Window GtkWindow
		{
			get { return this.mainWindow; }
		}
		
		private VentanaChat MostrarVentanaChat(string nickRemoto, string mensaje)
		{
			this.logger.Debug("Metodo MostrarVentanaChat");
			VentanaChat ventanaChat;
			
			if (this.ventanasChat.ContainsKey(nickRemoto)) {
				this.logger.Debug("La ventana ya esta creada");
				ventanaChat = this.ventanasChat[nickRemoto];
			}
			else {
				this.logger.Debug("Creando la ventana, porque no esta");
				if (mensaje == null)
					ventanaChat = new VentanaChat(this, nickRemoto);
				else
					ventanaChat = new VentanaChat(this, nickRemoto, mensaje);
				this.logger.Debug("Agregandola al diccionario");
				this.ventanasChat.Add(nickRemoto, ventanaChat);
			}
			
//			this.logger.Debug("Activandola");
//			ventanaChat.Activar();
			
			this.logger.Debug("Listo, retornando");
			return ventanaChat;
		}
		
		private VentanaChat MostrarVentanaChat(string nickRemoto)
		{
			return this.MostrarVentanaChat(nickRemoto, null);
		}
		
		public static void Main(string[] args)
		{
			new MainWindow();
		}
	}
}
