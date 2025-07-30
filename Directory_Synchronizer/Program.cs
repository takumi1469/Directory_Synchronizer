using Directory_Synchronizer;
using System.Threading;

// Prepare variables for commandline arguments 
string source = "";
string dest = "";
int interval = 0;
string log = "";

// Assign commandline arguments to variables. Show instruction if input is invalid
try
{
    source = args[0];
    dest = args[1];
    interval = Convert.ToInt32(args[2]);
    log = args[3];
}
catch (Exception ex)
{
    Console.WriteLine("Invalid arguments. Enter arguments as follows, directories should be full paths:\n" +
"replicator.exe source_directory(string) destination_directory(string) interval(int, by seconds) logging_directory(string)");
    return;
}

//Make arguments object because Timer class accepts one object only as parameter for method to be called
CommandLineArguments arguments = new CommandLineArguments(source, dest, interval, log);

System.Threading.Timer timer = new Timer(Coordinator.DoReplication, arguments, 0, interval * 1000);

// wait for the Timer thread to display replication result
var autoEvent = new AutoResetEvent(false);
autoEvent.WaitOne();
