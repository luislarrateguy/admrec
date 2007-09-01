// Main.cs creado conh MonoDevelop
// User: nacho - 15:29 30/08/2007
//
//
// project created on 30/08/2007 at 15:29
using System;
using System.Collections;
using MensajeroRemoting;

namespace ClienteConsola
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			string ip1 = "127.0.0.1";
			string ip2 = "127.0.0.1";
			if (args.Length == 2) {
				ip1 = args[0];
				ip2 = args[1];
			}
			ControladorCliente cc = new ControladorCliente(ip1,ip2,"BigBrother");
			//string[] contactos = cc.Conectar("");
			string[] contactos = cc.ContactosConectados;
			
			Console.WriteLine("Contactos conectados:");
			Console.WriteLine("---------------------");
			
			foreach (string cont in contactos)
				Console.WriteLine("Nick: " + cont);
				
			//cc.Desconectar();
		}
	}
}