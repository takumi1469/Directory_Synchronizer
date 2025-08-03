using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Directory_Synchronizer
{
    public class Coordinator
    {
        public string source { get; set; } = "";
        public string dest { get; set; } = "";
        public int interval { get; set; }
        public string log { get; set; } = "";
        public Coordinator(string sourceArg, string destArg, int intervalArg, string logArg)
        {
            source = sourceArg;
            dest = destArg;
            interval = intervalArg;
            log = logArg;
        }
        public void DoReplication(Object state)
        {
            // This method is evoked by timer and calls other methods
            // to handle the actual replication and logging

            AutoResetEvent autoEvent = (AutoResetEvent)state;

            // Prepare List of strings to store file and subdirectory paths.
            // These will be used for comparison before and after replication
            List<string> newSourceList = new List<string>();
            List<string> oldDestList = new List<string>();
            List<string> newDestList = new List<string>();

            // Make FileSystem instance to make the project testable
            IFileSystem fileSystem = new FileSystem();

            // Make instance of classes with specified FileSystem
            ListSaver listSaver = new ListSaver(fileSystem);
            Synchronizer synchronizer = new Synchronizer(fileSystem);

            // Prepare DirectoryInfo instances for source and destination directories
            IDirectoryInfo dirSource = fileSystem.DirectoryInfo.New(source);
            IDirectoryInfo dirDest = fileSystem.DirectoryInfo.New(dest);

            if (!dirSource.Exists)
            {
                Console.WriteLine("Source directory doesn't exist.");
                autoEvent.Set();
                return;
            }
            else
            {
                newSourceList = listSaver.ListSave(dirSource, newSourceList);
            }

            if (!dirDest.Exists)
            {
                dirDest.Create();
            }
            else
            {
                // If replication folder already exists, then save the information of
                // contained items in oldDestList for comparison later
                oldDestList = listSaver.ListSave(dirDest, oldDestList);
            }

            // Do the replication by Synchronizer
            synchronizer.Synchronize(dirSource, dirDest);

            // Compare the following three lists for log:
            // 1. List of items in replication folder before replication
            // 2. List of items in source folder
            // 3. List of items in replication folder after replication
            newDestList = listSaver.ListSave(dirDest, newDestList);
            List<string> comparedResult = ListComparer.ListCompare(newSourceList, oldDestList, newDestList, dirSource.FullName,dirDest.FullName );

            // Log the result in console output and log file. Log files will be created with creation date and time
            IDirectoryInfo dirLog = fileSystem.DirectoryInfo.New(log);
            if(!dirLog.Exists)
            {
                dirLog.Create();
            }

            Logger logger = new Logger(fileSystem);
            logger.DoLogging(dirLog, comparedResult);
        }
    }
}
