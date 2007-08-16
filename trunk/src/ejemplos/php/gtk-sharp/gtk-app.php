<?
  // Import namespaces with the 
  // required objects (PHP/CLR extension)
  import namespace System; 
  import namespace Gtk;
 
  class MyProgram
  {
    // Application entry-point (PHP/CLR extension)
    static function Main()
    {
      // Start the application
      echo "Main";
      Application::Init();
      new MyProgram();
      Application::Run();
    }
 
    // Main application widget
    var $window;
    
    function MyProgram()
    {
      echo "Constructor";
      // Program and application initialization
      $this->window = new Window("MyProgram");
      $this->window->Resize(400, 300);
 
      // Create button
      $btn = new Button("Click me!");
      $btn->BorderWidth = 20;
      //$btn->Clicked->Add(new EventHandler(array($this, "ButtonClick")));

      $this->window->Add($btn);
      $this->window->DeleteEvent->Add(new DeleteEventHandler(array($this, "OnAppDelete")));
      
      // Start the application
      $this->window->ShowAll();
    }
 
    // User clicked on the button - show dialog window
    function ButtonClick($o, $e)
    {
      $dlg = new MessageDialog($this->app, 
         DialogFlags::DestroyWithParent,
         MessageType::Warning, ButtonsType::Close, 
         "Hello world from PHP Gtk#!");
      $dlg->Run();
      $dlg->Destroy();
    }
    
    // Main window was closed - end the application
    function OnAppDelete($o, $e)
    {
      Application::Quit();
    }
  }
?>

