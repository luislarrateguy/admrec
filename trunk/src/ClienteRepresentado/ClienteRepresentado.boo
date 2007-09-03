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
import MensajeroRemoting

public class ClienteRepresentado(ICliente):

	private listaClientesConectados as List
	private controladorCliente as ControladorCliente
	private ip as string
	private nick as string
	private servidor as string

	
	public def constructor():
		self.listaClientesConectados = List()
		self.controladorCliente = ControladorCliente(self, "127.0.0.1", "127.0.0.1", "")

	
	public def conectar(nick as string):
		self.controladorCliente.Conectar(nick)
	
	
	public def desconectar():
		self.listaClientesConectados.Clear()
		self.controladorCliente.Desconectar()
		
	public def enviarMensaje(nick as string, mensaje as string):
		self.controladorCliente.EnviarMensaje(nick, mensaje)
	
	public def getContactosConectados() as List:
		return listaClientesConectados

	
	public def ContactoConectado(nickCliente as string):
		if nickCliente.Equals(self.nick):
			return 
		self.listaClientesConectados.Add(nickCliente)
		
	public def ContactoDesconectado(nickCliente as string):
		if nickCliente.Equals(self.nick):
			return 	
		self.listaClientesConectados.Remove(nickCliente)
		
	public def RecibirMensaje(nickCliente as string, mensaje as string):
		Console.WriteLine('')
		Console.WriteLine(((('El contacto ' + nickCliente) + ' dice: ') + mensaje))

	public MetodoContactoConectado as ConexionClienteHandler:
		get:
			return ConexionClienteHandler(self.ContactoConectado)

	
	public MetodoContactoDesconectado as ConexionClienteHandler:
		get:
			return ConexionClienteHandler(self.ContactoDesconectado)

	
	public MetodoMensajeRecibido as MensajeRecibidoHandler:
		get:
			return MensajeRecibidoHandler(self.RecibirMensaje)

