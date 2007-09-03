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
namespace ClienteRepresentado

import System
import MensajeroRemoting

class ClienteRepresentado(ICliente):
	
		listaClientesConectados as List
		controladorCliente as ControladorCliente
		ip as string
		nick as string
		servidor as string
		
		def constructor(nick as string):
			self.ip = "127.0.0.1"
			self.servidor = "127.0.0.1"
			self.nick = nick

			self.listaClientesConectados = List()
			self.controladorCliente = ControladorCliente(self, self.ip, self.servidor, self.nick)
		
		/*	
		def Iniciar():
			while(true):
				Console.WriteLine()
				Console.WriteLine("Esperando eventos. Comandos:")
				Console.WriteLine("   'q': Salir")
				Console.WriteLine("   'c': Conectar")
				Console.WriteLine("   'd': Desconectar")
				Console.WriteLine("   'm': Enviar mensaje")
				Console.WriteLine("   'l': Lista clientes conectados")
				Console.WriteLine()
				
				Console.Write("Escriba su comando: ")
				
				string comando = Console.ReadLine()
				
				if comando.Equals("q"):
					break
				else:
					if comando.Equals( "c"):
						Console.WriteLine("Conectando...")
						Console.WriteLine("Nick: " + self.nick)
						Console.WriteLine("Servidor: " + self.servidor)
						Console.WriteLine("IP: " + self.ip)
						
						// Creo la instancia
						self.listaClientesConectados.AddRange(controladorCliente.Conectar(nick))
						
						Console.WriteLine("Conectado!")
						break
					else:
						if comando.Equals( "d"):
							self.listaClientesConectados.Clear()
							self.controladorCliente.Desconectar()
							Console.WriteLine("Desconectado")
							break
						else:
							if comando.Equals( "m"):
								Console.Write("Escriba el nick destino:")
								string nickDestino = Console.ReadLine()
								
								Console.Write("Escriba el mensaje:")
								string mensaje = Console.ReadLine()
								
								Console.Write("Enviando mensaje...")
								self.controladorCliente.EnviarMensaje(nickDestino, mensaje)
								Console.WriteLine("Enviado")
								break
							else:
								if comando.Equals( "l"):
									self.MostrarClientesConectados()
									break
								else:
									Console.WriteLine("Comando incorrecto")
									break
		*/
		
		def MostrarClientesConectados():
			Console.WriteLine("")
			
			if (self.listaClientesConectados.Count == 0):
				Console.WriteLine("No hay contactos conectados")
				return
			
			Console.WriteLine("Estos son los clientes conectados en este momento:")
			for ci in self.listaClientesConectados:
				Console.WriteLine("Nick: " + ci)
		
		def ContactoConectado(nickCliente as string):
			if nickCliente.Equals(self.nick):
				return
			
			self.listaClientesConectados.Add(nickCliente)
			
			Console.WriteLine("")
			Console.WriteLine("Se conectó un contacto. Nick: " + nickCliente)
			//self.MostrarClientesConectados();
		
		def ContactoDesconectado(nickCliente as string):
			if (nickCliente.Equals(self.nick)):
				return
			
			self.listaClientesConectados.Remove(nickCliente)
			
			Console.WriteLine("")
			Console.WriteLine("Se desconectó un contacto. Nick: " + nickCliente)


		
		def RecibirMensaje(nickCliente as string, mensaje as string):
			Console.WriteLine("")
			Console.WriteLine("El contacto " + nickCliente + " dice: " + mensaje)
		
		MetodoContactoConectado as ConexionClienteHandler:
			get:
				return (ConexionClienteHandler(self.ContactoConectado))

		MetodoContactoDesconectado as ConexionClienteHandler:
			get:
				return (ConexionClienteHandler(self.ContactoDesconectado))
			
		MetodoMensajeRecibido as MensajeRecibidoHandler:
			get:
				return (MensajeRecibidoHandler(self.RecibirMensaje))
		