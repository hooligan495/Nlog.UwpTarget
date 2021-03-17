using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.AI.MachineLearning;
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
            var folder = CreateDirectoryIfNotExist(path);
            
            var f = folder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists).AsTask();
            Task.WhenAll(f);
            StorageFile newFile = f.Result;

            var s = newFile.OpenAsync(FileAccessMode.ReadWrite).AsTask();
            Task.WhenAll(s);
            IRandomAccessStream streamNewFile = s.Result;

            return streamNewFile.AsStream();
        }

        public StorageFolder CreateDirectoryIfNotExist(string path)
        {
            StorageFolder folder = null;
            var name = Path.GetDirectoryName(path);
            try
            {
                var task = StorageFolder.GetFolderFromPathAsync(name).AsTask();
                Task.WhenAll(task);
                folder = task.Result;

                return folder;
            }
            catch (AggregateException ag)
            {
                ag.Handle(ex =>
                {
                    if (ex is FileNotFoundException)
                    {
                        folder = CreateDirectory(name);
                    }
                    return ex is FileNotFoundException;
                });
            }

            return folder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public StorageFolder CreateDirectory(string name)
        {
            var items = name.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);

            StorageFolder prevFolder = null;
            var task = StorageFolder.GetFolderFromPathAsync(items[0]).AsTask();
            Task.WhenAll(task);
            prevFolder = task.Result;

            for (int i = 1; i < items.Length; i++)
            {
                try
                {
                    var task1 = prevFolder.GetFolderAsync(items[i]).AsTask();
                    Task.WhenAll(task);
                    var folder = task1.Result;
                    prevFolder = folder;
                }
                catch
                {
                    var task1 = prevFolder.CreateFolderAsync(items[i]).AsTask();
                    Task.WhenAll(task1);
                    prevFolder = task1.Result;
                }
            }
            return prevFolder;
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