using Idox.LGDP.Apps.Common.OnSite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Idox.LGDP.Apps.ServiceRequest.Client;
using PopUpSample;
using ServiceRequest.AppContext;
#if __ANDROID__
using Android.OS;
#endif
using ServiceRequest.DependencyInterfaces;
using ServiceRequest.ViewModels;
using ServiceRequest.Views;
using ServiceRequest.Views.PopUp;
using Xamarin.Forms;
using Exception = System.Exception;

namespace ServiceRequest.Pages
{
    public partial class LoginPage : ContentPage
    {
        /// ------------------------------------------------------------------------------------------------
        #region   Private Variables
        /// ------------------------------------------------------------------------------------------------
        /// 
        private SRiUser _mOUser;
        private string _mSPin = "";
        private string m_oNoDigit = "nopin.png";
        private string m_oDigit = "pin.png";
        private string _mSAuthUrl, _mSRedirectUrl, _mSCompleteUrl;
        private static bool _mShowPin;
        private bool _isMiddle = true;
        private Action<string> _mOAuthComplete;
        private string FragmentSource { get; set; }
        private static PopupLayouts PopupContent { get; set; }
        private ScrollView PageScroll { get; set; }

        private bool _initialcount = true;

        private bool InitialAnimate
        {
            get
            {
                if (_initialcount)
                {
                    _initialcount = false;
                    return true;
                }
                return false;
            }
        }
        /// 
        #endregion
        ///-------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Variables
        ///-------------------------------------------------------------------------------------------------
        ///

