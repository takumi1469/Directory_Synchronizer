using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Directory_Synchronizer;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Directory_Synchronizer_Test
{
    //File system, directories and files are mocked for testing by using System.IO.Abstraction NuGet package
    public class AllTests
    {
        private MockFileSystem mockFileSystem;
        private MockFileData mockFileData;
        private MockFileData mockFileData2;
        private string sourcePath;
        private string destPath;
        private string subPath;
        private string logPath;
        IDirectoryInfo dirSource;
        IDirectoryInfo dirDest;
        IDirectoryInfo dirSub;
        IDirectoryInfo dirLog;

        public AllTests()
        {
            // The following directory structure is simulated:
            // C:\source
            //        |-------- in.txt
            //        |-------- sub1 
            //                   |----- inSub.txt

            mockFileSystem = new MockFileSystem();
            mockFileData = new MockFileData("line1\nline2\nline3");
            mockFileData2 = new MockFileData("Test string");
            sourcePath = @"C:\source";
            destPath = @"C:\dest";
            subPath = @"C:\source\sub1";
            logPath = @"C:\log";
            dirSource = mockFileSystem.DirectoryInfo.New(sourcePath);
            dirDest = mockFileSystem.DirectoryInfo.New(destPath);
            dirSub = mockFileSystem.DirectoryInfo.New(Path.Combine(sourcePath, subPath));
            dirLog = mockFileSystem.DirectoryInfo.New(logPath);
            mockFileSystem.AddDirectory(dirSource);
            mockFileSystem.AddDirectory(dirSub);
            mockFileSystem.AddDirectory(dirLog);
            mockFileSystem.AddFile(Path.Combine(sourcePath, @"in.txt"), mockFileData);
            mockFileSystem.AddFile(Path.Combine(subPath, @"inSub.txt"), mockFileData2);
        }

        [Fact]
        public void Test_ListSaver()
        {
            // Verifies that ListSaver.ListSave method saves all item infos in given directory into List<string>

            //ARRANGE
            List<string> fileList = new List<string>();
            //IDirectoryInfo directory = mockFileSystem.DirectoryInfo.New(sourcePath);
            //mockFileSystem.AddDirectory(directory);
            //mockFileSystem.AddFile(Path.Combine(sourcePath, @"in.txt"), mockFileData);

            //ACT
            ListSaver saver = new ListSaver(mockFileSystem);
            saver.ListSave(dirSource, fileList);

            //ASSERT
            Assert.Contains<string>(@"C:\source\in.txt", fileList);
            Assert.Contains<string>(@"C:\source\sub1", fileList);
            Assert.Contains<string>(@"C:\source\sub1\inSub.txt", fileList);
        }

        [Fact]
        public void Test_Synchronizer()
        {
            //Verifies that Synchronizer.Synchronize replicates all items from source to destination

            //ARRANGE
            //Arranging is done in constructor by creating directories and files

            //ACT
            Synchronizer synchronizer = new Directory_Synchronizer.Synchronizer(mockFileSystem);
            synchronizer.Synchronize(dirSource, dirDest);

            //ASSERT
            // Test that copied directories exist
            Assert.True(dirDest.Exists);
            IDirectoryInfo dirCopiedSub = mockFileSystem.DirectoryInfo.New(Path.Combine(destPath, subPath));
            Assert.True(dirCopiedSub.Exists);

            // Test that copied file exists and has the same content as original file
            IFileInfo copiedFile = mockFileSystem.FileInfo.New(Path.Combine(destPath, "in.txt"));
            Assert.True(copiedFile.Exists);
            string fileContent = "";
            using (StreamReader reader = mockFileSystem.File.OpenText(Path.Combine(destPath, "in.txt")))
            {
                fileContent = reader.ReadToEnd();
            }

            Assert.True(fileContent == "line1\nline2\nline3");
        }

        [Fact]
        public void Test_ListComparer()
        {
            //Verifies that ListComparer.ListCompare compares three lists and saves the result

            //ARRANGE
            List<string> oldDestList = new List<string>() { @"C:\dest\a.txt", @"C:\dest\b.txt", @"C:\dest\c.txt" };
            List<string> SourceList = new List<string>() { @"C:\source\a.txt", @"C:\source\b.txt", @"C:\source\d.txt" };
            List<string> newDestList = new List<string>() { @"C:\dest\a.txt", @"C:\dest\d.txt" };

            //ACT
            List<string> comparedResult = ListComparer.ListCompare(SourceList, oldDestList, newDestList, sourcePath, destPath);

            //ASSERT
            Assert.Contains<string>(@"\a.txt has been successfully copied to replica folder", comparedResult);
            Assert.Contains<string>(@"\b.txt was NOT copied to replica folder successfully!", comparedResult);
            Assert.Contains<string>(@"\d.txt has been added since last synchronization.", comparedResult);
            Assert.Contains<string>(@"\c.txt has been removed since last synchronization.", comparedResult);
        }

        [Fact]
        public void Test_Logger()
        {
            //Verifies that the contents of List<string> is written to a file by Logger.DoLogging

            //ARRANGE
            List<string> listLog = new List<string>([@"\a.txt has been successfully copied to replica folder", @"\c.txt has been removed since last replication."]);

            //ACT
            Logger logger = new Logger(mockFileSystem);
            logger.DoLogging(dirLog, listLog);

            //ASSERT
            IFileInfo[] logFileList = dirLog.GetFiles();
            IFileInfo logFileInfo = logFileList[0];
            string logFileFullName = logFileInfo.FullName;
            string logContent = "";

            using (StreamReader reader = mockFileSystem.File.OpenText(logFileFullName))
            {
                logContent = reader.ReadToEnd();
            }

            Assert.Contains(@"\a.txt has been successfully copied to replica folder", logContent);
        }

    }
}