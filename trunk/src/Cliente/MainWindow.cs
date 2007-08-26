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
	public class MainWindow : MarshalByRefObject
	{
		private string id;
		private bool conectado = false;
		
		[Widget]
		private Gtk.Window mainWindow;
		
		[Widget]
		private Gtk.ComboBox cmbEstado;
		
		[Widget]
		private Gtk.VBox vbox1;
		
		private Gtk.TreeView tvContactos;
		private Gtk.ListStore contactos;
		private Dictionary<string, TreeIter> treeItersContactos;
		private ListaContactosEventHelper helper;
		
		public MainWindow()
		{
			Application.Init();
			
			ControladorConexiones cc = ClienteManager.ControladorConexiones;
			this.helper = new ListaContactosEventHelper(cc);
			
			this.helper.ContactoConectado += new ControladorConexiones.ListaContactosHandler(this.ContactoConectado);
			this.helper.ContactoDesconectado += new ControladorConexiones.ListaContactosHandler(this.ContactoDesconectado);
			
			this.treeItersContactos = new Dictionary<string,Gtk.TreeIter>();
			
			Console.WriteLine("Creando interfaz glade...");
			Glade.XML gxml = new XML("mainwindow.glade", "mainWindow", null);
			gxml.Autoconnect(this);
			
			this.mainWindow.DeleteEvent += new DeleteEventHandler(this.OnDeleteMainWindow);	
			
			// tvContactos
			this.tvContactos = new TreeView();
			this.tvContactos.HeadersVisible = false;
			this.tvContactos.Selection.Mode = SelectionMode.Single;
			
			TreeViewColumn nick = new TreeViewColumn();
			CellRendererText nickText = new CellRendererText();
			nick.PackStart(nickText, true);
			
			this.tvContactos.AppendColumn(nick);
			
			nick.AddAttribute(nickText, "text", 0);
			
			this.contactos = new ListStore(typeof(string));
			this.tvContactos.Model = this.contactos;
			
			this.vbox1.PackStart(this.tvContactos);
			this.vbox1.ReorderChild(this.tvContactos, 1);
			
			this.mainWindow.ShowAll();
		}
		
		public string Id 
		{
			get { return this.id; }
			set { this.id = value; }
		}
		
		public void OnCmbEstadoChanged(object o, EventArgs args)
		{
			if (this.cmbEstado.ActiveText.Equals("Conectado")) {
				this.Conectar();
			}
			else if (this.cmbEstado.ActiveText.Equals("Desconectado")) {
				this.Desconectar();
			}
		}
		
		private void Conectar()
		{
			Console.WriteLine("Registrando handlers...");
			this.helper.RegistrarHandlers();
			
			string[] clientesConectados = ClienteManager.Conectar(out this.id);
			
			if (clientesConectados == null)
				return;
			
			this.conectado = true;

			// Limpio, por las dudas, el ListStore y el Dictionary
			this.contactos.Clear();
			this.treeItersContactos.Clear();
			
			// Agrego los contactos al TreeView y al Dictionary
			foreach (string cadena in clientesConectados) {
				TreeIter iter = this.contactos.AppendValues(cadena);
				this.treeItersContactos.Add(cadena, iter);
			}
		}
		
		private void Desconectar()
		{
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
			foreach (string cadenaConexion in this.treeItersContactos.Keys) {
				iter = this.treeItersContactos[cadenaConexion];
				this.contactos.Remove(ref iter);
			}
			
			this.treeItersContactos.Clear();
		}
		
		public void OnDeleteMainWindow(object o, DeleteEventArgs args)
		{
			this.Desconectar();
			
			Application.Quit();
		}
		
		public void ContactoConectado(string cadena)
		{
			Console.WriteLine("Notificación de contacto conectado!");

			if (this.id.Equals(cadena)) {
				Console.WriteLine("  Pero soy yo mismo. No me agrego");
				return;
			}
			
			TreeIter iter = this.contactos.AppendValues(cadena);
			this.treeItersContactos.Add(cadena, iter);
			Console.WriteLine("  Listo, agregado");
		}
		
		public void ContactoDesconectado(string cadena)
		{
			Console.WriteLine("Notificación de contacto desconectado!");
			
			if (this.id.Equals(cadena)) {
				Console.WriteLine("  Pero soy yo mismo. Paro aca nomas.");
				return;
			}
			
//			try {
			TreeIter iter = this.treeItersContactos[cadena];
			this.contactos.Remove(ref iter);
			this.treeItersContactos.Remove(cadena);
			
			Console.WriteLine("  Listo, quitado");
//			}
//			catch (KeyNotFoundException) {
//				Console.WriteLine("  ERROR: Contacto no presente en mi lista de contactos");
//			}
		}
		
		public void EnviarMensaje(string origen, string mensaje)
		{
			MessageDialog md = new MessageDialog(this.mainWindow, DialogFlags.Modal,
			                                     MessageType.Info, ButtonsType.Close,
			                                     mensaje);
			md.Title = "¡Nuevo mensaje de " + origen + "!";
			
			md.Run();
			md.Destroy();
		}
		
		public void Run()
		{
			Application.Run();
		}
		
		public override object InitializeLifetimeService()
		{
			return null;
		}
	}
}
