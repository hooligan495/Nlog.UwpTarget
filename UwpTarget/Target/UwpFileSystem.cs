using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace NLog.Targets
{
    /// <summary>
    /// Wrap the .net Native file management so that we can open and close files.
    /// UWP supports returning a "managed" Stream (Windows.Storage) as a File.IO.Stream type
    /// </summary>
    public class UwpFileSystem : IFileSystem
    {
        /// <summary>
        /// Delete specified file using the Windows.Storage API
        /// </summary>
        /// <param name="name">File name to be deleted</param>
        public void DeleteFile(string name)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Open a file using the Windows.Storage API and return a .net Runtime IO stream
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Stream OpenFileStream(string name)
        {
            var path = Path.GetDirectoryName(name);
            var filename = Path.GetFileName(name);
            var task = StorageFolder.GetFolderFromPathAsync(path).AsTask();
            Task.WhenAll(task);

            var folder = task.Result;
            
            var f = folder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists).AsTask();
            Task.WhenAll(f);
            StorageFile newFile = f.Result;

            var s = newFile.OpenAsync(FileAccessMode.ReadWrite).AsTask();
            Task.WhenAll(s);
            IRandomAccessStream streamNewFile = s.Result;

            return streamNewFile.AsStream();
        }


        public bool FileExists(string name)
        {
            var path = Path.GetDirectoryName(name);
            var filename = Path.GetFileName(name);
            var task = StorageFolder.GetFolderFromPathAsync(path).AsTask();
            Task.WhenAll(task);

            var folder = task.Result;

            var file = folder.TryGetItemAsync(filename).AsTask();
            Task.WhenAll(file);

            var result = file.Result;

            return (result != null);
        }

        /// <summary>
        /// lookup all the filenames in a directory and return the list
        /// </summary>
        /// <returns></returns>
        public List<string> LookupFileNames()
        {
            throw new System.NotImplementedException();
        }
    }
}