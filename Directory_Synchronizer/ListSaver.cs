using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Directory_Synchronizer
{
    internal class ListSaver
    {
        public static List<string> ListSave(DirectoryInfo folder, List<string> itemList)
        {
            // This method makes the list of paths of files and subdirectories of given folder

            // Make a list of DirectoryInfo for subdirectories in the replication folder 
            List<DirectoryInfo> destSubDirs = folder.GetDirectories().ToList<DirectoryInfo>();

            // Make a list of paths(string) of files in replication folder
            foreach (FileInfo file in folder.GetFiles())
            {
                string filePath = Path.Combine(folder.FullName, file.Name);
                itemList.Add(filePath);
            }

            // Add the path of subdirectory, then call this function to recursively perform the same in all subdirectories
            foreach (DirectoryInfo subDir in destSubDirs)
            {
                string newDestSub = Path.Combine(folder.FullName, subDir.Name);
                itemList.Add(newDestSub);
                DirectoryInfo subFolder = new DirectoryInfo(newDestSub);
                itemList = ListSave(subFolder, itemList);
            }

            return itemList;
        }
    }
}
