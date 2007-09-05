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
import System.Collections.Generic
import System.Threading
import MensajeroRemoting

public class ClienteRepresentado(ICliente):

	private listaClientesConectados as List[of string]
	private controladorCliente as ControladorCliente
	private mensajesRecibidos as List[of string]
	private ip as string
	private nick as string
	private servidor as string
	private mut as Mutex = Mutex()
	
	public def constructor(nick2 as string):
		self.listaClientesConectados = List[of string]()
		try:
			self.nick = nick2
			self.controladorCliente = ControladorCliente(self, "127.0.0.1", "localhost", self.nick)
			self.mensajesRecibidos = List[of string]()
			self.listaClientesConectados = List[of string]()
		except e as System.Runtime.Remoting.RemotingException:
			Console.WriteLine("Salto en ClienteRepresentado!")
			Console.WriteLine(e.Message)
			Console.WriteLine(e.StackTrace)
			
	public def conectar(nick as string):
		for c in self.controladorCliente.Conectar(nick):
			self.listaClientesConectados.Add(c)
	
	public def desconectar():
		mut.WaitOne()
		self.listaClientesConectados.Clear()
		mut.ReleaseMutex()
		self.controladorCliente.Desconectar()
		
	public def enviarMensaje(nick as string, mensaje as string):
		self.controladorCliente.EnviarMensaje(nick, mensaje)
	
	public ContactosConectados() as List[of string]:
		get:
			return List[of string](listaClientesConectados) 

	public def ContactoConectado(nickCliente as string):
		if nickCliente.Equals(self.nick):
			return
		mut.WaitOne()
		self.listaClientesConectados.Add(nickCliente)
		mut.ReleaseMutex()
		
		
	public def ContactoDesconectado(nickCliente as string):
		if nickCliente.Equals(self.nick):
			return 	
		mut.WaitOne()
		self.listaClientesConectados.Remove(nickCliente)
		mut.ReleaseMutex()
		
	public def RecibirMensaje(nickCliente as string, mensaje as string):
		mut.WaitOne()
		self.mensajesRecibidos.Add(nickCliente +" dice: "+mensaje)
		mut.ReleaseMutex()
		
	public def getUltimosMensajesRecibidos() as List[of string]:
		mut.WaitOne()
		msjs = List[of string](self.mensajesRecibidos)
		self.mensajesRecibidos.Clear()
		mut.ReleaseMutex()
		return msjs
		
	public MetodoContactoConectado as ConexionClienteHandler:
		get:
			return ConexionClienteHandler(self.ContactoConectado)
	
	public MetodoContactoDesconectado as ConexionClienteHandler:
		get:
			return ConexionClienteHandler(self.ContactoDesconectado)

	
	public MetodoMensajeRecibido as MensajeRecibidoHandler:
		get:
			return MensajeRecibidoHandler(self.RecibirMensaje)

