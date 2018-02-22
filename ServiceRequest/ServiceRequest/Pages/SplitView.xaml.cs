using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Idox.LGDP.Apps.Common.OnSite;
using ServiceRequest.DependencyInterfaces;
using PopUpSample;
using ServiceRequest.AppContext;
using ServiceRequest.Views;
using ServiceRequest.Views.PopUp;
using Xamarin.Forms;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.ViewModels;

namespace ServiceRequest.Pages
{
    public partial class SplitView : ContentPage
    {
        /// ------------------------------------------------------------------------------------------------        
        #region Private Variables and Properties

        private readonly Image _imgMenu;
        private readonly Image _imgFilter;
        private static SplitView _instance;
        public static Stack<View> _stackNavigationViews;
        private ScrollView PageScroll { get; set; }
        public static FullMapView Fullmapview { get; set; }
        private FilterView FilterContent { get; set; }

        #endregion
        /// ------------------------------------------------------------------------------------------------    

        /// ------------------------------------------------------------------------------------------------
        #region Public Variables and Properties

        public static IMapView MapView { get; private set; }
        public  static PropertySummary PropertySummary { get; set; }
        public static HubMasterView HubMaster;
        public static InspectionCountView InspectionCount { get; private set; }
        public static PopupLayouts PopupContent { get; private set; }
        public static PopupLayouts CenterPopupContent { get; private set; }
        public static Action DisplayLockScreen;

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Constructor
        /// ------------------------------------------------------------------------------------------------        
        ///        

