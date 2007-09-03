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
using MensajeroRemoting;

namespace ClienteConsola
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			string ip1 = "127.0.0.1";
			string ip2 = "127.0.0.1";
			if (args.Length == 2) {
				ip1 = args[0];
				ip2 = args[1];
			}
			ControladorCliente cc = new ControladorCliente(ip1,ip2,"BigBrother");
			//string[] contactos = cc.Conectar("");
			string[] contactos = cc.ContactosConectados;
			
			Console.WriteLine("Contactos conectados:");
			Console.WriteLine("---------------------");
			
			foreach (string cont in contactos)
				Console.WriteLine("Nick: " + cont);
				
			//cc.Desconectar();
		}
	}
}