using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using log4net;
using log4net.Config;

namespace MensajeroRemoting {
	public class Servidor
	{
		static public ILog log = LogManager.GetLogger("Servidor");
		public static void Main(string[] args)
		{
			BasicConfigurator.Configure();
			TcpChannel chanConnect = new TcpChannel(8085);
			ChannelServices.RegisterChannel(chanConnect);
			
			RemotingConfiguration.RegisterWellKnownServiceType(typeof(ControladorConexiones),
			                                                          "CC",
			                                                          WellKnownObjectMode.Singleton);
			log.Debug("Listo... presiona una tecla y vuelo de aca");
			Console.ReadLine();
		}
	}
}