        public static Action ReAuthenticate { get; set; }
        public static Action LogOut { get; set; }

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Initialise
        /// 
        /// <summary>	Initialises the content of the view.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public LoginPage()
        {
            try
            {
                InitializeComponent();
                Btn_Logout.Clicked += Btn_Logout_Clicked;
                Btn_DemoLogin.Clicked += BtnDemoLogin_Clicked;
                Btn_Delete.Clicked += BtnDelete_Clicked;
                Lbl_Version.Text = AppData.Version.Text;
                Btn_0.Clicked += (s, e) => PinEntry("0");
                Btn_1.Clicked += (s, e) => PinEntry("1");
                Btn_2.Clicked += (s, e) => PinEntry("2");
                Btn_3.Clicked += (s, e) => PinEntry("3");
                Btn_4.Clicked += (s, e) => PinEntry("4");
                Btn_5.Clicked += (s, e) => PinEntry("5");
                Btn_6.Clicked += (s, e) => PinEntry("6");
                Btn_7.Clicked += (s, e) => PinEntry("7");
                Btn_8.Clicked += (s, e) => PinEntry("8");
                Btn_9.Clicked += (s, e) => PinEntry("9");

                //if (Device.OS == TargetPlatform.iOS)
                //{
                //	Txt_DemoUsername.Focus();
                //}
                WebView_Credentials.Navigated += WebViewCredentialsNavigated;

                UpdateEnvironmentName();

                AppData.MainModel.PropertyUpdated += PropertyUpdated;
                AppData.PropertyModel.PropertyUpdated += PropertyUpdated;
                TapGestureRecognizer tapEnvironment = new TapGestureRecognizer();
                tapEnvironment.Tapped += (s, e) =>
                {

                    if (!_isMiddle)
                    {
                        Gl_Auth.TranslateTo(0, 0, 1);
                        _isMiddle = true;
                    }
                    if (PopupContent != null) PopupContent.Closed += PopupContentClosed;
                    PopupContent?.ShowPopupRelative(new EnvironmentsView(PopupContent), Lbl_Environment, 100, 200, true, "");
                    //AppContext.AppContext.ShowInput?.Invoke();
                };
                Lbl_Environment.GestureRecognizers.Add(tapEnvironment);
                Boxvw_Label.GestureRecognizers.Add(tapEnvironment);
                LogOut += Logout;
                MakeReady();
                ReAuthenticate += ReAuthenticateTheUser;
                //Onsoftkeyboardappearance
                if (Device.OS == TargetPlatform.iOS)
                {
                    var scrollview = new ScrollView()
                    {
                        Content = Sl_DemoCredentials
                    };
                    Gl_SubLoginSpace.Children.Remove(Sl_DemoCredentials);
                    Gl_SubLoginSpace.Children.Add(scrollview, 1, 0);
                }

            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }



        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		MakeReady
        /// 
        /// <summary>	Prepares the view for display and use.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void MakeReady()
        {
            try
            {
                _mOUser = AppData.MainModel.CurrentUser;
                Lbl_Environment.Text = AppData.Environment == OnSiteEnvironments.NotSpecified ? "Choose Environment" : AppData.Environment.GetDescription();
                if (_mOUser.Pin.Length > 0 && _mOUser.IsLoggedout == false)
                    ShowPinView(true);
                else
                {
                    ShowPinView(false);
                    AppData.Environment = AppData.Environment;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }


        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Private Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        /// Name		Btn_Logout_Clicked
        /// 
        /// <summary>	Handles the event when the logout button is clicked.
        /// </summary>
        /// <param name="sender">	Standard.</param>
        /// <param name="e">		Click args.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async void Btn_Logout_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (AppData.Environment != OnSiteEnvironments.Sales)
                {
                    if (Reachability.InternetConnectionStatus() != ReachabilityNetworkStatus.NotReachable)
                    {
                        if (await DisplayAlert("Log Out", "Do you want to Log Out ?", "YES", "NO"))
                            Logout();
                    }
                    else
                        await DisplayAlert("No Network Connection",
                            "Please ensure the device has a network connection before logging out entirely, re-authentication or authenticating a new user requires network access.",
                            "OK");
                }
                else
                {
                    if (await DisplayAlert("Log Out", "Do you want to Log Out ?", "YES", "NO"))
                    {
                        Logout();
                    }
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		WebViewCredentialsNavigated
        /// 
        /// <summary>	Handling the Keyboard when webpage content has loaded fully.
        /// </summary>        
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        ///
        void WebViewCredentialsNavigated(object sender, WebNavigatedEventArgs e)
        {
            OnSoftKeyboardAppearance();
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Poplockscreen
        /// 
        /// <summary>	Added Pop Lock Screen method to Pop the Lock screen manually instead of Poping in Property Updated method.
        /// </summary>        
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        ///
        private void Poplockscreen()
        {
            if (Device.OS == TargetPlatform.Android)
            {
                try
                {
                    Application.Current.MainPage.Navigation.PopModalAsync();
                }
                catch (Exception ex)
                {
                    // ignored
                }
            }
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		AuthCompleted
        /// 
        /// <summary>	handles completion of the authentication process.
        /// </summary>
        /// <param name="fragment">		The fragment from the authentication success url.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async void AuthCompleted(string fragment)
        {
            string token = null;
            string email = null;
            try
            {
                foreach (string param in fragment.Split('&'))
                {
                    var paramPair = param.Split('=');
                    if (paramPair.Length == 2)
                        switch (paramPair[0])
                        {
                            case "access_token":
                                token = paramPair[1];
                                break;
                            case "email":
                                email = paramPair[1];
                                break;
                        }
                }
                if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(email))
                {
                    var action = AppData.MainModel.CurrentUser.IdoxId.Equals(email) && AppData.Environment.Equals(AppData.MainModel.CurrentUser.Environment) ? LoginActions.Existing : LoginActions.New;
                  //  var action = AppData.MainModel.CurrentUser.IdoxId.Equals(email) ? LoginActions.Existing : LoginActions.New;
                    _mOUser = new SRiUser()
                    {
                        Environment = AppData.Environment,
                        IdoxId = email,
                        IsValidated = true,
                        LoginAction = action,
                        Pin = "",
                        Token = token,
                        Version = AppData.Version
                    };
                    if (action == LoginActions.New && !string.IsNullOrEmpty(AppData.MainModel.CurrentUser.IdoxId))
                    {

                        if (await DisplayAlert("Switch Accounts?", string.Format("You are about to log in with a different account from the last session ({0}), this could potentially lose any un-synced work from that user.\r\n\nContinue?", AppData.MainModel.CurrentUser.IdoxId), "YES", "NO"))
                            GetUserDetails();
                        else
                        {
                            LoadAuthenticationLogin(AppData.ActiveEnvironments[AppData.Environment].APIHost);
                            AppData.Environment = AppData.Environment;
                        }
                    }
                    else
                        GetUserDetails();
                }
                else
                {
                    ShowPinView(false);
                    await DisplayAlert("Authentication Error",
                                    "It was not possible to complete the authentication process, please enter your details again.", "OK");
                    AppData.Environment = AppData.Environment;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }


        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		AuthUserDetailsResponse
        /// 
        /// <summary>	Handles the response from getting the user details after authentication.
        /// </summary>
        /// <param name="e">			The response arguments.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async Task AuthUserDetailsResponse(RequestResponseEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    var userDetails = OnSiteUser.FromJson(e.Text);
                    if (userDetails != null)
                    {
                        _mOUser.Organisations = userDetails.Organisations;
                        foreach (OnSiteUserOrganisation org in userDetails.Organisations)
                            if (org.Organisation.HasEnterprise)
                            {
                                _mOUser.HasEnterprise = true;
                                break;
                            }

                        if (_mOUser.Pin.Equals(""))
                        {
                            await DisplayAlert("Setup Unlock Pin", "Please enter a 4 digit unlock pin that you can use for accessing the app in future.", "OK");
                            //await Gl_Auth.TranslateTo(0, 0, 1);
                            ShowPinView(true);
                        }
                        else
                            ShowPinView(true);
                    }
                }
                else
                {
                    await DisplayAlert("Error Retreiving User Details", e.Error.Message, "OK");
                    if (AppData.Environment != OnSiteEnvironments.NotSpecified)
                        LoadAuthenticationLogin(AppData.ActiveEnvironments[AppData.Environment].APIHost);
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name		cmdDelete_Click
        /// 
        /// <summary>	Removes the last digit entered for the pin.
        /// </summary>
        /// <param name="sender">	Standard.</param>
        /// <param name="e">		Click args.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 

        private void BtnDelete_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (_mSPin.Length > 0)
                {
                    RefreshDeletePinView();
                    _mSPin = _mSPin.Substring(0, _mSPin.Length - 1);
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		cmdDemoLogin_Click
        /// 
        /// <summary>	Handles a click on the login button for the sales demo environment.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 

        private void BtnDemoLogin_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (Txt_DemoUsername.Text != null && Txt_DemoPassword.Text != null)
                {
                    if ((Txt_DemoUsername.Text.ToLower().Equals("demo@idoxgroup.com") && Txt_DemoPassword.Text.Equals("demo")) || (AppData.MainModel.Environment == OnSiteEnvironments.NoAuth))
                    {
                        _mOUser = new SRiUser()
                        {
                            HasEnterprise = true,
                            IdoxId = "demo@idoxgroup.com",
                            IsValidated = true,
                            LoginAction = LoginActions.New,
                            Pin = "",
                            Environment = AppData.Environment,
                            Organisations = new List<OnSiteUserOrganisation>()
                    {
                        new OnSiteUserOrganisation()
                        {
                            Organisation = new OnSiteOrganisation()
                            {
                                Id = "SALES_DEMO",
                                Name = "SALES_DEMO"
                            }
                        }
                    }
                        };

                        if (AppData.Environment == OnSiteEnvironments.NoAuth)
                        {
                            _mOUser.IdoxId = Txt_DemoUsername.Text.ToLower();
                            _mOUser.Organisations = new List<OnSiteUserOrganisation>(new[]
                            {   new OnSiteUserOrganisation()
                            {
                                Organisation = new OnSiteOrganisation()
                                {
                                    Id = Txt_DemoPassword.Text.ToLower(),
                                    Name = Txt_DemoPassword.Text
                                },
                                IsAdmin = false
                             }
                        });
                        }

                        DisplayAlert("Setup Unlock Pin",
                        "Please enter a 4 digit unlock pin that you can use for accessing the app in future.", "OK");
                        ShowPinView(true);
                    }
                    else
                        DisplayAlert("Invalid Credentials", "The credentials entered for the sale demo were invalid.", "OK");
                }
                else
                    DisplayAlert("Invalid Credentials", "The credentials entered for the sale demo were invalid.", "OK");
                Txt_DemoUsername.Text = Txt_DemoPassword.Text = "";
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name CheckUserStatus
        /// 
        /// <summary>	Checks if the user is currently validated and either shows or hides the
        /// 			login modal.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 

        private void CheckUserSatus()
        {
            try
            {
                if (AppData.MainModel.CurrentUser.IsValidated)
                {
                    switch (AppData.MainModel.CurrentUser.LoginAction)
                    {
                        case LoginActions.New:
                            AppData.PropertyModel.Clear();
                            AppData.ConfigModel.Clear();
                            AppData.Content.Clear();
                            AppContext.AppContext.HelpPageInstance = new HelpPage();
                            if (AppData.MainModel.CurrentUser.Environment != OnSiteEnvironments.NotSpecified)
                                AppData.Content.SaveUser();

                            if (Device.OS == TargetPlatform.iOS)
                            {
                                PageNavigation.PushMainPage(SplitView.Instace());
                            }
                            else
                            {
                                PageNavigation.PushMainPage(SplitView.Instace());
                            }
                            if (AppContext.AppContext.IsForLockScreen)
                            {
                                try
                                {
                                    AppContext.AppContext.IsForLockScreen = false;
                                    //SplitView.Instace().Navigation.PopModalAsync();
                                    Application.Current.MainPage.Navigation.PopModalAsync();

                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }
                            SplitView.MapView.ClearPin();
                            AppContext.AppContext.LstCustomPin?.Clear();
                            // SplitView.Instace().Navigation.PushModalAsync(new HelpPage());
                            SplitView.Instace().FilterCheckAvailable(false);
                            break;
                        case LoginActions.Existing:
                            //If the app left idle with the DisplayAlert on the screen, LoginPage is not poped from the stacklist, 
                            //so in this case we are removing the loginpage from the stacklist. 
                            //Modified by Praveen on 17/04/2017
                            if (Device.OS == TargetPlatform.iOS)
                            {
                                var log = PageNavigation.GetStack().Where(x => x.GetType() == typeof(LoginPage));
                                if (log.Contains(AppContext.AppContext.LoginPage))
                                {
                                    PageNavigation.PopMainPage();
                                    AppContext.AppContext.IsForLockScreen = false; //Modified due to, DisplayAlert which is shown in locked case, retains the same 
                                                                                   //evenafter app is unlocked.  By Praveen                                                                                                                   
                                }
                                else
                                {
                                    PageNavigation.PushMainPage(SplitView.Instace());
                                }
                            }
                            else
                            {                 
                                if (!AppContext.AppContext.IsForLockScreen)
                                    PageNavigation.PushMainPage(SplitView.Instace());
                                // AppContext.AppContext.IsForLockScreen = true;
                            }
                            //Application.Current.MainPage = SplitView.Instace();;
                            //AppData.Content.LoadCaseData();
                            AppData.Content.LoadCaseData();

                            if (AppData.MainModel.CurrentUser.Environment != OnSiteEnvironments.NotSpecified)
                                AppData.Content.SaveUser();
                            break;
                        case LoginActions.Unlocking:
                            // If the user is the same but they have changed environment then the views must
                            // reset to the main map view so there is no cross over of data between environments.

                            if (Device.OS == TargetPlatform.Android)
                            {

                                if (AppContext.AppContext.IsForLockScreen)
                                {
                                    try
                                    {
                                        AppContext.AppContext.IsForLockScreen = false;
                                        //SplitView.Instace().Navigation.PopModalAsync();
                                        Application.Current.MainPage.Navigation.PopModalAsync();
                                        PageNavigation._mainPageStack.Clear();
                                        PageNavigation.PushMainPage(SplitView.Instace());
                                    }
                                    catch (Exception ex)
                                    {
                                        // ignored
                                    }
                                }
                                SplitView.HubMaster.VwSyncStatus.UpdateDateTime();
                            }
                            else
                            {
                                
                                if (AppContext.AppContext.IsForLockScreen)
                                {
                                    if (Device.OS == TargetPlatform.iOS)
                                        PageNavigation.PopMainPage();
                                    else
                                        Application.Current.MainPage.Navigation.PopModalAsync();
                                        //SplitView.Instace().Navigation.PopModalAsync();

                                    AppContext.AppContext.IsForLockScreen = false;
                                }
                               
                                SplitView.HubMaster.VwSyncStatus.UpdateDateTime();
                            }

                            break;
                        default:
                            throw new Exception("Login action not specified.");
                    }


                    AppData.MainModel.CurrentUser.LoginAction = LoginActions.Unlocking;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }

        }


        private void DrawScreen()
        {
            try
            {
                if (PopupContent == null)
                {
                    //first parameter will take any View 
                    //second parameter a reference to the current content window
                    //third parameter optional parent scroll view if you want relative popups to show relative to scroll position
                    PopupContent = new PopupLayouts(Content, this, PageScroll);
                }
                if (Device.OS != TargetPlatform.iOS)
                    Content = PopupContent;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }


        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetUserDetails
        /// 
        /// <summary>	Makes a call to the authentication service for the user details of the recently
        /// 			authenticated user.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async void GetUserDetails()
        {
            try
            {
                var client = new ServiceClient();
                client.AddAuthHeader(_mOUser.Token);
                await AuthUserDetailsResponse(await client.DownloadStringAsync(string.Format("{0}/uaa/user", AppData.ActiveEnvironments[AppData.Environment].APIHost)));
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }


        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		LoadConfig
        /// 
        /// <summary>	Loads config data from the device.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public async Task<bool> LoadConfig()
        {
            try
            {
                var response = await FileSystem.ReadTextAsync("configdata.txt");
                if (response.Error == null)
                {
                    Exception error;
                    var data = OnSiteConfigCache.FromJson(response.TextContents, out error);
                    if (error == null && data != null)
                    {
                        AppData.ConfigModel.Add(data.Configs, true);
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return false;
            }
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name		LoadAuthenticationLogin
        /// 
        /// <summary>	Loads the login page for the authentication service.
        /// </summary>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void LoadAuthenticationLogin(string host)
        {
            try
            {
                _mSAuthUrl = string.Format("{0}/uaa/oauth/authorize", host);
                _mSRedirectUrl = string.Format("{0}/uaa/success", host);
                _mSCompleteUrl = string.Format("{0}?response_type=token&client_id={1}&redirect_uri={2}&grant_type=implicit",
                              _mSAuthUrl, "SRMobile", _mSRedirectUrl);

                DependencyService.Get<IWebManager>().ClearCookies(_mSCompleteUrl);

                WebView_Credentials.Source = _mSCompleteUrl;
                WebView_Credentials.PropertyChanged += WebViewCredentials_PropertyChanged;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }


        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                DrawScreen();
                //if (!Gl_PinView.IsVisible)
                //  Txt_DemoUsername.Focus();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }


        /// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		PinEntry
		/// 
		/// <summary>	Enters the next digit in the pin number.
		/// </summary>
		/// <param name="number">		The digit being entered.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		private void PinEntry(string number)
        {
            try
            {
                if (_mSPin.Length < 4)
                {
                    _mSPin = string.Format("{0}{1}", _mSPin, number);
                    //
                    RefreshPinView();
                    if (_mSPin.Length == 4)
                        ValidatePin();
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private void PropertyUpdated(object sender, PropertyUpdatedEventArgs e)
        {
            try
            {
                if (e.PropertyType == PropertyType.User)
                {
                    CheckUserSatus();
                    if (AppData.MainModel.CurrentUser.Environment != OnSiteEnvironments.NotSpecified)
                        AppData.Content.SaveUser();
                }
                else if (e.PropertyType == PropertyType.Environment)
                {
                    UpdateEnvironmentName();
                    if (AppData.Environment.ToString() != "Sales")
                    {
                        Sl_DemoCredentials.IsVisible = false;

                        if (Reachability.InternetConnectionStatus() != ReachabilityNetworkStatus.NotReachable)
                        {
                            WebView_Credentials.IsVisible = true;
                            Sl_NoNetwork.IsVisible = false;
                            _mShowPin = true;
                            LoadAuthenticationLogin(AppData.ActiveEnvironments[AppData.Environment].APIHost);
                        }
                        else
                        {
                            WebView_Credentials.IsVisible = false;
                            Sl_NoNetwork.IsVisible = true;
                        }
                    }
                    else
                    {
                        WebView_Credentials.IsVisible = false;
                        Sl_DemoCredentials.IsVisible = true;
                        Sl_NoNetwork.IsVisible = false;
                    }
                }
                else if (e.PropertyType == PropertyType.CacheLoaded)
                {
                    if(!AppData.LastActivity.HasValue)
                    {
                        SplitView.HubMaster.Filter_List();
                        SplitView.Instace().FilterCheckAvailable(true);
                    }
                    SplitView.HubMaster.VwSyncStatus.UpdateDateTime();
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ReAuthenticateTheUser
        /// 
        /// <summary>	Sets the current user as no valid and forces the login screen to reappear.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void ReAuthenticateTheUser()
        {
            try
            {
                AppData.MainModel.CurrentUser.IsValidated = false;
                ShowPinView(false);
                //To change the environment load correctly after ReAuth
                AppData.Environment = AppData.Environment;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		PopupContentClosed
        /// 
        /// 
        /// <summary>	To trigger an event when popup is closed.
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void PopupContentClosed(bool changed)
        {
            try
            {
                OnSoftKeyboardAppearance();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		OnSoftKeyboardAppearance
        /// 
        /// 
        /// <summary>	To adjust the login page as per the appearance of soft keyboard.
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        /// 

        private void OnSoftKeyboardAppearance()
        {
            try
            {

                if (Reachability.InternetConnectionStatus() != ReachabilityNetworkStatus.NotReachable &&
                    Device.OS != TargetPlatform.Windows && Device.OS != TargetPlatform.WinPhone
                    && !PopupContent.PopupVisible)
                {

                    if (_isMiddle)
                    {
                        if (InitialAnimate)
                        {
                            Gl_Auth.TranslateTo(0, -(Height / 3.5), 2000, Easing.Linear);
                            //Moving to Top                            
                        }
                        else
                        {
                            Gl_Auth.TranslateTo(0, -(Height / 3.5), 500, Easing.Linear);
                            //Moving to Top                                
                        }
                        //AppContext.AppContext.ShowInput?.Invoke();
                        _isMiddle = false;
                    }
                }
            }

            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		RefreshPinView
        /// 
        /// <summary>	Refreshes the pin input counter.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void RefreshPinView()
        {
            try
            {
                if (_mSPin.Length <= 4)
                {
                    if (_mSPin.Length != 0)
                        Btn_Delete.IsVisible = true;
                    else
                        Btn_Delete.IsVisible = false;
                    if (_mSPin.Length == 1)
                        Img_1.Source = m_oDigit;
                    else if (_mSPin.Length == 2)
                        Img_2.Source = m_oDigit;
                    else if (_mSPin.Length == 3)
                        Img_3.Source = m_oDigit;
                    else if (_mSPin.Length == 4)
                        Img_4.Source = m_oDigit;
                    else
                    {
                        Img_1.Source = m_oNoDigit;
                        Img_2.Source = m_oNoDigit;
                        Img_3.Source = m_oNoDigit;
                        Img_4.Source = m_oNoDigit;
                    }
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private void RefreshDeletePinView()
        {
            try
            {
                if (_mSPin.Length <= 4)
                {
                    if (_mSPin.Length != 0)
                        Btn_Delete.IsVisible = true;
                    else
                        Btn_Delete.IsVisible = false;
                    if (_mSPin.Length == 4)
                        Img_4.Source = m_oNoDigit;
                    else if (_mSPin.Length == 3)
                        Img_3.Source = m_oNoDigit;
                    else if (_mSPin.Length == 2)
                        Img_2.Source = m_oNoDigit;
                    else if (_mSPin.Length == 1)
                        Img_1.Source = m_oNoDigit;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Logout
        /// 
        /// <summary/>	Handles a click on the logout button, reseting the pin for the current user and
        /// 			presenting the username and password field entries.
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void Logout()
        {
            try
            {
                AppData.MainModel.CurrentUser.IsLoggedout = true;
                AppData.Content.SaveUser();
                Lbl_CurrentUser.Text = "";
                ShowPinView(false);
                _mOUser = new SRiUser();
                AppData.Environment = AppData.Environment;
                AppData.MainModel.CurrentUser.IsValidated = false;
                AppData.MainModel.CurrentUser.Pin = "";
                SplitView.MapView?.ClearPin();
                AppContext.AppContext.LstCustomPin?.Clear();
                AppData.PropertyModel.Sort = SortMode.Date;
                AppData.PropertyModel.Filter = FilterMode.All;
                var currentView = SplitView.Instace().GetCurrentView();
                for (int i = 0; i < SplitView._stackNavigationViews.Count(); i++)
                {
                    if (currentView.ToString() != "ServiceRequest.Views.FullMapView")
                    {
                        SplitView.Instace().PopRightContent();
                    }
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ShowPinView
        /// 
        /// <summary>	Displays the Pin view.
        /// </summary>
        /// <param name="animated">			If true will animate the view into place.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void ShowPinView(bool animated)
        {
            try
            {
                _mSPin = "";
                RefreshPinView();
                Lbl_CurrentUser.Text = _mOUser.IdoxId;
                Gl_Auth.IsVisible = !animated;
                Gl_PinView.IsVisible = animated;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }

            //if (animated)
            //{
            //    Img_Logo.TranslateTo(0, 0, 1);
            //    Lbl_Version.TranslateTo(0, 0, 1);
            //    IsMiddle = true;
            //}
        }


        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		UpdateEnvironmentName
        /// 
        /// 
        /// <summary>	Updates the name displayed for the current environment.
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void UpdateEnvironmentName()
        {
            try
            {
                switch (AppData.Environment)
                {
                    case OnSiteEnvironments.Dev:
                        Lbl_Environment.Text = "Dev";
                        break;
                    case OnSiteEnvironments.Production:
                        Lbl_Environment.Text = "Live";
                        break;
                    case OnSiteEnvironments.QA:
                        Lbl_Environment.Text = "QA";
                        break;
                    case OnSiteEnvironments.Sales:
                        Lbl_Environment.Text = "Demo";
                        Txt_DemoUsername.Text = Txt_DemoPassword.Text = string.Empty;
                        break;
                    case OnSiteEnvironments.Staging:
                        Lbl_Environment.Text = "Test";
                        break;
                    default:
                        Lbl_Environment.Text = "Set Environment";
                        break;
                }
            }
            catch
                (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }


        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ValidatePin
        /// 
        /// <summary>	Validates the pin entry.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void ValidatePin()
        {
            try
            {
                _mOUser.IsLoggedout = false;
                if (_mOUser.Pin.Length == 0)
                {
                    _mOUser.IsValidated = true;
                    _mOUser.Pin = _mSPin;
                    AppData.MainModel.CurrentUser = _mOUser;
                }
                else if (_mOUser.Pin.Equals(_mSPin))
                {
                    _mOUser.IsValidated = true;
                    Poplockscreen();
                    AppData.MainModel.CurrentUser = _mOUser;
                    AppData.Environment = _mOUser.Environment;
                }
                else
                {
                    DisplayAlert("Incorrect Pin", "The pin entered is invalid, please try again or log out to reset your pin.", "OK");
                }
                _mSPin = "";
                RefreshPinView();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }


        /// ------------------------------------------------------------------------------------------------
        /// Name		WebViewCredentials_PropertyChanged
        /// 
        /// <summary>	Handles the OnSourceChanged event from the WebView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">			The response arguments.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        private void WebViewCredentials_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "Source")
                {
                    OnSourceChanged(WebView_Credentials.Source, AuthCompleted);
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		OnSourceChanged
        /// 
        /// <summary/>	
        /// 
        /// <summary>	Listens to the OnSourceChanged event of the WebView. Traps the url of the request
        /// 			to check if the url matches the redirect url for authentication completion.
        /// </summary>
        /// <param name="source">		The web view.</param>
        /// <param name="autoCompleted"></param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        ///         
        private void OnSourceChanged(WebViewSource source, Action<string> autoCompleted)
        {
            try
            {
                var url = (UrlWebViewSource)source;
                _mOAuthComplete = autoCompleted;
                if (url.Url.StartsWith(_mSRedirectUrl) && _mOAuthComplete != null & _mShowPin)
                {
                    FragmentSource = url.Url;
                    var startIndex = FragmentSource.IndexOf('#') + 1;
                    var length = FragmentSource.Length;
                    FragmentSource = FragmentSource.Substring(startIndex, (length - startIndex));
                    _mOAuthComplete(FragmentSource);
                    if (url.Url.StartsWith(_mSRedirectUrl))
                        _mShowPin = false;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
    }
}
#endregion
/// ------------------------------------------------------------------------------------------------