// IListaContactos.cs created with MonoDevelop
// User: miltondp at 18:14 25/08/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace MensajeroRemoting
{
	public interface IListaContactos
	{
		void OnContactoAgregado(string cadena);
		void OnContactoQuitado(string cadena);
	}
}
