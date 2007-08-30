// ClienteRemoto.cs creado conh MonoDevelop
// User: nacho - 22:53 29/08/2007
//
//

using System;

namespace MensajeroRemoting
{
	[Serializable()]
	public class EventsHelper: MarshalByRefObject
	{
		public event ConexionClienteHandler ContactoConectado;
		public event ConexionClienteHandler ContactoDesconectado;
		public event MensajeRecibidoHandler MensajeRecibido;
		
		public EventsHelper(ClienteRemoto cr)
		{
			Console.Write("Creando objeto EventHelper...");
			cr.ContactoConectado += new ConexionClienteHandler(this.clienteConectado);
			cr.ContactoDesconectado += new ConexionClienteHandler(this.clienteDesconectado);
			cr.MensajeRecibido += new MensajeRecibidoHandler(this.recibirMensaje);
			Console.WriteLine("Creado EventHelper!");
		}
		
		~EventsHelper()
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
