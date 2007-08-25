using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace MensajeroRemoting {
	public class Servidor
	{
		public static void Main(string[] args)
		{
			BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
			provider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
			IDictionary props = new Hashtable();
			props["port"] = 8085;
			props["name"] = "tcp://localhost";
			
//			TcpChannel chanConnect = new TcpChannel(8085);
//			ChannelServices.RegisterChannel(chanConnect);
//			
//			RemotingConfiguration.RegisterWellKnownServiceType(typeof(ControladorConexiones),
//			                                                          "CC",
//			                                                          WellKnownObjectMode.Singleton);
			
			ControladorConexiones cc = new ControladorConexiones();
			ChannelServices.RegisterChannel(new TcpChannel(props, null, provider));
			RemotingServices.Marshal(cc, "CC");
			
			Console.WriteLine("Listo... presiona una tecla y vuelo de aca");
			Console.ReadLine();
		}
	}
}
