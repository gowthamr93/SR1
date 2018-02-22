using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using ServiceRequest.DependencyInterfaces;
using ServiceRequest.ViewModels;
using FileSystem = ServiceRequest.UWP.DependencyClasses.FileSystem;

[assembly: Xamarin.Forms.Dependency(typeof(FileSystem))]
namespace ServiceRequest.UWP.DependencyClasses
{
    public class FileSystem : IFileSystem
    {
        public async Task<string> GetRootDir()
        {

            var rootDir = await ApplicationData.Current.LocalFolder.CreateFolderAsync("IdoxSRi", CreationCollisionOption.OpenIfExists);
            return rootDir.Path;
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name		DeleteAsync
        /// 
        /// <summary>	Asynchronously deletes a file.
        /// </summary>
        /// <param name="path">			The path segments for the file.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 

        public async Task<FileSystemArgs> DeleteAsync(params string[] path)
        {

            FileSystemArgs args;
            string filePath;
            //
            filePath = Path.Combine(await Paths(path));
            try
            {
                await DeleteAsync(filePath);
                args = new FileSystemArgs();
            }
            catch (Exception ex)
            {
                if (ex is FileNotFoundException)
                    args = new FileSystemArgs();
                else
                    args = new FileSystemArgs(ex);
            }
            //
            return args;
        }

        /// ------------------------------------------------------------------------------------------------
		/// Name		Exists
		/// 
		/// <summary>	Checks if a file exists.
		/// </summary>
		/// <param name="path">			The path segments for the file.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 

        public async Task<bool> Exists(params string[] path)
        {
            string filePath;
            //
            filePath = Path.Combine(await Paths(path));
            var result = File.Exists(filePath);
            return result;
        }

        /// ------------------------------------------------------------------------------------------------
		/// Name		GetFullPath
		/// 
		/// <summary>	Gets the full path for the file from it's local path segments.
		/// </summary>
		/// <param name="path">			The path segments for the file.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
        public async Task<string> GetFullPath(params string[] path)
        {
            return Path.Combine(await Paths(path));
        }

        /// ------------------------------------------------------------------------------------------------
		/// Name		ReadDataAsync
		/// 
		/// <summary>	Asynchronously reads binary data from a file.
		/// </summary>
		/// <param name="path">			The path segments for the file.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
        public async Task<FileSystemArgs> ReadDataAsync(params string[] path)
        {
            FileSystemArgs args;
            string filePath;
            byte[] contents;
            //
            filePath = Path.Combine(await Paths(path));
            try
            {
                contents = await ReadDataAsync(filePath);
                args = new FileSystemArgs(contents);
            }
            catch (Exception ex)
            {
                if (ex is FileNotFoundException)
                    args = new FileSystemArgs();
                else
                    args = new FileSystemArgs(ex);
            }
            //
            return args;
        }

        /// ------------------------------------------------------------------------------------------------
		/// Name		ReadText
		/// 
		/// <summary>	Reads the string text from a file.
		/// </summary>
		/// <param name="path">			The path segments for the file.</param>
		/// 
		/// <returns>	FileSystemArgs.
		/// </returns>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
        public async Task<FileSystemArgs> ReadText(params string[] path)
        {
            FileSystemArgs args;
            string filePath;
            //
            filePath = Path.Combine(await Paths(path));
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (StreamReader reader = new StreamReader(fs))
                    args = new FileSystemArgs(reader.ReadToEnd());
            }
            catch (FileNotFoundException)
            {
                args = new FileSystemArgs();
            }
            catch (Exception ex)
            {
                args = new FileSystemArgs(ex);
            }
            return args;
        }

        /// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		ReadTextAsync
		/// 
		/// <summary>	Asynchronously reads the string contents of a file.
		/// </summary>
		/// <param name="path">			The path segments for the file.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 

        public async Task<FileSystemArgs> ReadTextAsync(params string[] path)
        {
            FileSystemArgs args;
            string filePath;
            string contents;
            //
            filePath = Path.Combine(await Paths(path));
            try
            {
                contents = await ReadTextAsync(filePath);
                args = new FileSystemArgs(contents);
            }
            catch (Exception ex)
            {
                // Don't propergate a file not found exception, just return nothing in the event.
                if (ex is FileNotFoundException)
                    args = new FileSystemArgs();
                else
                    args = new FileSystemArgs(ex);
            }
            //
            return args;
        }

        /// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		RenameAsync
		/// 
		/// <summary>	Asynchronously renames the file by copying the contents from the source file
		/// 			to the destination file and deleting the source file.
		/// </summary>
		/// <param name="newFileName">		The new file name.</param>
		/// <param name="path">				The path segments for the source file.</param>
		/// 
		/// <remarks>	The path segments are resused for the destination file path, but the last segment 
		/// 			is replaced with the newFileName property.
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
        public async Task<FileSystemArgs> RenameAsync(string newFileName, params string[] path)
        {
            string source;
            string destination;
            FileSystemArgs args;
            //
            source = Path.Combine(await Paths(path));
            path[path.Length - 1] = newFileName;
            destination = Path.Combine(await Paths(path));
            //
            try
            {
                await CopyAsync(source, destination);
                await DeleteAsync(source);
                args = new FileSystemArgs();
            }
            catch (Exception ex)
            {
                args = new FileSystemArgs(ex);
            }
            //
            return args;
        }

