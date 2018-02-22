using System;
using ServiceRequest.iOS;
using ServiceRequest.DependencyInterfaces;
using System.Threading.Tasks;
using UIKit;
using System.IO;
using Foundation;
using QuickLook;
using Xamarin.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text;
using Xamarin.Forms.Platform.iOS;
using System.Linq;

[assembly: Xamarin.Forms.Dependency(typeof(FileViewer))]
namespace ServiceRequest.iOS
{
	public class FileViewer : IFileViewer
	{
		public FileViewer()
		{
		}
		public async Task OpenFile(string FilePath)
		{
			try
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					//
					// Here we are trying to get the navigation renderer to get the navigationcontroller from it.
					//

					var firstController = UIApplication.SharedApplication.KeyWindow.RootViewController.ChildViewControllers.FirstOrDefault();

					var navcontroller = firstController as UIViewController;

					var uidic = UIDocumentInteractionController.FromUrl(new NSUrl(FilePath, true));

					uidic.Delegate = new DocInteractionC(navcontroller);

					uidic.PresentPreview(true);

				});

			}
			catch (Exception e) { }

		}

		public class DocInteractionC : UIDocumentInteractionControllerDelegate
		{
			readonly UIViewController _navigationController;

			public DocInteractionC(UIViewController controller)
			{
				_navigationController = controller;
			}

			public override UIViewController ViewControllerForPreview(UIDocumentInteractionController controller)
			{

				return _navigationController;

			}

			public override UIView ViewForPreview(UIDocumentInteractionController controller)
			{

				return _navigationController.View;

			}

		}



	}
}