        /// <summary>
        /// Private Constructor for Singleton implementation
        /// </summary>
        private SplitView()
        {
            try
            {
                InitializeComponent();
                UpdateStatus();

                _stackNavigationViews = new Stack<View>();
                DisplayLockScreen += ShowLockScreen;
                _imgMenu = new Image
                {
                    Source = "menu.png",
                    Aspect = Aspect.AspectFill,
                    HeightRequest = 25,
                    WidthRequest = 25,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };
                _imgFilter = new Image
                {
                    Source = "filter.png",
                    IsVisible = true,
                    HeightRequest = 25,
                    WidthRequest = 25,
                    Aspect = Aspect.AspectFit,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };
                //FilterCheckAvailable(false);
                AppData.PropertyModel.PropertyUpdated += PropertyUpdated;
                AppData.ConfigModel.PropertyUpdated += PropertyUpdated;
                switch (Device.OS)
                {
                    case TargetPlatform.Android:
                        Lbl_ConnectionStatus.FontSize = 12;
                        Lbl_ConnectionType.FontSize = 12;
                        break;
                    case TargetPlatform.Windows:
                        Lbl_ConnectionStatus.FontSize = 11;
                        Lbl_ConnectionType.FontSize = 11;
                        break;
                    case TargetPlatform.iOS:
                        Lbl_ConnectionStatus.FontSize = 11;
                        Lbl_ConnectionType.FontSize = 11;
                        break;
                }
                AppContext.AppContext.isexecute = true;

            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }




        #endregion
        /// ------------------------------------------------------------------------------------------------               

        /// ------------------------------------------------------------------------------------------------
        #region Public Functions
        /// ------------------------------------------------------------------------------------------------        
        /// 

        /// <summary>
        /// Initialize the instance of Splitview page
        /// </summary>
        /// <returns>Current Instance of the splitview page</returns>
        /// 
        public static SplitView Instace()
        {
            try
            {
                if (_instance == null)
                {
                    _instance = new SplitView();
                    if (Device.OS == TargetPlatform.Android)
                        MapView = new AndroidMapView();
                    else
                        MapView = new WindowsMapView();
                    Fullmapview = new FullMapView(MapView);

                    FullMapView();
                    _instance.PushInspectionCountView(InspectionCount = new InspectionCountView());

                    _instance.PushLeftContent(HubMaster = new HubMasterView());
                }

                NavigationPage.SetHasNavigationBar(_instance, false);

                return _instance;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }


        public new static async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            return await LockScreen.ToDisplayAlert(SplitView.Instace(), title, message, accept, cancel);
        }

        public new async Task<string> DisplayActionSheet(string title, string cancel, string destruction, string[] button)
        {
            return await LockScreen.ToDisplayActionSheet(SplitView.Instace(), title, cancel, destruction, button);
        }



        /// <summary>
        /// Get the top view of PageStack
        /// </summary>
        /// <returns>Returns CurrentView</returns>
        public View GetCurrentView()
        {
            try
            {
                if (_stackNavigationViews.Count > 0)
                    return _stackNavigationViews.Peek();
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }


        /// <summary>
        /// Remove the current top most View in the stack or Child position
        /// </summary>
        /// <returns>Task for async</returns>
        public async Task PopRightContent()
        {
            try
            {
                var currentView = Instace().GetCurrentView();
                if (currentView.ToString() != "ServiceRequest.Views.FullMapView")
                {
                    Rl_Layout.Children.Remove(currentView);
                    _stackNavigationViews.Pop();
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        /// <summary>
        /// Clear all the Views in PageStack
        /// </summary>
        /// <returns>Result</returns>
        public async Task<bool> Clear()
        {
            try
            {
                var count = _stackNavigationViews.Count;
                for (int i = 0; i < count; i++)
                    await _instance.PopRightContent();
                return true;
            }
            catch (Exception ex)
            {
                //throw 
                Debug.WriteLine(ex.Message);
                LogTracking.LogTrace(ex.ToString());
                return false;
            }

        }

        /// <summary>
        /// Push the ContentView at leftside of SplitView
        /// </summary>
        /// <param name="view">The page need to display at leftside passed as view</param>
        /// <returns>Task for async</returns>
        public async Task PushRightContent(View view)
        {
            try
            {
                var currentView = GetCurrentView();
                if (currentView != view || currentView == null)
                {
                    _stackNavigationViews.Push(view);
                    currentView = GetCurrentView();
                }


                Rl_Layout.Children.Add(currentView,

                        Constraint.RelativeToParent((p) => currentView.X),
                        Constraint.RelativeToParent((p) => currentView.Y),
                        Constraint.RelativeToParent((p) => p.Width),
                        Constraint.RelativeToParent((p) => p.Height)
                    );
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// <summary>
        /// To Enable and Disable the Filter
        /// </summary>
        /// <param name="filterShow"></param>
        public void FilterCheckAvailable(bool filterShow)
        {
            try
            {
                if (!filterShow)
                {
                    _imgFilter.IsVisible = false;
                    Boxvw_Filter.IsVisible = false;
                }
                else
                {
                    _imgFilter.IsVisible = true;
                    Boxvw_Filter.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }



        /// <summary>
        /// Event of OnAppering to sync 
        /// </summary>
        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                DrawScreen();
                if (AppContext.AppContext.HelpPageInstance != null)
                {
                    ShowHelpScreen();
                    AppContext.AppContext.HelpPageInstance = null;
                }
                //Fullmapview.ShowHidePlus(true);

            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }



        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		UpdateStatus
        /// <summary>
        /// Updates the current network status of the application.
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        ///

        public void UpdateStatus()
        {
            try
            {
                switch (Reachability.InternetConnectionStatus())
                {
                    case ReachabilityNetworkStatus.NotReachable:
                        SetOffLine();
                        break;
                    case ReachabilityNetworkStatus.ReachableViaCarrierDataNetwork:
                        Lbl_ConnectionType.Text = "Data Connection";
                        SetOnline();
                        break;
                    case ReachabilityNetworkStatus.ReachableViaWiFiNetwork:
                        Lbl_ConnectionType.Text = "Wifi";
                        SetOnline();
                        break;
                }
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
        /// Name		TapGestures
        /// 
        /// <summary>	It contains the Tapgesture recognisers for the whole Page.
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        private  void TapGestures()
        {
            try
            {
                TapGestureRecognizer tapFilter = new TapGestureRecognizer();
                tapFilter.Tapped += (s, e) =>
                {
                    if (AppContext.AppContext.NewRecordInProgress)
                        DisplayAlert("New Record creation is in progress", "Please save or cancel new sevice record to proceed", "OK", null);
                    else
                        OnFilterTapped();
                };
                _imgFilter.GestureRecognizers.Add(tapFilter);
                Boxvw_Filter.GestureRecognizers.Add(tapFilter);
                TapGestureRecognizer tapMenu = new TapGestureRecognizer();
                tapMenu.Tapped += (s, e) =>
                {
                    Task.Delay(200);
                    PopupContent.ShowPopupRelative(new MenuView(PopupContent), _imgMenu, 250, 160, true, "");
                };
                _imgMenu.GestureRecognizers.Add(tapMenu);
                Boxvw_Menu.GestureRecognizers.Add(tapMenu);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		DrawScreen
        /// <summary>
        /// Initializes a new instance of the PopupLayouts
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        ///
        private void DrawScreen()
        {
            try
            {
                if (PopupContent == null)
                {
                    PopupContent = new PopupLayouts(Content, Instace(), PageScroll);
                    if (Device.OS != TargetPlatform.iOS)
                        Content = PopupContent;
                }

                if (CenterPopupContent == null)
                {
                    CenterPopupContent = new PopupLayouts(Content, Instace(), PageScroll);
                    if (Device.OS != TargetPlatform.iOS)
                        Content = CenterPopupContent;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// <summary>
        /// Displays the Lock screen if the app is idle.
        /// </summary>
        private void ShowLockScreen()
        {
            try
            {
                AppData.MainModel.CurrentUser.IsValidated = false;
                AppContext.AppContext.IsForLockScreen = true;
                AppContext.AppContext.LoginPage = new LoginPage();
                if (Device.OS != TargetPlatform.iOS)
                    Application.Current.MainPage.Navigation.PushModalAsync(AppContext.AppContext.LoginPage);
                else
                {
                    PageNavigation.PushMainPage(AppContext.AppContext.LoginPage);
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// <summary>
        /// Pushes the FullMap content into navigation stack
        /// </summary>
        private static async void FullMapView()
        {
            try
            {
                await _instance.PushRightContent(Fullmapview);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		PushLeftContent
        /// <summary>
        /// It pushes the hub master view to the Split View(Left Content).
        /// </summary>
        /// <param name="view"></param>
        /// ------------------------------------------------------------------------------------------------
        ///
        private void PushLeftContent(View view)
        {
            try
            {
                Gl_Parent.Children.Add(view, 0, 1);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		PushInspectionCountView
        /// <summary>
        /// It pushes the inspection count to the Split View.
        /// </summary>
        /// <param name="contentView"></param>
        /// ------------------------------------------------------------------------------------------------
        ///
        private void PushInspectionCountView(View contentView)
        {
            try
            {
                Gl_ParentSub.Children.Add(_imgMenu, 0, 0);
                Gl_ParentSub.Children.Add(contentView, 1, 0);
                Gl_ParentSub.Children.Add(_imgFilter, 2, 0);
                TapGestures();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		OnFilterTapped
        /// <summary>
        /// It is used to display the Filter Pop-up
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        ///
        private async void OnFilterTapped()
        {
            try
            {
                if (AppData.SyncInProgress)
                    await DisplayAlert("Sync In Progress", "Please wait for the sync to finish before taking further actions.", "OK", null);
                else
                {
                    Task.Delay(800);
                    PopupContent.ShowPopupRelative(FilterContent = new FilterView(PopupContent), _imgFilter, Device.OnPlatform<double>(280, 320, 300), 260 + (AppData.PropertyModel.FilterTypes.Take(3).ToList().Count * 40), true, "");
                }              
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SetOffLine
        /// <summary>
        /// To update offline mode to the current network status.
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        ///
        private void SetOffLine()
        {
            try
            {
                Lbl_ConnectionStatus.Text = "Offline";
                Lbl_ConnectionType.TextColor = Styles.Online;
                Lbl_ConnectionStatus.TextColor = Styles.Offline;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SetOnline
        /// <summary>
        /// To update Online mode to the current network status.
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        ///
        private void SetOnline()
        {
            try
            {
                Lbl_ConnectionStatus.Text = "Online";
                Lbl_ConnectionStatus.TextColor = Styles.Online;
                Lbl_ConnectionType.TextColor = Styles.CellHighlight;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

		private async void PropertyUpdated(object sender, PropertyUpdatedEventArgs e)
        {
            try
            {
                switch (e.PropertyType)
                {
                    case PropertyType.Config:
                        AppData.Content.SaveConfig();
                        break;
                    case PropertyType.PropertyData:
                        AppData.Content.SaveCaseData();
                        InspectionCount?.Update();
                        break;
                    case PropertyType.PropertyList:
						await HubMaster.ReloadPropertyData();
                        InspectionCount?.Update();
                        break;
                    case PropertyType.PropertyFilters:
                        // HubMaster.Filter_List();
                        InspectionCount?.Update();
                        break;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }

        }

        private async void SaveAction()
        {
            try
            {
                Dictionary<string, string> cache;
                OnSiteSettings.ShouldSerialize = ShouldSerializeRule.None;
                cache = AppData.PropertyModel.Cache;
                var write = await FileSystem.WriteAsync(cache[AppData.VISITS], AppData.VISITS);
                if (write.Error != null)
                    await DisplayAlert("Failed to save Visit", write.Error.Message, "OK");
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }


        void ShowHelpScreen()
        {
            SplitView.Instace().Navigation.PushModalAsync(AppContext.AppContext.HelpPageInstance);
        }


        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
