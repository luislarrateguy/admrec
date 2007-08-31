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
using log4net;
using log4net.Config;
using log4net.Appender;

namespace MensajeroRemoting
{
	// Delegate. Quiza haya que sacar las instancias
	public delegate void ConexionClienteHandler(string nick);

	[Serializable()]
	public class NickOcupadoException : System.ApplicationException
	{
	    public NickOcupadoException() {}
	    public NickOcupadoException(string message) {}
	    public NickOcupadoException(string message, System.Exception inner) {}
	 
	    // Constructor necesario para la serializacion (remoting) 
	    protected NickOcupadoException(System.Runtime.Serialization.SerializationInfo info,
	        System.Runtime.Serialization.StreamingContext context) {}
	}
	
	public class ControladorConexiones : MarshalByRefObject
	{
		public event ConexionClienteHandler ClienteConectado;
		public event ConexionClienteHandler ClienteDesconectado;
		
		private Dictionary<string, string> clientesConectados;
		
		public ControladorConexiones()
		{
			System.IO.FileInfo fi = new System.IO.FileInfo("log4net.config.xml");
			Console.WriteLine(fi.Exists);
			XmlConfigurator.Configure(fi);

			this.clientesConectados = new Dictionary<string, string>();
		}
		
		~ControladorConexiones()
		{
			log4net.LogManager.GetLogger(this.GetType()).Debug("Se está destruyendo el ControladorConexiones!");
		}
		
		public string[] Conectar (string cadenaConexion, string nick)
		{
			log4net.LogManager.GetLogger(this.GetType()).Debug("");
			log4net.LogManager.GetLogger(this.GetType()).Debug("Petición de conexion. Cadena: " + cadenaConexion);
			
			///BEGIN Estas 2 hacen lo mismo
			if (this.clientesConectados.ContainsKey(nick))
				throw new NickOcupadoException("El cliente ya esta conectado. Imposible continuar");
			
			// Verifico que el nick no esté ocupado
			if (this.NickOcupado(nick))
				throw new NickOcupadoException("El nick ya está ocupado");
			///END Estas 2 hacen lo mismo
			
			
			log4net.LogManager.GetLogger(this.GetType()).Debug("El cliente es nuevo...");
					
			log4net.LogManager.GetLogger(this.GetType()).Debug("Pasando clientesConectados a array...");
			List<string> clientesSinElNuevo = new List<string>();
			foreach(string unNick in this.clientesConectados.Keys) {
				log4net.LogManager.GetLogger(this.GetType()).Debug("  procesando " + unNick);
				clientesSinElNuevo.Add(unNick);
			}
			
			log4net.LogManager.GetLogger(this.GetType()).Debug("Agrego el cliente a mi lista de clientes conectados");
			this.clientesConectados.Add(nick, cadenaConexion);
			
			log4net.LogManager.GetLogger(this.GetType()).Debug("Listo!");
			
			this.NotifContactoConectado(nick);
			
			return clientesSinElNuevo.ToArray();
		}
		
		public void Desconectar(string nick)
		{
			log4net.LogManager.GetLogger(this.GetType()).Debug("");
			log4net.LogManager.GetLogger(this.GetType()).Debug("Petición de desconextion de " + nick);
			
			if (!this.clientesConectados.ContainsKey(nick))
				throw new Exception("El cliente no esta conectado. Imposible descontarlo");
			
			log4net.LogManager.GetLogger(this.GetType()).Debug("Bien, el cliente estaba conectado. Lo saco de la lista...");
			this.clientesConectados.Remove(nick);
			
			this.NotifContactoDesconectado(nick);

			log4net.LogManager.GetLogger(this.GetType()).Debug("Listo!");
		}
		
		public void EnviarMensaje(string nickOrigen, string nickDestino, string mensaje)
		{

			log4net.LogManager.GetLogger(this.GetType()).Debug("ControladorConexiones.EnviarMensaje ejecutado...");
			
			if (!this.clientesConectados.ContainsKey(nickOrigen))
				throw new Exception("No se puede enviar un mensaje desde un cliente no conectado");
			
			if (!this.clientesConectados.ContainsKey(nickDestino))
				throw new Exception("El cliente destino no esta conectado. No se puede enviar el mensaje");
			
			log4net.LogManager.GetLogger(this.GetType()).Debug("Cachando objeto ClienteRemoto desde ControladorConexiones...");
			ClienteRemoto clienteRemoto = (ClienteRemoto)Activator.GetObject(typeof(ClienteRemoto),
			                                                                 this.clientesConectados[nickDestino]);
			
			log4net.LogManager.GetLogger(this.GetType()).Debug("Ejecutando método recibirMensaje del clienteRemoto...");
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
		public void NotifContactoConectado(string nick) 
		{
			ClienteRemoto clienteRemoto; 
			log4net.LogManager.GetLogger(this.GetType()).Debug("Avisando a los otros que se conecto uno nuevo, pero despues de haberlo agregado");
			foreach (string cadenaConex in this.clientesConectados.Values) {
				clienteRemoto = (ClienteRemoto)Activator.GetObject(typeof(ClienteRemoto),cadenaConex);
				clienteRemoto.clienteConectado(nick);
			}
		}
		public void NotifContactoDesconectado(string nick) 
		{
			ClienteRemoto clienteRemoto;
			log4net.LogManager.GetLogger(this.GetType()).Debug("Avisando a los otros que se desconecto");
			foreach (string cadenaConex in this.clientesConectados.Values) {
				clienteRemoto = (ClienteRemoto)Activator.GetObject(typeof(ClienteRemoto),cadenaConex);
				clienteRemoto.clienteDesconectado(nick);
			}
		}
	}
	
}
