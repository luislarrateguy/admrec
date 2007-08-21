using System;
using System.Collections.Generic;
using log4net;
using log4net.Config;  
using utilidades.patrones.observer;

 
namespace MensajeroRemoting
{
	public class ListaContactos :   MarshalByRefObject 
	{
		private List<string> contactos;
		static public ILog log = LogManager.GetLogger("ListaContactos");
		
		public ListaContactos()
		{
			BasicConfigurator.Configure();
			this.contactos = new List<string>();
		}
		
		public bool addContacto(string cadenaNuevoContacto)
		{
			log.Debug("Me están notificando que alguien se conectó: " + cadenaNuevoContacto);		
			
			if (this.contactos.Contains(cadenaNuevoContacto)) {
				log.Debug("Error: ya tenía ese contacto conectado...");
				return false;
			} else {
				this.contactos.Add(cadenaNuevoContacto);
				log.Debug(" - Nuevo cliente agregado: " + cadenaNuevoContacto);
			}
			log.Debug("Contactos conectados:");
			foreach (string h in this.contactos)
				log.Debug(h);
			return true;
		}
		public void delContacto(string cadenaContacto)
		{
			log.Debug("Me están notificando que alguien se desconectó: " + cadenaContacto);			
			
			if (this.contactos.Contains(cadenaContacto)) {
				this.contactos.Remove(cadenaContacto);
				log.Debug(" - Nuevo borrado: " + cadenaContacto);
			} else {
				log.Debug("Error: no se tenía ese contacto conectado...");
			}
			log.Debug("Contactos conectados:");
			foreach (string h in this.contactos)
				log.Debug(h);
		}
		public string[] cont {
			get {
				return this.contactos.ToArray();
			}
			
			set {
				this.contactos.Clear();
				this.contactos.AddRange(value);
				
				log.Debug("Contactos conectados:");
				foreach (string h in this.contactos)
					log.Debug(h);
			}
		}
		
		public override bool Equals (object o)
		{
			if (!(o is ListaContactos))
				return false;

			return true;
		}
		

	}
}
