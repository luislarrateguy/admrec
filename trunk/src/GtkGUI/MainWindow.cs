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

namespace MensajeroRemoting
{
	
	public class MainWindow
	{

		private string servidor;
		private string nick;
		private bool conectado = false;
		private string cadenaConexion;
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
		
		// para el cliente remoto y eventos
		//private ClienteRemoto miCliente;
		private EventsHelper eventHelper;
		private ControladorCliente controladorCliente;
		
		public MainWindow()
		{
			Application.Init();
			
			this.treeItersContactos = new Dictionary<string, Gtk.TreeIter>();
			this.ventanasChat = new Dictionary<string, VentanaChat>();
			
			Console.WriteLine("Creando interfaz glade...");
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
		
		public ControladorCliente ControladorCliente
		{
			get { return this.controladorCliente; }
		}
		
		public string CadenaConexion 
		{
			get { return this.cadenaConexion; }
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
			
			Console.WriteLine("Cadena seleccionada: " + nickSeleccionado);
			// Si ya hay una ventana para chatear con el contacto, la activo
			if (this.ventanasChat.ContainsKey(nickSeleccionado)) {
				VentanaChat vc = this.ventanasChat[nickSeleccionado];
				vc.Activar();
			}
			else {
				// En cambio si no hay una, la creo...
				VentanaChat ventanaChat = new VentanaChat(this, nickSeleccionado);
				this.ventanasChat.Add(nickSeleccionado, ventanaChat);
			}
		}
		
		public void VentanaChatCerrada(string nick)
		{
			// La cadena de conexion identifica a la ventana cerrada
			this.ventanasChat.Remove(nick);
		}
		
		public void OnCmbEstadoChanged(object o, EventArgs args)
		{
			Console.WriteLine("Ejecutando OnCmbEstadoChanged...");
			
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
			Console.WriteLine("Ejecutando Conectar (MainWindow)");
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
				Console.WriteLine("En MainWindow el servidor resultó ser: " + this.servidor);
				
				servidorInput.Destroy();
				
				//Deberia hacer lo mismo con la IP
				this.ip  = "127.0.0.1";
				
				// Creo la instancia
				this.controladorCliente = new ControladorCliente(this.ip,this.servidor,this.Nick);
				try {
				
					string[] clientesConectados = controladorCliente.Conectar(this.nick);
					
					this.cmbEstado.Entry.Text = "Conectado";
					this.eventHelper = new EventsHelper(this.controladorCliente.miClienteRemoto);
					
					this.eventHelper.ContactoConectado += new ConexionClienteHandler(this.ContactoConectado);
					this.eventHelper.ContactoDesconectado += new ConexionClienteHandler(this.ContactoDesconectado);
					this.eventHelper.MensajeRecibido += new MensajeRecibidoHandler(this.RecibirMensaje);
					
					this.conectado = true;

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
				catch (NickOcupadoException e) {
					this.conectado = false;
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
				Console.WriteLine("Fin de Conectar (MainWindow)");
			}
		}
		
		private void Desconectar()
		{
			Console.WriteLine("Ejecutando Desconectar (MainWindow)");
			
			if (!this.conectado) return;
			
			this.eventHelper.ContactoConectado -= new ConexionClienteHandler(this.ContactoConectado);
			this.eventHelper.ContactoDesconectado -= new ConexionClienteHandler(this.ContactoDesconectado);
			this.eventHelper.MensajeRecibido -= new MensajeRecibidoHandler(this.RecibirMensaje);
			
			this.eventHelper.DesregistrarHandlers();
			
			// Primero me desconecto del servidor
			controladorCliente.Desconectar();
			
			/* Le digo al objeto ListaContactosEventHelper que desregistre sus
			 * métodos en ControladorConexiones */
			//this.helper.DesregistrarHandlers();
			
			this.conectado = false;
			
			/* Luego limpio la lista de contactos (GUI - TreeView) y luego
			 * limpio el diccionario <cadenaConexion,TreeIter> */
			TreeIter iter;
			foreach (string ci in this.treeItersContactos.Keys) {
				iter = this.treeItersContactos[ci];
				this.contactos.Remove(ref iter);
			}
			
			this.treeItersContactos.Clear();
			
			Console.WriteLine("Fin de Desconectar (MainWindow)");
		}
		
		public void OnDeleteMainWindow(object o, DeleteEventArgs args)
		{
			Console.WriteLine("Ejecutando OnDeleteMainWindow...");
			
			this.Desconectar();
			
			Application.Quit();
		}
		
		public void ContactoConectado(string nickCliente)
		{
			//ClienteInfo unCliente = new ClienteInfo();
			//unCliente.cadenaConexion = unCliente2.cadenaConexion;
			//unCliente.nick = unCliente2.nick;
			
			Console.WriteLine("Notificación de contacto conectado!");

			if (this.nick.Equals(nickCliente)) {
				Console.WriteLine("  Pero soy yo mismo. No me agrego, porque no me gusta chatear conmigo, salvo cuando estoy solo, y no tengo ganas de estudiar o programar");
				return;
			}
			
			if (this.treeItersContactos.ContainsKey(nickCliente)) {
				Console.WriteLine("Ops, el cliente que se conecto no soy yo, pero ya lo tengo agregado :S");
				return;
			}
			
			TreeIter iter = this.contactos.AppendValues(nickCliente);
			this.treeItersContactos.Add(nickCliente, iter);

			Console.WriteLine("  Listo, agregado");
		}
		
		public void ContactoDesconectado(string nickClienteDesconectado)
		{
			Console.WriteLine("Notificación de contacto desconectado!");
			
//			Console.WriteLine("Mi cadena es '" + this.cadenaConexion + "', y mi nick es '" +
//			                  this.nick);
//			Console.WriteLine("La cadena del otro es '" + unCliente + "', y su nick es '" +
//			                  unCliente);
			
			if (this.nick.Equals(nickClienteDesconectado)) {
				Console.WriteLine("  Pero soy yo mismo. Paro aca nomas.");
				return;
			}
			
			if (!this.treeItersContactos.ContainsKey(nickClienteDesconectado)) {
				Console.WriteLine("Se desconectó un cliente pero no lo tengo!");
				return;
			}
			
//			try {
			TreeIter iter = this.treeItersContactos[nickClienteDesconectado];
			this.contactos.Remove(ref iter);
			this.treeItersContactos.Remove(nickClienteDesconectado);
			
			Console.WriteLine("  Listo, quitado");
//			}
//			catch (KeyNotFoundException) {
//				Console.WriteLine("  ERROR: Contacto no presente en mi lista de contactos");
//			}
		}
		
		public void RecibirMensaje(string nickOrigen, string mensaje)
		{
			Console.WriteLine("Mostrando mensaje recibido...");
			
			if (this.ventanasChat.ContainsKey(nickOrigen)) {
				VentanaChat vc = this.ventanasChat[nickOrigen];
				vc.MensajeRecibido(nickOrigen, mensaje);
			}
			else {
				VentanaChat ventanaChat = new VentanaChat(this, nickOrigen);
				ventanaChat.MensajeRecibido(nickOrigen, mensaje);
				this.ventanasChat.Add(nickOrigen, ventanaChat);
			}
		}
		
		public static void Main(string[] args)
		{
			new MainWindow();
		}
	}
}
