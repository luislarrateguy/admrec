delegate void AlgunaAccion();

public class MiClase {
    static void Main (string[] args) {
        AlgunaAccion a = delegate {
            foreach (string str in args)
                Console.WriteLine(str);
        }
    }
}
