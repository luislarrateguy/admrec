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

namespace Controles

import System
import Gtk

public class NickInputEnBoo(Dialog):

	protected entryNick as Gtk.Entry
	protected btnAceptar as Gtk.Button
	protected btnCancelar as Gtk.Button
	
	protected nickEscogido as string

	public def constructor(w as Gtk.Window):
		super('Escoja un nick', w, DialogFlags.Modal)

		self.Deletable = false
		self.entryNick = Entry(15)
		self.VBox.Add(self.entryNick)
		self.entryNick.Show()
		
		self.btnCancelar = Button(Stock.Cancel)
		self.ActionArea.Add(self.btnCancelar)
		self.btnCancelar.Clicked += self.OnBtnCancelarClicked
		self.btnCancelar.Show()
		
		self.btnAceptar = Button(Stock.Ok)
		self.ActionArea.Add(self.btnAceptar)
		self.btnAceptar.Clicked += self.OnBtnAceptarClicked
		self.btnAceptar.Show()
		self.nickEscogido = null

	
	public NickEscogido as string:
		get:
			return self.nickEscogido

	
	public abstract def OnBtnAceptarClicked(o as object, args as EventArgs):
		pass

	public def OnBtnCancelarClicked(o as object, args as EventArgs):
		self.nickEscogido = null
		self.Respond(ResponseType.Cancel)
		self.Destroy()
