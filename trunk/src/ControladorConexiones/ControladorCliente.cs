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
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

using log4net.Config;
using log4net.Appender;

namespace MensajeroRemoting
{

	public class ControladorCliente
	{
		private const string NOMBRE_SERVICIO = "Cliente";
		
		private ICliente objetoCliente;
		private EventsHelper eventsHelper;
		private ClienteRemoto clienteRemoto;
		private ControladorConexiones controladorConexiones;
		private TcpChannel canalBidireccional;
		
		private static string direccionServidor;
		private string cadenaConexion;
		private string nick;
		private bool conectado = false;
		
		private log4net.ILog logger;
		
		public ControladorCliente(ICliente objetoCliente, string ipPropia, string direccionServidor, string nick)
		{
			this.logger = log4net.LogManager.GetLogger(this.GetType());
			
			this.objetoCliente = objetoCliente;
			this.nick = nick;
			
			IServerChannelSinkProvider provider = new BinaryServerFormatterSinkProvider();
			IDictionary props = new Hashtable();
			props["port"] = 0;
			props["name"] = "tcp";
			props["bindTo"] = ipPropia;
			
			this.canalBidireccional = new TcpChannel(props, null, provider);
			ChannelServices.RegisterChannel(this.canalBidireccional);
			
			ChannelDataStore cds = (ChannelDataStore)this.canalBidireccional.ChannelData;	
			
			this.cadenaConexion = cds.ChannelUris[0]  + "/" + NOMBRE_SERVICIO;
			this.logger.Debug("Mi canal escucha: " + this.cadenaConexion);

			this.logger.Debug("Creando un nuevo ClienteRemoto");
			this.clienteRemoto = new ClienteRemoto();
			
			RemotingServices.Marshal(clienteRemoto, NOMBRE_SERVICIO);
			
			this.logger.Debug("Cachando el ControladorConexiones...");
			ControladorCliente.direccionServidor = direccionServidor;
			controladorConexiones = ObtenerControladorConexiones();

			this.logger.Debug("Registrando métodos handlers del cliente en el EventHandler");
			this.eventsHelper = new EventsHelper(this.clienteRemoto);
			this.eventsHelper.ContactoConectado += this.objetoCliente.MetodoContactoConectado;
			this.eventsHelper.ContactoDesconectado += this.objetoCliente.MetodoContactoDesconectado;
			this.eventsHelper.MensajeRecibido += this.objetoCliente.MetodoMensajeRecibido;
		}
		
		~ControladorCliente()
		{
			this.logger.Debug("Destruyendo objeto ControladorCliente");
			
			if (this.conectado)
				this.logger.Error("Se esta destruyendo el objeto ControladorCliente, pero todavia estoy conectado");
		}
		
		// Devolvería los nicks de los contactos
		public string[] Conectar(string nuevoNick) {
			if (!nuevoNick.Equals(""))
				this.nick = nuevoNick;
			
			if (this.conectado) {
				this.logger.Error("Ya estoy conectado, pero se esta intentando conectar otra vez");
				return null;
			}
			
			this.logger.Debug("Conectando...");
			
			string[] nicksContactosConectados;
			nicksContactosConectados = controladorConexiones.Conectar(this.cadenaConexion, this.nick);
			
			if (nicksContactosConectados == null)
				throw new ApplicationException("No fue posible la conexión");
			
			this.conectado = true;
			this.logger.Debug("Conectado!");
			
			// Copio los nicks a otro array, porque hay problemas sino
//			this.logger.Debug("Procesando contactos conectados recibidos...");
//			string[] nicksCopiados = new string[nicksContactosConectados.Length];
//			for (int i=0; i<nicksContactosConectados.Length; i++) {
//				nicksCopiados[i] = nicksContactosConectados[i];
//				this.logger.Debug("   Nick recibido: " + nicksCopiados[i]);
//			}
			
			//Registro handlers para los eventos de conexión
			this.logger.Debug("Registrando handlers de mi objeto ClienteRemoto");
			this.miClienteRemoto.RegistrarHandlers();
			
			this.logger.Debug("Listo. Retornando lista de contactos conectados");
			return nicksContactosConectados;
		}
		
		public void Desconectar() {
			if (!this.conectado) {
				this.logger.Error("Estoy desconectado, y se esta intentando desconectar");
				return;
			}
			
			this.logger.Debug("Desregistrando metodos de objetoCliente del EventsHelper");
			this.eventsHelper.ContactoConectado -= this.objetoCliente.MetodoContactoConectado;
			this.eventsHelper.ContactoDesconectado -= this.objetoCliente.MetodoContactoDesconectado;
			this.eventsHelper.MensajeRecibido -= this.objetoCliente.MetodoMensajeRecibido;
			
			this.logger.Debug("Desregistrando metodos del EventsHelper del ClienteRemoto");
			this.eventsHelper.DesregistrarHandlers();
			
			this.miClienteRemoto.DesregistrarHandlers();			
			
			this.logger.Debug("Desconectando...");
			controladorConexiones.Desconectar(this.nick);
			this.logger.Debug(" Desconectado");
			
			this.conectado = false;
			
			this.logger.Debug("Desregistrando el canal bidireccional... ");
			ChannelServices.UnregisterChannel(this.canalBidireccional);
			RemotingServices.Disconnect(this.clienteRemoto);
			this.logger.Debug("Listo");
		}
		
		public void EnviarMensaje(string nickDestino, string mensaje)
		{
			this.logger.Debug("Enviando mensaje (ControladorCliente)...");
			controladorConexiones.EnviarMensaje(this.nick, nickDestino, mensaje);
			this.logger.Debug("Enviado! (ControladorCliente)...");
		}
		
		public ClienteRemoto miClienteRemoto {
			get {
				return clienteRemoto;
			}
		}
		public static ControladorConexiones ObtenerControladorConexiones()
		{
			// Direccion servidor deberia ser seteable de otro lado
			ControladorConexiones cc = (ControladorConexiones)Activator.GetObject(typeof(ControladorConexiones),
					"tcp://" + ControladorCliente.direccionServidor + ":8085/CC");
			return cc;
		}
		
		public string[] ContactosConectados
		{
			get
			{
				string[] nicksContactosConectados;
				nicksContactosConectados = controladorConexiones.ContactosConectados;

				string[] nicksCopiados = new string[nicksContactosConectados.Length];
				for (int i=0; i<nicksContactosConectados.Length; i++) {
					nicksCopiados[i] = nicksContactosConectados[i];
				}
				return nicksCopiados;
			}
		}
	}
}
