using System.Collections.Generic;

public class Foo <T> where T : IComparable {
    private T bar;
}

public class MiClase {
    private Foo<string> unNombre;
}
