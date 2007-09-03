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

using System;
using System.Collections;
using System.Collections.Generic;

namespace MensajeroRemoting
{
	public class ClienteConsola : ICliente
	{
		private List<string> listaClientesConectados;
		private ControladorCliente controladorCliente;
		
		private string ip;
		private string nick;
		private string servidor;
		
		public static void Main(string[] args)
		{
			ClienteConsola cce = new ClienteConsola(args);
			cce.Iniciar();
		}
		
		public ClienteConsola(string[] args)
		{
			if (args.Length == 3) {
				this.ip = args[0];
				this.servidor = args[1];
				this.nick = args[2];
			}
			else if (args.Length == 1) {
				this.ip = "127.0.0.1";
				this.servidor = "127.0.0.1";
				this.nick = args[0];
			}
			else {
				throw new Exception("Cantidad de argumentos incorrectos");
			}
			
			this.listaClientesConectados = new List<string>();
			this.controladorCliente = new ControladorCliente(this, this.ip, this.servidor, this.nick);
		}
		
		public void Iniciar()
		{
			while(true) {
				Console.WriteLine();
				Console.WriteLine("Esperando eventos. Comandos:");
				Console.WriteLine("   'q': Salir");
				Console.WriteLine("   'c': Conectar");
				Console.WriteLine("   'd': Desconectar");
				Console.WriteLine("   'm': Enviar mensaje");
				Console.WriteLine("   'l': Lista clientes conectados");
				Console.WriteLine();
				
				Console.Write("Escriba su comando: ");
				
				string comando = Console.ReadLine();
				
				if (comando.Equals("q"))
					break;
				else if (comando.Equals("c")) {
					Console.WriteLine("Conectando...");
					Console.WriteLine("Nick: " + this.nick);
					Console.WriteLine("Servidor: " + this.servidor);
					Console.WriteLine("IP: " + this.ip);
					
					// Creo la instancia
					this.listaClientesConectados.AddRange(controladorCliente.Conectar(nick));
					
					Console.WriteLine("Conectado!");
				}
				else if (comando.Equals("d")) {
					this.listaClientesConectados.Clear();
					this.controladorCliente.Desconectar();
					Console.WriteLine("Desconectado");
				}
				else if (comando.Equals("m")) {
					Console.Write("Escriba el nick destino:");
					string nickDestino = Console.ReadLine();
					
					Console.Write("Escriba el mensaje:");
					string mensaje = Console.ReadLine();
					
					Console.Write("Enviando mensaje...");
					this.controladorCliente.EnviarMensaje(nickDestino, mensaje);
					Console.WriteLine("Enviado");
				}
				else if (comando.Equals("l"))
				{
					this.MostrarClientesConectados();
				}
				else {
					Console.WriteLine("Comando incorrecto");
				}
			}
		}
		
		public void MostrarClientesConectados() {
			Console.WriteLine("");
			
			if (this.listaClientesConectados.Count == 0) {
				Console.WriteLine("No hay contactos conectados");
				return;
			}
			
			Console.WriteLine("Estos son los clientes conectados en este momento:");
			foreach (string ci in this.listaClientesConectados) {
				Console.WriteLine("Nick: " + ci);
			}
		}
		
		public void ContactoConectado(string nickCliente) {
			if (nickCliente.Equals(this.nick))
				return;
			
			this.listaClientesConectados.Add(nickCliente);
			
			Console.WriteLine("");
			Console.WriteLine("Se conectó un contacto. Nick: " + nickCliente);
			//this.MostrarClientesConectados();
		}
		
		public void ContactoDesconectado(string nickCliente) {
			if (nickCliente.Equals(this.nick))
				return;
			
			this.listaClientesConectados.Remove(nickCliente);
			
			Console.WriteLine("");
			Console.WriteLine("Se desconectó un contacto. Nick: " + nickCliente);
			//this.MostrarClientesConectados();
		}
		
		public void RecibirMensaje(string nickCliente, string mensaje) {
			Console.WriteLine("");
			Console.WriteLine("El contacto " + nickCliente + " dice: " + mensaje);
		}
		
		public ConexionClienteHandler MetodoContactoConectado
		{
			get { return (new ConexionClienteHandler(this.ContactoConectado)); }
		}
		
		public ConexionClienteHandler MetodoContactoDesconectado
		{
			get { return (new ConexionClienteHandler(this.ContactoDesconectado)); }
		}
		
		public MensajeRecibidoHandler MetodoMensajeRecibido
		{
			get { return (new MensajeRecibidoHandler(this.RecibirMensaje)); }
		}
	}
}