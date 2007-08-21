using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using log4net;
using log4net.Config;

namespace MensajeroRemoting
{
	public class ControladorConexiones : MarshalByRefObject
	{
		private ListaContactos clientesConectados;
		static public ILog log = LogManager.GetLogger("ControladorConexiones");
		
		public ControladorConexiones()
		{
			BasicConfigurator.Configure();
			this.clientesConectados = new ListaContactos();
			log.Debug(" - Objeto ControladorConexiones creado");
		}
		
		public bool Conectar (string nickCliente)
		{
			log.Debug("");
			log.Debug("Petición de conexión. Nick: " + nickCliente);
			
			if (!this.clientesConectados.addContacto(nickCliente)) {
				log.Debug(" - Error: cliente ya conectado");
				return false;
			}
			
			log.Debug("El cliente es nuevo...");
			
			log.Debug("Listo!");
			return true;
		}
		public ListaContactos contactos {
			get { return this.clientesConectados;}
		}
	}
	
}
