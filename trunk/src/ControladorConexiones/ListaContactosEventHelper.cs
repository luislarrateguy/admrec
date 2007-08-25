// ListaContactosEventHelper.cs created with MonoDevelop
// User: miltondp at 16:50 25/08/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace MensajeroRemoting
{
	[Serializable()]
	public class ListaContactosEventHelper : MarshalByRefObject
	{
		private ControladorConexiones controladorConexiones;
		
		public event ControladorConexiones.ListaContactosHandler ContactoConectado;
		public event ControladorConexiones.ListaContactosHandler ContactoDesconectado;
		
		public ListaContactosEventHelper(ControladorConexiones controladorConexiones)
		{
			this.controladorConexiones = controladorConexiones;
			
			if (this.controladorConexiones == null)
				Console.WriteLine("aah, es null");
			
			Console.WriteLine(" ---- Registrando metodo agregarContacto");
			this.controladorConexiones.ContactoConectado +=
				new ControladorConexiones.ListaContactosHandler(this.OnContactoAgregado);
			
			Console.WriteLine(" ---- Registrando metodo quitarContacto");
			this.controladorConexiones.ContactoDesconectado +=
				new ControladorConexiones.ListaContactosHandler(this.OnContactoQuitado);
			
			Console.WriteLine("Suscribiendo...");
//			this.controladorConexiones.Suscribir(this);
		}
		
		public void OnContactoAgregado(string cadena)
		{
			this.ContactoConectado(cadena);
		}
		
		public void OnContactoQuitado(string cadena)
		{
			this.ContactoDesconectado(cadena);
		}
		
		public override object InitializeLifetimeService()
		{
			return null;
		}
	}
}
