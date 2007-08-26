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
