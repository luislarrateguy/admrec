/*
    MensajeroRemoting - Mensajero instantáneo hecho con .NET Remoting
    y otras tecnologías de .NET.
    Copyright (C) 2007  Luis Ignacio Larrateguy, Milton Pividori y César Sandrigo

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;

namespace MensajeroRemoting
{
	[Serializable()]
	public class EventsHelper: MarshalByRefObject
	{
		private ClienteRemoto clienteRemoto;
		private log4net.ILog logger;
		
		public event ConexionClienteHandler ContactoConectado;
		public event ConexionClienteHandler ContactoDesconectado;
		public event MensajeRecibidoHandler MensajeRecibido;
		
		public EventsHelper(ClienteRemoto cr)
		{
			this.logger = log4net.LogManager.GetLogger(this.GetType());
			this.clienteRemoto = cr;
			
			this.logger.Debug("Creando objeto EventHelper y registrando a los eventos de ControladorConexiones...");
			this.RegistrarHandler();
			this.logger.Debug("Creado EventHelper!");
		}
		
		~EventsHelper()
		{
			this.logger.Debug("Se está destruyendo el EventHelper!");
		}
		
		public void recibirMensaje(string nick, string mensaje)
		{
			this.logger.Debug("(EventHelper) Evento RecibirMensaje recibido. Disparando el propio...");
			if (this.MensajeRecibido != null)
				this.MensajeRecibido(nick,mensaje);
			this.logger.Debug("listo");
		}
		
		public void clienteConectado(string nick)
		{
			this.logger.Debug("(EventHelper) Evento ContactoConectado recibido. Disparando el propio...");
			if (this.ContactoConectado != null)
				this.ContactoConectado(nick);
			this.logger.Debug("listo");
		}
		
		public void clienteDesconectado(string nick)
		{
			this.logger.Debug("(EventHelper) Evento ContactoDesconectado recibido. Disparando el propio...");
			if (this.ContactoDesconectado != null)
				this.ContactoDesconectado(nick);
			this.logger.Debug("listo");
		}
		
		public void RegistrarHandler()
		{
			this.logger.Debug(" ---- (EventHelper) Registrando metodo clienteConectado");
			this.clienteRemoto.ContactoConectado += new ConexionClienteHandler(this.clienteConectado);
			
			this.logger.Debug(" ---- (EventHelper) Registrando metodo clienteDesconectado");
			this.clienteRemoto.ContactoDesconectado += new ConexionClienteHandler(this.clienteDesconectado);
			
			this.logger.Debug(" ---- (EventHelper) Registrando metodo recibirMensaje");
			this.clienteRemoto.MensajeRecibido += new MensajeRecibidoHandler(this.recibirMensaje);
		}
		
		public void DesregistrarHandlers()
		{
			this.logger.Debug(" ---- (EventHelper) Desregistrando metodo clienteConectado");
			this.clienteRemoto.ContactoConectado -= new ConexionClienteHandler(this.clienteConectado);
			
			this.logger.Debug(" ---- (EventHelper) Desregistrando metodo clienteDesconectado");
			this.clienteRemoto.ContactoDesconectado -= new ConexionClienteHandler(this.clienteDesconectado);
			
			this.logger.Debug(" ---- (EventHelper) Desregistrando metodo recibirMensaje");
			this.clienteRemoto.MensajeRecibido -= new MensajeRecibidoHandler(this.recibirMensaje);
		}
		
		public override object InitializeLifetimeService()
		{
			return null;
		}
	}
}
