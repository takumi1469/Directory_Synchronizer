# Directory Synchronizer
## What is this?
This is a console application to replicate one directory (folder) to another. 

## What does it do?
All the files, subfolders, and all the files in the subfolders will be replicated to a specified directory. 
The application also makes the log files to show what has been copied, what was newly created, and what was removed compared to last synchronization.

## How do you use it?
First please publish the solution and make an executable. There are two ways to do this, A) from VisualStudio and B) with dotnet CLI

**A. From VisualStudio**
1. Clone the repository
2. Open the solution with VisualStudio
3. Go on menu on top to Build > Publish Selection > Folder > Folder (default location is fine) > click Publish button

**B. With dotnet CLI**
1. Clone the repository
2. On terminal console, move to the solution folder (Directory_Synchronizer-main\Directory_Synchronizer-main) with Directory_Synchronizer.sln in it 
3. Run this Dotnet CLI command: dotnet publish -c Release -r win-x64 --self-contained true

After publishing the solution, please run the executable as follows
1. On console terminal, move to the following directory in the project directory of Directory_Synchronizer: Directory_Synchronizer\bin\Release\net8.0 
2. From there, run the executable with arguments as follows: **Directory_Synchronizer.exe** **<source_path(string)>** **<destination_path(string)>** **<interval(int)>** **<log_path(string)>**

## Was AI used to develop this solution?
AI was NOT used to develop this application for code generation. 

## What did I learn during the development process?
I learned the following, thanks to this project:
 - Usage of DirectoryInfo and FileInfo
 - Usage of System.Threading.Timer for periodic execution of a method
 - Usage of System.IO.Abstractions package to test with mocked file system
 - Unit testing by xUnits

## How is the application designed?
This application is designed with classes with single responsibility.
Each class has a method to perform each step needed for replication and logging.
- First, the Main method accepts commandline arguments
- Then, the Timer is made in Main to call Coordinator.DoReplication method. This method calls other methods for actual synchronization as follows:
- ListSaver.ListSave is called to make the list of the paths of the items in source folder and replication folder
- Synchronizer.Synchronize is called to copy the items in the source folder to replication folder
- ListSaver.ListSave is called again to make the list of the paths of the items in replication folder after the copying
- ListComparer.ListCompare compares the three lists(source, destinationOld and destinationNew) to determine what has been copied, what has been newly created and what has been removed since last replication
- Logger.DoLogging logs the result returned from ListComparer.ListCompare to console output and specified log file

## Has the solution been tested?
Yes, unit testing has been done with xUnits, and integration test has been done by actually running the solution

## Can this solution run in the background with terminal closed
No, this solution is not a Windows Service. The terminal window that is running this solution must stay open to keep running.
