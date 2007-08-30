// ClienteRemoto.cs creado conh MonoDevelop
// User: nacho - 22:53 29/08/2007
//
//

using System;

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
		}
		
		public void recibirMensaje(string nick, string mensaje)
		{
			if (this.MensajeRecibido != null)
				this.MensajeRecibido(nick,mensaje);
		}
		
		public void clienteConectado(string nick)
		{
			if (this.ContactoConectado != null)
				this.ContactoConectado(nick);
		}
		
		public void clienteDesconectado(string nick)
		{
			if (this.ContactoDesconectado != null)
				this.ContactoDesconectado(nick);
		}
		
		public void RegistrarHandlers()
		{
			ControladorConexiones cc = ControladorCliente.controladorConexiones;
			
			Console.WriteLine(" ---- Registrando metodo agregarContacto");
			cc.ClienteConectado += new ConexionCliente(this.clienteConectado);
			
			Console.WriteLine(" ---- Registrando metodo quitarContacto");
			cc.ClienteDesconectado += new ConexionCliente(this.clienteDesconectado);
		}
		
		public void DesregistrarHandlers()
		{
			ControladorConexiones cc = ControladorCliente.controladorConexiones;
			
			Console.Write(" ---- Desregistrando metodo agregarContacto");
			cc.ClienteConectado -=
				new ConexionCliente(this.clienteConectado);
			Console.WriteLine("... Listo!");
			
			Console.Write(" ---- Desregistrando metodo quitarContacto");
			cc.ClienteDesconectado -=
				new ConexionCliente(this.clienteDesconectado);
			Console.WriteLine("... Listo!");
		}

	}
}
