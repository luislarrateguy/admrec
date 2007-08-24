// application.java creado conh MonoDevelop
// User: nacho - 10:37 24/08/2007
//
//
/*
 * application.java
 *
 * created on 24/08/2007 at 10:37
 */
import cli.Gtk.*;
import cli.Glade.*;

public class Test {
    public static void main (String args[]) {
        Application.Init();
		Window w = new Window ("Hello Mono with Java#");
		Button b = new Button ("Click me");
		w.Add (b);
		w.ShowAll ();
		Application.Run ();
    }
}