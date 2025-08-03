using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Directory_Synchronizer
{
    public class ListComparer
    {
        // This method compares the lists of items in folders and checks what is new and what is removed.

        static public List<string> ListCompare(List<string> Source, List<string> DestOld, List<string> DestNew,
            string sourcePath, string destPath)
        {
            Source = Source.Select(x => x.Replace(sourcePath, "")).ToList();
            DestOld=DestOld.Select(x=>x.Replace(destPath,"")).ToList();
            DestNew = DestNew.Select(x => x.Replace(destPath, "")).ToList();

            List<string> result = new List<string>();

            foreach (string item in Source)
            {
                if (DestNew.Contains(item))
                    result.Add($"{item} has been successfully copied to replica folder");
            }
            foreach (string item in Source)
            {
                if (!DestNew.Contains(item))
                {
                    result.Add($"{item} was NOT copied to replica folder successfully!");
                }
            }
            foreach (string item in Source)
            {
                if (!DestOld.Contains(item))
                {
                    result.Add($"{item} has been added since last replication.");
                }
            }
            foreach (string item in DestOld)
            {
                if (!Source.Contains(item))
                {
                    result.Add($"{item} has been removed since last replication.");
                }
            }

            return result;
        }
    }
}
