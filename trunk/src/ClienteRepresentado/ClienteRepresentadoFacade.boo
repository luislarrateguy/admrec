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



public class ClienteRepresentadoFacade(MarshalByRefObject):

	private crc as Dictionary[of string, ClienteRepresentado] 
	
	def constructor():
		self.crc  =  Dictionary[of string, ClienteRepresentado]()
	
	def createClienteRepresentado(id as string):
		if not self.crc.ContainsKey(id):
			self.crc.Add(id,ClienteRepresentado(id+"(ws)"))
			return true
		else:
			return false
			
	def destroyClienteRepresentado(id as string):
		if self.crc.ContainsKey(id):
			self.crc.Remove(id)
			return true
		else:
			return false
			
	def conectar(id as string) as bool:
		if self.crc.ContainsKey(id):
			self.crc[id].conectar(id+"(ws)")
			return true
		else:
			return false
		
	def enviarMensaje(id as string,nickDestino as string,mensaje as string) as bool:
		if self.crc.ContainsKey(id):
			self.crc[id].enviarMensaje(nickDestino,mensaje)
			return true
		else:
			return false
		
	def desconectar(id as string) as bool:
		if self.crc.ContainsKey(id):
			self.crc[id].desconectar()
			return true
		else:
			return false
	
	def getUltimosMensajesRecibidos(id as string):
		return [k for k in self.crc[id].getUltimosMensajesRecibidos()]
		
	def getContactosConectados(id):
		return [k for k in self.crc[id].ContactosConectados]