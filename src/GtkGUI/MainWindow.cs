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
		private ClienteInfo yo;
		private string servidor;
		private bool conectado = false;
		
		[Widget]
		private Gtk.Window mainWindow;
		
		[Widget]
		private Gtk.ComboBoxEntry cmbEstado;
		
		[Widget]
		private Gtk.VBox vbox1;
		
		private Gtk.TreeView tvContactos;
		private Gtk.ListStore contactos;
		private Dictionary<ClienteInfo, TreeIter> treeItersContactos;
		private Dictionary<string, VentanaChat> ventanasChat;
		private ListaContactosEventHelper helper;
		
		public MainWindow()
		{
			Application.Init();
			
			this.treeItersContactos = new Dictionary<ClienteInfo, Gtk.TreeIter>();
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
			this.contactos = new ListStore(typeof(string), typeof(string));
			this.tvContactos.Model = this.contactos;
			
			// Handler para cuando se hace doble click
			this.tvContactos.RowActivated += new RowActivatedHandler(this.OnContactosRowActivated);
			
			this.vbox1.PackStart(this.tvContactos);
			this.vbox1.ReorderChild(this.tvContactos, 1);
			
			this.mainWindow.ShowAll();
			
			Application.Run();
		}
		
		public ClienteInfo ClienteInfo
		{
			get { return this.yo; }
		}
		
		public string CadenaConexion 
		{
			get { return this.yo.cadenaConexion; }
		}
		
		public string Nick
		{
			get { return this.yo.nick; }
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
			string cadenaConexionDestino = (string)this.contactos.GetValue(filaSeleccionada, 1);
			
			Console.WriteLine("Cadena seleccionada: " + cadenaConexionDestino);
			// Si ya hay una ventana para chatear con el contacto, la activo
			if (this.ventanasChat.ContainsKey(cadenaConexionDestino)) {
				VentanaChat vc = this.ventanasChat[cadenaConexionDestino];
				vc.Activar();
				return;
			}
			
			// En cambio si no hay una, la creo...
			VentanaChat ventanaChat = new VentanaChat(this, cadenaConexionDestino, this.yo.nick);
			this.ventanasChat.Add(cadenaConexionDestino, ventanaChat);
		}
		
		public void VentanaChatCerrada(string cadenaConexionDestino)
		{
			// La cadena de conexion identifica a la ventana cerrada
			this.ventanasChat.Remove(cadenaConexionDestino);
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
			
			DataInput servidorInput = new DataInput(this.mainWindow);
			ResponseType respuesta = (ResponseType)servidorInput.Run();
			if (respuesta == ResponseType.Cancel) {
				this.cmbEstado.Entry.Text = "Desconectado";
				this.cmbEstado.Changed += new EventHandler(this.OnCmbEstadoChanged);
				return;
			}
			
			this.yo.nick = servidorInput.NickEscogido;
			this.servidor = servidorInput.getServidorEscogido();
			Console.WriteLine("En MainWindow el servidor resultó ser: " + this.servidor);
			
			servidorInput.Destroy();
			
			ClienteInfo[] clientesConectados = ClienteManager.Conectar(out this.yo.cadenaConexion);
			this.cmbEstado.Entry.Text = "Conectado";
			this.cmbEstado.Changed += new EventHandler(this.OnCmbEstadoChanged);
			
			// Me registro a los eventos que me interesan en el servidor
			if (this.helper == null) {
				ControladorConexiones cc = ClienteManager.ControladorConexiones;
				this.helper = new ListaContactosEventHelper(cc);
				
				this.helper.ContactoConectado += new ControladorConexiones.ListaContactosHandler(this.ContactoConectado);
				this.helper.ContactoDesconectado += new ControladorConexiones.ListaContactosHandler(this.ContactoDesconectado);
				
				Console.WriteLine("Registrando handlers...");
				this.helper.RegistrarHandlers();
			}
			
			if (clientesConectados == null)
				return;
			
			this.conectado = true;

			// Limpio, por las dudas, el ListStore y el Dictionary
			this.contactos.Clear();
			this.treeItersContactos.Clear();
			
			// Agrego los contactos al TreeView y al Dictionary
			TreeIter iter;
			foreach (ClienteInfo ci in clientesConectados) {
				iter = this.contactos.AppendValues(ci.nick, ci.cadenaConexion);
				this.treeItersContactos.Add(ci, iter);
			}
			
			Console.WriteLine("Fin de Conectar (MainWindow)");
		}
		
		private void Desconectar()
		{
			Console.WriteLine("Ejecutando Desconectar (MainWindow)");
			
			if (!this.conectado) return;
			
			// Primero me desconecto del servidor
			ClienteManager.Desconectar();
			
			/* Le digo al objeto ListaContactosEventHelper que desregistre sus
			 * métodos en ControladorConexiones */
			this.helper.DesregistrarHandlers();
			
			this.conectado = false;
			
			/* Luego limpio la lista de contactos (GUI - TreeView) y luego
			 * limpio el diccionario <cadenaConexion,TreeIter> */
			TreeIter iter;
			foreach (ClienteInfo ci in this.treeItersContactos.Keys) {
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
		
		public void ContactoConectado(ClienteInfo unCliente2)
		{
			ClienteInfo unCliente = new ClienteInfo();
			unCliente.cadenaConexion = unCliente2.cadenaConexion;
			unCliente.nick = unCliente2.nick;
			
			Console.WriteLine("Notificación de contacto conectado!");

			if (this.yo.Equals(unCliente)) {
				Console.WriteLine("  Pero soy yo mismo. No me agrego");
				return;
			}
			
			TreeIter iter = this.contactos.AppendValues(unCliente.nick, unCliente.cadenaConexion);
			this.treeItersContactos.Add(unCliente, iter);
			
			Console.WriteLine("  Listo, agregado");
		}
		
		public void ContactoDesconectado(ClienteInfo unCliente2)
		{
			ClienteInfo unCliente = new ClienteInfo();
			unCliente.cadenaConexion = unCliente2.cadenaConexion;
			unCliente.nick = unCliente2.nick;
			
			Console.WriteLine("Notificación de contacto desconectado!");
			
			Console.WriteLine("Mi cadena es '" + yo.cadenaConexion + "', y mi nick es '" +
			                  yo.nick);
			Console.WriteLine("La cadena del otro es '" + unCliente.cadenaConexion + "', y su nick es '" +
			                  unCliente.nick);
			
			if (this.yo.Equals(unCliente)) {
				Console.WriteLine("  Pero soy yo mismo. Paro aca nomas.");
				return;
			}
			
//			try {
			TreeIter iter = this.treeItersContactos[unCliente];
			this.contactos.Remove(ref iter);
			this.treeItersContactos.Remove(unCliente);
			
			Console.WriteLine("  Listo, quitado");
//			}
//			catch (KeyNotFoundException) {
//				Console.WriteLine("  ERROR: Contacto no presente en mi lista de contactos");
//			}
		}
		
		public void EnviarMensaje(ClienteInfo origen, string mensaje)
		{
			Console.WriteLine("Mostrando mensaje recibido...");
			
			if (this.ventanasChat.ContainsKey(origen.cadenaConexion)) {
				VentanaChat vc = this.ventanasChat[origen.cadenaConexion];
				vc.MensajeRecibido(origen, mensaje);
			}
			else {
				VentanaChat ventanaChat = new VentanaChat(this, origen.cadenaConexion, this.yo.nick);
				ventanaChat.MensajeRecibido(origen, mensaje);
				this.ventanasChat.Add(origen.cadenaConexion, ventanaChat);
			}
		}
		
		public static void Main(string[] args)
		{
			new MainWindow();
		}
		
		public override object InitializeLifetimeService()
		{
			return null;
		}
	}
}