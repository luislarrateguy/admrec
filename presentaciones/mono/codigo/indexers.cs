private string[] apellidos;
...
public string this [int index]
{
    get { return this.apellidos[index]; }
    set { this.apellidos[index] = value; }
}
