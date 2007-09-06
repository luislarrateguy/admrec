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



namespace ProyectoServidorIntermediario

import System
import System.Collections
import System.Runtime.Remoting
import System.Runtime.Remoting.Channels
import System.Runtime.Remoting.Channels.Tcp


try:
	provider = BinaryServerFormatterSinkProvider()
	provider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full
	props as IDictionary = Hashtable()
	props['port'] = 8086
	props['name'] = 'tcp'
	clientesCreator = ClienteRepresentadoFacade()
	ChannelServices.RegisterChannel(TcpChannel(props, null, provider))
	RemotingServices.Marshal(clientesCreator, 'ClienteCreator')
	Console.WriteLine('Listo... presiona una tecla y vuelo de aca')
	Console.ReadLine()
except e as System.Runtime.Remoting.RemotingException:
	Console.WriteLine("Salto en el servidor de clientes!")
	Console.WriteLine(e.Message)
	Console.WriteLine(e.StackTrace)
	