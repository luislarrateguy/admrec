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
		
		public Cliente() {
			
		}
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
			RemotingConfiguration.RegisterWellKnownServiceType(typeof(HostCliente),
			                                                          "Host",
			                                                          WellKnownObjectMode.Singleton);
			
			
			Console.WriteLine("Cachando mi propio objeto para modificarlo...");
			HostCliente yo = (HostCliente)Activator.GetObject(typeof(HostCliente),
			                                              "tcp://localhost:" + puerto.ToString() + "/Host");
			Console.WriteLine("Modificando mi objeto...");
			yo.Id = puerto.ToString();
			
			bool ahoraConectar = true;
			while (true) {
				Console.WriteLine("");
				Console.WriteLine("Indique la acción: (c)onectar ; (d)desconectar ; " +
				                  "un número de puerto para enviar mensaje");
				string accion = Console.ReadLine();
				
				if (accion.ToLower().Equals("c")) {
					Console.Write("Conectando...");
					if (controladorConexiones.Conectar("tcp://localhost:" + puerto.ToString() + "/Host"))
						Console.WriteLine("¡Conectado!");
					else
						Console.WriteLine("No anduvo la conexion");
				}
				else if (accion.ToLower().Equals("d")) {
					Console.Write("Desconectando...");
					controladorConexiones.Desconectar("tcp://localhost:" + puerto.ToString() + "/Host");
					Console.WriteLine(" Desconectado");
				}
				else {
					int puertoDestino = -1;
					try {
						puertoDestino = Convert.ToInt32(accion);
					}
					catch (Exception) {
						Console.WriteLine("Comando desconocido...");
						continue;
					}
					
					Console.WriteLine("Cachando el destino...");
					HostCliente hostDestino = this.GetHostByConnectionString("tcp://localhost:" +
					                                                         puertoDestino.ToString() +
					                                                         "/Host");
					
					Console.WriteLine("Cachado!");
					
					Console.Write("Escriba el mensaje a enviar:");
					string mensaje = Console.ReadLine();
					
					Console.Write("Enviando mensaje...");
					hostDestino.EnviarMensaje(puerto.ToString(), mensaje);
					Console.WriteLine("Enviado!");
				}
			}
		}
		
		private HostCliente GetHostByConnectionString(string cadenaConexion)
		{
			HostCliente nuevoCliente = (HostCliente)Activator.GetObject(typeof(HostCliente),
			                                              cadenaConexion);
			
			return nuevoCliente;
		}
		
		public static void Main(string[] args)
		{
			cliente = new Cliente(Convert.ToInt32(args[0]));
		}
	}
}
