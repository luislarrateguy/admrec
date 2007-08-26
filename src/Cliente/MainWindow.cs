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
		
		[Widget]
		Gtk.Window mainWindow;
		
		[Widget]
		Gtk.ComboBox cmbEstado;
		
		[Widget]
		Gtk.VBox vbox1;
		
		Gtk.TreeView tvContactos;
		Gtk.ListStore contactos;
		Dictionary<string, TreeIter> treeItersContactos;
		
		public MainWindow()
		{
			Application.Init();
			
			ControladorConexiones cc = ClienteManager.ControladorConexiones;
			
			Console.WriteLine("Creando objeto ListContactosHelperEvent...");
			ListaContactosEventHelper helper = new ListaContactosEventHelper(cc);

			Console.WriteLine("Registrando handlers...");
			helper.ContactoConectado += new ControladorConexiones.ListaContactosHandler(this.ContactoConectado);
			helper.ContactoDesconectado += new ControladorConexiones.ListaContactosHandler(this.ContactoDesconectado);
			
//			cc.ContactoConectado += new ControladorConexiones.ListaContactosHandler(this.ContactoConectado);
//			cc.ContactoDesconectado += new ControladorConexiones.ListaContactosHandler(this.ContactoDesconectado);
			
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
			
//			Application.Run();
		}
		
		public string Id 
		{
			get { return this.id; }
			set { this.id = value; }
		}
//		
//		public ClienteManager ClienteManager
//		{
//			set {
//				Console.WriteLine("Seteando handlers para los eventos...");
//				this.clienteManager = value;
//				
//				Console.WriteLine("Seteando handlers para los eventos...");
//				ControladorConexiones cc = this.clienteManager.ControladorConexiones;
//				cc.ContactoConectado += new ControladorConexiones.ListaContactosHandler(this.ContactoConectado);
//				cc.ContactoDesconectado += new ControladorConexiones.ListaContactosHandler(this.ContactoDesconectado);
//			}
//		}
		
		public void OnCmbEstadoChanged(object o, EventArgs args)
		{
			if (this.cmbEstado.ActiveText.Equals("Conectado")) {
				string[] clientesConectados = ClienteManager.Conectar();
				
				if (clientesConectados == null)
					return;
				
				foreach (string cadena in clientesConectados) {
					TreeIter iter = this.contactos.AppendValues(cadena);
					this.treeItersContactos.Add(cadena, iter);
				}
			}
			else if (this.cmbEstado.ActiveText.Equals("Desconectado"))
				ClienteManager.Desconectar();
		}
		
		public void OnDeleteMainWindow(object o, DeleteEventArgs args)
		{
			ClienteManager.Desconectar();
			Application.Quit();
		}
		
		public void ContactoConectado(string cliente)
		{
			Console.WriteLine("Notificación de contacto conectado!");
			TreeIter iter = this.contactos.AppendValues(cliente);
			this.treeItersContactos.Add(cliente, iter);
		}
		
		public void ContactoDesconectado(string cliente)
		{
			Console.WriteLine("Notificación de contacto desconectado!");
			TreeIter iter = this.treeItersContactos[cliente];
			this.contactos.Remove(ref iter);
			this.treeItersContactos.Remove(cliente);
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
