all:
	# Compilación
	javac A.java
	ikvmc.exe -out:A.dll A.class
	gmcs -r:A.dll B.cs
	
	# Ejecución
	mono B.exe

clean:
	rm A.class A.dll B.exe
