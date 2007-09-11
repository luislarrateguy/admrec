delegate void AlgunaAccion(string nombre);

public class MiClase {
    static void Main (string[] args) {
        AlgunaAccion a = delegate {
            foreach (string str in args)
                Console.WriteLine(str);
        }

        Metodo(a);
    }

    static void Metodo(AlgunaAccion accion) {
        accion("Algún nombre");
    }
}
