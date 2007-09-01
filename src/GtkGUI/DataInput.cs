// DataInput.cs created with MonoDevelop
// User: miltondp at 6:02 PM 8/28/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;

namespace MensajeroRemoting
{
	public class DataInput : ServidorInput
	{
		private string nickAnterior;
		
		public DataInput(Window w, string nick) : base(w)
		{
			this.entryNick.Text = nick;
		}
		
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
			
			Console.WriteLine("Listo, nick escogido...");
			this.nickEscogido = this.entryNick.Text;
			this.servidorEscogido = this.entryServidor.Text;
			this.Respond(ResponseType.Ok);
		}
	}
}
