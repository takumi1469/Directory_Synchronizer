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
        // It also removes given phrases in the full paths of source and destination directories for comparison.

        static public List<string> ListCompare(List<string> Source, List<string> DestOld, List<string> DestNew,
            string sourcePath, string destPath)
        {
            // Turn the FullName of source and destination items (full paths) to just Name (relative paths)
            Source = Source.Select(x => x.Replace(sourcePath, "")).ToList();
            DestOld=DestOld.Select(x=>x.Replace(destPath,"")).ToList();
            DestNew = DestNew.Select(x => x.Replace(destPath, "")).ToList();

            List<string> result = new List<string>();

            // If items in Source are also in DestNew, things that should be copied have been properly copied
            foreach (string item in Source)
            {
                if (DestNew.Contains(item))
                    result.Add($"{item} has been successfully copied to replica folder");
            }
            // If items in Source are NOT in DestNew, things that should be copied have NOT been properly copied
            foreach (string item in Source)
            {
                if (!DestNew.Contains(item))
                {
                    result.Add($"{item} was NOT copied to replica folder successfully!");
                }
            }
            // If items in Source are NOT in DestOld, these items were newly created since last synchronization
            foreach (string item in Source)
            {
                if (!DestOld.Contains(item))
                {
                    result.Add($"{item} has been added since last synchronization.");
                }
            }
            // If items in DestOld are NOT in Source, these items were removed since last synchronization
            foreach (string item in DestOld)
            {
                if (!Source.Contains(item))
                {
                    result.Add($"{item} has been removed since last synchronization.");
                }
            }

            return result;
        }
    }
}
