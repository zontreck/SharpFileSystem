using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpFileSystem.IO;
using SharpFileSystem.SharpZipLib;

namespace SharpFileSystem.Tests.SharpZipLib
{
    [TestClass]
    public class SharpZipLibFileSystemTest
    {
        private Stream? zipStream;
        private SharpZipLibFileSystem? fileSystem;

        [TestInitialize]
        public void Initialize()
        {
            var memoryStream = new MemoryStream();
            zipStream = memoryStream;
            var zipOutput = new ZipOutputStream(zipStream);

            var fileContentString = "this is a file";
            var fileContentBytes = Encoding.ASCII.GetBytes(fileContentString);
            zipOutput?.PutNextEntry(new ZipEntry("textfileA.txt")
            {
                Size = fileContentBytes.Length
            });
            zipOutput?.Write(fileContentBytes);
            zipOutput?.PutNextEntry(new ZipEntry("directory/fileInDirectory.txt"));
            zipOutput?.Finish();

            memoryStream.Position = 0;
            fileSystem = SharpZipLibFileSystem.Open(zipStream);
        }

        [TestCleanup]
        public void Cleanup()
        {
            fileSystem?.Dispose();
            zipStream?.Dispose();
        }

        private readonly FileSystemPath directoryPath = FileSystemPath.Parse("/directory/");
        private readonly FileSystemPath textfileAPath = FileSystemPath.Parse("/textfileA.txt");
        private readonly FileSystemPath fileInDirectoryPath = FileSystemPath.Parse("/directory/fileInDirectory.txt");

        [TestMethod]
        public void GetEntitiesOfRootTest()
        {
            CollectionAssert.AreEquivalent(new[]
            {
                textfileAPath,
                directoryPath
            }, fileSystem?.GetEntities(FileSystemPath.Root).ToArray());
        }

        [TestMethod]
        public void GetEntitiesOfDirectoryTest()
        {
            CollectionAssert.AreEquivalent(new[]
            {
                fileInDirectoryPath
            }, fileSystem?.GetEntities(directoryPath).ToArray());
        }

        [TestMethod]
        public void ExistsTest()
        {
            Assert.IsTrue(fileSystem?.Exists(FileSystemPath.Root));
            Assert.IsTrue(fileSystem?.Exists(textfileAPath));
            Assert.IsTrue(fileSystem?.Exists(directoryPath));
            Assert.IsTrue(fileSystem?.Exists(fileInDirectoryPath));
            Assert.IsFalse(fileSystem?.Exists(FileSystemPath.Parse("/nonExistingFile")));
            Assert.IsFalse(fileSystem?.Exists(FileSystemPath.Parse("/nonExistingDirectory/")));
            Assert.IsFalse(fileSystem?.Exists(FileSystemPath.Parse("/directory/nonExistingFileInDirectory")));
        }
    }
}
