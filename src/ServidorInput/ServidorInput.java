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

import cli.System.Net.*;
import cli.Gtk.*;

import cli.Controles.NickInputEnBoo;

public abstract class ServidorInput extends NickInputEnBoo
{
	protected Entry entryServidor;
	protected ComboBox cmbIps;
	
	protected String servidorEscogido;
	
	public ServidorInput(cli.Gtk.Window w)
	{
		super(w);
		
		this.set_Title("Opciones de conexión");
		
		this.entryServidor = new Entry();
		this.get_VBox().Add(this.entryServidor);
		this.entryServidor.set_Text("localhost");
		this.entryServidor.Show();
		
		this.cmbIps = ComboBox.NewText();
		this.cmbIps.AppendText("127.0.0.1");
		
		IPHostEntry ipEntry = Dns.GetHostByName(Dns.GetHostName());
		IPAddress [] ipAddr = ipEntry.get_AddressList();
		
		for (IPAddress ipa : ipAddr)
			this.cmbIps.AppendText(ipa.ToString());
			
		this.get_VBox().Add(this.cmbIps);
		this.cmbIps.set_Active(0);
		this.cmbIps.Show();
		
		this.entryNick.set_Text("Su nick");
	}
	
	public String getServidorEscogido()
	{
		return this.entryServidor.get_Text();
	}
	
	public String getIpEscogida()
	{
		return this.cmbIps.get_ActiveText();
	}
	
	public abstract void OnBtnAceptarClicked(java.lang.Object o, cli.System.EventArgs args);
}
