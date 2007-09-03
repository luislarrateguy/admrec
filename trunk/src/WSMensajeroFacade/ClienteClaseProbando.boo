// ClienteClaseProbando.cs creado conh MonoDevelop
// User: nacho - 10:11 03/09/2007
//
//

namespace MensajeroRemoting

import System
import System.Runtime.Remoting
import System.Runtime.Remoting.Channels
import ProyectServidorIntermediario

class ClienteClaseProbando():
	
	static def getCC() as ClientesCreator:
		cc as ClientesCreator = Activator.GetObject(typeof(ClientesCreator),
				"tcp://127.0.0.1:8099/CC")
		return cc
