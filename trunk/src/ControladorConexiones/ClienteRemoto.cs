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

namespace MensajeroRemoting
{
	public class ClienteRemoto : MarshalByRefObject
	{
		public delegate void RecepcionMensaje(string nick, string mensaje);
		public event RecepcionMensaje MensajeRecibido;
		
		public delegate void ConexionCliente(string nick);
		public event ConexionCliente ClienteConectado;
		public event ConexionCliente ClienteDesconectado;
		
		public ClienteRemoto()
		{
		}
		
		public void RecibirMensaje(string nickOrigen, string mensaje)
		{
			
		}
	}
}
