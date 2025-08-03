using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Directory_Synchronizer
{
    public class Synchronizer
    {
        private readonly IFileSystem _fileSystem;
        public Synchronizer(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }
        public void Synchronize(IDirectoryInfo dirSource, IDirectoryInfo dirDest)
        {
            // This method copies all the files and subdirectories of given source folder to destination folder.

            // Delete existing replication folder and create one newly
            if (dirDest.Exists)
                dirDest.Delete(true);

            dirDest.Create();

            List<IDirectoryInfo> sourceSubDirs = dirSource.GetDirectories().ToList<IDirectoryInfo>();

            foreach (IFileInfo file in dirSource.GetFiles())
            {
                string filePath = Path.Combine(dirDest.FullName, file.Name);
                file.CopyTo(filePath);
            }

            foreach (IDirectoryInfo subDir in sourceSubDirs)
            {
                string newDestSub = Path.Combine(dirDest.FullName, subDir.Name);
                IDirectoryInfo newDestSubDir = _fileSystem.DirectoryInfo.New(newDestSub);
                Synchronize(subDir, newDestSubDir);
            }
        }
    }
}
