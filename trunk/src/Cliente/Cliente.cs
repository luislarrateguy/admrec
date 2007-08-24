// Cliente.cs creado conh MonoDevelop
// User: nacho - 19:23 24/08/2007
//
//

using System;

namespace MensajeroRemoting {
	
	
	public class Cliente
	{
		
		public Cliente()
		{
		}
		public static void Main(string[] args)
		{
			ClienteManager cliente = new ClienteManager();
			while (true) {
				Console.WriteLine("");
				Console.WriteLine("Indique la acción: (c)onectar ; (d)desconectar ; " +
				                  "un número de puerto para enviar mensaje");
				string accion = Console.ReadLine();
				
				if (accion.ToLower().Equals("c")) {
					cliente.conectar();
				}
				else if (accion.ToLower().Equals("d")) {
					cliente.desconectar();
				}
				else {
					int puertoDestino = -1;
					try {
						puertoDestino = Convert.ToInt32(accion);
					}
					catch (Exception) {
						Console.WriteLine("Comando desconocido...");
						continue;
					}
					
					HostCliente destino = cliente.obtenerDestino(puertoDestino);
								
					Console.Write("Escriba el mensaje a enviar:");
					string mensaje = Console.ReadLine();
					
					cliente.enviarMensaje(destino,mensaje);
				}
			}
		}
	}
}
