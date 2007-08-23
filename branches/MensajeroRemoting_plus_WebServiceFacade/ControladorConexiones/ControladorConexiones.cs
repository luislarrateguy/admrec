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
		
		private HostCliente GetHostByConnectionString(string cadenaConexion)
		{
			HostCliente nuevoCliente = (HostCliente)Activator.GetObject(typeof(HostCliente),
			                                              cadenaConexion);
			
			return nuevoCliente;
		}
		
		public void Desconectar(string cadenaConexion)
		{
			Console.WriteLine("");
			Console.WriteLine("Petición de conextion. Cadena: " + cadenaConexion);
			
			if (!this.clientesConectados.Contains(cadenaConexion)) {
				Console.WriteLine(" - Error: cliente no conectado");
				return;
			}
			
			Console.WriteLine("Bien, el cliente estaba conectado. Lo saco de la lista...");
			this.clientesConectados.Remove(cadenaConexion);
			
			foreach (string h in this.clientesConectados) {
				HostCliente host = this.GetHostByConnectionString(h);
				Console.WriteLine("Avisando a " + h + " que " + cadenaConexion + " se desconectó");
				host.ContactoDesconectado(cadenaConexion);
			}
			
			Console.WriteLine("Listo!");
			return;
		}
		
		public bool Conectar (string cadenaConexion)
		{
			Console.WriteLine("");
			Console.WriteLine("Petición de conextion. Cadena: " + cadenaConexion);
			Console.WriteLine("Cachando el objeto remoto...");
			HostCliente nuevoCliente = this.GetHostByConnectionString(cadenaConexion);
			Console.WriteLine("Cachado!");
			
			if (this.clientesConectados.Contains(cadenaConexion)) {
				Console.WriteLine(" - Error: cliente ya conectado");
				return false;
			}
			
			Console.WriteLine("El cliente es nuevo...");
			
			List<string> clientesAntes = new List<string>();
			foreach (string h in this.clientesConectados) {
				HostCliente host = this.GetHostByConnectionString(h);
				Console.WriteLine("Avisando a " + h + " que " + nuevoCliente.Id + " se conectó");
				host.ContactoConectado(cadenaConexion);
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
