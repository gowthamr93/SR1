using System;
using System.Threading.Tasks;
using Plugin.Media;
using System.IO;
using System.Reflection;
using Idox.LGDP.Apps.ServiceRequest.Client;
using Plugin.Media.Abstractions;
using ServiceRequest.AppContext;
using ServiceRequest.Pages;
using Xamarin.Forms;

namespace ServiceRequest.Classes
{
    /// <summary>  It Executes when the camera operation is called in the application.
    /// </summary>
    /// ------------------------------------------------------------------------------------------------
    /// 
    public class CameraCapture
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private variables
        private static MemoryStream _imageStream;
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Publicfunctions

        /// <summary>
        /// To capture the image by Device camera
        /// </summary>
        /// <returns></returns>
        public static async Task TakePhoto()
        {
            try
            {
                await CrossMedia.Current.Initialize();
                AppContext.AppContext.ImageSource = null;
                CameraCapture CameraCapture = new CameraCapture();
                var data = CameraCapture.ImageDataFromResource("ServiceRequest.nms.png");
                if (data != null)
                {
                    _imageStream = new MemoryStream(data);
                }
                if (CrossMedia.Current.IsCameraAvailable)
                {
                    var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Directory = "InspectionPhoto",
                        Name = "InspectionPhoto.jpg",
                        PhotoSize = PhotoSize.Medium,
                        DefaultCamera= CameraDevice.Rear,
                    });

                    if (file != null)
                    {
                        var stream = file.GetStream();
                        AppContext.AppContext.ImageSource = stream;
                        file.Dispose();
                    }
                }

                else
                {
                    if (_imageStream != null)
                        AppContext.AppContext.ImageSource = _imageStream;
                }

            }
            catch (Exception ex)
            {
                if (_imageStream != null)
                    AppContext.AppContext.ImageSource = _imageStream;

                System.Diagnostics.Debug.WriteLine("Error : {0}", ex.Message);
                LogTracking.LogTrace(ex.ToString());

            }


        }
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region private Functions
        private byte[] ImageDataFromResource(string imagepath)
        {
            try
            {
                // Ensure "this" is an object that is part of your implementation within your Xamarin forms project
                var assembly = typeof(API).GetTypeInfo().Assembly;
                byte[] buffer = null;

                using (Stream stream = assembly.GetManifestResourceStream(imagepath))
                {
                    if (stream != null)
                    {
                        long length = stream.Length;
                        buffer = new byte[length];
                        stream.Read(buffer, 0, (int)length);
                    }
                }
                return buffer;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }

        public static async Task SelectPhoto()
        {
            try
            {
                await CrossMedia.Current.Initialize();
                var file = await CrossMedia.Current.PickPhotoAsync();

                if (file != null)
                {
                    var stream = file.GetStream();
                    AppContext.AppContext.ImageSource = stream;
                    file.Dispose();
				}
                else
                {
                    AppContext.AppContext.ImageSource = null;
                }

            }
            catch (Exception ex)
            {
				
            }
        }
        public static async Task AssistTakePhoto()
        {
            string[] button = { "Camera", "Photo Library", "Cancel" };
            string option=await SplitView.Instace().DisplayActionSheet("Select your option",null,null,button);
            if (option == "Photo Library")
               await SelectPhoto();
            else if (option == "Camera")
               await TakePhoto();
            else
                AppContext.AppContext.ImageSource = null;
        }

        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
