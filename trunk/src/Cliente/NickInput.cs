//    MensajeroRemoting - Mensajero instantáneo hecho con .NET Remoting
//    y otras tecnologías de .NET.
//    Copyright (C) 2007  Luis Ignacio Larrateguy, Milton Pividori y César Sandrigo
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;

using Gtk;
using Gdk;

namespace MensajeroRemoting
{
	public class NickInput : Dialog
	{
		private Gtk.Entry entryNick;
		private Gtk.Button btnAceptar;
		private Gtk.Button btnCancelar;
		
		private string nickEscogido;
		
		public NickInput(Gtk.Window w) : base("Escoja un nick", w, DialogFlags.Modal)
		{
			//this.Response += new ResponseHandler(this.OnResponse);
			//this.DeleteEvent += new DeleteEventHandler(this.OnDeleteEvent);
			this.Deletable = false;
			
			this.entryNick = new Entry(15);
			this.VBox.Add(this.entryNick);
			this.entryNick.Show();
			
			this.btnCancelar = new Button(Stock.Cancel);
			this.ActionArea.Add(this.btnCancelar);
			this.btnCancelar.Clicked += new EventHandler(this.OnBtnCancelarClicked);
			this.btnCancelar.Show();
			
			this.btnAceptar = new Button(Stock.Ok);
			this.ActionArea.Add(this.btnAceptar);
			this.btnAceptar.Clicked += new EventHandler(this.OnBtnAceptarClicked);
			this.btnAceptar.Show();
			
			this.nickEscogido = null;
		}
		
		public string NickEscogido
		{
			get { return this.nickEscogido; }
		}
		
//		protected void OnClose(Event e)
//		{
//			Console.WriteLine("OnClose ejecutado");
//		}
//		
//		public void OnResponse(object o, ResponseArgs args)
//		{
//			Console.WriteLine(args.ResponseId);
//			if (args.ResponseId == ResponseType.None) {
//				Console.WriteLine("aja, none");
//				args.RetVal = false;
//			}
//		}
		
		public void OnBtnAceptarClicked(object o, EventArgs args)
		{
			// Verifico que el nick no sea nulo
			if (this.entryNick.Text.Trim().Equals("")) {
				MessageDialog md = new MessageDialog(this, DialogFlags.Modal,
				                                     MessageType.Error, ButtonsType.Ok,
				                                     "El nick no puede ser nulo");
				
				md.Run();
				md.Destroy();
				
				this.nickEscogido = null;
				this.entryNick.Text = "";
				return;
			}
			
			// Verifico que el nick no esté ocupado
			Console.WriteLine("Viendo si el nick esta ocupado...");
			if (ClienteManager.NickOcupado(this.entryNick.Text)) {
				MessageDialog md = new MessageDialog(this, DialogFlags.Modal,
				                                     MessageType.Error, ButtonsType.Ok,
				                                     "El nick está ocupado. Escoja otro.");
				
				md.Run();
				md.Destroy();
				
				this.nickEscogido = null;
				this.entryNick.Text = "";
				
				return;
			}
			
			Console.WriteLine("Listo, nick escogido...");
			this.nickEscogido = this.entryNick.Text;
			this.Respond(ResponseType.Ok);
		}
		
		public void OnBtnCancelarClicked(object o, EventArgs args)
		{
			this.nickEscogido = null;
			this.Respond(ResponseType.Cancel);
			this.Destroy();
		}
	}
}
