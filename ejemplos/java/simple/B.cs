using System;

public class B : A
{
	public B() : base() {
		Console.WriteLine("Mono: B()");
	}

	public B(int num, String nom) : base (num, nom) {
		Console.WriteLine("Mono: B(int, String)");
	}
	
	public static void Main(string[] args)
	{
		new B();
	}
}
