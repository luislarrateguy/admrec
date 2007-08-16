using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace MensajeroRemoting {
	public class Cliente
	{
		private static Cliente cliente;
		private static ControladorConexiones controladorConexiones;
		
		private int puerto;
		
		public Cliente(int puerto)
		{
			this.puerto = puerto;
			
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
			TcpChannel chanServe = new TcpChannel(puerto);
			ChannelServices.RegisterChannel(chanServe);
			
			Console.WriteLine("Registrando mi objeto remoto...");
			RemotingConfiguration.RegisterWellKnownServiceType(typeof(Host),
			                                                          "Host",
			                                                          WellKnownObjectMode.Singleton);
			
			
			Console.WriteLine("Cachando mi propio objeto para modificarlo...");
			Host yo = (Host)Activator.GetObject(typeof(Host),
			                                              "tcp://localhost:" + puerto.ToString() + "/Host");
			Console.WriteLine("Modificando mi objeto...");
			yo.Id = puerto.ToString();
			
			Console.WriteLine("Conectando con servidor...");
			if (controladorConexiones.Conectar("tcp://localhost:" + puerto.ToString() + "/Host"))
				Console.WriteLine("Anduvo la conexion");
			else
				Console.WriteLine("No anduvo la conexion");
			
			Console.WriteLine("Listo... esperando que el usuario presione una tecla...");
			Console.ReadLine();
		}
		
		public static void Main(string[] args)
		{
			cliente = new Cliente(Convert.ToInt32(args[0]));
		}
	}
}
