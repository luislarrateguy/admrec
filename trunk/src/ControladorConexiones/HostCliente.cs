using System;
using System.Collections.Generic;

namespace MensajeroRemoting
{
	public class HostCliente : MarshalByRefObject
	{
		private string id;
		private List<string> contactos;
		
		public HostCliente()
		{
			this.id = "no tiene id";
			this.contactos = new List<string>();
		}
		
		public void AgregarCliente(string cadenaNuevoContacto)
		{
			Console.WriteLine("");
			Console.WriteLine("Me están notificando que alguien se conectó: " + cadenaNuevoContacto);
			Console.WriteLine("Cachando objeto remoto del mismo...");
			HostCliente nuevoContacto = (HostCliente)Activator.GetObject(typeof(HostCliente),
			                                               cadenaNuevoContacto);
			
			Console.WriteLine("Veo si no lo tengo ya en mi lista, no debería...");			
			
			if (this.contactos.Contains(cadenaNuevoContacto))
				Console.WriteLine("Error: ya tenía ese contacto conectado...");
			else {
				this.contactos.Add(cadenaNuevoContacto);
				Console.WriteLine(" - Nuevo cliente agregado: " + nuevoContacto.id);
			}
			
			this.ContactoConectado(cadenaNuevoContacto);
			
			Console.WriteLine("Contactos conectados:");
			foreach (string h in this.contactos)
				Console.WriteLine(h);
		}
		
		public void QuitarCliente(string cadenaContactoDesconectado)
		{
			Console.WriteLine("");
			Console.WriteLine("Me están notificando que alguien se desconectó: " + cadenaContactoDesconectado);
			
			Console.WriteLine("Veo si no lo tengo ya en mi lista, debería...");
			
			if (this.contactos.Contains(cadenaContactoDesconectado)) {
				this.contactos.Remove(cadenaContactoDesconectado);
				Console.WriteLine(" - Cliente quitado: " + cadenaContactoDesconectado);
			}
			else {
				Console.WriteLine("Error: no tenía ese contacto como conectado...");
			}
			
			this.ContactoDesconectado(cadenaContactoDesconectado);
			
			Console.WriteLine("Contactos conectados:");
			foreach (string h in this.contactos)
				Console.WriteLine(h);
		}
		
		public void EnviarMensaje(string origen, string mensaje)
		{
			Console.WriteLine("");
			Console.WriteLine(" - ¡Mensaje desde " + origen + "! : " + mensaje);
		}
		
		public string[] Contactos {
			get {
				return this.contactos.ToArray();
			}
			
//			set {
//				this.contactos.Clear();
//				this.contactos.AddRange(value);
//				
//				Console.WriteLine("Contactos conectados:");
//				foreach (string h in this.contactos)
//					Console.WriteLine(h);
//			}
		}
		
		public string Id {
			get { return this.id; }
			set { this.id = value; }
		}

		public override bool Equals (object o)
		{
			if (!(o is HostCliente))
				return false;
			
			if (this.id != ((HostCliente)o).id)
				return false;
			
			return true;
		}
		
		public override object InitializeLifetimeService ()
		{
			return null;
		}

	}
}
