using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

using Prueba;

namespace MensajeroRemoting {
	public class Servidor
	{
		public static void Main(string[] args)
		{
			TcpChannel chanConnect = new TcpChannel(8085);
			ChannelServices.RegisterChannel(chanConnect);
			
			RemotingConfiguration.RegisterWellKnownServiceType(typeof(ControladorConexiones),
			                                                          "CC",
			                                                          WellKnownObjectMode.Singleton);
			
			Console.WriteLine("Listo... presiona una tecla y vuelo de aca");
			Console.ReadLine();
		}
	}
}
