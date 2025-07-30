using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Directory_Synchronizer
{
    internal class Logger
    {
        public static void DoLogging(string pathToDelete, string logPath, List<string> listToLog)
        {
            // This method logs what has been copied, newly made or removed since last replication.

            // Prepare name of log file
            string nowDateTime = DateTime.Now.ToString("dd_MMM_yyyy HH_mm_ss");
            string logFileName = $"{nowDateTime}.txt";

            // Prepare DirectoryInfo for log path
            DirectoryInfo dirLog = new DirectoryInfo(logPath);
            if (!dirLog.Exists)
                dirLog.Create();

            // Write the logs on console and log file, together with logging datetime
            using (StreamWriter logStreamAppend = File.CreateText(Path.Combine(logPath, logFileName)))
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
