// ClientesCreator.cs creado conh MonoDevelop
// User: nacho - 01:28 03/09/2007
//
//



namespace ProyectoServidorIntermediario

import System


[Serializable]
public class ClientesCreator(MarshalByRefObject):

	private static cc as ClienteRepresentado

	
	public def constructor():
		pass

	
	public def getClienteRepresentado(id as string) as ClienteRepresentado:
		if cc is null:
			cc = ClienteRepresentado()
		return cc

