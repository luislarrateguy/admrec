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
	
	public interface IControladorConexiones 
	{
		event ConexionClienteHandler ClienteConectado;
		event ConexionClienteHandler ClienteDesconectado;
		
		
		string[] Conectar (string cadenaConexion, string nick);
		string[] ContactosConectados { get; }
		void Desconectar(string nick);
		void EnviarMensaje(string nickOrigen, string nickDestino, string mensaje);
		void NotifContactoConectado(string nick);
		void NotifContactoDesconectado(string nick); 
		
	}
	
}
