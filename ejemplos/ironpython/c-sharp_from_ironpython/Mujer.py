import clr
clr.AddReferenceToFile("Persona.dll")

import Persona

class Mujer(Persona):

	def imprimirNombreCompleto(self):
		print self.Nombre + ' ' + self.Apellido

	def nombreEnMinusculas(self):
		return self.Nombre.lower()

	def apellidoEnMinusculas(self):
		return self.Apellido.lower()

x = Mujer('Juana', 'Vargas')
x.imprimirNombreCompleto()

print x.NombreEnMayusculas + ' ' + x.ApellidoEnMayusculas
print x.nombreEnMinusculas() + ' ' + x.apellidoEnMinusculas()
print x.nombreEnMinusculas() + ' ' + x.ApellidoEnMayusculas

