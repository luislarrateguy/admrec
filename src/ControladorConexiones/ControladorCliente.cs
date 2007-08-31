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

namespace MensajeroRemoting
{
	public class ControladorCliente
	{
		private const string NOMBRE_SERVICIO = "Cliente";
		
		public static ControladorConexiones controladorConexiones;
		
		private ClienteRemoto clienteRemoto;
		private TcpChannel canalBidireccional;
		//private TcpServerChannel canalEscucha;
		private string cadenaConexion;
		private string nick;
		private bool conectado = false;
		
		public ControladorCliente(string ipPropia, string direccionServidor, string nick)
		{
			IServerChannelSinkProvider provider = new BinaryServerFormatterSinkProvider();
			IDictionary props = new Hashtable();
			props["port"] = 0;
			props["name"] = "tcp";
			props["bindTo"] = ipPropia;
			
			// Este canal es para servir a mi ClienteRemoto
//            this.canalEscucha = new TcpServerChannel(props, provider);
//			ChannelServices.RegisterChannel(canalEscucha);
//			
//			ChannelDataStore cds = (ChannelDataStore)canalEscucha.ChannelData;
			
			this.canalBidireccional = new TcpChannel(props, null, provider);
			ChannelServices.RegisterChannel(this.canalBidireccional);
			
			ChannelDataStore cds = (ChannelDataStore)this.canalBidireccional.ChannelData;	
			
			this.nick = nick;
			
			this.cadenaConexion = cds.ChannelUris[0]  + "/" + NOMBRE_SERVICIO;
			Console.WriteLine("Mi canal escucha: " + this.cadenaConexion);
			
			// Este canal es para la comunicación bidireccional con el server
//			TcpChannel chanServe = new TcpChannel();
//			ChannelServices.RegisterChannel(chanServe);

			Console.WriteLine("Registrando mi objeto remoto...");
//			RemotingConfiguration.RegisterWellKnownServiceType(typeof(ClienteRemoto),
//			                                                          NOMBRE_SERVICIO,
//			                                                          WellKnownObjectMode.Singleton);

			Console.WriteLine("Creando un nuevo ClienteRemoto!!!");
			this.clienteRemoto = new ClienteRemoto();
			
			RemotingServices.Marshal(clienteRemoto, NOMBRE_SERVICIO);
			
			// Cacho el ControladorConexiones solo si es necesario
//			if (controladorConexiones == null) {
				Console.Write("Cachando el ControladorConexiones...");
				controladorConexiones = (ControladorConexiones)Activator.GetObject(typeof(ControladorConexiones),
				                                                                   "tcp://" + direccionServidor + ":8085/CC");
				Console.WriteLine("Cachado!");
//			}
		}
		
		/* Chau Singleton!
		public static ControladorCliente GetInstancia() {
			deif (instancia == null)
				instancia = new ControladorCliente();
			return instancia;				
		}
		*/
		// Devolvería los nicks de los contactos
		public string[] Conectar(string nuevoNick) {
			if (!nuevoNick.Equals(""))
				this.nick = nuevoNick;
			
			Console.Write("Conectando...");
			
			// Copio los nicks a otro array, porque hay problemas sino
			string[] nicksContactosConectados = controladorConexiones.Conectar(this.cadenaConexion, this.nick);
			
			if (nicksContactosConectados == null)
				throw new Exception("No fue posible la conexión");
			
			this.conectado = true;
			Console.WriteLine("Conectado!");
			
			Console.WriteLine("Procesando contactos conectados recibidos...");
			string[] nicksCopiados = new string[nicksContactosConectados.Length];
			for (int i=0; i<nicksContactosConectados.Length; i++) {
				nicksCopiados[i] = nicksContactosConectados[i];
				Console.WriteLine("   Nick recibido: " + nicksCopiados[i]);
			}
			
			Console.WriteLine("Listo. Retorno la lista de contactos");
			
			//Registro handlers para los eventos de conexión
			this.miClienteRemoto.RegistrarHandlers();
			
			return nicksCopiados;
		}
		
		public void Desconectar() {
			if (!this.conectado)
				return;
			
			this.miClienteRemoto.DesregistrarHandlers();			
			
			Console.Write("Desconectando...");
			controladorConexiones.Desconectar(this.nick);
			Console.WriteLine(" Desconectado");
			
			this.conectado = false;
			
			Console.Write("Desregistrando el canal bidireccional... ");
			ChannelServices.UnregisterChannel(this.canalBidireccional);
			RemotingServices.Disconnect(this.clienteRemoto);
			Console.WriteLine("Listo");
		}
		
		public void EnviarMensaje(string nickDestino, string mensaje)
		{
			Console.WriteLine("Enviando mensaje (ControladorCliente)...");
			controladorConexiones.EnviarMensaje(this.nick, nickDestino, mensaje);
			Console.WriteLine("Enviado! (ControladorCliente)...");
		}
		
		public ClienteRemoto miClienteRemoto {
			get {
				ClienteRemoto miClienteRemoto = (ClienteRemoto)Activator.GetObject(typeof(ClienteRemoto), this.cadenaConexion);
				
				return miClienteRemoto;
			}
		}
	}
}
