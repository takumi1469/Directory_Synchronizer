using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Directory_Synchronizer
{
    public class Logger
    {
        private readonly IFileSystem _fileSystem;
        public Logger(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }
        public void DoLogging(IDirectoryInfo dirLog, List<string> listToLog)
        {
            // This method logs what has been copied, newly made or removed since last replication.

            // Prepare name of log file
            string nowDateTime = DateTime.Now.ToString("dd_MMM_yyyy HH_mm_ss");
            string logFileName = $"{nowDateTime}.txt";

            // Write the logs on console and log file, together with logging datetime
            using (StreamWriter logStreamAppend = _fileSystem.File.CreateText(Path.Combine(dirLog.FullName, logFileName)))
            {
                logStreamAppend.WriteLine(nowDateTime);
                Console.WriteLine(nowDateTime);

                foreach (string item in listToLog)
                {
                    logStreamAppend.WriteLine(item);
                    Console.WriteLine(item);
                }
            }
        }

    }
}
