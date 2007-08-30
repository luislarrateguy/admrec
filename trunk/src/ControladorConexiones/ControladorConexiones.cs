/*
    MensajeroRemoting - Mensajero instantáneo hecho con .NET Remoting
    y otras tecnologías de .NET.
    Copyright (C) 2007  Luis Ignacio Larrateguy, Milton Pividori y César Sandrigo

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;

namespace MensajeroRemoting
{
	public class ControladorConexiones : MarshalByRefObject
	{
		public delegate void ConexionCliente(ClienteInfo unCliente);
		
		public event ListaContactosHandler ClienteConectado;
		public event ListaContactosHandler ClienteDesconectado;
		
		// Lista de las cadenas de conexion
		private List<ClienteInfo> clientesConectados;
//		
//		// Lista de los nicks ocupados
//		private List<string> nicksOcupados;
		
		public ControladorConexiones()
		{
			this.clientesConectados = new List<ClienteInfo>();
			//this.nicksOcupados = new List<string>();
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
		
		/* El parámetro "nick" no sería necesario para desconectar, pero así es más facil,
		 * ya que no hay que relacionar los nicks con las cadenas de conexion. */
		public void Desconectar(ClienteInfo unCliente)
		{
			Console.WriteLine("");
			Console.WriteLine("Petición de desconextion. Cadena: " + unCliente.cadenaConexion);
			
			if (!this.clientesConectados.Contains(unCliente)) {
				Console.WriteLine(" - Error: cliente no conectado");
				return;
			}
			
			Console.WriteLine("Bien, el cliente estaba conectado. Lo saco de la lista...");
			this.clientesConectados.Remove(unCliente);
			
//			Console.WriteLine("Saco también su nick...");
//			this.nicksOcupados.Remove(nick);
			
//			foreach (string h in this.clientesConectados) {
//				HostCliente host = this.GetHostByConnectionString(h);
//				Console.WriteLine("Avisando a " + h + " que " + cadenaConexion + " se desconectó");
//				host.QuitarCliente(cadenaConexion);
//			}
			
			Console.WriteLine("Avisando a los otros que se desconecto");
			if (this.ClienteDesconectado != null)
				this.ClienteDesconectado(unCliente);
			
			Console.WriteLine("Listo!");
			return;
		}
		
		public ClienteInfo[] Conectar (ClienteInfo unCliente)
		{
			Console.WriteLine("");
			Console.WriteLine("Petición de conextion. Cadena: " + unCliente.cadenaConexion);
			
			if (this.clientesConectados.Contains(unCliente)) {
				Console.WriteLine(" - Error: cliente ya conectado");
			}
			
			// Verifico que el nick no esté ocupado
			if (this.NickOcupado(unCliente.nick)) {
				Console.WriteLine(" ¡El nick ya está ocupado!");
				throw new Exception("El nick ya está ocupado");
			}
			
			Console.WriteLine("El cliente es nuevo...");
			
			Console.WriteLine("Avisando a los otros que se conecto uno nuevo");
			if (this.ClienteConectado != null)
				this.ClienteConectado(unCliente);
			
			Console.WriteLine("Pasando clientesConectados a array");
			ClienteInfo[] clientesSinElNuevo = this.clientesConectados.ToArray();
			
			Console.WriteLine("Agrego el cliente a mi lista de clientes conectados");
			this.clientesConectados.Add(unCliente);
			
//			Console.WriteLine("Agrego el nick del cliente nuevo...");
//			this.nicksOcupados.Add(nick);
			
			Console.WriteLine("Listo!");
			
			return clientesSinElNuevo;
		}
		
		public bool NickOcupado(string nick)
		{
			foreach (ClienteInfo ci in this.clientesConectados) {
				if (nick.Equals(ci.nick))
					return true;
			}
			
			return false;
		}
		
		public bool NickDisponible(string nick)
		{
			return (!this.NickOcupado(nick));
		}
		
		public override object InitializeLifetimeService()
		{
			return null;
		}
	}
	
}
