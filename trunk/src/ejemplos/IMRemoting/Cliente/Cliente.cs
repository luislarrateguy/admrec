using System;

using log4net;
using log4net.Config;



namespace MensajeroRemoting {

	public class Cliente
	{
		private static Cliente cliente;
		static public ILog log = LogManager.GetLogger("Cliente");

		public Cliente(string nick)
		{
			ClienteManager c = new ClienteManager();
			
			
			
			if (c.conectar(nick))
				log.Debug("Anduvo la conexion");
			else
				log.Debug("No anduvo la conexion");
			log.Debug(c.getContactos());
			
			log.Debug("Listo... esperando que el usuario presione una tecla...");
			Console.ReadLine();
		}
		
		public static void Main(string[] args)
		{
			cliente = new Cliente(args[0]);
		}
	}
}
