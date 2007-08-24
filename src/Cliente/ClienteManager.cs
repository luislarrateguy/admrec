using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace MensajeroRemoting {
	public class ClienteManager
	{
		private ControladorConexiones controladorConexiones;
		private TcpServerChannel miCanalEscucha;
		private int puerto;
		private HostCliente hc;
		

		public ClienteManager()
		{
			TcpChannel chanConnect  = new TcpChannel();
			ChannelServices.RegisterChannel(chanConnect);
			
			Console.WriteLine("Cachando servidor...");
			controladorConexiones = (ControladorConexiones)Activator.GetObject(typeof(ControladorConexiones),
			                                                                   "tcp://localhost:8085/CC");
			
			if (controladorConexiones == null) {
				Console.WriteLine("No se pudo cachar el controlador...");
				return;
			}
			
			Console.WriteLine("Servidor cachado!");
			
			ChannelServices.UnregisterChannel(chanConnect);
			
			/* Esto hace que busque un puerto disponible
			 * Cambio el canal bidireccional por uno Servidor únicamente
			 * Solo se registra debajo (con RegisterWellKnownServiceType)
			 */
            this.miCanalEscucha = new TcpServerChannel(0);
			Console.WriteLine("Mi canal escucha: "+miCanalEscucha.GetChannelUri());
			
			//TcpChannel chanServe = new TcpChannel(puerto);
			//ChannelServices.RegisterChannel(chanServe);
			
			Console.WriteLine("Registrando mi objeto remoto...");
			RemotingConfiguration.RegisterWellKnownServiceType(typeof(HostCliente),
			                                                          "Host",
			                                                          WellKnownObjectMode.Singleton);
			
			
			Console.WriteLine("Cachando mi propio objeto para modificarlo...");
			this.hc = (HostCliente)Activator.GetObject(typeof(HostCliente),
			                                              miCanalEscucha.GetChannelUri()+ "/Host");
			Console.WriteLine("Modificando mi objeto...");
			this.hc.Id = miCanalEscucha.GetChannelUri();
			
			bool ahoraConectar = true;
			
		}
		
		private HostCliente GetHostByConnectionString(string cadenaConexion)
		{
			HostCliente nuevoCliente = (HostCliente)Activator.GetObject(typeof(HostCliente),
			                                              cadenaConexion);
			
			return nuevoCliente;
		}
		
		public bool conectar() {
			Console.Write("Conectando...");
			if (controladorConexiones.Conectar(this.miCanalEscucha.GetChannelUri() + "/Host")) {
				Console.WriteLine("¡Conectado!");
				return true;
			} else {
				Console.WriteLine("No anduvo la conexion");
				return false;
			}
		}
		
		public bool desconectar() {
			Console.Write("Desconectando...");
			controladorConexiones.Desconectar(miCanalEscucha.GetChannelUri() + "/Host");
			Console.WriteLine(" Desconectado");
			return true;
		}
		
		public HostCliente obtenerDestino(int puertoDestino) {
			Console.WriteLine("Cachando el destino...");
			HostCliente hostDestino = this.GetHostByConnectionString("tcp://localhost:" +
				puertoDestino + "/Host");
			Console.WriteLine("Cachado!");
			return hostDestino;
		}
		
		public bool enviarMensaje(HostCliente h, string m) {
			Console.Write("Enviando mensaje...");
			h.EnviarMensaje(this.hc.Id, m);
			Console.WriteLine("Enviado!");
			return true;
		}	

	}
}
