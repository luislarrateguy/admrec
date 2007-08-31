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
			ControladorCliente cc = new ControladorCliente(args[0],args[1],args[2]);
			string[] contactos = cc.Conectar("");
			
			Console.WriteLine("Contactos conectados:");
			Console.WriteLine("---------------------");
			
			foreach (string cont in contactos)
				Console.WriteLine("Nick: " + cont);
				
			cc.Desconectar();
		}
	}
}