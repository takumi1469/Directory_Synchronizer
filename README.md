# Directory Synchronizer
## What is this?
This is a console application to replicate one directory (folder) to another. 
## What does it do?
All the files, subfolders, and all the files in the subfolders will be replicated. 
The application also makes the log files to show what has been copied, what was newly created, and what was removed compared to last synchronization.
## How do you use it?
Execute the application from console terminal (command prompt for Windows, for example) with the arguments as follows:
synchronizer.exe **source_path(string)** **destination_path(string)** **interval(int)** **log_path(string)**
## Was AI used to develop this solution?
AI was NOT used to develop this application, not even to ask for help. 
## What did you learn during the development process?
I learned the following, thanks to this project:
 - Usage of DirectoryInfo and FileInfo
 - Usage of System.Threading.Timer for periodic execution of a method
## How is the application designed?
This application is designed with classes with single responsibility.
Each class has a method to perform each step needed for replication and logging.
- First, the Main method accepts commandline arguments, put them in an object because the Timer accepts only one object as a parameter
- Then, the Timer is made in Main to call Coordinator.DoReplication method. This method calls other methods to everything as follows:
- ListSaver.ListSave is called to make the list of the paths of the items in replication folder and source folder
- Synchronizer.Synchronize is called to copy the items in the source folder to replication folder
- ListSaver.ListSave is called again to make the list of the paths of the items in replication folder after the copying
- ListComparer.ListCompare compares the three lists to determine what has been copied, what has been newly created and what has been removed since last replication
- Logger.DoLogging logs the result returned from ListComparer.ListCompare to console output and specified log file

