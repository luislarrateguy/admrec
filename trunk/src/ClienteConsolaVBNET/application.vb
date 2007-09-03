'
'    MensajeroRemoting - Mensajero instantáneo hecho con .NET Remoting
'    y otras tecnologías de .NET.
'    Copyright (C) 2007  Luis Ignacio Larrateguy, Milton Pividori y César Sandrigo
'
'    This program is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    This program is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <http://www.gnu.org/licenses/>.
'


Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports MensajeroRemoting

Namespace MensajeroRemoting
	Public Class ClienteConsolaVBNET
		Implements ICliente
		Private listaClientesConectados As List(Of String)
		Private controladorCliente As ControladorCliente

		Private ip As String
		Private nick As String
		Private servidor As String

		Public Shared Sub Main(ByVal args As String())
			Dim cce As New ClienteConsolaVBNET(args)
			cce.Iniciar()
		End Sub

		Public Sub New(ByVal args As String())
			If args.Length = 3 Then
				Me.ip = args(0)
				Me.servidor = args(1)
				Me.nick = args(2)
			ElseIf args.Length = 1 Then
				Me.ip = "127.0.0.1"
				Me.servidor = "127.0.0.1"
				Me.nick = args(0)
			Else
				Throw New ApplicationException("Cantidad de argumentos incorrectos")
			End If

			Me.listaClientesConectados = New List(Of String)()
			Me.controladorCliente = New ControladorCliente(Me, Me.ip, Me.servidor, Me.nick)
		End Sub

		Public Sub Iniciar()
			While True
				Console.WriteLine()
				Console.WriteLine("Esperando eventos. Comandos:")
				Console.WriteLine("   'q': Salir")
				Console.WriteLine("   'c': Conectar")
				Console.WriteLine("   'd': Desconectar")
				Console.WriteLine("   'm': Enviar mensaje")
				Console.WriteLine("   'l': Lista clientes conectados")
				Console.WriteLine()

				Console.Write("Escriba su comando: ")

				Dim comando As String = Console.ReadLine()

				If comando.Equals("q") Then
					Exit While
ElseIf comando.Equals("c") Then
					Console.WriteLine("Conectando...")
					Console.WriteLine("Nick: " + Me.nick)
					Console.WriteLine("Servidor: " + Me.servidor)
					Console.WriteLine("IP: " + Me.ip)

					' Creo la instancia
					Me.listaClientesConectados.AddRange(controladorCliente.Conectar(nick))

					Console.WriteLine("Conectado!")
ElseIf comando.Equals("d") Then
					Me.listaClientesConectados.Clear()
					Me.controladorCliente.Desconectar()
					Console.WriteLine("Desconectado")
ElseIf comando.Equals("m") Then
					Console.Write("Escriba el nick destino:")
					Dim nickDestino As String = Console.ReadLine()

					Console.Write("Escriba el mensaje:")
					Dim mensaje As String = Console.ReadLine()

					Console.Write("Enviando mensaje...")
					Me.controladorCliente.EnviarMensaje(nickDestino, mensaje)
					Console.WriteLine("Enviado")
ElseIf comando.Equals("l") Then
					Me.MostrarClientesConectados()
				Else
					Console.WriteLine("Comando incorrecto")
				End If
			End While
		End Sub

		Public Sub MostrarClientesConectados()
			Console.WriteLine("")

			If Me.listaClientesConectados.Count = 0 Then
				Console.WriteLine("No hay contactos conectados")
				Return
			End If

			Console.WriteLine("Estos son los clientes conectados en este momento:")
			For Each ci As String In Me.listaClientesConectados
				Console.WriteLine("Nick: " + ci)
			Next
		End Sub

		Public Sub ContactoConectado(ByVal nickCliente As String)
			If nickCliente.Equals(Me.nick) Then
				Return
			End If

			Me.listaClientesConectados.Add(nickCliente)

			Console.WriteLine("")
			Console.WriteLine("Se conectó un contacto. Nick: " + nickCliente)
			'this.MostrarClientesConectados();
		End Sub

		Public Sub ContactoDesconectado(ByVal nickCliente As String)
			If nickCliente.Equals(Me.nick) Then
				Return
			End If

			Me.listaClientesConectados.Remove(nickCliente)

			Console.WriteLine("")
			Console.WriteLine("Se desconectó un contacto. Nick: " + nickCliente)
			'this.MostrarClientesConectados();
		End Sub

		Public Sub RecibirMensaje(ByVal nickCliente As String, ByVal mensaje As String)
			Console.WriteLine("")
			Console.WriteLine("El contacto " + nickCliente + " dice: " + mensaje)
		End Sub

		Public ReadOnly Property MetodoContactoConectado() As ConexionClienteHandler
			Get
				Return (New ConexionClienteHandler(AddressOf Me.ContactoConectado))
			End Get
		End Property

		Public ReadOnly Property MetodoContactoDesconectado() As ConexionClienteHandler
			Get
				Return (New ConexionClienteHandler(AddressOf Me.ContactoDesconectado))
			End Get
		End Property

		Public ReadOnly Property MetodoMensajeRecibido() As MensajeRecibidoHandler
			Get
				Return (New MensajeRecibidoHandler(AddressOf Me.RecibirMensaje))
			End Get
		End Property
'		Public Function get_MetodoContactoConectado() As ConexionClienteHandler
'				Return (New ConexionClienteHandler(AddressOf Me.ContactoConectado))
'		End Function
'
'		Public Function get_MetodoContactoDesconectado() As ConexionClienteHandler
'				Return (New ConexionClienteHandler(AddressOf Me.ContactoDesconectado))
'		End Function
'
'		Public Function get_MetodoMensajeRecibido() As MensajeRecibidoHandler
'				Return (New MensajeRecibidoHandler(AddressOf Me.RecibirMensaje))
'		End Function
	End Class
End Namespace
