public class NumeroComplejo {
    private float real;
    private float imag;

    public static NumeroComplejo operator +(NumeroComplejo a,
                                            NumeroComplejo b)
    {
        NumeroComplejo com = new NumeroComplejo();
        com.real = a.real + b.real;
        com.imag= a.imag + b.imag;
        return com;
    }
}
