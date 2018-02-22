using ServiceRequest.DependencyInterfaces;
using System;
using System.Threading.Tasks;
using ServiceRequest.UWP.DependencyClasses;
using Windows.Storage;
using ServiceRequest.AppContext;

[assembly: Xamarin.Forms.Dependency(typeof(FileViewer))]
namespace ServiceRequest.UWP.DependencyClasses
{
    class FileViewer : IFileViewer
    {
        public async Task OpenFile(string FilePath)
        {
            try
            {
                if (FilePath != null)
                {
                    StorageFile fileToLaunch = await StorageFile.GetFileFromPathAsync(FilePath);

                    await Windows.System.Launcher.LaunchFileAsync(fileToLaunch, new Windows.System.LauncherOptions { DisplayApplicationPicker = false });
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
    }
}
