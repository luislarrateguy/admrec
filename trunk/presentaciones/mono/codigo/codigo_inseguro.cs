public unsafe class Persona {
    private int* edad;
}

public class Trabajador {
    unsafe {
        float* a;
    }

    // No se pueden usar punteros aquí

    public unsafe int CalcularEdad() {
        // Se pueden usar punteros aquí
    }
}
