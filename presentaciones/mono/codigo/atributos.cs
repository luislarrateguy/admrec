[TableName("personas")]
public class Persona : Persona
{
    [TableColumn("nombre"), PrimaryKey]
    private string nombre;
...
