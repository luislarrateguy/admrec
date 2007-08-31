// ClienteRemoto.cs creado conh MonoDevelop
// User: nacho - 22:53 29/08/2007
//
//

using System;
using System.Runtime.Remoting.Lifetime;

namespace MensajeroRemoting
{

	public delegate void MensajeRecibidoHandler(string nick, string mensaje);
	public delegate void ConexionClienteHandler(string nick);
	
	[Serializable()]
	public class ClienteRemoto: MarshalByRefObject
	{
		public event ConexionClienteHandler ContactoConectado;
		public event ConexionClienteHandler ContactoDesconectado;
		public event MensajeRecibidoHandler MensajeRecibido;
		
		public ClienteRemoto()
		{
		}
		
		~ClienteRemoto()
		{
			Console.WriteLine("Se está destruyendo el ClienteRemoto!");
//			this.DesregistrarHandlers();
		}
		
		public void recibirMensaje(string nick, string mensaje)
		{
			Console.Write("(ClienteRemoto) Evento MensajeRecibido recibido. Disparando el propio...");
			if (this.MensajeRecibido != null)
				this.MensajeRecibido(nick,mensaje);
			Console.WriteLine("listo");
		}
		
		public void clienteConectado(string nick)
		{
			Console.Write("(ClienteRemoto) Evento ContactoConectado recibido. Disparando el propio...");
			if (this.ContactoConectado != null)
				this.ContactoConectado(nick);
			Console.WriteLine("listo");
		}
		
		public void clienteDesconectado(string nick)
		{
			Console.Write("(ClienteRemoto) Evento ContactoDesconectado recibido. Disparando el propio...");
			if (this.ContactoDesconectado != null)
				this.ContactoDesconectado(nick);
			Console.WriteLine("listo");
		}
		
		public void RegistrarHandlers()
		{
			ControladorConexiones cc = ControladorCliente.controladorConexiones;
			
			Console.WriteLine(" ---- (ClienteRemoto) Registrando metodo clienteConectado");
			cc.ClienteConectado += new ConexionCliente(this.clienteConectado);
			
			Console.WriteLine(" ---- (ClienteRemoto) Registrando metodo clienteDesconectado");
			cc.ClienteDesconectado += new ConexionCliente(this.clienteDesconectado);
		}
		
		public void DesregistrarHandlers()
		{
			ControladorConexiones cc = ControladorCliente.controladorConexiones;
			
			Console.WriteLine(" ---- (ClienteRemoto) Desregistrando metodo clienteConectado");
			cc.ClienteConectado -=
				new ConexionCliente(this.clienteConectado);
			
			Console.WriteLine(" ---- (ClienteRemoto) Desregistrando metodo clienteDesconectado");
			cc.ClienteDesconectado -=
				new ConexionCliente(this.clienteDesconectado);
		}
		
		public override object InitializeLifetimeService()
		{
			return null;
		}
	}
}
