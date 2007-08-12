all:
	gmcs -target:library ITask.cs
	gmcs -target:library -r:ITask.dll,System.Runtime.Remoting TaskRunner.cs
	gmcs -target:exe -r:ITask.dll,TaskRunner.dll,System.Runtime.Remoting TaskServer.cs
	gmcs -target:exe -r:ITask.dll,TaskRunner.dll,System.Runtime.Remoting TaskClient.cs

clean:
	rm -rf *.dll *.exe

