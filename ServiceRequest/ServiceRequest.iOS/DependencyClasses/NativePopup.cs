using UIKit; using System; using CoreGraphics; using Xamarin.Forms; using Xamarin.Forms.Platform.iOS; using ServiceRequest;
using Idox.LGDP.Apps.ServiceRequest.Client; using ServiceRequest.iOS;
using ServiceRequest.Views.PopUp;

[assembly: Dependency(typeof(NativePopup))] namespace ServiceRequest.iOS { 	public class NativePopup : INativePopup 	{  		private UIPopoverController popoverController;   		public void NativeCenterPopup(View view, double width, double height) 		{ 			var size = new CGRect(0, 0, width, height); 			var iOSView = ConvertFormsToNative(view, size);  			var viewController = new UIViewController(); 			viewController.Add(iOSView); 			viewController.View.Frame = size;  			popoverController = new UIPopoverController(viewController); 			popoverController.ContentViewController.View.BackgroundColor = viewController.View.BackgroundColor; 			popoverController.PopoverContentSize = size.Size;  			var frame = UIApplication.SharedApplication.KeyWindow.RootViewController.View;  			popoverController.PresentFromRect(frame.Frame, frame, 0, true);  			popoverController.ShouldDismiss += (popoverController) =>
			{
				return false; 			}; 			AppContext.AppContext.IsAlertOnDisplay = true; 			PopUpSample.PopupLayouts.DismissPopup = () => 			{
				if (popoverController.PopoverVisible)
				{
					AppContext.AppContext.IsAlertOnDisplay = false;
				}
				popoverController.Dismiss(true); 			}; 		}  		public void NativeRelativePopup(View view, Point point, double width, double height) 		{ 			var size = new CGRect(0, 0, width, height); 			var pos_x = 0.05; 			var pos_y = 0.04;  			if (point.X > 700) 			{ 				pos_x = 0.12; 				pos_y = 0.05; 			} 			if (point.Y > 500) 			{ 				pos_x = 0.06; 				pos_y = 0.18; 			}


			//var p = new CGRect(new CGPoint(point.X, point.Y), new  CGSize(0, 0));
			var position = new CGRect(point.X + (point.X * pos_x), point.Y + (point.Y * pos_y), 0, 0); 			var iOSView = ConvertFormsToNative(view, size); 			var arrowDiretion = UIPopoverArrowDirection.Up;  			if (point.Y <= 500) 				arrowDiretion = UIPopoverArrowDirection.Up; 			else 				arrowDiretion = UIPopoverArrowDirection.Down;  			var viewController = new UIViewController(); 			viewController.Add(iOSView); 			viewController.View.Frame = size; 			popoverController = new UIPopoverController(viewController); 			popoverController.ContentViewController.View.BackgroundColor = viewController.View.BackgroundColor; 			popoverController.PopoverContentSize = size.Size;  			var frame = UIApplication.SharedApplication.KeyWindow.RootViewController.View;

			if (view.ToString() == "ServiceRequest.Views.PopUp.AddActionView") 			{

				position = new CGRect(500, 400, 0, 0); 				popoverController.PresentFromRect(position, frame, 0, true); 			}
			else if (view.ToString() == ("ServiceRequest.Views.PopUp.AddVisitsPopUp")) 			{

				position = new CGRect(500, 400, 0, 0); 				popoverController.PresentFromRect(position, frame, 0, true); 			} 			else if (view.ToString() == ("ServiceRequest.Views.PopUp.AddressesNearbyView")) 			{

				position = new CGRect(500, 400, 0, 0); 				popoverController.PresentFromRect(position, frame, 0, true); 			}  			else if (Xamarin.Forms.Application.Current.MainPage.ToString() == "ServiceRequest.Pages.VisitActionPage") 			{ 				position = new CGRect(500, 400, 0, 0);
				popoverController.PresentFromRect(position, frame, 0, true);
			}  			else 			{ 				popoverController.PresentFromRect(position, frame, arrowDiretion, true); 			} 			AppContext.AppContext.IsAlertOnDisplay = true; 			PopUpSample.PopupLayouts.DismissPopup = () =>
			{
				if (popoverController.PopoverVisible)
				{
					AppContext.AppContext.IsAlertOnDisplay = false;
				}
				popoverController.Dismiss(true); 			}; 		}  		private static UIView ConvertFormsToNative(Xamarin.Forms.View view, CGRect size) 		{ 			var renderer = RendererFactory.GetRenderer(view); 			renderer.NativeView.Frame = size; 			renderer.Element.Layout(size.ToRectangle());  			var nativeView = renderer.NativeView; 			nativeView.SetNeedsLayout();  			return nativeView; 		}  	} } 