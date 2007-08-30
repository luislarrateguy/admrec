// ClienteRemoto.cs creado conh MonoDevelop
// User: nacho - 22:53 29/08/2007
//
//

using System;

namespace MensajeroRemoting
{
	public delegate  void MensajeRecibidoHandler(string nick, string mensaje);
	public delegate  void ConexionClienteHandler(string nick);
	
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
		
		/* No estoy seguro de la utilidad de esto, pero no se borra
		public void registrarHandler()
		{
			Console.WriteLine(" ---- Registrando metodo agregarContacto");
			this.controladorConexiones.ContactoConectado +=
				new ControladorConexiones.ListaContactosHandler(this.OnContactoAgregado);
			
			Console.WriteLine(" ---- Registrando metodo quitarContacto");
			this.controladorConexiones.ContactoDesconectado +=
				new ControladorConexiones.ListaContactosHandler(this.OnContactoQuitado);
		}
		
		public void desregistrarHandlers()
		{
			Console.Write(" ---- Desregistrando metodo agregarContacto");
			this.controladorConexiones.ContactoConectado -=
				new ControladorConexiones.ListaContactosHandler(this.OnContactoAgregado);
			Console.WriteLine("... Listo!");
			
			Console.Write(" ---- Desregistrando metodo quitarContacto");
			this.controladorConexiones.ContactoDesconectado -=
				new ControladorConexiones.ListaContactosHandler(this.OnContactoQuitado);
			Console.WriteLine("... Listo!");
		}
		*/
	}
}
