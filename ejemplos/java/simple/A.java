
public class A {
	protected int numero;
	protected String nombre;
	
	public A() {
		this.numero = 0;
		this.nombre = "";
		System.out.println("Java: A()");
	}

	public A(int num, String nom) {
		this.numero = num;
		this.nombre = nom;
		System.out.println("Java: A(int, String)");
	}
}
