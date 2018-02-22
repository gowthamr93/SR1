using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Idox.LGDP.Apps.Common.OnSite;
using Idox.LGDP.Apps.ServiceRequest.Client;
using Idox.LGDP.Apps.ServiceRequest.Client.Models;
using PopUpSample;
using ServiceRequest.AppContext;
using ServiceRequest.Custom;
using ServiceRequest.DependencyInterfaces;
using ServiceRequest.Models;
using ServiceRequest.Pages;
using ServiceRequest.ViewModels;
using ServiceRequest.Views.CaseListViewControl;
using ServiceRequest.Views.PopUp;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Distance = Xamarin.Forms.Maps.Distance;
using Position = Xamarin.Forms.Maps.Position;

namespace ServiceRequest.Views
{
    public partial class AddNewCaseView : ContentView
    {
        private readonly Plugin.Geolocator.Abstractions.Position _casePosition;
        public static AddNewCaseViewModel NewCaseVm { get; set; }

        private readonly ScrollView _mScrollView;
        private CancellationToken _token;

        public AddNewCaseViewModel UpdateNewCaseVm { get; set; }
        public static SRiUtilityAddresses UtilityAddressesByCoord { get; set; }
        public static SRiUtilityAddresses UtilityAddressesByAdd { get; set; }
        public static SRiUtilityAddresses UtilityAddressesByPost { get; set; }
        public double SelectedLatitude { get; set; }
        public double SelectedLongitude { get; set; }
        public static EditLocationMapView EditLocInstance { get; set; }
        public static bool isAddressChanged { get; set; } = false;
        public static PopupLayouts m_PopupContent { get; set; }
        private ScrollView m_PageScroll { get; set; }
        private static IMapView pinMapView { get; set; }
        public AddNewCaseView(double selectedLat = default(double), double selectedLong = default(double))
        {
            try
            {
                InitializeComponent();
                AppContext.AppContext.NewRecordInProgress = true;
                AppContext.AppContext.LocationSelected = false;
                SelectedLatitude = selectedLat;
                SelectedLongitude = selectedLong;
                Lbl_UPRN.Text = AppData.PropertyModel.SelectedRecord?.Record?.Record?.UPRN != null ? "UPRN Validated address" : "Address not validated, No UPRN";
                EdAddress.Placeholder = "Please type address or use set by\nlocation to get a validated address";
                LoadButton();
                TapGestures();
                LoadMap();

                if (Device.OS == TargetPlatform.iOS)
                {
                    _mScrollView = new ScrollView()
                    {
                        IsClippedToBounds = true,
                        Orientation = ScrollOrientation.Vertical,
                        Content = Gl_Main,
                    };
                    Content = _mScrollView;
                    EdAddress.Text = "Please type address or use set by\nlocation to get a validated address";
                    EdPropertyDetails.TextColor = Color.Gray;
                    EdPropertyDetails.Text = "Please fill the details";
                    EdAddress.TextColor = Color.Gray;
                }
                Update();
                EdPropertyDetails.Focused += FocusedDetails;
                EdAddress.Focused += FocusedAddress;
                EdPropertyDetails.Unfocused += UnFocusedDetails;
                EdAddress.Unfocused += UnFocusedAddress;

            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                throw;
            }
        }

