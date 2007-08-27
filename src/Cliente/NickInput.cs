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
using Controles;

namespace MensajeroRemoting
{
	public class NickInput : Controles.NickInputEnBoo
	{
		
		public NickInput(Gtk.Window w) : base(w) {}
		
		public override void OnBtnAceptarClicked(object o, EventArgs args)
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
	}
}
