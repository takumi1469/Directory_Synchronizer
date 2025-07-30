using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Directory_Synchronizer
{
    internal class Coordinator
    {
        public static void DoReplication(Object state)
        {
            // This method is evoked by timer and calls other methods
            // to handle the actual replication and logging

            // Extract commandline arguments from the passed Object
            CommandLineArguments arguments = (CommandLineArguments)state;
            string source = arguments.source;
            string dest = arguments.dest;
            int interval = arguments.interval;
            string log = arguments.log;

            // Prepare List of strings to store file and subdirectory paths.
            // These will be used for comparison before and after replication
            List<string> newSourceList = new List<string>();
            List<string> oldDestList = new List<string>();
            List<string> newDestList = new List<string>();

            // Prepare DirectoryInfo instances for source and destination directories
            DirectoryInfo dirSource = new DirectoryInfo(source);
            DirectoryInfo dirDest = new DirectoryInfo(dest);

            if (!dirSource.Exists)
            {
                Console.WriteLine("Source directory doesn't exist.");
                return;
            }
            else
            {
                newSourceList = ListSaver.ListSave(dirSource, newSourceList);
            }

            if (!dirDest.Exists)
            {
                dirDest.Create();
            }
            else
            {
                // If replication folder already exists, then save the information of
                // contained items in oldDestList for comparison later
                oldDestList = ListSaver.ListSave(dirDest, oldDestList);
            }

            // Do the replication by Synchronizer
            Synchronizer.Synchronize(dirSource, dirDest);

            // Compare the following three lists for log:
            // 1. List of items in replication folder before replication
            // 2. List of items in source folder
            // 3. List of items in replication folder after replication
            newDestList = ListSaver.ListSave(dirDest, newDestList);
            List<string> comparedResult = ListComparer.ListCompare(newSourceList, oldDestList, newDestList, dirSource.FullName,dirDest.FullName );

            // Log the result in console output and log file. Log files will be created with creation date and time
            Logger.DoLogging(dest, log, comparedResult);
        }
    }
}
