// ClientesCreator.cs creado conh MonoDevelop
// User: nacho - 01:28 03/09/2007
//
//

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