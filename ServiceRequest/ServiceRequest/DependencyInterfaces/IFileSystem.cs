using System;
using System.Threading.Tasks;
using ServiceRequest.ViewModels;

namespace ServiceRequest.DependencyInterfaces
{
    public interface IFileSystem
    {
        Task<string> GetRootDir();
        Task<FileSystemArgs> DeleteAsync(params string[] path);
        Task<bool> Exists(params string[] path);
        Task<string> GetFullPath(params string[] path);
        Task<FileSystemArgs> ReadDataAsync(params string[] path);
        Task<FileSystemArgs> ReadText(params string[] path);
        Task<FileSystemArgs> ReadTextAsync(params string[] path);
        Task<FileSystemArgs> RenameAsync(string newFileName, params string[] path);
        Task<FileSystemArgs> Write(byte[] contents, params string[] path);
        Task<FileSystemArgs> Write(string contents, params string[] path);
        Task WriteAsync(byte[] contents, Action<FileSystemArgs> callback, params string[] path);
        Task WriteAsync(string contents, Action<FileSystemArgs> callback, params string[] path);
        Task<FileSystemArgs> WriteAsync(byte[] contents, params string[] path);
        Task<FileSystemArgs> WriteAsync(string contents, params string[] path);
        Task<bool> AppendText(string contents, params string[] filePath);
        Task<string[]> Paths(string[] path);
        Task CopyAsync(string filePathSource, string filePathDestination);
        Task DeleteAsync(string filePath);
        Task<byte[]> ReadDataAsync(string filePath, bool tryAgain = true);
        Task<string> ReadTextAsync(string filePath, bool tryAgain = true);
        Task WriteAsync(string filePath, byte[] contents, int tryCount = 0);
        Task WriteAsync(string filePath, string contents, int tryCount = 0);

    }
}
