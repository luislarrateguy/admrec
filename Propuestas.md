# Algunas propuestas #

  * Integrar el proyecto ControladorConexiones con el Servidor
  * HostCliente debería estar en el paquete de Cliente
  * Crear Manager como entry points, para desacoplar los objetos.
  * Usar el servidor como mediador para conseguir los objetos. Es decir, en vez desde el cliente utilizar los servicios de remoting para obtener el HostCliente remoto, pedirselo al servidor y que este lo pida y lo devuelva. Eso simplificaría la implementacion (a pesar de parecer lo contrario).
  * Usar el patrón de diseño Facade para implementar una mascara sobre los servicios, para poder acceder mediante clientes no convencionales sin soporte de Remoting http://www.dofactory.com/Patterns/PatternFacade.aspx http://en.wikipedia.org/wiki/Fa%C3%A7ade_pattern
  * Cuando querramos cambiar una interfaz mas vale utilizar Adapter (de paso probamos implementaciones) http://www.dofactory.com/Patterns/PatternAdapter.aspx