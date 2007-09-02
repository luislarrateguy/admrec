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

using log4net.Appender;
using log4net.Config;

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
		
		private string nickDestino;
		private MainWindow mainWindow;
		
		private log4net.ILog logger;
		
		public VentanaChat(MainWindow mainWindow, string nickDestino)
		{
			this.logger = log4net.LogManager.GetLogger(this.GetType());
			System.IO.FileInfo fi = new System.IO.FileInfo("log4net.config.xml");
			this.logger.Debug(fi.Exists);
			XmlConfigurator.Configure(fi);
			
			this.mainWindow = mainWindow;
			this.nickDestino = nickDestino;
			
			this.logger.Debug("Creando interfaz glade de ventanaChat...");
			Glade.XML gxml = new XML("ventanachat.glade", "ventanaChat", null);
			gxml.Autoconnect(this);
			
			this.logger.Debug("Seteando título...");
			this.ventanaChat.Title = this.mainWindow.Nick + " (yo) hablando con: "+nickDestino;
			
			this.logger.Debug("Seteando foco a textviewMensaje...");
			this.textviewMensaje.HasFocus = true;
			
			this.logger.Debug("Agregando evento DeleteEvent...");
			this.ventanaChat.DeleteEvent += new DeleteEventHandler(this.OnVentanaChatDelete);
			
			this.logger.Debug("Ejecutando ShowAll...");
			this.ventanaChat.ShowAll();
		}
		
		public VentanaChat(MainWindow mainWindow, string nickDestino, string mensajeInicial) : this(mainWindow, nickDestino)
		{
			this.MensajeRecibido(mensajeInicial);
		}
		
		public void OnBtnEnviarClicked(object o, EventArgs args)
		{
			this.logger.Debug("OnBtnEnviarClicked");
			string mensaje = this.textviewMensaje.Buffer.Text;
			
			if (mensaje.Equals(String.Empty))
				return;
			
			this.mainWindow.ControladorCliente.EnviarMensaje(this.nickDestino, mensaje);
			
			this.logger.Debug("Agregando mensaje enviado a mi ventana");
			this.textviewChat.Buffer.InsertAtCursor(this.mainWindow.Nick + ": " + mensaje + "\n");
			this.logger.Debug("Limpiando mi textview de mensaje");
			this.textviewMensaje.Buffer.Clear();
			this.logger.Error("Seteando foco a textview de mensaje");
			this.textviewMensaje.HasFocus = true;
		}
		
		public void OnVentanaChatDelete(object o, DeleteEventArgs args)
		{
			this.logger.Debug("VentanaChat " + this.nickDestino + " - DeleteEvent");
			this.mainWindow.VentanaChatCerrada(this.nickDestino);
		}
		
		public void MensajeRecibido(string mensaje)
		{
			this.logger.Debug("Ejecutando MensajeRecibido");
			this.textviewChat.Buffer.InsertAtCursor(this.nickDestino + ": " + mensaje + "\n");
		}
		
		public bool Activar()
		{
//			this.logger.Debug("Ejecutando método Activar");
//			this.ventanaChat.Activate();
			return true;
		}
	}
}
