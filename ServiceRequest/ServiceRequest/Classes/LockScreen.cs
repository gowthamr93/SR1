using System;
using System.Linq;
using System.Threading.Tasks;
using Idox.LGDP.Apps.Common.OnSite;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.AppContext;
using ServiceRequest.Pages;
using ServiceRequest.Views;
using Xamarin.Forms;

namespace ServiceRequest
{
	public class LockScreen
	{
		public LockScreen()
		{
		}public static async Task<bool> ToDisplayAlert(Page page, String Title, String Message, String Accept, String cancel = null) 		{

			try 			{ 				bool result; 				AppContext.AppContext.IsAlertOnDisplay = true; 				if (cancel != null) 				{ 					result = await page.DisplayAlert(Title, Message, Accept, cancel); 				} 				else 				{ 					await page.DisplayAlert(Title, Message, Accept); 					result = true; 				} 				AppContext.AppContext.IsAlertOnDisplay = false; 				if (AppContext.AppContext.IsForLockScreen != true) 				{ 					return result; 				} 				else 				{ 					CheckLockScreen(); 					if (result)
						await Application.Current.MainPage.DisplayAlert("Attention", "You can not perform the " + Title + " operation while app was locked.", "OK"); 						//await page.DisplayAlert("Attention", "You can not perform the " + Title + " operation while app was locked.", "OK"); 					return false;
 				} 			} 			catch (Exception ex) 			{ 				return false; 			} 		}  		public static async Task<string> ToDisplayActionSheet(Page page, string message, string cancel = null, string destruction = null, params string[] buttons) 		{ 			string result = ""; 			AppContext.AppContext.IsAlertOnDisplay = true; 			result = await page.DisplayActionSheet(message, cancel, destruction, buttons); 			AppContext.AppContext.IsAlertOnDisplay = false; 			if (!AppContext.AppContext.IsForLockScreen) 			{ 				return result; 			} 			else 			{ 				CheckLockScreen(); 				if (!result.Equals("Cancel")) 				{ 					await 						Application.Current.MainPage.DisplayAlert("Attention", 							"You can not perform the " + result + " operation while app was locked.", "OK"); 				} 				return "Cancel";  			} 		}  		public static void CheckLockScreen() 		{ 			if (AppContext.AppContext.IsAlertOnDisplay == false) 			{ 				if (AppContext.AppContext.IsForLockScreen == true) 				{ 					AppData.MainModel.CurrentUser.LoginAction = LoginActions.Existing; 					ClosePopups(); 					SplitView.DisplayLockScreen.Invoke(); 				} 			} 		}

	    public static void ClosePopups()
	    {
	    try
	    {
	        //var page = SplitView.Navigation.ModalStack.LastOrDefault();
	        var page = PageNavigation.GetStack().LastOrDefault();

	        if (page != null)
	            if (page.GetType() == typeof(HelpPage))
	                //SplitView.Navigation.PopModalAsync();
	                PageNavigation.PopMainPage();

	        if (VisitActionPage.CenterPopupContent != null && VisitActionPage.CenterPopupContent.PopupVisible)
	            VisitActionPage.CenterPopupContent?.DismisPopup();
	        if (VisitActionPage.PopupContent != null && VisitActionPage.PopupContent.PopupVisible)
	            VisitActionPage.PopupContent?.DismisPopup();

	        //
	        if (SplitView.CenterPopupContent != null && SplitView.CenterPopupContent.PopupVisible)
	            SplitView.CenterPopupContent?.DismisPopup();
	        if (SplitView.PopupContent != null && SplitView.PopupContent.PopupVisible)
	            SplitView.PopupContent?.DismisPopup();

	        //

	        if (VisitActionDetailsPage.CenterPopup != null && VisitActionDetailsPage.CenterPopup.PopupVisible)
	            VisitActionDetailsPage.CenterPopup?.DismisPopup();
	        if (VisitActionDetailsPage.RelativePopup != null && VisitActionDetailsPage.RelativePopup.PopupVisible)
	            VisitActionDetailsPage.RelativePopup?.DismisPopup();

	        if (AppContext.AppContext.IsAlertOnDisplay == true)
	        {
	            VisitActionPage.CenterPopupContent?.DismisPopup();
	            VisitActionPage.PopupContent?.DismisPopup();
	            SplitView.CenterPopupContent?.DismisPopup();
	            SplitView.PopupContent?.DismisPopup();

	        }
	        if (FullMapView.NewCaseAddView != null)
	            FullMapView.NewCaseAddView = null;
	        if (AddNewCaseView.NewCaseVm != null)
	            AddNewCaseView.NewCaseVm = null;
	    }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

}
}
