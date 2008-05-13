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
using ProyectoServidorIntermediario;

namespace TestingThings
{
	class MainClass
	{
		public static void Main(string[] args)
		{
//			Dictionary<string,ClienteRepresentado> crc = new Dictionary<string,ClienteRepresentado>();
//			crc.ContainsKey(id);
//			crc.Add(id,new ClienteRepresentado());
			
			
			
//			IDictionary props = new Hashtable();
//			props["port"] = 0;
//			props["name"] = "tcp";
//			props["bindTo"] = "127.0.0.1";
//
//			IServerChannelSinkProvider provider = new BinaryServerFormatterSinkProvider();		
//			TcpChannel canalBidireccional = new TcpChannel(props, null, provider);
//			ChannelServices.RegisterChannel(canalBidireccional);
			
//			try  {
				ClienteRepresentadoFacade c =  (ClienteRepresentadoFacade) 
											Activator.GetObject(typeof(ClienteRepresentadoFacade),
											"tcp://127.0.0.1:8086/ClienteCreator");
				
				c.createClienteRepresentado("nacho");
//				ClienteRepresentado cr = new ClienteRepresentado();
				c.conectar("nacho");
				c.enviarMensaje("nacho","milton","holaaaaa milton");
				c.desconectar("nacho");
//			}
//			catch (System.Runtime.Remoting.RemotingException e) {
//				Console.WriteLine(e.StackTrace);
//				
//			}
			
		}
	}
}