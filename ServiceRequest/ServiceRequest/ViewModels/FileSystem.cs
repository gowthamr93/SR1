using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using ServiceRequest.DependencyInterfaces;
using ServiceRequest.AppContext;

namespace ServiceRequest.ViewModels
{
    public static class FileSystem
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Static Variables
        /// ------------------------------------------------------------------------------------------------
        /// 
        private static string _rootDir;
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public static Functions
        /// 
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
        public static async Task<FileSystemArgs> DeleteAsync(params string[] path)
        {
            try
            {
                return await DependencyService.Get<IFileSystem>().DeleteAsync(path);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }

        }
        /// 
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
        public static async Task<bool> Exists(params string[] path)
        {
            try
            {
                return await DependencyService.Get<IFileSystem>().Exists(path);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return false;
            }
        }
        /// 
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
        public static async Task<string> GetFullPath(params string[] path)
        {
            try
            {
                //return await DependencyService.Get<IFileSystem>().GetFullPath(path);
                _rootDir = await DependencyService.Get<IFileSystem>().GetRootDir();
                var filePath = System.IO.Path.Combine(Paths(path));
                return filePath;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }

        }

        /// 
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
        public static async Task<FileSystemArgs> ReadDataAsync(params string[] path)
        {
            try
            {
                return await DependencyService.Get<IFileSystem>().ReadDataAsync(path);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }
        /// 
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
        public static async Task<FileSystemArgs> ReadText(params string[] path)
        {
            try
            {
                return await DependencyService.Get<IFileSystem>().ReadText(path);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
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
        public static async Task<FileSystemArgs> ReadTextAsync(params string[] path)
        {
            try
            {
                return await DependencyService.Get<IFileSystem>().ReadTextAsync(path);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
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
        public static async Task<FileSystemArgs> RenameAsync(string newFileName, params string[] path)
        {
            try
            {
                return await DependencyService.Get<IFileSystem>().RenameAsync(newFileName, path);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
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
        public static async Task<FileSystemArgs> Write(byte[] contents, params string[] path)
        {
            try
            {
                return await DependencyService.Get<IFileSystem>().Write(contents, path);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
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
        public static async Task<FileSystemArgs> Write(string contents, params string[] path)
        {
            try
            {
                return await DependencyService.Get<IFileSystem>().Write(contents, path);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		WriteAsync
        /// 
        /// <summary>	Asynchronously writes binary content to a file and calls the callback 
        /// 			method upon completion.
        /// </summary>
        /// <param name="contents">			The binary content to write.</param>
        /// <param name="callback">			The callback method.</param>
        /// <param name="path">				The file path segments.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static async void WriteAsync(byte[] contents, Action<FileSystemArgs> callback, params string[] path)
        {
            try
            {
                await DependencyService.Get<IFileSystem>().WriteAsync(contents, callback, path);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		WriteAsync
        /// 
        /// <summary>	Asynchronously writes string content to a file and calls the callback 
        /// 			method upon completion.
        /// </summary>
        /// <param name="contents">			The string content to write.</param>
        /// <param name="callback">			The callback method.</param>
        /// <param name="path">				The file path segments.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static async void WriteAsync(string contents, Action<FileSystemArgs> callback, params string[] path)
        {
            try
            {
                await DependencyService.Get<IFileSystem>().WriteAsync(contents, callback, path);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        public static async Task<FileSystemArgs> WriteAsync(byte[] contents, params string[] path)
        {
            try
            {
                return await DependencyService.Get<IFileSystem>().WriteAsync(contents, path);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
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
        public static async Task<FileSystemArgs> WriteAsync(string contents, params string[] path)
        {
            try
            {
                return await DependencyService.Get<IFileSystem>().WriteAsync(contents, path);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		AppendText
        /// 
        /// <summary> writes the string to a file, or if file doesnot exist creating a new file.
        /// </summary>
        /// <param name="filePath">			The file path.</param>
        /// <param name="contents">			The string to write.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static async Task<bool> AppendText(string contents, params string[] filePath)
        {
            try
            {
                return await DependencyService.Get<IFileSystem>().AppendText(contents, filePath);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return false;
            }
        }
        /// 
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        
        /// ------------------------------------------------------------------------------------------------
        #region Private Static Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
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
        private static string[] Paths(string[] path)
        {
            try
            {
                string[] paths;
                //
                paths = new string[path.Length + 1];
                paths[0] = _rootDir;
                for (int i = 0; i < path.Length; i++)
                    paths[i + 1] = path[i];
                //
                return paths;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
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
        private static async Task CopyAsync(string filePathSource, string filePathDestination)
        {
            try
            {
                await DependencyService.Get<IFileSystem>().CopyAsync(filePathSource, filePathDestination);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
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
        private static async Task DeleteAsync(string filePath)
        {
            try
            {
                await DependencyService.Get<IFileSystem>().DeleteAsync(filePath);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

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
        private static async Task<byte[]> ReadDataAsync(string filePath, bool tryAgain = true)
        {
            try
            {
                return await DependencyService.Get<IFileSystem>().ReadDataAsync(filePath, tryAgain);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
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
        private static async Task<string> ReadTextAsync(string filePath, bool tryAgain = true)
        {
            try
            {
                return await DependencyService.Get<IFileSystem>().ReadTextAsync(filePath, tryAgain);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		WriteAsync
        /// 
        /// <summary>	Asynchronously writes the byte array to a file, overwriting or creating a new file.
        /// </summary>
        /// <param name="filePath">			The file path.</param>
        /// <param name="contents">			The binary data to write.</param>
        /// <param name="tryCount">			Whether the task should be allowed to try again if it fails.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private static async Task WriteAsync(string filePath, byte[] contents, int tryCount = 0)
        {
            try
            {
                await DependencyService.Get<IFileSystem>().WriteAsync(filePath, contents, tryCount);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

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
        /// <param name="tryCount">			Whether the task should be allowed to try again if it fails.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private static async Task WriteAsync(string filePath, string contents, int tryCount = 0)
        {
            try
            {
                await DependencyService.Get<IFileSystem>().WriteAsync(filePath, contents, tryCount);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        #endregion
        /// ------------------------------------------------------------------------------------------------

    }

    public class FileSystemArgs
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public Constructors
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		FileSystemArgs
        /// 
        /// <summary>	Creates a new instance of the FileSystemArgs class, instantiating it as
        /// 			an error response with no contents.
        /// </summary>
        /// <param name="error">		The exception to populate.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public FileSystemArgs(Exception error)
        {
            try
            {
                BinaryContents = new byte[0];
                Error = error;
                TextContents = "";
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		FileSystemArgs
        /// 
        /// <summary>	Creates a new instance of the FileSystemArgs class, instantiating it as a
        /// 			string response, populated with the string contents of a file.
        /// </summary>
        /// <param name="contents">			The string contents of a file.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public FileSystemArgs(string contents)
        {
            try
            {
                BinaryContents = new byte[0];
                Error = null;
                TextContents = contents;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		FileSystemArgs
        /// 
        /// <summary>	Creates a new instance of the FileSystemArgs class, instantiating it as a
        /// 			data response, populated with the binary contents of a file.
        /// </summary>
        /// <param name="contents">			The binary contents of a file.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public FileSystemArgs(byte[] contents)
        {
            try
            {
                BinaryContents = contents;
                Error = null;
                TextContents = "";
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		FileSystemArgs
        /// 
        /// <summary>	Creates a new instance of the FileSystemArgs class, instantiating it as an
        /// 			empty response, no file contents or error were generated in response to the action.
        /// </summary>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public FileSystemArgs()
        {
            try
            {
                BinaryContents = new byte[0];
                Error = null;
                TextContents = "";
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        public byte[] BinaryContents
        {
            get;
            private set;
        }
        /// 
        public Exception Error
        {
            get;
            private set;
        }
        /// 
        public string TextContents
        {
            get;
            private set;
        }
        #endregion
    }
}
