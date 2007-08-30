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
	public delegate void ConexionCliente(string nick);
	
	public class ControladorConexiones : MarshalByRefObject
	{
		public event ConexionCliente ClienteConectado;
		public event ConexionCliente ClienteDesconectado;
		
		private Dictionary<string, string> clientesConectados;
		
		public ControladorConexiones()
		{
			this.clientesConectados = new Dictionary<string, string>();
		}
		
		public string[] Conectar (string cadenaConexion, string nick)
		{
			Console.WriteLine("");
			Console.WriteLine("Petición de conextion. Cadena: " + cadenaConexion);
			
			if (this.clientesConectados.ContainsKey(nick))
				throw new Exception("El cliente ya esta conectado. Imposible continuar");
			
			// Verifico que el nick no esté ocupado
			if (this.NickOcupado(nick))
				throw new Exception("El nick ya está ocupado");
			
			Console.WriteLine("El cliente es nuevo...");
			
			Console.WriteLine("Avisando a los otros que se conecto uno nuevo");
			if (this.ClienteConectado != null)
				this.ClienteConectado(nick);
			
			Console.WriteLine("Pasando clientesConectados a array");
			List<string> clientesSinElNuevo = new List<string>();
			foreach(string unNick in this.clientesConectados.Keys)
				clientesSinElNuevo.Add(unNick);
			
			Console.WriteLine("Agrego el cliente a mi lista de clientes conectados");
			this.clientesConectados.Add(nick, cadenaConexion);
			
			Console.WriteLine("Listo!");
			
			return clientesSinElNuevo.ToArray();
		}
		
		public void Desconectar(string nick)
		{
			Console.WriteLine("");
			Console.WriteLine("Petición de desconextion de " + nick);
			
			if (!this.clientesConectados.ContainsKey(nick))
				throw new Exception("El cliente no esta conectado. Imposible descontarlo");
			
			Console.WriteLine("Avisando a los otros que se desconecto");
			if (this.ClienteDesconectado != null)
				this.ClienteDesconectado(nick);
			
			Console.WriteLine("Bien, el cliente estaba conectado. Lo saco de la lista...");
			this.clientesConectados.Remove(nick);
			
			Console.WriteLine("Listo!");
		}
		
		public void EnviarMensaje(string nickOrigen, string nickDestino, string mensaje)
		{
			if (!this.clientesConectados.ContainsKey(nickOrigen))
				throw new Exception("No se puede enviar un mensaje desde un cliente no conectado");
			
			if (!this.clientesConectados.ContainsKey(nickDestino))
				throw new Exception("El cliente destino no esta conectado. No se puede enviar el mensaje");
			
			ClienteRemoto clienteRemoto = (ClienteRemoto)Activator.GetObject(typeof(ClienteRemoto),
			                                                                 this.clientesConectados[nickDestino]);
			
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
		
//		public bool NickDisponible(string nick)
//		{
//			return (!this.NickOcupado(nick));
//		}
		
		public override object InitializeLifetimeService()
		{
			return null;
		}
	}
	
}
