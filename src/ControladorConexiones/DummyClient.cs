// DummyClient.cs created with MonoDevelop
// User: miltondp at 8:36 AM 9/3/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace MensajeroRemoting
{
	internal class DummyClient : ICliente
	{
		public DummyClient()
		{
		}
		
		public MensajeRecibidoHandler MetodoMensajeRecibido
		{
			get {
				return new MensajeRecibidoHandler(delegate {});
			}
		}
		
		public ConexionClienteHandler MetodoContactoConectado
		{
			get {
				return new ConexionClienteHandler(delegate {});
			}
		}
		
		public ConexionClienteHandler MetodoContactoDesconectado
		{
			get {
				return new ConexionClienteHandler(delegate {});
			}
		}
		
	}
}