        /// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Write
		/// 
		/// <summary>	Writes the binary content to a file, any error encountered is caught and
		/// 			returned in the args.
		/// </summary>
		/// <param name="contents">			The binary content to write.</param>
		/// <param name="path">				The file path segments.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 

        public async Task<FileSystemArgs> Write(byte[] contents, params string[] path)
        {
            string filePath;
            FileSystemArgs args;
            //
            filePath = Path.Combine(await Paths(path));
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    fs.Write(contents, 0, contents.Length);
                //

                // NSFileManager.SetSkipBackupAttribute(filePath, true);
                //
                args = new FileSystemArgs();
            }
            catch (Exception ex)
            {
                args = new FileSystemArgs(ex);
            }
            //
            return args;
        }

        /// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Write
		/// 
		/// <summary>	Writes the string content to a file, any error encountered is caught and
		/// 			returned in the args.
		/// </summary>
		/// <param name="contents">			The string content to write.</param>
		/// <param name="path">				The file path segments.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
        public async Task<FileSystemArgs> Write(string contents, params string[] path)
        {
            string filePath;
            FileSystemArgs args;
            //
            filePath = Path.Combine(await Paths(path));
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                using (StreamWriter writer = new StreamWriter(fs))
                    writer.Write(contents);
                //
                //NSFileManager.SetSkipBackupAttribute(filePath, true);
                //
                args = new FileSystemArgs();
            }
            catch (Exception ex)
            {
                args = new FileSystemArgs(ex);
            }
            //
            return args;
        }


        public async Task WriteAsync(byte[] contents, Action<FileSystemArgs> callback, params string[] path)
        {
            string filePath;
            FileSystemArgs args;
            //
            filePath = Path.Combine(await Paths(path));
            try
            {
                await WriteAsync(filePath, contents);
                args = new FileSystemArgs();
            }
            catch (Exception ex)
            {
                args = new FileSystemArgs(ex);
            }
            //
            if (callback != null)
                callback(args);
        }

        public async Task WriteAsync(string contents, Action<FileSystemArgs> callback, params string[] path)
        {
            string filePath;
            FileSystemArgs args;
            //
            filePath = Path.Combine(await Paths(path));
            try
            {
                await WriteAsync(filePath, contents);
                args = new FileSystemArgs();
            }
            catch (Exception ex)
            {
                args = new FileSystemArgs(ex);
            }
            //
            if (callback != null)
                callback(args);
        }

        /// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		WriteAsync
		/// 
		/// <summary>	Asynchronously writes binary content to a file and calls the callback 
		/// 			method upon completion.
		/// </summary>
		/// <param name="contents">			The binary content to write.</param>
		/// <param name="path">				The file path segments.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
        public async Task<FileSystemArgs> WriteAsync(byte[] contents, params string[] path)
        {
            string filePath;
            FileSystemArgs args;
            //
            filePath = Path.Combine(await Paths(path));
            try
            {
                await WriteAsync(filePath, contents);
                args = new FileSystemArgs();
            }
            catch (Exception ex)
            {
                args = new FileSystemArgs(ex);
            }
            //
            return args;
        }

        /// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		WriteAsync
		/// 
		/// <summary>	Asynchronously writes string content to a file and calls the callback 
		/// 			method upon completion.
		/// </summary>
		/// <param name="contents">			The string content to write.</param>
		/// <param name="path">				The file path segments.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
        public async Task<FileSystemArgs> WriteAsync(string contents, params string[] path)
        {
            string filePath;
            FileSystemArgs args;
            //
            filePath = Path.Combine(await Paths(path));
            try
            {
                await WriteAsync(filePath, contents);
                args = new FileSystemArgs();
            }
            catch (Exception ex)
            {
                args = new FileSystemArgs(ex);
            }
            //
            return args;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		AppendText
        /// 
        /// <summary>	appends string content to a existing file.
        /// </summary>
        /// <param name="contents">			The string content to write.</param>
        /// <param name="path">				The file path segments.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        ///
        public async Task<bool> AppendText(string contents, string[] path)
        {
            string filePath;
            //
            filePath = Path.Combine(await Paths(path));
            try
            {
                var dataBefore = ReadTextAsync(filePath);
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(contents);
                }
                var dataAfter = ReadTextAsync(filePath);
                if (dataAfter.ToString().Length > dataBefore.ToString().Length)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// ------------------------------------------------------------------------------------------------
		/// Name		Paths
		/// 
		/// <summary>	Creates the full path using the root directory for the documents folder on the
		/// 			device with the supplied path segments combined.
		/// </summary>
		/// <param name="path">			The additional path segments for the file path.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
        public async Task<string[]> Paths(string[] path)
        {
            string[] paths = new string[path.Length + 1];
            paths[0] = await GetRootDir();
            for (int i = 0; i < path.Length; i++)
                paths[i + 1] = path[i];

            return paths;
        }

        /// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		CopyAsync
		/// 
		/// <summary>	Copies the contents of the source file to the destination file asynchronously.
		/// </summary>
		/// <param name="filePathSource">		The source file path.</param>
		/// <param name="filePathDestination">	The destination file path.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
        public async Task CopyAsync(string filePathSource, string filePathDestination)
        {
            using (FileStream fsSource = new FileStream(filePathSource, FileMode.Open, FileAccess.Read))
            using (FileStream fsDestination = new FileStream(filePathDestination, FileMode.Create, FileAccess.Write))
                await fsSource.CopyToAsync(fsDestination);
            //
            //NSFileManager.SetSkipBackupAttribute(filePathDestination, true);
        }


        /// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		DeleteAsync
		/// 
		/// <summary>	Deletes a file asynchronously.
		/// </summary>
		/// <param name="filePath">			The file path of the file to delete.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
        public async Task DeleteAsync(string filePath)
        {
            FileInfo fi;
            //
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                fi = new FileInfo(filePath);
                fs.Dispose();
                await Task.Factory.StartNew(() => fi.Delete());
            }

        }

        /// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		ReadDataAsync
		/// 
		/// <summary>	Asynchronously reads the contents of a file as a byte array.
		/// </summary>
		/// <param name="filePath">			The file path.</param>
		/// <param name="tryAgain">			Whether the task should be allowed to try again if it fails.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
        public async Task<byte[]> ReadDataAsync(string filePath, bool tryAgain = true)
        {
            byte[] contents;
            //
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    contents = new byte[fs.Length];
                    await fs.ReadAsync(contents, 0, (int)fs.Length);
                }
            }
            catch (Exception)
            {
                if (tryAgain)
                {
                    await Task.Delay(500);
                    contents = await ReadDataAsync(filePath, false);
                }
                else
                    throw;
            }
            //
            return contents;
        }

        /// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		ReadTextAsync
		/// 
		/// <summary>	Asynchronously reads the contents of a file as a string.
		/// </summary>
		/// <param name="filePath">			The file path.</param>
		/// <param name="tryAgain">			Whether the task should be allowed to try again if it fails.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
        public async Task<string> ReadTextAsync(string filePath, bool tryAgain = true)
        {
            string contents;
            //
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (StreamReader reader = new StreamReader(fs))
                    contents = await reader.ReadToEndAsync();
            }
            catch (Exception)
            {
                if (tryAgain)
                {
                    await Task.Delay(500);
                    contents = await ReadTextAsync(filePath, false);
                }
                else
                    throw;
            }
            //
            return contents;
        }

        /// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		WriteAsync
		/// 
		/// <summary>	Asynchronously writes the byte array to a file, overwriting or creating a new file.
		/// </summary>
		/// <param name="filePath">			The file path.</param>
		/// <param name="contents">			The binary data to write.</param>
		/// <param name="tryCount">			The amount of times the operation has been attempted.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
        public async Task WriteAsync(string filePath, byte[] contents, int tryCount = 0)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    await fs.WriteAsync(contents, 0, contents.Length);
                //
                //NSFileManager.SetSkipBackupAttribute(filePath, true);
            }
            catch (Exception ex)
            {
                // Always try again if the exception is a sharing violation, unless the operation has been
                // attempted too many times already.
                if (tryCount > 5)
                    throw new Exception("Operation has failed too many times.", ex);
                else if (ex.Message.StartsWith("Sharing violation", StringComparison.CurrentCulture) ||
                         tryCount == 0)
                {
                    // Delay the thread for 2 seconds before trying again.
                    await Task.Delay(2000);
                    tryCount++;
                    await WriteAsync(filePath, contents, tryCount);
                }
                else
                    throw;
            }
        }

        /// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		WriteAsync
		/// 
		/// <summary>	Asynchronously writes the string to a file, overwriting or creating a new file.
		/// </summary>
		/// <param name="filePath">			The file path.</param>
		/// <param name="contents">			The string to write.</param>
		/// <param name="tryCount">			The amount of times the operation has been attempted.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
        public async Task WriteAsync(string filePath, string contents, int tryCount = 0)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    await writer.WriteAsync(contents);
                }
                //
                //NSFileManager.SetSkipBackupAttribute(filePath, true);
            }
            catch (Exception ex)
            {
                // Always try again if the exception is a sharing violation, unless the operation has been
                // attempted too many times already.
                if (tryCount > 5)
                    throw new Exception("Operation has failed too many times.", ex);
                else if (ex.Message.StartsWith("Sharing violation", StringComparison.CurrentCulture) ||
                         tryCount == 0)
                {
                    // Delay the thread for 2 seconds before trying again.
                    await Task.Delay(2000);
                    tryCount++;
                    await WriteAsync(filePath, contents, tryCount);
                }
                else
                    throw;
            }
        }
    }
}
