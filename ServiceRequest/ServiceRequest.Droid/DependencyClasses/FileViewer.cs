using System;
using System.Linq;
using Android.Content;
using ServiceRequest.Droid.DependencyClasses;
using ServiceRequest.DependencyInterfaces;
using System.Threading.Tasks;
using Xamarin.Forms;
using Uri = Android.Net.Uri;
using ServiceRequest.AppContext;

[assembly: Dependency(typeof(FileViewer))]
namespace ServiceRequest.Droid.DependencyClasses
{
    public class FileViewer : IFileViewer
    {
        public async Task OpenFile(string FilePath)
        {
            try
            {
                var file = new Java.IO.File(FilePath);

                if (file.Exists())
                {
                    if (FilePath != null)
                    {
                        Intent intent = new Intent(Intent.ActionView);
                        intent.SetDataAndType(Uri.Parse("file:///" + FilePath), FilePath.Split('.').LastOrDefault().GetContentType());
                        intent.SetFlags(ActivityFlags.ClearTop);
                        Forms.Context.StartActivity(intent);
                    }
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
    }
}