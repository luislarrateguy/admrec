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
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace MensajeroRemoting {
	public class Servidor
	{
		private static ControladorConexiones controladorConexiones;
		
		public static void Main(string[] args)
		{
			BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
			provider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
			
			IDictionary props = new Hashtable();
			props["port"] = 8085;
			props["name"] = "tcp";
						
			controladorConexiones = new ControladorConexiones();
			ChannelServices.RegisterChannel(new TcpChannel(props, null, provider));
			RemotingServices.Marshal(controladorConexiones, "CC");
			
			Console.WriteLine("Listo... presiona una tecla y vuelo de aca");
			Console.ReadLine();
		}
	}
}
