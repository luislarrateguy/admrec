using System;
using System.Collections.Generic;

namespace MensajeroRemoting
{
	public class Host : MarshalByRefObject
	{
		private string id;
		private List<string> contactos;
		
		public Host()
		{
			this.id = "no tiene id";
			this.contactos = new List<string>();
		}
		
		public void NotificarNuevoContacto(string cadenaNuevoContacto)
		{
			Console.WriteLine("Me están notificando que alguien se conectó: " + cadenaNuevoContacto);
			Console.WriteLine("Cachando objeto remoto del mismo...");
			Host nuevoContacto = (Host)Activator.GetObject(typeof(Host),
			                                               cadenaNuevoContacto);
			
			Console.WriteLine("Veo si no lo tengo ya en mi lista, no debería...");			
			
			if (this.contactos.Contains(cadenaNuevoContacto))
				Console.WriteLine("Error: ya tenía ese contacto conectado...");
			else {
				this.contactos.Add(cadenaNuevoContacto);
				Console.WriteLine(" - Nuevo cliente agregado: " + nuevoContacto.id);
			}
			
			Console.WriteLine("Contactos conectados:");
			foreach (string h in this.contactos)
				Console.WriteLine(h);
		}
		
		public string[] Contactos {
			get {
				return this.contactos.ToArray();
			}
			
			set {
				this.contactos.Clear();
				this.contactos.AddRange(value);
				
				Console.WriteLine("Contactos conectados:");
				foreach (string h in this.contactos)
					Console.WriteLine(h);
			}
		}
		
		public string Id {
			get { return this.id; }
			set { this.id = value; }
		}

		public override bool Equals (object o)
		{
			if (!(o is Host))
				return false;
			
			if (this.id != ((Host)o).id)
				return false;
			
			return true;
		}

	}
}
