public class Persona {
    public delegate
       void CambioNombreHandler(string nuevoNombre);
    public event CambioNombreHandler CambioNombre;
}

Persona unaPersona = new Persona();
unaPersona.CambioNombre += this.AlgunMetodo;

public void AlgunMetodo(string nuevoNombre);
