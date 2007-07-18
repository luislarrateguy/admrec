using System;

public class Persona
{
	private string nombre;
	private string apellido;

	public Persona(string nombre, string apellido)
	{
		this.nombre = nombre;
		this.apellido = apellido;

		Console.WriteLine("Constructor C#");
	}

	public string Nombre
	{
		get { return this.nombre; }
		set { this.nombre = value; }
	}

	public string Apellido
	{
		get { return this.apellido; }
		set { this.apellido = value; }
	}

	public string NombreEnMayusculas
	{
		get {
			return this.nombre.ToUpper();
		}
	}

	public string ApellidoEnMayusculas
	{
		get {
			return this.apellido.ToUpper();
		}
	}
}

