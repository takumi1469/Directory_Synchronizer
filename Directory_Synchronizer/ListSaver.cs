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
            // This method makes the list of paths of files and subdirectories of given directory

            // Make a list of paths(string) of files in the given directory
            foreach (IFileInfo file in dir.GetFiles())
            {
                string filePath = Path.Combine(dir.FullName, file.Name);
                itemList.Add(filePath);
            }

            // Make a list of IDirectoryInfo for subdirectories in the given directory
            List<IDirectoryInfo> destSubDirs = dir.GetDirectories().ToList<IDirectoryInfo>();

            // Add subdirectories to the list, and call this function itself for all sub directories
            // to recursively perform the same list-saving
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
