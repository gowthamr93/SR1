using System;
using System.Linq;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.Classes;
using Plugin.Connectivity;
using ServiceRequest.AppContext;
using ServiceRequest.Pages;
using ServiceRequest.ViewModels;
using Xamarin.Forms;
using System.Threading.Tasks;
using ServiceRequest.DependencyInterfaces;
using Idox.LGDP.Apps.Common.OnSite;
using ServiceRequest.Views.PopUp;

namespace ServiceRequest
{
    public class App : Application
    {
        public static string ErrorMessage;
        public LoginPage LoginPageInstance;
        public App()
        {
            try
            {
                CrossConnectivity.Current.ConnectivityChanged += Reachability.OnNetworkChanged;
                // The root page of your application
                //MainPage = SplitView.Instace();
                AppData.API = new API();
                AppData.Content = new ContentController();
                AppData.Content.LoadUser();
                LoginPageInstance = new LoginPage();
				MainPage = LoginPageInstance;
				//PageNavigation.PushMainPage(LoginPageInstance);
				ButtonStyles();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        protected async override void OnStart()
        {
            try
            {
                // Handle when your app starts
                ReadError();
                if (!await FileSystem.Exists("error.txt"))
                {
                    var write = await FileSystem.WriteAsync("", "error.txt");
                }
                if (!await FileSystem.Exists("log.txt"))
                {
                    var write = await FileSystem.WriteAsync("LogTrace", "log.txt");
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        protected async override void OnSleep()
        {
            try
            {
                // Handle when your app sleeps
                AppData.LastActivity = DateTime.Now;
                if (await FileSystem.Exists("log.txt"))
                {
                    LogTracking.EntryToLogFile();
                }
                if (AddNewAudioView._isRecording)
                    AddNewAudioView.StopRecord.Invoke();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        protected override void OnResume()
        {
            try
            {
                // Handle when your app resumes
                if (AppData.LastActivity != null)
                {
                    if (AppData.LastActivity.HasValue &&
                        DateTime.Now > AppData.LastActivity.Value.AddMinutes(2))
                    {
                        if (App.Current.MainPage.ToString() != "ServiceRequest.Pages.LoginPage"
                         && !SplitView.Instace().Navigation.ModalStack.Contains(AppContext.AppContext.LoginPage))
                        {
                            AppData.MainModel.CurrentUser.LoginAction = LoginActions.Existing;
                            AppContext.AppContext.IsForLockScreen = true;
                            LockScreen.ClosePopups();
                          //  SplitView.DisplayLockScreen.Invoke();
                            LockScreen.CheckLockScreen();
                        }
                        ClosePopups();
                    }
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        private void ClosePopups()
        {
            //
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
				VisitActionDetailsPage.RelativePopup?.DismisPopup();
				VisitActionDetailsPage.CenterPopup?.DismisPopup();
				VisitActionPage.CenterPopupContent?.DismisPopup();
				VisitActionPage.PopupContent?.DismisPopup();
				SplitView.CenterPopupContent?.DismisPopup();
				SplitView.PopupContent?.DismisPopup();

			}
        }

        private async void ReadError()
        {
            FileSystemArgs read;

            FileSystemArgs tempRead;
            //
            if (await FileSystem.Exists("error.txt"))
            {
                tempRead = read = await FileSystem.ReadTextAsync("error.txt");

                if (read.Error == null && tempRead.TextContents.Trim().Length > 0)
                {
                    //   AppData.ViewModel.ErrorMessage = read.TextContents;
                    ErrorMessage = read.TextContents;
                    await DisplayCrashError();
                    var write = await FileSystem.WriteAsync("", "error.txt");
                }
            }
        }
        public async Task DisplayCrashError()
        {
            try
            {
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
					if (await App.Current.MainPage.DisplayAlert("Unhandled Error", ErrorMessage, "Send Report", "Cancel"))
                    {
                        if (Reachability.InternetConnectionStatus() != ReachabilityNetworkStatus.NotReachable)
                        {
                            var LogDetails = await GetLogData();
                            if (await DependencyService.Get<ISendMail>().Send(ErrorMessage + LogDetails, "Unhandled Error in Service Request App"))
                            {
                                if (await FileSystem.Exists("log.txt"))
                                {
                                    var write = await FileSystem.WriteAsync("LogTrace", "log.txt");
                                }
                            }
                        }
                        else
                            await LoginPageInstance.DisplayAlert("No Network Connection", "Unable to send Report, Please ensure the device has a network connection.", "OK");
                    }
                    ErrorMessage = "";

                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetLogData
        /// 
        /// <summary>	To get the Log Data
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// 
        public async Task<string> GetLogData()
        {
            FileSystemArgs logread;
            FileSystemArgs logtempRead;
            String Result;
            try
            {
                if (await FileSystem.Exists("log.txt"))
                {
                    logtempRead = logread = await FileSystem.ReadTextAsync("log.txt");
                    if (logread.Error == null && logtempRead.TextContents.Trim().Length > 0)
                    {
                        Result = "\n Log Entry Details : " + logread.TextContents;
                    }
                    else
                    {
                        Result = "Log entry is empty";
                    }
                    var write = await FileSystem.WriteAsync("LogTrace:", "log.txt");
                }
                else
                {
                    Result = "File log.txt doesn't exist.";
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                Result = "Exception while loading Log details : " + ex;
            }
            return Result;
        }




		/// <summary>
		/// /Round button style design
		/// </summary>


			public void ButtonStyles()
		{
			Style RoundButtonStyle;
			Application.Current.Resources = new ResourceDictionary();
			if (Device.OS == TargetPlatform.iOS)
			{
				RoundButtonStyle = new Style(typeof(Button))
				{
					Setters =
					{
						new Setter {Property = Button.BorderRadiusProperty, Value = 32}, 
						new Setter {Property = Button.MarginProperty, Value = new Thickness(1, 3, 1, 3)}, 
						new Setter {Property = Button.HorizontalOptionsProperty, Value = LayoutOptions.FillAndExpand}, 
						new Setter {Property = Button.FontAttributesProperty, Value = FontAttributes.Bold}, 
						new Setter {Property = Button.VerticalOptionsProperty, Value = LayoutOptions.FillAndExpand}, 
						new Setter {Property = Button.TextColorProperty, Value = Color.White}, 
						new Setter {Property = Button.FontSizeProperty, Value = Device.GetNamedSize(NamedSize.Large, typeof(Button))}, new Setter {Property = Button.BackgroundColorProperty, Value = Color.FromHex("#104E8B")}
					}
				};
			}
			else
			{
				//for Android and Windows
				RoundButtonStyle = new Style(typeof(Button))
				{
					Setters =
					{
						new Setter {Property = Button.HorizontalOptionsProperty, Value = LayoutOptions.FillAndExpand},
						new Setter {Property = Button.VerticalOptionsProperty, Value = LayoutOptions.FillAndExpand}, 
						new Setter {Property = Button.TextColorProperty, Value = Color.White}, 
						new Setter {Property = Button.FontSizeProperty, Value = Device.GetNamedSize(NamedSize.Medium, typeof(Button))}, 
						new Setter {Property = Button.BackgroundColorProperty, Value = Color.FromHex("#104E8B")}
					}
				};
			}
			Application.Current.Resources.Add(RoundButtonStyle);
		}


    }
}
