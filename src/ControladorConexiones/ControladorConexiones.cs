using System;
using System.Collections.Generic;

namespace MensajeroRemoting
{
	public class ControladorConexiones : MarshalByRefObject
	{
		public delegate void ListaContactosHandler(string cadena);
		
		public event ListaContactosHandler ContactoConectado;
		public event ListaContactosHandler ContactoDesconectado;
		
		private List<string> clientesConectados;
		//private List<IListaContactos> clientes;
		
		public ControladorConexiones()
		{
			this.clientesConectados = new List<string>();
			//this.clientes = new List<MensajeroRemoting.IListaContactos>();
			Console.WriteLine(" - Objeto ControladorConexiones creado");
		}
		
//		private HostCliente GetHostByConnectionString(string cadenaConexion)
//		{
//			HostCliente nuevoCliente = (HostCliente)Activator.GetObject(typeof(HostCliente),
//			                                              cadenaConexion);
//			
//			return nuevoCliente;
//		}
		
//		public void Suscribir(IListaContactos lc)
//		{
//			this.clientes.Add(lc);
//			
////			this.ContactoConectado += lc.OnContactoAgregado;
////			this.ContactoDesconectado += lc.OnContactoQuitado;
//		}
		
		public void Desconectar(string cadena)
		{
			Console.WriteLine("");
			Console.WriteLine("Petición de desconextion. Cadena: " + cadena);
			
			if (!this.clientesConectados.Contains(cadena)) {
				Console.WriteLine(" - Error: cliente no conectado");
				return;
			}
			
			Console.WriteLine("Bien, el cliente estaba conectado. Lo saco de la lista...");
			this.clientesConectados.Remove(cadena);
			
//			foreach (string h in this.clientesConectados) {
//				HostCliente host = this.GetHostByConnectionString(h);
//				Console.WriteLine("Avisando a " + h + " que " + cadenaConexion + " se desconectó");
//				host.QuitarCliente(cadenaConexion);
//			}
			
			Console.WriteLine("Avisando a los otros que se desconecto");
			if (this.ContactoDesconectado != null)
				this.ContactoDesconectado(cadena);
//			foreach (IListaContactos lc in this.clientes)
//				lc.OnContactoQuitado(cadena);
			
			Console.WriteLine("Listo!");
			return;
		}
		
		public string[] Conectar (string cadena)
		{
			Console.WriteLine("");
			Console.WriteLine("Petición de conextion. Cadena: " + cadena);
			Console.WriteLine("Cachando el objeto remoto...");
			//HostCliente nuevoCliente = this.GetHostByConnectionString(cadenaConexion);
			Console.WriteLine("Cachado!");
			
			if (this.clientesConectados.Contains(cadena)) {
				Console.WriteLine(" - Error: cliente ya conectado");
				return null;
			}
			
			Console.WriteLine("El cliente es nuevo...");
			
			//List<string> clientesAntes = new List<string>();
//			foreach (string h in this.clientesConectados) {
//				HostCliente host = this.GetHostByConnectionString(h);
//				Console.WriteLine("Avisando a " + h + " que " + nuevoCliente.Id + " se conectó");
//				host.AgregarCliente(cadenaConexion);
//				
//				nuevoCliente.AgregarCliente(h);
//			}
			
			Console.WriteLine("Avisando a los otros que se conecto uno nuevo");
			if (this.ContactoConectado != null)
				this.ContactoConectado(cadena);
//			foreach (IListaContactos lc in this.clientes)
//				lc.OnContactoAgregado(cadena);
			
			string[] clientesSinElNuevo = this.clientesConectados.ToArray();
			
			Console.WriteLine("Agrego el cliente a mi lista de clientes conectados");
			this.clientesConectados.Add(cadena);
			
			Console.WriteLine("Listo!");
			
			return clientesSinElNuevo;
		}
		
		public override object InitializeLifetimeService()
		{
			return null;
		}
	}
	
}
