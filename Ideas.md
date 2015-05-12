# Características generales perseguidas #
  * Utilizar única y exclusivamente Software Libre, y así demostrar (o no) que se pueden hacer cosas serias con Software Libre.
  * Utilizar la plataforma Mono.
    * [Página oficial](http://www.mono-project.com/)
  * Utilizar varios lenguajes de programación. Por lo tanto lo que vayamos a implementar debería ser modular... no?
    * [Lista de lenguajes según página de Mono](http://www.mono-project.com/Languages).
    * [Otro link que pasó nacho](http://www.dotnetpowered.com/languages.aspx).
  * Utilizar [Mono.Unix](http://www.mono-project.com/Internationalization) para traducir la aplicación.
  * Utilizar, en algún módulo, una base de datos orientada a objetos, como [db4o](http://www.db4o.com/).
  * Utilizar Web Services.
    * [Wikipedia](http://en.wikipedia.org/wiki/Web_service).

# Software a desarrollar #
¿Qué vamos a hacer?
  * Idea 1:
> Usar [XFN](http://gmpg.org/xfn/) o bien [FOAF](http://www.foaf-project.org/) ([XFN and FOAF](http://www.gmpg.org/xfn/and/foaf)) (http://rdfweb.org/topic/FAQ) en nuestro Blogs (y de otros amigos) y sacar algún tipo de conclusiones de las relaciones establecidas. Quizá un software que te permita consultar, o buscar en los blogs, y que tire cosas ordenadas por relevancia. Algo similar a lo que contó César acerca de last.fm
> [Web Semantica Hoy: etiquetas meta, rdf y microformatos](http://www.wshoy.sidar.org/index.php?2006/04/03/32-etiquetas-meta-ficheros-rdf-microformatos-3-sabores-de-la-web-semaacutentica)
> [Qué es la web semantica?](http://en.wikipedia.org/wiki/Semantic_Web)
> ![http://upload.wikimedia.org/wikipedia/en/thumb/4/47/W3c-semantic-web-layers.svg/300px-W3c-semantic-web-layers.svg.png](http://upload.wikimedia.org/wikipedia/en/thumb/4/47/W3c-semantic-web-layers.svg/300px-W3c-semantic-web-layers.svg.png)
> [Presentacion](http://www.w3.org/2006/Talks/0718-aaai-tbl/Overview.html)

  * Idea 2:
> Usar [Las API de Google](http://code.google.com/apis/) que hay varias, para hacer una miniaplicación que permita a un webmaster hacer diversas cosas, como por ejemplo generar un [sitemap](http://nacho.larrateguy.com.ar/sitemap.xml) con la [api correspondiente](http://www.google.com/webmasters/sitemaps/docs/en/about.html) y editarlo a preferencia. También podría ver sus [últimos correos](http://gmail.google.com/support/bin/answer.py?answer=13465), ver los resultados de validaciones de su web ante la W3C, etc. El generador de sitemaps por ejemplo, está escrito en Python, por lo tanto tal vez se pueda usar con IronPython.

  * Idea 3:
> Usar alguna base de datos orientada a objetos, o base de datos basada en XML, y aprovechar que queremos usar varios lenguajes, para integrar mini sistemas (y schemas) diferentes a un solo programa.
**Observación sobre la idea 3 (milton)**: Esto más que una idea de un programa a desarrollar, es una proposición para utilizar alguna tecnología. Me parece muy buena. Lo puse en "Características generales perseguidas" (arriba).

  * Idea 4:
> Si acordamos en los objetivos de arriba (Características generales perseguidas), mi propuesta es desarrollar algo bien simple apuntando a probar esas cosas, y también que sea modular, para así poder implementar cada módulo con un lenguaje distinto.

> Mi idea es desarrollar un cliente de mensajería instantáneo. Por supuesto que NO se conectaría a redes populares, como MSN, Jabber, etc. La idea es desarrollar algo muy simple. Constaría de un cliente (con módulos implementados en distintos lenguajes) y un servidor. Éste último sería simplemente un servicio web que almacena una lista de clientes conectados. Así cuando un cliente quiera conectarse, se registra en la base del servidor, y éste le devuelve una lista de IPs con algunos otros datos de los otros clientes conectados.

> Se podrían también, si estan de acuerdo y hay tiempo, utilizar algunas cosas interesantes que anda dando vueltas por ahí, como [Mono.AddIns](http://www.mono-project.com/Mono.Addins) para crear plugins fácilmente.

## Arquitectura ##
Algunos links, donde tenemos arquitecturas similares a la que podemos llegar a implementar.

> [Using Web Services for Remoting over the Internet. - The Code Project - C# WebServices](http://www.codeproject.com/cs/webservices/remotingoverinternet.asp)

> [Remoting Architecture in .NET - The Code Project - .NET](http://www.codeproject.com/useritems/Remoting_Architecture.asp)

> [.NET Remoting with an easy example - The Code Project - C# Programming](http://www.codeproject.com/csharp/Net_Remoting.asp)

> [Instant Messaging / Chat using Remoting for LAN](http://www.codeproject.com/useritems/ChatMasala.asp)


## Lenguajes utilizados ##
### C# ###
Lo vamos a utilizar para desarrollar tal o cual parte de la aplicación...

### IronPython ###
Y a este para hacer tal o cual cosa...

### Java ###
A través de IKVM para ejecutar la API de Jena si es que hacemos algo con RDF y algo de inferencia.

### Boo ###
Boo y GTK# en Monodevelop se pueden utilizar con Stetic.






