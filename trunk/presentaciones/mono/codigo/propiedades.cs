public string Nick {
    get {
      return this.nick;
    }

    set {
      // Código de validación
      this.nick = value;
    }
}

unObjeto.Nick = "Algún nick";
Console.WriteLine(unObjeto.Nick);
