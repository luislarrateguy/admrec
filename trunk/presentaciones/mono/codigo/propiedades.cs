public string Nick
{
    get { return this.nick; }
    set { this.nick = value; }
}

unObjeto.Nick = "Algún nick";
Console.WriteLine(unObjeto.Nick);
