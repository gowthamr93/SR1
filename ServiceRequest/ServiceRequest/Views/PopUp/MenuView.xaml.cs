using System;
using ServiceRequest.Pages;
using Xamarin.Forms;
using ServiceRequest.AppContext;
using PopUpSample;
using Idox.LGDP.Apps.ServiceRequest.Client;
using System.Linq;

namespace ServiceRequest.Views.PopUp
{
    public partial class MenuView
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables 
        /// ------------------------------------------------------------------------------------------------
        /// 

        private TapGestureRecognizer Tap_Back, Tap_LogOut, Tap_Help, Tap_About, Tap_ClrContent;
        private PopupLayouts parentPopup;
        private bool _isExecute;

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        /// ------------------------------------------------------------------------------------------------
        /// 

        public MenuView(PopupLayouts parentPopup)
        {
            try
            {
                InitializeComponent();
                BackgroundColor = Styles.GetColour("StatusBlue");
                this.parentPopup = parentPopup;
                TapGestures();
                _isExecute = true;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Private Functions
        /// ------------------------------------------------------------------------------------------------
        /// 

        private void TapGestures()
        {
            try
            {
                //Logout Tapped

                Tap_LogOut = new TapGestureRecognizer();
                Boxvw_LogOut.GestureRecognizers.Add(Tap_LogOut);
                Lbl_LogOut.GestureRecognizers.Add(Tap_LogOut);
                Tap_LogOut.Tapped += async (s, args) =>
                {
                    if (AppData.Environment.ToString() != "Sales")
                    {
                        parentPopup.DismisPopup();
                        if (Reachability.InternetConnectionStatus() !=
                            ReachabilityNetworkStatus.NotReachable)
                        {

                            if (await SplitView.DisplayAlert("Log Out", "Do you want to Log Out ?", "YES", "NO"))
                            {
                                //On Logout or ReAuthentication when new request is in progress the new request operation must cancel
                                AppContext.AppContext.NewRecordInProgress = false;
                                LoginPage.LogOut();
                                Application.Current.MainPage = new LoginPage();
                            }
                        }
                        else
                            await
                                SplitView.Instace()
                                    .DisplayAlert("No Network Connection",
                                        "Please ensure the device has a network connection before logging out entirely, re-authentication or authenticating a new user requires network access.",
                                        "OK");
                    }
                    else
                    {
                        parentPopup.DismisPopup();
                        if (await SplitView.DisplayAlert("Log Out", "Do you want to Log Out ?", "YES", "NO"))
                        {
                            //On Logout when new request is on progress the new request operation must cancel
                            AppContext.AppContext.NewRecordInProgress = false;
                            LoginPage.LogOut();
                            Application.Current.MainPage = new LoginPage();
                        }
                    }

                    //parentPopup.DismisPopup();

                };
                //Help Tapped
                Tap_Help = new TapGestureRecognizer();
                Boxvw_Help.GestureRecognizers.Add(Tap_Help);
                Lbl_Help.GestureRecognizers.Add(Tap_Help);
                Tap_Help.Tapped += async (s, args) =>
                {
                    if (_isExecute)
                    {
                        _isExecute = false;
                        parentPopup.DismisPopup();
                        if (!SplitView.Instace().Navigation.ModalStack.Contains(AppContext.AppContext.HelpPageInstance))
                            await SplitView.Instace().Navigation.PushModalAsync(AppContext.AppContext.HelpPageInstance = new HelpPage(), true);
                        _isExecute = true;
                    }
                };

                //About Tapped
                Tap_About = new TapGestureRecognizer();
                Boxvw_About.GestureRecognizers.Add(Tap_About);
                Lbl_About.GestureRecognizers.Add(Tap_About);
                Tap_About.Tapped += (s, args) =>
                {
                    parentPopup.DismisPopup();
                    SplitView.Instace().Navigation.PushModalAsync(new AboutPage(), true);

                };
                Tap_ClrContent = new TapGestureRecognizer();
                Boxvw_ClrContent.GestureRecognizers.Add(Tap_ClrContent);
                Lbl_ClrContent.GestureRecognizers.Add(Tap_ClrContent);
                Tap_ClrContent.Tapped += async (s, args) =>
                {
                    parentPopup.DismisPopup();
                    if (AppContext.AppContext.NewRecordInProgress)
                    {
                        SplitView.DisplayAlert("New Record creation is in progress", "Please save or cancel new sevice record to proceed", "OK", null);
                        return;
                    }
                    else
                    {
                        if (await SplitView.DisplayAlert("Clear Content",
                                    "Are you sure you want to remove all content from the app?\r\nAll unsaved changes will be lost.", "Yes", "No"))
                        {
                            AppData.PropertyModel.Clear();
                            SplitView.Instace().FilterCheckAvailable(false);
                            AppData.PropertyModel.Sort = SortMode.Date;
                            AppData.PropertyModel.Filter = FilterMode.All;
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
