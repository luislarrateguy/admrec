// /home/miltondp/Desktop/IM/GUI/MainWindow.cs created with MonoDevelop
// User: miltondp at 18:16 21/07/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;
using Glade;

namespace IM.GUI
{
	public class MainWindow
	{
		[Widget]
		Gtk.Window mainWindow;
		
		public MainWindow()
		{
			Glade.XML gxml = new XML("mainwindow.glade", "mainWindow", null);
			gxml.Autoconnect(this);
			
			this.mainWindow.DeleteEvent += new DeleteEventHandler(this.OnDeleteMainWindow);
			
			
			
			this.mainWindow.ShowAll();
		}
		
		public void OnDeleteMainWindow(object o, DeleteEventArgs args)
		{
			Application.Quit();
		}
		
		public static void Main(string[] args)
		{
			Application.Init();
			new MainWindow();
			Application.Run();
		}
	}
}
