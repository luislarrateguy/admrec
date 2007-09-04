// ClientesCreator.cs creado conh MonoDevelop
// User: nacho - 01:28 03/09/2007
//
//



namespace ProyectoServidorIntermediario

import System



public class ClienteRepresentadoFacade(MarshalByRefObject):

	private cc as ClienteRepresentado
	
	def constructor():
		pass
	
	def createClienteRepresentado(id as string):
		if self.cc is null:
			self.cc = ClienteRepresentado()
			
	def conectar(id as string):
		cc.conectar(id)
		
	def enviarMensaje(id as string,nickDestino as string,mensaje as string):
		cc.enviarMensaje(nickDestino,mensaje)
		
	def desconectar(id as string):
		cc.desconectar()
