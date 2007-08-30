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
using System.Net;
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
		
		private ControladorConexiones controladorConexiones;
		private string cadenaConexion;
		private string nick;
		private static ControladorCliente instancia = null;

		private ControladorCliente(string ipPropia, string direccionServidor, string nick)
		{
			/* Esto hace que busque un puerto disponible
			 * Cambio el canal bidireccional por uno Servidor únicamente
			 * Solo se registra debajo (con RegisterWellKnownServiceType)
			 */
			IServerChannelSinkProvider provider = new BinaryServerFormatterSinkProvider();
			IDictionary props = new Hashtable();
			props["port"] = 0;
			props["name"] = "tcp";
			props["bindTo"] = ipPropia;
			
			// Este canal es para servir a mi ClienteRemoto
            TcpServerChannel canalEscucha = new TcpServerChannel(props, provider);
			Console.WriteLine("Mi canal escucha: " + canalEscucha.GetChannelUri());
			
			this.cadenaConexion = canalEscucha.GetChannelUri() + "/" + NOMBRE_SERVICIO;
			
			// Este canal es para la comunicación bidireccional con el server
			TcpChannel chanServe = new TcpChannel(0);
			ChannelServices.RegisterChannel(chanServe);
			
			Console.WriteLine("Registrando mi objeto remoto...");
			RemotingConfiguration.RegisterWellKnownServiceType(typeof(ClienteRemoto),
			                                                          NOMBRE_SERVICIO,
			                                                          WellKnownObjectMode.Singleton);
			
			// Obtengo mi propio objeto ClienteRemoto (TODO: ¿Para qué?)
			//this.ObtenerClienteRemoto(miCanalEscucha.GetChannelUri() + "/Cliente");
			
			this.controladorConexiones = (ControladorConexiones)Activator.GetObject(typeof(ControladorConexiones),
			                                                                   "tcp://" + direccionServidor + ":8085/CC");
			
			this.nick = nick;
		}
		
		public static ControladorCliente GetInstancia() {
			if (instancia == null)
				instancia = new ControladorCliente();
			return instancia;				
		}
			
		// Devolvería los nicks de los contactos
		public string[] Conectar() {
			Console.Write("Conectando...");
			
			// Copio los nicks a otro array, porque hay problemas sino
			string[] nicksContactosConectados = controladorConexiones.Conectar(this.cadenaConexion, this.nick);
			
			if (nicksContactosConectados == null)
				throw new Exception("No fue posible la conexión");
			
			string[] nicksCopiados = new string[nicksContactosConectados.Length];
			for (int i=0; i<nicksContactosConectados.Length; i++)
				nicksCopiados[i] = nicksContactosConectados[i];
			
			Console.WriteLine("Conectado");
			
			return nicksCopiados;
		}
		
		public void Desconectar() {
			Console.Write("Desconectando...");
			
			controladorConexiones.Desconectar(this.nick);
			
			Console.WriteLine(" Desconectado");
		}
		
		public void EnviarMensaje(string nickDestino, string mensaje)
		{
			this.controladorConexiones.EnviarMensaje(this.nick, nickDestino, mensaje);
		}
		
		public ClienteRemoto miClienteRemoto {
			get {
				ClienteRemoto miClienteRemoto = (ClienteRemoto)Activator.GetObject(typeof(ClienteRemoto), this.cadenaConexion);
				return miClienteRemoto;
			}
		}
	}
}