        public void LoadButton()
        {
            try
            {
                if (Reachability.InternetConnectionStatus() != ReachabilityNetworkStatus.NotReachable)
                {
                    if (AppData.MainModel.Environment == OnSiteEnvironments.Sales)
                    {
                        Bt_SearchAddress.IsEnabled = false;
                        Bt_EditLocation.IsEnabled = false;
                        Lbl_Network.Text = "Not Possible in Demo Mode";
                    }
                    else
                    {
                        Lbl_Network.Text = string.Empty;
                        Bt_SearchAddress.IsEnabled = true;
                        Bt_EditLocation.IsEnabled = true;
                    }
                }
                else
                {
                    Bt_SearchAddress.IsEnabled = false;
                    Bt_EditLocation.IsEnabled = false;
                    Lbl_Network.Text = "No Internet";
                    Lbl_Network.TextColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private void TapGestures()
        {
            try
            {
                var tapCancel = new TapGestureRecognizer();
                tapCancel.Tapped += (s, e) => OnCancelTapped();
                LblCancel.GestureRecognizers.Add(tapCancel);
                Boxvw_Cancel.GestureRecognizers.Add(tapCancel);
                var tapSave = new TapGestureRecognizer();
                tapSave.Tapped += async (s, e) =>
                {
                    if (AppContext.AppContext.isexecute)
                    {
                        if (await Validation())
                        {
                            LblSave.IsVisible = false;
                            Boxvw_Save.IsEnabled = false;
                            Boxvw_Save.IsVisible = false;
                            LblSave.IsEnabled = false;
                            var uiSyncContext = TaskScheduler.FromCurrentSynchronizationContext();
                            await Task.Run(async () =>
                            {
                                await Task.Delay(100, _token);
                                await Task.Factory.StartNew(OnSaveTapped, _token, TaskCreationOptions.PreferFairness, uiSyncContext);
                            }, _token);
                        }
                    }
                };
                LblSave.GestureRecognizers.Add(tapSave);
                Boxvw_Save.GestureRecognizers.Add(tapSave);
                Bt_SearchAddress.Clicked += ChooseAddressClicked;
                EdAddress.TextChanged += AddressChanged;
                Bt_EditLocation.Clicked += EditLocationClicked;
            }
            // ReSharper disable once RedundantCatchClause
            catch (Exception)
            {
                throw;
            }
        }

        private void FocusedDetails(object sender, FocusEventArgs e)
        {
            if (EdPropertyDetails.Text == "Please fill the details")
            {
                EdPropertyDetails.Text = "";
            }
            EdPropertyDetails.TextColor = Color.Black;
        }

        private void FocusedAddress(object sender, FocusEventArgs e)
        {
            if (EdAddress.Text == "Please type address or use set by\nlocation to get a validated address")
            {
                EdAddress.Text = "";
            }
            EdAddress.TextColor = Color.Black;
        }

        private void UnFocusedDetails(object sender, FocusEventArgs e)
        {
            if (EdPropertyDetails.Text == "")
            {
                EdPropertyDetails.Text = "Please fill the details";
                EdPropertyDetails.TextColor = Color.Gray;
            }

        }

        private void UnFocusedAddress(object sender, FocusEventArgs e)
        {
            if (EdAddress.Text == "")
            {
                EdAddress.Text = "Please type address or use set by\nlocation to get a validated address";
                EdAddress.TextColor = Color.Gray;
            }

        }

        private async Task OnSaveTapped()
        {
            try
            {
                if (await Validation())
                {
                    LblSave.IsVisible = Boxvw_Save.IsEnabled = LblSave.IsEnabled = false;
                    if (Device.OS == TargetPlatform.Android)
                        await Task.Delay(1000);
                    //
                    if (AppData.PropertyModel.SelectedProperty == null)
                        await SaveNewCase();
                    else
                        await SaveExistCase(VerifyUtilityAddress(EdAddress.Text));
                    //
                    CloseView();
                    SplitView.MapView?.ClearPin();
                    if (Device.OS == TargetPlatform.Android)
                    {
                        var formsMap = (AndroidMapView)SplitView.MapView;
                        SplitView.Fullmapview.FindByName<Grid>("GlMapView").Children.Clear();
                        SplitView.Fullmapview.FindByName<Grid>("GlMapView").Children.Add(formsMap, 0, 0);
                    }
                    SplitView.MapView?.LoadPins(AppContext.AppContext.LstGooglePin);

                    //Set null to addresses list if already loaded
                    UtilityAddressesByCoord = null;
                    UtilityAddressesByAdd = null;
                    UtilityAddressesByPost = null;
                    //
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private async void OnCancelTapped()
        {
            try
            {
                if (
                    await
                        SplitView.DisplayAlert("Cancel", "Are you sure you want to discard changes for this case", "Yes",
                            "No"))
                {
                    CloseView();
                    if (Device.OS == TargetPlatform.Android)
                    {
                        SplitView.MapView?.ClearPin();
                        var pagestack = SplitView._stackNavigationViews;
                        if (pagestack.Count <= 1)
                        {
                            var formsMap = (AndroidMapView)SplitView.MapView;
                            SplitView.Fullmapview.FindByName<Grid>("GlMapView").Children.Clear();
                            SplitView.Fullmapview.FindByName<Grid>("GlMapView").Children.Add(formsMap, 0, 0);
                            SplitView.MapView?.LoadPins(AppContext.AppContext.LstGooglePin);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }


        private async void EditLocationClicked(object sender, EventArgs e)
        {
            try
            {
                if (Device.OS == TargetPlatform.Android)
                    FullMapView.PointOnMapView = new AndroidMapView();
                else
                    FullMapView.PointOnMapView = new WindowsMapView();
                if (Device.OS == TargetPlatform.Android)
                    await SplitView.Instace().PopRightContent();

                await SplitView.Instace().PushRightContent(EditLocInstance = new EditLocationMapView(FullMapView.PointOnMapView, _casePosition));
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        public void UpdateLatLong(double lat, double lon)
        {
            try
            {
                var sRecord = AppData.PropertyModel?.SelectedRecord?.Record?.Record;
                //
                AppContext.AppContext.LocationSelected = false;
                EdAddress.Text = string.Empty;
                if (sRecord != null)
                {
                    sRecord.Latitude = SelectedLatitude = lat;
                    sRecord.Longitude = SelectedLongitude = lon;
                }

                //Set pin by tapped location
                LoadMapPin(lat, lon);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private async void ChooseAddressClicked(object sender, EventArgs e)
        {
            try
            {
                var s_Record = AppData.PropertyModel?.SelectedRecord?.Record?.Record;
                if (UtilityAddressesByCoord?.Addresses == null)
                {
                    if (await SplitView.DisplayAlert("No Address Found", "Please use edit location try again or manually type an address.", "Ok", null))
                    {
                        return;
                    }
                }
                else
                {
                    if (SelectedLatitude != 0 && SelectedLongitude != 0)
                    {
                        SplitView.PopupContent.ShowPopupRelative(new AddressesNearbyView(this, UtilityAddressesByCoord), new BoxView(), Width * 0.7, (GetHeight(UtilityAddressesByCoord.Addresses.Count, AddressesNearbyView.Groupedcount * AddressesNearbyView.CellHeight)), true, " ");
                    }
                    else if (UtilityAddressesByCoord != null && UtilityAddressesByCoord.Results > 0)
                    {
                        SplitView.PopupContent.ShowPopupRelative(new AddressesNearbyView(this, UtilityAddressesByCoord), new BoxView(), Width * 0.7, (GetHeight(UtilityAddressesByCoord.Addresses.Count, AddressesNearbyView.Groupedcount * AddressesNearbyView.CellHeight)), true, " ");
                    }
                    else
                    {
                        //API Call for preserved Lat & long
                        if (s_Record != null)
                        {
                            if (s_Record.Latitude == 0 && s_Record.Longitude == 0)
                            {
                                await SplitView.DisplayAlert("Invalid Location", "Please use edit location try again or manually type an address.", "Ok", null);
                            }
                            else
                                await AppData.API.UtilityByCoordinates(s_Record.Latitude.Value, s_Record.Longitude.Value);
                        }
                        else if (SelectedLatitude != 0 && SelectedLongitude != 0)
                            await AppData.API.UtilityByCoordinates(SelectedLatitude, SelectedLongitude);
                        else
                        {
                            SplitView.PopupContent.ShowPopupRelative(new AddressesNearbyView(this, UtilityAddressesByCoord), new BoxView(), Width * 0.7, (GetHeight(UtilityAddressesByCoord.Addresses.Count, AddressesNearbyView.Groupedcount * AddressesNearbyView.CellHeight)), true, " ");
                        }
                        //
                        if (UtilityAddressesByCoord == null) return;
                        if (UtilityAddressesByCoord.Results > 0)
                        {
                            SplitView.PopupContent.ShowPopupRelative(new AddressesNearbyView(this, UtilityAddressesByCoord), new BoxView(), Width * 0.7, (GetHeight(UtilityAddressesByCoord.Addresses.Count, AddressesNearbyView.Groupedcount * AddressesNearbyView.CellHeight)) + 40, true, " ");
                        }
                    }

                }

                //string[] button = { "Address", "Location", "Cancel" };
                //string option = await SplitView.Instace().DisplayActionSheet("Choose Address By", null, null, button);
                //if (option == "Address")
                //{
                //    if (UtilityAddressesByAdd != null && UtilityAddressesByAdd.Results > 0)
                //    {
                //        SplitView.PopupContent.ShowPopupCenter(new AddressesNearbyView(this, UtilityAddressesByAdd), 0.5, "");
                //    }
                //    else
                //    {
                //        if (!string.IsNullOrEmpty(EdAddress.Text))
                //        {
                //            //API Call for preserved address
                //            await AppData.API.UtilityByAddresses(EdAddress.Text);
                //            //
                //            if (UtilityAddressesByAdd == null) return;
                //            if (UtilityAddressesByAdd.Results > 0)
                //            {
                //                SplitView.PopupContent.ShowPopupCenter(new AddressesNearbyView(this, UtilityAddressesByAdd), 0.5, "");
                //            }
                //        }
                //        else
                //            await SplitView.DisplayAlert("Address not filled", " Please fill the address field to proceed ", "OK", null);
                //    }
                //}
                //else if (option == "Location")
                //{
                //    if (SelectedLatitude != 0 && SelectedLongitude != 0)
                //    {
                //        SplitView.PopupContent.ShowPopupCenter(new AddressesNearbyView(this, UtilityAddressesByCoord), 0.5, "");
                //    }
                //    else if (UtilityAddressesByCoord != null && UtilityAddressesByCoord.Results > 0)
                //    {
                //        SplitView.PopupContent.ShowPopupCenter(new AddressesNearbyView(this, UtilityAddressesByCoord), 0.5, "");
                //    }
                //    else
                //    {
                //        //API Call for preserved Lat & long
                //        if (s_Record != null)
                //            await AppData.API.UtilityByCoordinates(s_Record.Latitude.Value, s_Record.Longitude.Value);
                //        else if (SelectedLatitude != 0 && SelectedLongitude != 0)
                //            await AppData.API.UtilityByCoordinates(SelectedLatitude, SelectedLongitude);
                //        else
                //            SplitView.PopupContent.ShowPopupCenter(new AddressesNearbyView(this, UtilityAddressesByCoord), 0.5, "");
                //        //
                //        if (UtilityAddressesByCoord == null) return;
                //        if (UtilityAddressesByCoord.Results > 0)
                //        {
                //            SplitView.PopupContent.ShowPopupCenter(new AddressesNearbyView(this, UtilityAddressesByCoord), 0.5, "");
                //        }
                //    }
                //}
                //else
                //    return;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private void AddressChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (e.OldTextValue != null)
                {
                    if (UtilityAddressesByCoord != null && VerifyUtilityAddress(EdAddress.Text) != null)
                    {
                        Lbl_UPRN.Text = "UPRN Validated address";
                    }
                    else
                    {
                        Lbl_UPRN.Text = "Address not validated, No UPRN";
                    }
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        public void UpdateFieldAddress(string selectedAddress)
        {
            try
            {
                EdAddress.Text = selectedAddress;
                EdAddress.TextColor= Color.Black;
                Lbl_UPRN.Text = VerifyUtilityAddress(selectedAddress)?.UPRN != null ? "UPRN Validated address" : "Address not validated, No UPRN";
                var address = VerifyUtilityAddress(selectedAddress);
                SelectedLatitude = address.Latitude.Value;
                SelectedLongitude = address.Longitude.Value;
                if (Device.OS == TargetPlatform.Android)
                    AppContext.AppContext.LoadMapPin(address?.Latitude, address?.Longitude);
                else
                    LoadMapPin(address?.Latitude, address?.Longitude);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private void LoadMap()
        {
            try
            {
                if (Device.OS == TargetPlatform.Android)
                {
                    //AppContext.AppContext.MapView = new AndroidMapView();
                    Gl_RadiusPoint.Children.Add((AndroidMapView)SplitView.MapView, 0, 0);
                    //
                    LoadMapPin(SelectedLatitude, SelectedLongitude);
                    //
                    AndroidMapView.HideLocator();
                }
                else
                {
                    pinMapView = new WindowsMapView();
                    Gl_RadiusPoint.Children.Add((WindowsMapView)pinMapView, 0, 0);
                    var t = Task.Run(async delegate
                    {
                        await Task.Delay(TimeSpan.FromSeconds(0.5));
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            //
                            LoadMapPin(SelectedLatitude, SelectedLongitude);
                            //
                        });
                        return 42;
                    });
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private async Task<bool> Validation()
        {
            try
            {
                var result = false;
                if (!string.IsNullOrWhiteSpace(EdAddress.Text) && EdAddress.Text != "Please type address or use set by\nlocation to get a validated address" && EdPropertyDetails.Text != "Please fill the details" && !string.IsNullOrWhiteSpace(EdPropertyDetails.Text))
                {
                    result = true;
                }
                else
                {
                    await SplitView.DisplayAlert("Warning", "Please ensure you have added an address and details prior to saving.", "Ok", null);
                }
                return result;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return false;
            }
        }

        private async void CloseView()
        {
            try
            {
                await SplitView.Instace().PopRightContent();
                //Dispose Map
                pinMapView?.DisposeMap();
                GC.Collect(0, GCCollectionMode.Forced);
                //
                FullMapView.NewCaseAddView = null;
                AppContext.AppContext.NewRecordInProgress = false;
                if (Device.OS == TargetPlatform.Android)
                    AndroidMapView.ShowLocator();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private void Update()
        {
            try
            {
                if (AppData.PropertyModel.SelectedProperty != null)
                {
                    UpdateExistingPickers();
                }
                else
                {
                    NewCaseVm = new AddNewCaseViewModel();
                    UpdatePickers();
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private void UpdatePickers()
        {
            try
            {
                //Received
                RcvdDueDate.Date = DateTime.Now.Date;
                RcvdDueDate.MinimumDate = DateTime.Now.AddDays(-30);
                RcvdDueDate.MaximumDate = DateTime.Now.AddDays(30);
                RcvdDueTime.Time = DateTime.Now.TimeOfDay;

                //Modified for IDXSR-316 on 03.05.2017
                //RequestType
                var reqType = (from x in AppData.ConfigModel.AllRequestTypes(AppData.MainModel.CurrentUser.Organisations.Select(x => x.Organisation.Name).FirstOrDefault())
                               orderby x.Description ascending
                               select x.Description).ToList();
                foreach (var req in reqType)
                {
                    PkrRequestType.Items.Add(req);
                }

                //InspectionType
                var insType = (from x in AppData.ConfigModel.Inspections(AppData.MainModel.CurrentUser.Organisations.Select(x => x.Organisation.Name).FirstOrDefault())
                               orderby x.Description ascending
                               select x.Description).ToList();
                foreach (var ins in insType)
                {
                    PkrInspectionType.Items.Add(ins);
                }

                if (PkrRequestType.Items.Count > 0) PkrRequestType.SelectedIndex = 0;
                if (PkrInspectionType.Items.Count > 0) PkrInspectionType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }

        }

        private void UpdateExistingPickers()
        {
            try
            {
                var selectedRecord = AppData.PropertyModel.SelectedRecord.Record.Record;
                var reqTypeList = (from x in AppData.ConfigModel.AllRequestTypes(AppData.MainModel.CurrentUser.Organisations.Select(x => x.Organisation.Name).FirstOrDefault())
                                   orderby x.Description ascending
                                   select x.Description).ToList();
                var insTypeList = (from x in AppData.ConfigModel.Inspections(AppData.MainModel.CurrentUser.Organisations.Select(x => x.Organisation.Name).FirstOrDefault())
                                   orderby x.Description ascending
                                   select x.Description).ToList();
                //

                var s = AppData.PropertyModel.SelectedRecord;
                //Trade Name
                TxtTradeName.Text = selectedRecord.TradeName;

                //Address
                EdAddress.Text = selectedRecord.Address;
                if (Device.OS == TargetPlatform.iOS)
                {
                    if (EdAddress.Text != "Please type address or use set by\nlocation to get a validated address")
                        EdPropertyDetails.TextColor = Color.Black;
                }

                //RequestType
                foreach (var req in reqTypeList)
                {
                    PkrRequestType.Items.Add(req);
                }

                //InspectionType
                foreach (var ins in insTypeList)
                {
                    PkrInspectionType.Items.Add(ins);
                }

                PkrRequestType.SelectedIndex = PkrRequestType.Items.IndexOf(AppData.ConfigModel.AllRequestTypes(
                    AppData.MainModel.CurrentUser.Organisations.Select(x => x.Organisation.Name).FirstOrDefault())
                    .FirstOrDefault(x => x.Code == selectedRecord.RequestType).Description);

                PkrInspectionType.SelectedIndex = PkrInspectionType.Items.IndexOf(AppData.ConfigModel.Inspections(
                    AppData.MainModel.CurrentUser.Organisations.Select(x => x.Organisation.Name).FirstOrDefault())
                    .FirstOrDefault(x => x.Code == selectedRecord.Inspections[0].InspectionType).Description);

                //Details
                EdPropertyDetails.Text = selectedRecord.Details;
                if (Device.OS == TargetPlatform.iOS)
                {
                    if (EdPropertyDetails.Text != "Please fill the details")
                        EdAddress.TextColor = Color.Black;
                }
                //Received
                RcvdDueDate.Date = selectedRecord.Received.Value.Date;
                RcvdDueDate.MinimumDate = DateTime.Now.AddDays(-30);
                RcvdDueDate.MaximumDate = DateTime.Now.AddDays(30);
                RcvdDueTime.Time = selectedRecord.Received.Value.TimeOfDay;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }


        private async Task SaveNewCase()
        {
            try
            {
                List<SRiProperty> properties;
                SRiUtilityAddress selectedAddress = null;
                //
                SaveNewCaseModel();
                //
                selectedAddress = VerifyUtilityAddress(NewCaseVm.NewCaseAddressValue);
                //
                var newRecordEntityKey = Guid.NewGuid().ToString();
                AppData.PropertyModel.AddNewRecord(new SRiRecordMeta()
                {
                    ID = newRecordEntityKey,
                    Organisation = AppData.MainModel.CurrentUser.Organisations[0].Organisation.Name,
                    Received = "2",
                    Version = "",
                    QueryState = "",
                    EntityMeta = new SRiEntityMeta("entitykey", newRecordEntityKey),
                    Record = new SRiRecord()
                    {
                        EntityKey = newRecordEntityKey,
                        KeyVal = OnSiteSettings.NewTempKey,
                        Address = NewCaseVm.NewCaseAddressValue?.Trim(),
                        Details = NewCaseVm.NewCaseDetails?.Trim(),
                        Latitude = selectedAddress?.Latitude ?? SelectedLatitude,
                        Longitude = selectedAddress?.Longitude ?? SelectedLongitude,
                        UPRN = selectedAddress?.UPRN.ToString(),
                        Received = NewCaseVm.NewCaseReceivedDate + NewCaseVm.NewCaseReceivedTime,
                        RequestType = NewCaseVm.NewCaseRequestTypeText,
                        TradeName = NewCaseVm.NewCaseTradeName?.Trim(),
                        Status = SyncStatus.New,
                        Inspections = new List<SRiInspection>()
                        {
                            new SRiInspection()
                            {
                                KeyVal = Guid.NewGuid().ToString(),
                                InspectionType = NewCaseVm.NewInspectionTypeText,
                                RiskRecords = new List<SRiXRRec>(),
                            }
                        }
                    },
                });
                //
                await SplitView.HubMaster.ReloadPropertyData();
                SplitView.InspectionCount?.Update();
                //
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
            //AppData.PropertyModel.Update(properties, true);
        }

        private void SaveNewCaseModel()
        {
            try
            {
                //Trade Name
                NewCaseVm.NewCaseTradeName = TxtTradeName.Text;
                //Address
                if (NewCaseVm.NewCaseIsOffline)
                    NewCaseVm.NewCaseAddressValue = EdAddress.Text;
                if (NewCaseVm.NewCaseIsOnline)
                {
                    NewCaseVm.NewCaseAddressValue = EdAddress.Text;
                }
                //RequestType
                NewCaseVm.NewCaseRequestTypeText = AppData.ConfigModel.AllRequestTypes(
                    AppData.MainModel.CurrentUser.Organisations.Select(x => x.Organisation.Name).FirstOrDefault())
                    .FirstOrDefault(x => x.Description == PkrRequestType.Items[PkrRequestType.SelectedIndex]).Code;

                NewCaseVm.NewInspectionTypeText = AppData.ConfigModel.Inspections(
                    AppData.MainModel.CurrentUser.Organisations.Select(x => x.Organisation.Name).FirstOrDefault())
                    .FirstOrDefault(x => x.Description == PkrInspectionType.Items[PkrInspectionType.SelectedIndex]).Code;
                //Received Date/Time
                NewCaseVm.NewCaseReceivedDate = RcvdDueDate.Date;
                NewCaseVm.NewCaseReceivedTime = RcvdDueTime.Time;
                //Details
                NewCaseVm.NewCaseDetails = EdPropertyDetails.Text;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private SRiUtilityAddress VerifyUtilityAddress(string caseAddress)
        {
            try
            {
                SRiUtilityAddress selectedAddress = null;
                if (caseAddress == null || UtilityAddressesByCoord?.Addresses == null) return null;
                if (UtilityAddressesByCoord != null)
                {
                    foreach (var address in UtilityAddressesByCoord.Addresses)
                    {
                        if (caseAddress == address.FormattedAddress())
                        {
                            selectedAddress = address;
                            break;
                        }
                    }
                }
                else if (UtilityAddressesByAdd != null)
                {
                    foreach (var address in UtilityAddressesByAdd.Addresses)
                    {
                        if (caseAddress == address.FormattedAddress())
                        {
                            selectedAddress = address;
                            break;
                        }
                    }
                }
                return selectedAddress;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task SaveExistCase(SRiUtilityAddress uaddress)
        {
            try
            {
                Exception err;
                SRiRequestGroup s_RequestGroup = AppData.PropertyModel.SelectedProperty.RequestGroups[0];
                SRiRecordMeta s_Record = AppData.PropertyModel.SelectedRecord.Record;
                //
                s_Record.Record.Address = EdAddress.Text;
                s_Record.Record.UPRN = uaddress?.UPRN.ToString() ?? null;

                s_Record.Record.Latitude = uaddress?.Latitude ?? s_Record.Record.Latitude;
                s_Record.Record.Longitude = uaddress?.Longitude ?? s_Record.Record.Longitude;

                s_Record.Record.TradeName = TxtTradeName.Text;
                s_Record.Record.Received = RcvdDueDate.Date + RcvdDueTime.Time;
                s_Record.Record.RequestType = AppData.ConfigModel.AllRequestTypes(
                    AppData.MainModel.CurrentUser.Organisations.Select(x => x.Organisation.Name).FirstOrDefault())
                    .FirstOrDefault(x => x.Description == PkrRequestType.Items[PkrRequestType.SelectedIndex]).Code;
                s_RequestGroup.Name = s_Record.RequestTypeDescription;
                s_RequestGroup.GroupType = s_Record.Record.RequestType;
                s_Record.Record.Inspections[0].InspectionType = AppData.ConfigModel.Inspections(
                    AppData.MainModel.CurrentUser.Organisations.Select(x => x.Organisation.Name).FirstOrDefault())
                    .FirstOrDefault(x => x.Description == PkrInspectionType.Items[PkrInspectionType.SelectedIndex]).Code;
                s_Record.Record.Details = EdPropertyDetails.Text;
                //
                AppData.PropertyModel.Update(s_Record, s_Record);
                //AppData.PropertyModel.SelectedProperty.UpdateFields();
                await SplitView.HubMaster.ReloadPropertyData();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private void LoadMapPin(double? latitude, double? longitude)
        {
            try
            {
                if (Device.OS == TargetPlatform.Android)
                {
                    if (latitude != null && longitude != null)
                    {
                        SplitView.MapView?.ClearPin();
                        SplitView.MapView?.LoadPin(new Xamarin.Forms.GoogleMaps.Pin()
                        {
                            Type = PinType.Place,
                            Position = new Xamarin.Forms.GoogleMaps.Position(latitude.Value, longitude.Value),
                            Label =
                                AppData.PropertyModel.AlphaIndex(AppData.PropertyModel.SelectedProperty) == "?"
                                    ? "A"
                                    : AppData.PropertyModel.AlphaIndex(AppData.PropertyModel.SelectedProperty),
                            Icon =
                                BitmapDescriptorFactory.FromView(
                                    new Views.PinView(
                                        AppData.PropertyModel.AlphaIndex(AppData.PropertyModel.SelectedProperty) == "?"
                                            ? "A"
                                            : AppData.PropertyModel.AlphaIndex(AppData.PropertyModel.SelectedProperty)))
                        });
                        SplitView.MapView?.MoveToRegion(new Xamarin.Forms.GoogleMaps.Position(latitude.Value - 00.000991, longitude.Value), Xamarin.Forms.GoogleMaps.Distance.FromKilometers(0.3));
                    }
                }
                else
                {
                    if (latitude != null && longitude != null)
                    {
                        pinMapView?.ClearPin();
                        pinMapView?.LoadPin(new CustomPin()
                        {
                            Pin = new Xamarin.Forms.Maps.Pin()
                            {
                                Position = new Position(latitude.Value, longitude.Value),
                                Label = AppData.PropertyModel.AlphaIndex(AppData.PropertyModel.SelectedProperty) == "?" ? "A" : AppData.PropertyModel.AlphaIndex(AppData.PropertyModel.SelectedProperty)
                            }
                        });
                        pinMapView?.MoveToRegion(new Position(latitude.Value, longitude.Value), Distance.FromKilometers(0.2));
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
        private double GetHeight(int count, int groupedCount)
        {
            int minHeight;
            double maxHeight;
            int headerHeight = 40;
            if (count == 1)
                minHeight = (count * AddressesNearbyView.CellHeight + groupedCount) + headerHeight + 80;
            else
                minHeight = count * AddressesNearbyView.CellHeight + groupedCount + headerHeight;//for two counts => 160 = 1*40+40

            if (Device.OS == TargetPlatform.iOS)
                maxHeight = SplitView.Instace().Height * 0.7;
            else
                maxHeight = SplitView.Instace().Height * 0.5;

            return maxHeight < minHeight ? maxHeight : minHeight;

        }

    }
}
