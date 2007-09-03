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



namespace MensajeroRemoting

import System
import System.Collections
import System.Collections.Generic

public class ClienteConsola(ICliente):

	private listaClientesConectados as List[of string]

	private controladorCliente as ControladorCliente

	
	private ip as string

	private nick as string

	private servidor as string

	
	public static def Main(args as (string)):
		cce = ClienteConsola(args)
		cce.Iniciar()

	
	public def constructor(args as (string)):
		if args.Length == 3:
			self.ip = args[0]
			self.servidor = args[1]
			self.nick = args[2]
		else:
			if args.Length == 1:
				self.ip = '127.0.0.1'
				self.servidor = '127.0.0.1'
				self.nick = args[0]
			else:
				raise ApplicationException('Cantidad de argumentos incorrectos')
		
		self.listaClientesConectados = List[of string]()
		self.controladorCliente = ControladorCliente(self, self.ip, self.servidor, self.nick)

	
	public def Iniciar():
		while true:
			Console.WriteLine()
			Console.WriteLine('Esperando eventos. Comandos:')
			Console.WriteLine('   \'q\': Salir')
			Console.WriteLine('   \'c\': Conectar')
			Console.WriteLine('   \'d\': Desconectar')
			Console.WriteLine('   \'m\': Enviar mensaje')
			Console.WriteLine('   \'l\': Lista clientes conectados')
			Console.WriteLine()
			
			Console.Write('Escriba su comando: ')
			
			comando as string = Console.ReadLine()
			
			if comando.Equals('q'):
				break 
			else:
				if comando.Equals('c'):
					Console.WriteLine('Conectando...')
					Console.WriteLine(('Nick: ' + self.nick))
					Console.WriteLine(('Servidor: ' + self.servidor))
					Console.WriteLine(('IP: ' + self.ip))
					
					// Creo la instancia
					self.listaClientesConectados.AddRange(controladorCliente.Conectar(nick))
					
					Console.WriteLine('Conectado!')
				else:
					if comando.Equals('d'):
						self.listaClientesConectados.Clear()
						self.controladorCliente.Desconectar()
						Console.WriteLine('Desconectado')
					else:
						if comando.Equals('m'):
							Console.Write('Escriba el nick destino:')
							nickDestino as string = Console.ReadLine()
							
							Console.Write('Escriba el mensaje:')
							mensaje as string = Console.ReadLine()
							
							Console.Write('Enviando mensaje...')
							self.controladorCliente.EnviarMensaje(nickDestino, mensaje)
							Console.WriteLine('Enviado')
						else:
							if comando.Equals('l'):
								self.MostrarClientesConectados()
							else:
								Console.WriteLine('Comando incorrecto')

	
	public def MostrarClientesConectados():
		Console.WriteLine('')
		
		if self.listaClientesConectados.Count == 0:
			Console.WriteLine('No hay contactos conectados')
			return 
		
		Console.WriteLine('Estos son los clientes conectados en este momento:')
		for ci as string in self.listaClientesConectados:
			Console.WriteLine(('Nick: ' + ci))

	
	public def ContactoConectado(nickCliente as string):
		if nickCliente.Equals(self.nick):
			return 
		
		self.listaClientesConectados.Add(nickCliente)
		
		Console.WriteLine('')
		Console.WriteLine(('Se conectó un contacto. Nick: ' + nickCliente))
		//this.MostrarClientesConectados();

	
	public def ContactoDesconectado(nickCliente as string):
		if nickCliente.Equals(self.nick):
			return 
		
		self.listaClientesConectados.Remove(nickCliente)
		
		Console.WriteLine('')
		Console.WriteLine(('Se desconectó un contacto. Nick: ' + nickCliente))
		//this.MostrarClientesConectados();

	
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

ClienteConsola.Main(argv)
