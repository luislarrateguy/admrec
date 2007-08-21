// ClienteManager.cs created with MonoDevelop
// User: nacho at 10:57 21/08/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using MensajeroRemoting;
using log4net;
using log4net.Config;

namespace MensajeroRemoting
{
	
	
	public class ClienteManager
	{

		static public ILog log = LogManager.GetLogger("ClienteManager");
		private ControladorConexiones controladorConexiones;
		
		public ClienteManager()
		{
		}
		
		public bool conectar(string nombre) {	
			BasicConfigurator.Configure();

			
			TcpChannel chanConnect  = new TcpChannel();
			ChannelServices.RegisterChannel(chanConnect);
			
			log.Debug("Cachando servidor...");
			controladorConexiones = (ControladorConexiones)
				Activator.GetObject(typeof(ControladorConexiones),
				"tcp://localhost:8085/CC");
			
			if (controladorConexiones == null) {
				log.Debug("No se pudo cachar el controlador...");
				return false;
			}
			
			log.Debug("Servidor cachado!");
			
			ChannelServices.UnregisterChannel(chanConnect);
			
			/*
			TcpChannel chanServe = new TcpChannel(puerto);
			ChannelServices.RegisterChannel(chanServe);
			
			log.Debug("Registrando mi objeto remoto...");
			RemotingConfiguration.RegisterWellKnownServiceType(typeof(Host),
			                                                          "Host",
			                                                          WellKnownObjectMode.Singleton);
			*/
			/*
			log.Debug("Cachando mi propio objeto para modificarlo...");
			Host yo = (Host)Activator.GetObject(typeof(Host),
			                                              "tcp://localhost:" + puerto.ToString() + "/Host");
			log.Debug("Modificando mi objeto...");
			yo.Id = puerto.ToString();
			*/
			
			log.Debug("Conectando con servidor...");
			if (controladorConexiones.Conectar(nombre)) {
				log.Debug("Anduvo la conexion");
			} else {
				log.Debug("No anduvo la conexion");
				return false;
			}
			return true;
		}
		public ListaContactos getContactos() {
			return this.controladorConexiones.contactos;
		}
	}
}
