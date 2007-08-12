// TaskClient.cs
//
// ************************
// (c) 2001 Luke Venediger
//
// For Comments, Feedback and good South African Beer
// please contact me on the address below:
// lukev123@hotmail.com
// ************************
//
// This is the client application domain. The goal of our 
// client is to create a task object and give it to the 
// All-Powerful task server. 
// 
// Our client's machine is a lowly Pentium 1 with limited resources 
// and very little power. He, however, would like to perform a complex
// mathematical operation that will take years for him to finish if he
// was to run it in his own application domain.
//
// Thus he will make use of the TaskServer's enormous multiprocessor-equipped
// backbone and simply submit the task and await a speedy response.
//
// The moral of this tale is that a task is going to be created in
// our local Application Domain and then packaged and sent to another
// better-equipped remote Application Domain, namely the TaskServer.
//
// The mechanics of the excercise will be to use the ITask interface 
// when creating our task, and ofcourse getting a reference to 
// the Server's TaskRunner object which we will use to submit and
// run our task.

using System;

// Libraries necessary for setting up a connection
// to the TaskServer.
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

// We must reference the TaskServer namespace, as well as
// tell the compiler to reference the DLLs that we received from
// the TaskServer System Administrator.
using TaskServer;

// A nice descriptice NameSpace title
namespace HappyClientWithTask {

    // Before we create our task it is important that we 
    // specify the Serialisable compiler attribute. Anything that
    // we want to send to the server must be serialized first. So,
    // anything used by the task object including the object itself
    // must be able to be serialized.    
    [Serializable()]
    
    // This is our task!
    class ClientTask : ITask {
        
        // The two numbers we will ask
        // the task server to add
        private int num1, num2;
        private int result;
        
        // Constructor, taking two numbers as input
        public ClientTask(int num1, int num2) {
            this.num1 = num1;
            this.num2 = num2;
        }
        
        // Our implementation of the Run() method.
        // It adds two numbers together and returns 
        // the answer as an upcasted object
        public object Run() {
            result = num1 * num2;
            return (object)result;
        }
        
        // Used to identify the task to the server.
        public string Identify() {
            return("I am a multiplication task.");
        }
    }

    // This is our client application that will create the task object,
    // and submit it to the TaskServer.    
    public class Client {
        
        public static void Main() {
            
            Console.WriteLine("\nWelcome to the humble, lowly client Application Domain.\n");
            
            // Create our task object
            // (we want to multiply 100 and 100)
            ClientTask clientTask = new ClientTask(100,100);            

            // We will exit on all exceptions            
            try {
                // Attempt to get a connection and a
                // channel opening to the TaskServer
                Console.WriteLine("[i] Connecting to TaskServer...");
                Console.WriteLine("[i] - Opening TCP Channel");            
                TcpChannel chan  = new TcpChannel();
                Console.WriteLine("[i] - Registering the channel");
                ChannelServices.RegisterChannel(chan);
                Console.WriteLine("[i] Connected to TaskServer");
                
                // Get a reference of the TaskRunner object 
                // from the TaskServer.
                // The GetObject method from the Activator class takes two
                // parameters:
                //   * The Type of object that must be retrieved
                //   * The URI location of the object
                // The resulting object must be down-casted to the 
                // TaskRunner object from a generic Object.
                Console.WriteLine("[i] Getting a reference to the TaskRunner Object");
                TaskRunner taskRunner = (TaskRunner)Activator.GetObject(typeof(TaskServer.TaskRunner),
                                                            "tcp://localhost:8085/TaskServer");
                
                // Check if we have an object reference
                if(taskRunner == null) {
                    Console.WriteLine("[e] Could not locate server.");
                    Console.WriteLine("[i] Exiting...");                
                    return;
                }
                
                // If we are here then we have a vaild object reference
                Console.WriteLine("[i] We have an object reference!");
                
                // Here is where we pass the task object to the server,
                // using the taskRunner's LoadTask method. The entire object and
                // all of it's dependency objects are serialized and passed over
                // the channel to the taskRunner object in the server's Application
                // Domain.
                Console.WriteLine("\n[i] Submitting our task to the server...");
                string response = taskRunner.LoadTask(clientTask);
                
                // Display the server's response
                Console.WriteLine("[i] Server says: " + response);
    
                // We now ask the taskRunner to run the task on our behalf. The result
                // or feedback of the task is passed back to us as a native object.
                // We will downcast it to the correct type after receiving it from the
                // task runner.
                Console.WriteLine("\n[i] Running the task and awaiting feedback...");
                object result = taskRunner.RunTask();
                
                // Display the results on the client screen. We downcast the results
                // to 'int', which will then get it's ToString() method called. Infact,
                // if we wanted to we could simply place the object there without any
                // typecasting, because ToString() is a method of the base class Object.
                Console.WriteLine("[aaa-uuuum] The Great and Powerful TaskServer Says: " + (int)result);
            } catch (Exception e) {
                
                // Oh-boy, there was an exception. So dump the stack trace to the 
                // console.
                Console.WriteLine("[e] An exception occurred.");
                Console.WriteLine(e);
            }
    
        }
    }
}
