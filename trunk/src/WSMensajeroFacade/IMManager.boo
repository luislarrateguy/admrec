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

namespace WSMensajeroFacade

import System
import System.Collections
import System.Web
import System.Web.Services
import MensajeroRemoting
import System.Runtime.Remoting
import System.Runtime.Remoting.Channels
import ProyectServidorIntermediario

[WebService (Description:"Impossible is nothing", Namespace:"http://localhost/webservices/examples/representante")]
class IMManager():
	
	[WebMethod(Description:"Autentica y crea un objeto remoto que te represente")]
	public def Conectar(nick as string) as string:
//		cc = ClienteClaseProbando.getCC()
//		cr as ClienteRepresentado = cc.getClienteRepresentado("nacho")
//		cr.conectar("nacho")
		cc as ClientesCreator = Activator.GetObject(typeof(ClientesCreator),
				"tcp://127.0.0.1:8099/CC")
		cr = cc.getClienteRepresentado("nacho")
//		cr.conectar("nacho2")
		return "true"
		
	[WebMethod(Description:"Desconecta del respresentante")]
	public def Desconectar(key as string) as bool:
		return true
		
	[WebMethod(Description:"Renueva la sesion por 5 minutos mas. La idea seria siempre desconectarse bien. Quiza lo saque a este metodo.")]
	public def Renovar(key as string) as bool:
		return true
	
	[WebMethod(Description:"Devuelve la lista de contactos")]
	public def getListaContactos() as List:
		return List(["nacho","milton"])
		
	[WebMethod (Description:"Envia un mensaje a fulano")]
	public def enviarMensajeA(key as string, mensaje as string, nick as string) as bool:
		
		return true
		
	[WebMethod (Description:"Devuelve true si hubo cambios: en la lista de contactos, o un mensaje recibido")]
	public def hayCambios(key as string) as int:
		return 1
		
	[WebMethod (Description:"Devuelve el mensaje entrante de nick (FIFO)")]
	public def recibirMensaje(key as string, nick as string) as string:
		return "hola nacho"
		