using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace MensajeroRemoting
{
	public class ControladorConexiones : MarshalByRefObject
	{
		private List<string> clientesConectados;
		
		public ControladorConexiones()
		{
			this.clientesConectados = new List<string>();
			Console.WriteLine(" - Objeto ControladorConexiones creado");
		}
		
		private Host GetHostByConnectionString(string cadenaConexion)
		{
			Host nuevoCliente = (Host)Activator.GetObject(typeof(Host),
			                                              cadenaConexion);
			
			return nuevoCliente;
		}
		
		
		public bool Conectar (string cadenaConexion)
		{
			Console.WriteLine("");
			Console.WriteLine("Petición de conextion. Cadena: " + cadenaConexion);
			Console.WriteLine("Cachando el objeto remoto...");
			Host nuevoCliente = this.GetHostByConnectionString(cadenaConexion);
			Console.WriteLine("Cachado!");
			
			if (this.clientesConectados.Contains(cadenaConexion)) {
				Console.WriteLine(" - Error: cliente ya conectado");
				return false;
			}
			
			Console.WriteLine("El cliente es nuevo...");
			
			List<string> clientesAntes = new List<string>();
			foreach (string h in this.clientesConectados) {
				Host host = this.GetHostByConnectionString(h);
				Console.WriteLine("Avisando a " + h + " que " + nuevoCliente.Id + " se conectó");
				host.NotificarNuevoContacto(cadenaConexion);
				clientesAntes.Add(h);
			}
			
			Console.WriteLine("Ahora le meto los contactos existentes al nuevo cliente...");
			
			nuevoCliente.Contactos = clientesAntes.ToArray();
			
			Console.WriteLine("Agrego el cliente a mi lista de clientes conectados");
			this.clientesConectados.Add(cadenaConexion);
			
			Console.WriteLine("Listo!");
			return true;
		}
	}
	
}
