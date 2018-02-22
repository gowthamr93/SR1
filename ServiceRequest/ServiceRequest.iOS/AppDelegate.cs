using Foundation;
using Idox.LGDP.Apps.Common.OnSite;
using Idox.LGDP.Apps.ServiceRequest.Client;
using UIKit;
using Xamarin.Forms;

namespace ServiceRequest.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
			DependencyService.Register<FileSystem>();
			DependencyService.Register<WebManager>();
			DependencyService.Register<FileViewer>();
			DependencyService.Register<SendMail>();
			var d = NSBundle.MainBundle.InfoDictionary;
			AppData.Version = new OnSiteVersion(d["CFBundleVersion"].ToString());

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

		//To Lock Screen Rotation
		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, UIWindow forWindow)
		{
			switch (Device.Idiom)
			{
				case TargetIdiom.Phone:
					return UIInterfaceOrientationMask.Landscape;
				case TargetIdiom.Tablet:
					return UIInterfaceOrientationMask.Landscape;
				default:
					return UIInterfaceOrientationMask.Landscape;
			}
		}
    }
}
