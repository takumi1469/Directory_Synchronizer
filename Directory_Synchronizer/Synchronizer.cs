using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Directory_Synchronizer
{
    internal class Synchronizer
    {
        public static void Synchronize(DirectoryInfo dirSource, DirectoryInfo dirDest)
        {
            // This method copies all the files and subdirectories of given source folder to destination folder.

            // Delete existing replication folder and create one newly
            if (dirDest.Exists)
                dirDest.Delete(true);

            dirDest.Create();

            List<DirectoryInfo> sourceSubDirs = dirSource.GetDirectories().ToList<DirectoryInfo>();

            foreach (FileInfo file in dirSource.GetFiles())
            {
                string filePath = Path.Combine(dirDest.FullName, file.Name);
                file.CopyTo(filePath);
            }

            foreach (DirectoryInfo subDir in sourceSubDirs)
            {
                string newDestSub = Path.Combine(dirDest.FullName, subDir.Name);
                DirectoryInfo newDestSubDir = new DirectoryInfo(newDestSub);
                Synchronize(subDir, newDestSubDir);
            }
        }
    }
}
