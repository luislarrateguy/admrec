// /home/miltondp/Desktop/IM/GUI/MainWindow.cs created with MonoDevelop
// User: miltondp at 18:16 21/07/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections;
using System.Collections.Generic;

using Gtk;
using Glade;

namespace MensajeroRemoting
{
	public class GUICliente : MarshalByRefObject
	{
		//private ClienteManager clienteManager;
		
		[Widget]
		Gtk.Window mainWindow;
		
		[Widget]
		Gtk.ComboBox cmbEstado;
		
		[Widget]
		Gtk.VBox vbox1;
		
		Gtk.TreeView tvContactos;
		Gtk.ListStore contactos;
		Dictionary<string, TreeIter> treeItersContactos;
		
		public GUICliente()
		{
//			this.clienteManager = new ClienteManager();
//
//			Console.WriteLine("Seteando... algo que no te importa");
//			HostCliente hc = this.clienteManager.HostCliente;
//			hc.ContactoConectado += new HostCliente.ListaContactosHandler(this.ContactoConectado);
//			hc.ContactoDesconectado += new HostCliente.ListaContactosHandler(this.ContactoDesconectado);
//			Console.WriteLine("listo el seteo...");
			
			this.treeItersContactos = new Dictionary<string,Gtk.TreeIter>();
			
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
		
		public void OnCmbEstadoChanged(object o, EventArgs args)
		{
			if (this.cmbEstado.ActiveText.Equals("Conectado"))
				this.clienteManager.Conectar();
			else if (this.cmbEstado.ActiveText.Equals("Desconectado"))
				this.clienteManager.Desconectar();
		}
		
		public void OnDeleteMainWindow(object o, DeleteEventArgs args)
		{
			Application.Quit();
		}
		
		public void ContactoConectado(string cliente)
		{
			TreeIter iter = this.contactos.AppendValues(cliente);
			this.treeItersContactos.Add(cliente, iter);
		}
		
		public void ContactoDesconectado(string cliente)
		{
			TreeIter iter = this.treeItersContactos[cliente];
			this.contactos.Remove(ref iter);
			this.treeItersContactos.Remove(cliente);
		}
		
		public static void Main(string[] args)
		{
			Application.Init();
			new MainWindow();
			Application.Run();
		}
	}
}
