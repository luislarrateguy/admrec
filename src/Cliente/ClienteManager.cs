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

using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

using Gtk;

namespace MensajeroRemoting {
	public class ClienteManager
	{
		private static ControladorConexiones controladorConexiones;
		private static TcpServerChannel miCanalEscucha;
		private static int puerto;
		private static MainWindow hostCliente;

		public static void Inicializar()
		{
			TcpChannel chanConnect  = new TcpChannel();
			ChannelServices.RegisterChannel(chanConnect);
			
			Console.WriteLine("Cachando servidor...");
			controladorConexiones = (ControladorConexiones)Activator.GetObject(typeof(ControladorConexiones),
			                                                                   "tcp://localhost:8085/CC");
			
			if (controladorConexiones == null) {
				Console.WriteLine("No se pudo cachar el controlador...");
				return;
			}
			
			Console.WriteLine("Servidor cachado!");
			
			ChannelServices.UnregisterChannel(chanConnect);
			
			/* Esto hace que busque un puerto disponible
			 * Cambio el canal bidireccional por uno Servidor únicamente
			 * Solo se registra debajo (con RegisterWellKnownServiceType)
			 */
            miCanalEscucha = new TcpServerChannel(0);
			Console.WriteLine("Mi canal escucha: "+miCanalEscucha.GetChannelUri());
			
			TcpChannel chanServe = new TcpChannel(puerto);
			ChannelServices.RegisterChannel(chanServe);
			
			Console.WriteLine("Registrando mi objeto remoto...");
			RemotingConfiguration.RegisterWellKnownServiceType(typeof(MainWindow),
			                                                          "Host",
			                                                          WellKnownObjectMode.Singleton);
			
			hostCliente = GetHostByConnectionString(miCanalEscucha.GetChannelUri() + "/Host");
			hostCliente.Run();
			
//			TcpChannel tcp_chan = new TcpChannel();
//			ChannelServices.RegisterChannel(tcp_chan);
//			
//			Console.WriteLine("Creando objeto...");
//			MainWindow mw = new MainWindow();
//			Console.WriteLine("Objeto creado...");
//			RemotingServices.Marshal(mw, "Host");
//			Console.WriteLine("Corriendo el MainWindow...");
//			mw.Run();
//			hostCliente = GetHostByConnectionString(miCanalEscucha.GetChannelUri() + "/Host");
		}
		
		public static ControladorConexiones ControladorConexiones
		{
			get { return controladorConexiones; }
		}
		
		private static MainWindow GetHostByConnectionString(string cadena)
		{
			MainWindow hostCliente = (MainWindow)Activator.GetObject(typeof(MainWindow),
			                                              cadena);
			
			return hostCliente;
		}
		
		public static ClienteInfo[] Conectar(out string cadena) {
			Console.Write("Conectando...");
			cadena = miCanalEscucha.GetChannelUri() + "/Host";
			
			ClienteInfo yo = new ClienteInfo();
			yo.cadenaConexion = cadena;
			yo.nick = hostCliente.Nick;
			
			List<ClienteInfo> contactos = new List<ClienteInfo>();
			ClienteInfo[] contactosRecibidos = controladorConexiones.Conectar(yo);
			foreach (ClienteInfo ci in contactosRecibidos) {
				ClienteInfo clienteInfo = new ClienteInfo();
				clienteInfo.cadenaConexion = ci.cadenaConexion;
				clienteInfo.nick = ci.nick;
				
				contactos.Add(clienteInfo);
			}
			
			if (contactos != null)
				Console.WriteLine("¡Conectado!");
			else
				Console.WriteLine("No anduvo la conexion");
			
			return contactos.ToArray();
		}
		
		public static bool Desconectar() {
			Console.Write("Desconectando...");

			ClienteInfo yo = new ClienteInfo();
			yo.cadenaConexion = miCanalEscucha.GetChannelUri() + "/Host";
			yo.nick = hostCliente.Nick;
			
			controladorConexiones.Desconectar(yo);
			Console.WriteLine(" Desconectado");
			return true;
		}
		
		public static MainWindow ObtenerDestino(int puertoDestino) {
			Console.WriteLine("Cachando el destino...");
			MainWindow hostDestino = GetHostByConnectionString("tcp://localhost:" +
				puertoDestino + "/Host");
			Console.WriteLine("Cachado!");
			return hostDestino;
		}
		
		public static bool EnviarMensaje(MainWindow h, string m) {
			Console.Write("Enviando mensaje...");
			h.EnviarMensaje(hostCliente.CadenaConexion, m);
			Console.WriteLine("Enviado!");
			return true;
		}
		
		public static bool NickDisponible(string nick)
		{
			Console.WriteLine("Preguntando si el nick esta disponible...");
			return controladorConexiones.NickDisponible(nick);
		}
		
		public static bool NickOcupado(string nick)
		{
			Console.WriteLine("Preguntando si el nick esta ocupado...");
			return controladorConexiones.NickOcupado(nick);
		}
		
		public static void Main(string[] args)
		{
			ClienteManager.Inicializar();
		}
	}
}
