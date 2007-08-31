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
		private ClienteRemoto clienteRemoto;
		
		public event ConexionClienteHandler ContactoConectado;
		public event ConexionClienteHandler ContactoDesconectado;
		public event MensajeRecibidoHandler MensajeRecibido;
		
		public EventsHelper(ClienteRemoto cr)
		{
			this.clienteRemoto = cr;
			
			Console.WriteLine("Creando objeto EventHelper y registrando a los eventos de ControladorConexiones...");
			this.RegistrarHandler();
			Console.WriteLine("Creado EventHelper!");
		}
		
		~EventsHelper()
		{
			Console.WriteLine("Se está destruyendo el EventHelper!");
		}
		
		public void recibirMensaje(string nick, string mensaje)
		{
			Console.Write("(EventHelper) Evento RecibirMensaje recibido. Disparando el propio...");
			if (this.MensajeRecibido != null)
				this.MensajeRecibido(nick,mensaje);
			Console.WriteLine("listo");
		}
		
		public void clienteConectado(string nick)
		{
			Console.Write("(EventHelper) Evento ContactoConectado recibido. Disparando el propio...");
			if (this.ContactoConectado != null)
				this.ContactoConectado(nick);
			Console.WriteLine("listo");
		}
		
		public void clienteDesconectado(string nick)
		{
			Console.Write("(EventHelper) Evento ContactoDesconectado recibido. Disparando el propio...");
			if (this.ContactoDesconectado != null)
				this.ContactoDesconectado(nick);
			Console.WriteLine("listo");
		}
		
		public void RegistrarHandler()
		{
			Console.WriteLine(" ---- (EventHelper) Registrando metodo clienteConectado");
			this.clienteRemoto.ContactoConectado += new ConexionClienteHandler(this.clienteConectado);
			
			Console.WriteLine(" ---- (EventHelper) Registrando metodo clienteDesconectado");
			this.clienteRemoto.ContactoDesconectado += new ConexionClienteHandler(this.clienteDesconectado);
			
			Console.WriteLine(" ---- (EventHelper) Registrando metodo recibirMensaje");
			this.clienteRemoto.MensajeRecibido += new MensajeRecibidoHandler(this.recibirMensaje);
		}
		
		public void DesregistrarHandlers()
		{
			Console.WriteLine(" ---- (EventHelper) Desregistrando metodo clienteConectado");
			this.clienteRemoto.ContactoConectado -= new ConexionClienteHandler(this.clienteConectado);
			
			Console.WriteLine(" ---- (EventHelper) Desregistrando metodo clienteDesconectado");
			this.clienteRemoto.ContactoDesconectado -= new ConexionClienteHandler(this.clienteDesconectado);
			
			Console.WriteLine(" ---- (EventHelper) Desregistrando metodo recibirMensaje");
			this.clienteRemoto.MensajeRecibido -= new MensajeRecibidoHandler(this.recibirMensaje);
		}
		
		public override object InitializeLifetimeService()
		{
			return null;
		}
	}
}
