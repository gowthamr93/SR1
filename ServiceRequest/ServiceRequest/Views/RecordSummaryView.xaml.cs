using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Idox.LGDP.Apps.Common.OnSite;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.Pages;
using ServiceRequest.ViewModels;
using ServiceRequest.Views.CaseListViewControl;
using ServiceRequest.Views.PopUp;
using Xamarin.Forms;
using ServiceRequest.AppContext;

namespace ServiceRequest.Views
{
    public partial class RecordSummaryView : ContentView
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public variables and properties

        private CreateRecordList _recordSummary;
        private List<CreateVisitsList> _lstVisitsList;
        private CreateCustomerList _customerLists;
        private CreateVisitsList _visitsData;

        public AddNewCaseView UpdateNewCase { get; set; }
        public static Action CheckMatchaddres;
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region public constructor
        public RecordSummaryView()
        {
            try
            {
                InitializeComponent();

                Lbl_TitleName.Text = AppData.PropertyModel.SelectedRecord.Record.Record.RefVal;
                OnLoad();
                TapGestures();
                //Option must not be available for new case added
                if (AppData.PropertyModel.SelectedProperty.Status == SyncStatus.New)
                    Img_Add.IsVisible = Boxvw_Add.IsVisible = false;

                //CheckMatchaddres += CheckMatchaddress;
                //if (Reachability.InternetConnectionStatus() != ReachabilityNetworkStatus.NotReachable &&
                //    !AppData.PropertyModel.SelectedProperty.HasValidCoords && AppData.PropertyModel.SelectedProperty.Latitude == null && AppData.PropertyModel.SelectedProperty.Longitude == null)
                //{
                //    Lbl_MatchAddress.IsVisible = true;
                //}
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }


        #endregion
        /// 

        /// ------------------------------------------------------------------------------------------------
        #region Private Functions
        /// ------------------------------------------------------------------------------------------------
        /// <summary>
        /// Handles all Tap events in this page
        /// </summary>
        private void TapGestures()
        {
			try
			{
				//
				var tapback = new TapGestureRecognizer();
				tapback.Tapped += BackImageTapped;
				Lbl_CaseName.GestureRecognizers.Add(tapback);
				//
				var onAddTapped = new TapGestureRecognizer();
				Img_Add.GestureRecognizers.Add(onAddTapped);
				Boxvw_Add.GestureRecognizers.Add(onAddTapped);
				onAddTapped.Tapped += (s, e) =>
				{
					Task.Delay(300);
					SplitView.PopupContent.ShowPopupRelative(new AddVisitPopupView(SplitView.PopupContent), Img_Add, 150, 100, true, "");
				};
				//
				//Match Address
				//var tapMatchAddress = new TapGestureRecognizer();
				//tapMatchAddress.Tapped += OnMatchAddressTapped;
				//Lbl_MatchAddress.GestureRecognizers.Add(tapMatchAddress);
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());
			}
        }
        /// ------------------------------------------------------------------------------------------------
        /// <summary>
        /// Load the Record details
        /// </summary>
        private void OnLoad()
        {
            try
            {
                int visitcount = -1;
                //To Load the Record Summary Details 
                _recordSummary = new CreateRecordList();
                if (_recordSummary.Details.Count > 0)
                {
                    var recordSummaryStackLayout = new RecordSummaryStackLayout(_recordSummary);
                    S_RecordSummary.Children.Add(recordSummaryStackLayout);
                }
                //To load the Visits Details
                _lstVisitsList = new List<CreateVisitsList>();
                if (AppData.PropertyModel.SelectedRecord.Record.Record.Inspections.Count > 0)
                {
                    visitcount = AppData.PropertyModel.SelectedRecord.Record.Record.Inspections[0].Visits.Count;
                    for (int j = 0; j < visitcount; j++)
                    {
                        var data = AppData.PropertyModel.SelectedRecord.Record.Record.Inspections[0].Visits[j];
                        _visitsData = new CreateVisitsList()
                        {
                            VisitTypeDescription = data.VisitTypeDescription,
                            ScheduleDate =
                                data.Visit.DateScheduled.ToString("dd MMM yyyy", "Scheduled for ", "Not scheduled"),
                            CompletedDate = data.Visit.DateVisit.ToString("dd MMM yyyy", "Completed on ", "Outstanding"),
                            VisitListIndex = j
                        };
                        _lstVisitsList.Add(_visitsData);
                    }
                }
                if (_lstVisitsList.Count > 0)
                {
                    var recordSummaryStackLayout = new VisitsView(_lstVisitsList);
                    S_RecordSummary.Children.Add(recordSummaryStackLayout);
                }
                else
                {
                    var recordSummaryStackLayout = new VisitsView(_lstVisitsList);
                    S_RecordSummary.Children.Add(recordSummaryStackLayout);
                    //if visit has no value need to show add visit
                }
                //To Load the Customer Details
                _customerLists = new CreateCustomerList();
                if (_customerLists.Details.Count > 0 || (AppData.PropertyModel.SelectedProperty.Status == SyncStatus.New))
                {
                    var recordSummaryStackLayout = new RecordSummaryStackLayout(_customerLists);
                    S_RecordSummary.Children.Add(recordSummaryStackLayout);
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        //private void CheckMatchaddress()
        //{
        //    try
        //    {
        //        if (Reachability.InternetConnectionStatus() != ReachabilityNetworkStatus.NotReachable &&
        //                !AppData.PropertyModel.SelectedProperty.HasValidCoords && AppData.PropertyModel.SelectedProperty.Latitude == null && AppData.PropertyModel.SelectedProperty.Longitude == null)
        //        {
        //            Lbl_MatchAddress.IsVisible = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogTracking.LogTrace(ex.ToString());
        //    }
        //} 

        private async void BackImageTapped(object sender, EventArgs e)
        {
            try
            {
                View currentView;
                var recordsLst = HubMasterView.CaseListView.MEntries[CaseListControl.SectionIndex]?.View.FindByName<ListView>("Lstvw_Inspections");
                if (recordsLst != null) recordsLst.SelectedItem = null;
                do
                {
                    await SplitView.Instace().PopRightContent();
                    InspectionCellView.DeSelectItem();

                    if (Device.OS == TargetPlatform.Android)
                    {
                        if (AppData.PropertyModel.SelectedProperty.HasValidCoords)
                        {
                            SplitView.PropertySummary.FindByName<Grid>("Gl_CaseSummary")
                                .Children.Add((AndroidMapView) SplitView.MapView, 0, 0);
                            var googleMapPin = MapViewModel.GetCustomePin();
                            if (googleMapPin != null)
                            {
                                SplitView.MapView?.LoadPin(googleMapPin);
                            }

                            AndroidMapView.HideLocator();
                        }
                    }

                    //await SplitView.Instace().PushRightContent(new PropertySummary());
                    currentView = SplitView.Instace().GetCurrentView();
                } while (currentView.ToString() != "ServiceRequest.Views.PropertySummary");
                AppData.PropertyModel.SelectedRecord = null;

            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
        //private async void OnMatchAddressTapped(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (Reachability.InternetConnectionStatus() != ReachabilityNetworkStatus.NotReachable)
        //        {
        //            //API Call to Match Address
        //            await AppData.API.UtilityByAddresses(AppData.PropertyModel.SelectedProperty.Address.RawAddress);

        //            if (AddNewCaseView.UtilityAddressesByAdd == null) return;
        //            if (AddNewCaseView.UtilityAddressesByAdd.Results > 0)
        //            {
        //                //Open Exist Add Case View
        //                await SplitView.Instace().PushRightContent(UpdateNewCase = new AddNewCaseView());
        //            }
        //            else
        //                await SplitView.Instace().DisplayAlert("Invalid Address Selected", "No Data Retrieved. Please try another location", "Ok");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogTracking.LogTrace(ex.ToString());
        //    }
        //}
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
