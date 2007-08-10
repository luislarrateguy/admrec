using System;

public class Client {
	public static void Main(string[] args)
	{
		SecurityWebService sws = new SecurityWebService();
		
		SecurityInfo respuesta = sws.GetSecurityInfo(args[0]);
		
		Console.WriteLine("## Resultado ##");
		Console.WriteLine("Code: " + respuesta.Code);
		Console.WriteLine("Company name: " + respuesta.CompanyName);
		Console.WriteLine("Price: " + respuesta.Price);
	}
}

