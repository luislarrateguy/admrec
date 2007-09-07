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
using System.Runtime.Remoting.Lifetime;

namespace MensajeroRemoting
{

	public delegate void MensajeRecibidoHandler(string nick, string mensaje);
	
	[Serializable()]
	public class ClienteRemoto: MarshalByRefObject
	{
		public event ConexionClienteHandler ContactoConectado;
		public event ConexionClienteHandler ContactoDesconectado;
		public event MensajeRecibidoHandler MensajeRecibido;
		
		private log4net.ILog logger;
		
		public ClienteRemoto()
		{
			this.logger = log4net.LogManager.GetLogger(this.GetType());
		}
		
		~ClienteRemoto()
		{
			this.logger.Debug("Se está destruyendo el ClienteRemoto!");
//			this.DesregistrarHandlers();
		}
		
		public void recibirMensaje(string nick, string mensaje)
		{
			this.logger.Debug("(ClienteRemoto) Evento MensajeRecibido recibido. Disparando el propio...");
			if (this.MensajeRecibido != null)
				this.MensajeRecibido(nick,mensaje);
			this.logger.Debug("listo");
		}
		
		public void clienteConectado(string nick)
		{
			this.logger.Debug("(ClienteRemoto) Evento ContactoConectado recibido. Disparando el propio...");
			if (this.ContactoConectado != null)
				this.ContactoConectado(nick);
			this.logger.Debug("listo");
		}
		
		public void clienteDesconectado(string nick)
		{
			this.logger.Debug("(ClienteRemoto) Evento ContactoDesconectado recibido. Disparando el propio...");
			if (this.ContactoDesconectado != null)
				this.ContactoDesconectado(nick);
			this.logger.Debug("listo");
		}
		
		public void RegistrarHandlers()
		{
			IControladorConexiones cc = ControladorCliente.ObtenerControladorConexiones();
			
			this.logger.Debug(" ---- (ClienteRemoto) Registrando metodo clienteConectado");
			cc.ClienteConectado += new ConexionClienteHandler(this.clienteConectado);
			
			this.logger.Debug(" ---- (ClienteRemoto) Registrando metodo clienteDesconectado");
			cc.ClienteDesconectado += new ConexionClienteHandler(this.clienteDesconectado);
		}
		
		public void DesregistrarHandlers()
		{
			IControladorConexiones cc = ControladorCliente.ObtenerControladorConexiones();
			
			this.logger.Debug(" ---- (ClienteRemoto) Desregistrando metodo clienteConectado");
			cc.ClienteConectado -= new ConexionClienteHandler(this.clienteConectado);
			
			this.logger.Debug(" ---- (ClienteRemoto) Desregistrando metodo clienteDesconectado");
			cc.ClienteDesconectado -= new ConexionClienteHandler(this.clienteDesconectado);
		}
		
		public override object InitializeLifetimeService()
		{
			return null;
		}
	}
}
