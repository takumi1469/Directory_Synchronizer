using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Directory_Synchronizer
{
    internal class CommandLineArguments : Object
    {
        public string source {  get; set; }
        public string dest { get; set; }
        public int interval { get; set; }
        public string log { get; set; }
        public CommandLineArguments() { }
        public CommandLineArguments(string source, string dest, int interval, string log)
        {
            this.source = source;
            this.dest = dest;
            this.interval = interval;
            this.log = log;
        }
    }
}
