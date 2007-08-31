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
	public delegate void ConexionClienteHandler(string nick);
	
	public class ControladorConexiones : MarshalByRefObject
	{
		public event ConexionClienteHandler ClienteConectado;
		public event ConexionClienteHandler ClienteDesconectado;
		
		private Dictionary<string, string> clientesConectados;
		
		public ControladorConexiones()
		{
			this.clientesConectados = new Dictionary<string, string>();
		}
		
		~ControladorConexiones()
		{
			Console.WriteLine("Se está destruyendo el ControladorConexiones!");
		}
		
		public string[] Conectar (string cadenaConexion, string nick)
		{
			Console.WriteLine("");
			Console.WriteLine("Petición de conexion. Cadena: " + cadenaConexion);
			
			///BEGIN Estas 2 hacen lo mismo
			if (this.clientesConectados.ContainsKey(nick))
				throw new Exception("El cliente ya esta conectado. Imposible continuar");
			
			// Verifico que el nick no esté ocupado
			if (this.NickOcupado(nick))
				throw new Exception("El nick ya está ocupado");
			///END Estas 2 hacen lo mismo
			
			
			Console.WriteLine("El cliente es nuevo...");
					
			Console.WriteLine("Pasando clientesConectados a array...");
			List<string> clientesSinElNuevo = new List<string>();
			foreach(string unNick in this.clientesConectados.Keys) {
				Console.WriteLine("  procesando " + unNick);
				clientesSinElNuevo.Add(unNick);
			}
			
			Console.WriteLine("Agrego el cliente a mi lista de clientes conectados");
			this.clientesConectados.Add(nick, cadenaConexion);
			
			Console.WriteLine("Listo!");
			
			Console.WriteLine("Avisando a los otros que se conecto uno nuevo, pero despues de haberlo agregado");
			if (this.ClienteConectado != null)
				this.ClienteConectado(nick);
			
			return clientesSinElNuevo.ToArray();
		}
		
		public void Desconectar(string nick)
		{
			Console.WriteLine("");
			Console.WriteLine("Petición de desconextion de " + nick);
			
			if (!this.clientesConectados.ContainsKey(nick))
				throw new Exception("El cliente no esta conectado. Imposible descontarlo");
			
			Console.WriteLine("Bien, el cliente estaba conectado. Lo saco de la lista...");
			this.clientesConectados.Remove(nick);
			
			Console.WriteLine("Avisando a los otros que se desconecto");
			if (this.ClienteDesconectado != null)
				this.ClienteDesconectado(nick);
			Console.WriteLine("Listo!");
		}
		
		public void EnviarMensaje(string nickOrigen, string nickDestino, string mensaje)
		{
			Console.WriteLine();
			Console.WriteLine("ControladorConexiones.EnviarMensaje ejecutado...");
			
			if (!this.clientesConectados.ContainsKey(nickOrigen))
				throw new Exception("No se puede enviar un mensaje desde un cliente no conectado");
			
			if (!this.clientesConectados.ContainsKey(nickDestino))
				throw new Exception("El cliente destino no esta conectado. No se puede enviar el mensaje");
			
			Console.WriteLine("Cachando objeto ClienteRemoto desde ControladorConexiones...");
			ClienteRemoto clienteRemoto = (ClienteRemoto)Activator.GetObject(typeof(ClienteRemoto),
			                                                                 this.clientesConectados[nickDestino]);
			
			Console.WriteLine("Ejecutando método recibirMensaje del clienteRemoto...");
			clienteRemoto.recibirMensaje(nickOrigen, mensaje);
		}
		
		private bool NickOcupado(string nick)
		{
			foreach (string unNick in this.clientesConectados.Values) {
				if (nick.Equals(unNick))
					return true;
			}
			
			return false;
		}
		
		public override object InitializeLifetimeService()
		{
			return null;
		}
	}
	
}
