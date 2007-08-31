// Main.cs creado conh MonoDevelop
// User: nacho - 22:51 30/08/2007
//
//
// project created on 30/08/2007 at 22:51
using System;
using System.Collections;
using MensajeroRemoting;

namespace ClienteConsolaEscucha
{
	class MainClass
	{
		private static string[] listaClientesConectados;
		private static ControladorCliente controladorCliente;
		private static EventsHelper eventHelper;
		
		public static void Main(string[] args)
		{
			string ip,nick,servidor;
			ip = args[0];
			servidor = args[1];
			nick = args[2];
			
			Console.WriteLine("Ejecutando Conectar");
			Console.WriteLine("Nick: "+nick);
			Console.WriteLine("Servidor: "+servidor);
			Console.WriteLine("IP: "  +ip);
			
			
			// Creo la instancia
			controladorCliente = new ControladorCliente(ip,servidor,nick);
			listaClientesConectados = controladorCliente.Conectar(nick);
			
			MostrarClientesConectados();
			
			eventHelper = new EventsHelper(controladorCliente.miClienteRemoto);
			eventHelper.ContactoConectado += new ConexionClienteHandler(ContactoConectado);
			eventHelper.ContactoDesconectado += new ConexionClienteHandler(ContactoDesconectado);
			eventHelper.MensajeRecibido += new MensajeRecibidoHandler(RecibirMensaje);

			Console.WriteLine("Fin de Conectar. Conectado.");
			Console.WriteLine("Esperando eventos. Presione 'd' y enter para salir.");
			string c = "";
			while(c != "d")
				c = Console.ReadLine();
		}
		static void MostrarClientesConectados() {
			Console.WriteLine("");
			Console.WriteLine("Estos son los clientes conectados en este momento:");
			foreach (string ci in listaClientesConectados) {
				Console.WriteLine("Nick: " + ci);
			}
		}
		static void ContactoConectado(string cliente) {
			Console.WriteLine("");
			Console.WriteLine("Se conectó un contacto. Nick: " + cliente);
			MostrarClientesConectados();
		}
		static void ContactoDesconectado(string cliente) {
			Console.WriteLine("");
			Console.WriteLine("Se desconectó un contacto. Nick: " + cliente);
			MostrarClientesConectados();
		}
		static void RecibirMensaje(string cliente, string mensaje) {
			Console.WriteLine("");
			Console.WriteLine("El contacto " + cliente + " dice: "+mensaje);
		}
	}
}