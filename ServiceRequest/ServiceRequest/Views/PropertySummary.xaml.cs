using System;
using Idox.LGDP.Apps.Common.OnSite;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.Pages;
using ServiceRequest.ViewModels;
using ServiceRequest.Views.CaseListViewControl;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using ServiceRequest.AppContext;

namespace ServiceRequest.Views
{
    public partial class PropertySummary
    {
        public AddNewCaseView UpdateNewCase { get; set; }
        public static Action RefreshCount;

        /// ------------------------------------------------------------------------------------------------
        #region Public Constructors
        /// ------------------------------------------------------------------------------------------------
        /// 
        public PropertySummary()
        {
            try
            {
                InitializeComponent();
                AppData.PropertyModel.SelectedProperty = null;
                AppData.PropertyModel.SelectedProperty = AppContext.AppContext.LstSriproperty[CaseListControl.SectionIndex];
                Update();
                TapGestures();
                OnLoad();
                RefreshCount += ShowDetails;
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
        /// ------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adding the Property summary stacklayout on appearing
        /// </summary>
        /// <returns></returns>
        private void OnLoad()
        {
            try
            {
                foreach (var properyDetails in AppData.PropertyModel.SelectedProperty.PropertyDetails)
                {
                    var propertySummaryStackLayout = new PropertySummaryStacklayout(properyDetails);
                    Sl_Summary.Children.Add(propertySummaryStackLayout);
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name		TapGestures
        /// 
        /// <summary>	It contains the Tapgesture recognisers for the whole Page.
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        private void TapGestures()
        {
            try
            {
                //Documents
                var tapDocuments = new TapGestureRecognizer();
                tapDocuments.Tapped += (s, e) => OnDocumentsTapped();
                Boxvw_DocumentBackground.GestureRecognizers.Add(tapDocuments);
                Boxvw_DocumentCountBackground.GestureRecognizers.Add(tapDocuments);
                Lbl_Documents.GestureRecognizers.Add(tapDocuments);
                Lbl_DocumentsCount.GestureRecognizers.Add(tapDocuments);
                Img_Documents.GestureRecognizers.Add(tapDocuments);

                //Property History
                var tapHistory = new TapGestureRecognizer();
                tapHistory.Tapped += (s, e) => OnHistoryTapped();
                Boxvw_PropertyHistoryBackground.GestureRecognizers.Add(tapHistory);
                Boxvw_PropertyHistoryCountBackground.GestureRecognizers.Add(tapHistory);
                Lbl_PropHistory.GestureRecognizers.Add(tapHistory);
                Lbl_PropHistoryCount.GestureRecognizers.Add(tapHistory);
                Img_History.GestureRecognizers.Add(tapHistory);

                //Property Notes
                var tapNotes = new TapGestureRecognizer();
                tapNotes.Tapped += (s, e) => OnNotesTapped();
                Boxvw_PropertyNotesBackground.GestureRecognizers.Add(tapNotes);
                Boxvw_PropertyNotesCountBackground.GestureRecognizers.Add(tapNotes);
                Lbl_PropNotesCount.GestureRecognizers.Add(tapNotes);
                Lbl_Notes.GestureRecognizers.Add(tapNotes);
                Img_Notes.GestureRecognizers.Add(tapNotes);

                ////Match Address
                //var tapMatchAddress = new TapGestureRecognizer();
                //tapMatchAddress.Tapped += OnMatchAddressTapped;
                //Lbl_MatchAddress.GestureRecognizers.Add(tapMatchAddress);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        //private async void OnMatchAddressTapped(object sender, EventArgs e)
        //{
        //    if (Reachability.InternetConnectionStatus() != ReachabilityNetworkStatus.NotReachable)
        //    {
        //        //API Call to Match Address
        //        await AppData.API.UtilityByAddresses(AppData.PropertyModel.SelectedProperty.Address.RawAddress);

        //        if (AddNewCaseView.UtilityAddressesByAdd == null) return;
        //        if (AddNewCaseView.UtilityAddressesByAdd.Results > 0)
        //        {
        //            //Open Exist Add Case View
        //            await SplitView.Instace().PushRightContent(UpdateNewCase = new AddNewCaseView());
        //        }
        //        else
        //            await SplitView.DisplayAlert("Invalid Address Selected", "No Data Retrieved. Please try another location", "Ok",null);
        //    }
        //}

        /// <summary>
        /// Loads the map content on the right side for the specific platform.
        /// </summary>
        /// <returns></returns>
        private void Update()
        {
            try
            {
                if (Device.OS == TargetPlatform.Android)
                {
                    // AppContext.AppContext.MapView = new AndroidMapView();
                    SplitView.MapView?.ClearPin();
                    if (AppData.PropertyModel.SelectedProperty.HasValidCoords)
                    {
                        Gl_CaseSummary.Children.Add((AndroidMapView)SplitView.MapView, 0, 0);
                        AndroidMapView.HideLocator();
                        var googleMapPin = MapViewModel.GetCustomePin();
                        if (googleMapPin != null)
                        {
                            SplitView.MapView?.LoadPin(googleMapPin);
                        }
                    }
                }
                else
                {
                    //AppContext.AppContext.MapView = new WindowsMapView();
	                    if (AppData.PropertyModel.SelectedProperty.HasValidCoords)
                    {
                        var windowMapPin = MapViewModel.GetCurrentPin();
                        if (windowMapPin != null)
                        {
                            AppContext.AppContext.MapView?.MoveToRegion(
                            new Position(windowMapPin.Pin.Position.Latitude, windowMapPin.Pin.Position.Longitude), Distance.FromKilometers(0.3));
                        }
                    }
                    Gl_CaseSummary.Children.Add((WindowsMapView)AppContext.AppContext.MapView, 0, 0);
                }

                ShowDetails();

            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// <summary>
        /// Loads the Binding context values to the view
        /// </summary>
        /// <returns></returns>
        private void ShowDetails()
        {
            try
            {
                BindingContext = new
                {
                    DocumentCount = AppData.PropertyModel.SelectedProperty.DocumentCount,
                    PropertyNotesCount = AppData.PropertyModel.SelectedProperty.PropertyNotesCount,
                    PropertyHistoryCount = AppData.PropertyModel.SelectedProperty.PropertyHistoryCount,
                    Address = AppData.PropertyModel.SelectedProperty.Address,
                };
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        /// <summary>
        /// Executes the OnHistoryTapped event 
        /// </summary>
        /// <returns></returns>
        private async void OnHistoryTapped()
        {
            try
            {
                await SplitView.Instace().PushRightContent(new PropertyHistory());
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        /// <summary>
        /// Executes the OnDocumentsTapped event 
        /// </summary>
        /// <returns></returns>
        private async void OnDocumentsTapped()
        {
            try
            {
                await SplitView.Instace().PushRightContent(new DocumentListView());
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        /// <summary>
        /// Executes the OnNotesTapped event 
        /// </summary>
        /// <returns></returns>
        private async void OnNotesTapped()
        {
            try
            {
                await SplitView.Instace().PushRightContent(new PropertyNotesView());
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        //private void ToggleMatchAddress(bool change)
        //{
        //    //Lbl_MatchAddress.IsVisible = change;
        //}

        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
