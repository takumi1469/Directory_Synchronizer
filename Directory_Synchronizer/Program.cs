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

//Make Coordinator class instance with the commandline arguments
Coordinator coordinator = new Coordinator(source, dest, interval, log);
var autoEvent = new AutoResetEvent(false);

System.Threading.Timer timer = new Timer(coordinator.DoReplication, autoEvent, 0, interval * 1000);

// wait for the Timer thread to display replication result
autoEvent.WaitOne();

