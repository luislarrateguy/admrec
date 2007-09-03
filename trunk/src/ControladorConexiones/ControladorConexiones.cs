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
		
		private log4net.ILog logger;
		
		public ControladorConexiones()
		{
			this.logger = log4net.LogManager.GetLogger(this.GetType());
			System.IO.FileInfo fi = new System.IO.FileInfo("log4net.config.xml");
			this.logger.Debug(fi.Exists);
			XmlConfigurator.Configure(fi);

			this.clientesConectados = new Dictionary<string, string>();
		}
		
		~ControladorConexiones()
		{
			this.logger.Debug("Se está destruyendo el ControladorConexiones!");
		}
		
		public string[] Conectar (string cadenaConexion, string nick)
		{
			this.logger.Debug("");
			this.logger.Debug("Petición de conexion. Cadena: " + cadenaConexion);
			
			///BEGIN Estas 2 hacen lo mismo
			if (this.clientesConectados.ContainsKey(nick))
				throw new NickOcupadoException("El cliente ya esta conectado. Imposible continuar");
			
			// Verifico que el nick no esté ocupado
			if (this.NickOcupado(nick))
				throw new NickOcupadoException("El nick ya está ocupado");
			///END Estas 2 hacen lo mismo
			
			
			this.logger.Debug("El cliente es nuevo...");
					
			this.logger.Debug("Pasando clientesConectados a array...");
			List<string> clientesSinElNuevo = new List<string>();
			foreach(string unNick in this.clientesConectados.Keys) {
				this.logger.Debug("  procesando " + unNick);
				clientesSinElNuevo.Add(unNick);
			}
			
			this.logger.Debug("Agrego el cliente a mi lista de clientes conectados");
			this.clientesConectados.Add(nick, cadenaConexion);
			
			this.logger.Debug("Listo!");
			
			this.NotifContactoConectado(nick);
			
			return clientesSinElNuevo.ToArray();
		}
		
		public string[] ContactosConectados
		{
			get
			{
				return (new List<string>(this.clientesConectados.Keys)).ToArray();
			}
		}
		
		public void Desconectar(string nick)
		{
			this.logger.Debug("");
			this.logger.Debug("Petición de desconextion de " + nick);
			
			if (!this.clientesConectados.ContainsKey(nick))
				throw new ApplicationException("El cliente no esta conectado. Imposible descontarlo");
			
			this.logger.Debug("Bien, el cliente estaba conectado. Lo saco de la lista...");
			this.clientesConectados.Remove(nick);
			
			this.NotifContactoDesconectado(nick);

			this.logger.Debug("Listo!");
		}
		
		public void EnviarMensaje(string nickOrigen, string nickDestino, string mensaje)
		{

			this.logger.Debug("ControladorConexiones.EnviarMensaje ejecutado...");
			this.logger.Debug("Nick origen: " + nickOrigen);
			this.logger.Debug("Nick destino: " + nickDestino);
			
			if (!this.clientesConectados.ContainsKey(nickOrigen))
				throw new ApplicationException("No se puede enviar un mensaje desde un cliente no conectado");
			
			if (!this.clientesConectados.ContainsKey(nickDestino))
				throw new ApplicationException("El cliente destino no esta conectado. No se puede enviar el mensaje");
			
			this.logger.Debug("Cachando objeto ClienteRemoto desde ControladorConexiones...");
			ClienteRemoto clienteRemoto = (ClienteRemoto)Activator.GetObject(typeof(ClienteRemoto),
			                                                                 this.clientesConectados[nickDestino]);
			
			this.logger.Debug("Ejecutando método recibirMensaje del clienteRemoto...");
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
			this.logger.Debug("Avisando a los otros que se conecto uno nuevo, pero despues de haberlo agregado");
			foreach (string cadenaConex in this.clientesConectados.Values) {
				this.logger.Debug("Cachando objeto remoto (" + cadenaConex + ")");
				clienteRemoto = (ClienteRemoto)Activator.GetObject(typeof(ClienteRemoto),cadenaConex);
				this.logger.Debug("Notificando...");
				clienteRemoto.clienteConectado(nick);
			}
			
			this.logger.Debug("Notificación hecha");
		}
		public void NotifContactoDesconectado(string nick) 
		{
			ClienteRemoto clienteRemoto;
			this.logger.Debug("Avisando a los otros que se desconecto");
			foreach (string cadenaConex in this.clientesConectados.Values) {
				clienteRemoto = (ClienteRemoto)Activator.GetObject(typeof(ClienteRemoto),cadenaConex);
				clienteRemoto.clienteDesconectado(nick);
			}
		}
	}
	
}
