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

using Gtk;
using Glade;

namespace MensajeroRemoting
{
	public class VentanaChat
	{
		[Widget]
		Gtk.Window ventanaChat;
		
		[Widget]
		Gtk.TextView textviewMensaje;
		
		[Widget]
		Gtk.TextView textviewChat;
		
		private string cadenaConexionDestino;
		private string nickPropio;
		private MainWindow mainWindow;
		private ControladorCliente ClienteManager;
		
		public VentanaChat(MainWindow mainWindow, string cadenaConexionDestino, string nickPropio, ControladorCliente cl, string nickDestino)
		{
			this.ClienteManager = cl;
			this.mainWindow = mainWindow;
			this.cadenaConexionDestino = cadenaConexionDestino;
			this.nickPropio = nickPropio;
			
			Console.WriteLine("Creando interfaz glade de ventanaChat...");
			Glade.XML gxml = new XML("ventanachat.glade", "ventanaChat", null);
			gxml.Autoconnect(this);
			
			Console.WriteLine("Seteando título...");
			this.ventanaChat.Title = nickDestino;
			
			Console.WriteLine("Seteando foco a textviewMensaje...");
			this.textviewMensaje.HasFocus = true;
			
			Console.WriteLine("Agregando evento DeleteEvent...");
			this.ventanaChat.DeleteEvent += new DeleteEventHandler(this.OnVentanaChatDelete);
			
			Console.WriteLine("Ejecutando ShowAll...");
			this.ventanaChat.ShowAll();
		}
		
		public void OnBtnEnviarClicked(object o, EventArgs args)
		{
			string mensaje = this.textviewMensaje.Buffer.Text;
			
			if (mensaje.Equals(String.Empty))
				return;
			
			ClienteManager.EnviarMensaje(this.cadenaConexionDestino, mensaje);
			
			this.textviewChat.Buffer.InsertAtCursor(this.nickPropio + ": " + mensaje + "\n");
			this.textviewMensaje.Buffer.Clear();
			this.textviewMensaje.HasFocus = true;
		}
		
		public void OnVentanaChatDelete(object o, DeleteEventArgs args)
		{
			Console.WriteLine("VentanaChat " + this.cadenaConexionDestino + " - DeleteEvent");
			this.mainWindow.VentanaChatCerrada(this.cadenaConexionDestino);
		}
		
		public void MensajeRecibido(string origen, string mensaje)
		{
			this.textviewChat.Buffer.InsertAtCursor(origen + ": " + mensaje + "\n");
		}
		
		public bool Activar()
		{
			return this.ventanaChat.Activate();
		}
	}
}
