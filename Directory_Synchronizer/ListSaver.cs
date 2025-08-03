using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Directory_Synchronizer
{
    public class ListSaver
    {
        private readonly IFileSystem _fileSystem;
        public ListSaver (IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }


        public List<string> ListSave(IDirectoryInfo dir, List<string> itemList)
        {
            // This method makes the list of paths of files and subdirectories of given folder

            //var dir = _fileSystem.DirectoryInfo.New(path);
            //if(dir.Exists )
            //{
            //    return new List<string>(["non-existent"]);
            //}

            // Make a list of DirectoryInfo for subdirectories in the replication folder 
            List<IDirectoryInfo> destSubDirs = dir.GetDirectories().ToList<IDirectoryInfo>();

            // Make a list of paths(string) of files in replication folder
            foreach (IFileInfo file in dir.GetFiles())
            {
                string filePath = Path.Combine(dir.FullName, file.Name);
                itemList.Add(filePath);
            }

            // Add the path of subdirectory, then call this function to recursively perform the same in all subdirectories
            foreach (IDirectoryInfo subDir in destSubDirs)
            {
                string newDestSub = Path.Combine(dir.FullName, subDir.Name);
                itemList.Add(newDestSub);
                IDirectoryInfo subFolder = _fileSystem.DirectoryInfo.New(newDestSub);
                itemList = ListSave(subFolder, itemList);
            }

            return itemList;
        }
    }
}
