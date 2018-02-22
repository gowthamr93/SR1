using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Idox.LGDP.Apps.ServiceRequest.Client;
using Plugin.Geolocator;
using ServiceRequest.Custom;
using ServiceRequest.DependencyInterfaces;
using ServiceRequest.Pages;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using ServiceRequest.AppContext;
using GeoLocation = Plugin.Geolocator.Abstractions;
using Geocoder = Xamarin.Forms.GoogleMaps.Geocoder;
using ServiceRequest.Views.PopUp;
using Idox.LGDP.Apps.Common.OnSite;

namespace ServiceRequest.Views
{
    public partial class FullMapView
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public variables
        public static bool IsLocationVisible;
        public Plugin.Geolocator.Abstractions.Position _posistion;
        public static Action<bool> ShowHideAdd;
        public static AddNewCaseView NewCaseAddView { get; set; }
        public static AddNewCasePointOnMapView AddNewCasePointOnMapView { get; set; }
        public static GeoLocation.Position UserPosition { get; set; }
        public static IMapView PointOnMapView { get; set; }
        #endregion
        /// ------------------------------------------------------------------------------------------------
        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        public FullMapView(IMapView mapview)
        {
            try
            {
                InitializeComponent();
                if (Device.OS == TargetPlatform.Android)
                {
                    Img_Location.IsVisible = false;
                    GlMapView.Children.Add((AndroidMapView)mapview, 0, 0);
                }
                else
                    GlMapView.Children.Add((WindowsMapView)mapview, 0, 0);


                TapGestureRecognizer tapLocation = new TapGestureRecognizer();
                tapLocation.Tapped += Tap_Location_Tapped;
                Img_Location.GestureRecognizers.Add(tapLocation);

                GlMapView.Children.Add(Img_Location, 0, 0);

                var NewServiceRfequest = new TapGestureRecognizer();
                NewServiceRfequest.Tapped += Nrequest;
                Lbl_NewRequest.GestureRecognizers.Add(NewServiceRfequest);
                BxVw_NewRequest.GestureRecognizers.Add(NewServiceRfequest);

                //var tapImgAdd = new TapGestureRecognizer();
                //            tapImgAdd.Tapped += OnImgAddTapped;
                //            Img_Add.GestureRecognizers.Add(tapImgAdd);
                // Img_Add.IsVisible = AppData.ConfigModel.IsNotEmpty;
                //   ShowHideAdd += (change) => ShowHidePlus(change);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
        private async void Nrequest(object sender, EventArgs e)
        {
            try
            {
                if (AppData.SyncInProgress)
                    await SplitView.DisplayAlert("Sync In Progress", "Please wait for the sync to finish before taking further actions.", "OK", null);
                else if (!AppData.ConfigModel.IsNotEmpty)
                    await SplitView.DisplayAlert("No data", "Please download the data to proceed", "OK", null);
                else
                {
                    UserPosition = await SplitView.Fullmapview.GetCurrentLocation();
                    if (Reachability.InternetConnectionStatus() == ReachabilityNetworkStatus.NotReachable)
                    {
                        await SplitView.Instace().PushRightContent(NewCaseAddView = new AddNewCaseView());
                    }
                    else
                    {
                        if (Device.OS == TargetPlatform.Android)
                            PointOnMapView = new AndroidMapView();
                        else
                            PointOnMapView = new WindowsMapView();

                        await SplitView.Instace().PushRightContent(AddNewCasePointOnMapView = new AddNewCasePointOnMapView(PointOnMapView, UserPosition));
                    }
                    if (Device.OS != TargetPlatform.Android)
                    {
                        var t = Task.Run(async delegate
                        {
                            await Task.Delay(TimeSpan.FromSeconds(1));
                            AddNewCasePointOnMapView.MoveToPosition?.Invoke();
                            return 42;
                        });
                    }
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

        #region Get Current Location

        public async Task<Plugin.Geolocator.Abstractions.Position> GetCurrentLocation()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                //locator.AllowsBackgroundUpdates = true;
                //locator.DesiredAccuracy = 50;
                _posistion = await locator.GetPositionAsync(1000);
            }

            catch (Plugin.Geolocator.Abstractions.GeolocationException geoException)
            {
                AppContext.AppContext.CurrentLocation = null;
                if (geoException.Message.Contains("Unavailable"))
                {
                    await SplitView.DisplayAlert(" GPS Disabled ", " Please enable the GPS to access your current location ", "OK", null);
                }
                if (geoException.Message.Contains("Unauthorized"))
                    await SplitView.DisplayAlert("Get Device Location", " Please grant permission for this application to access your current location ", "OK", null);
                _posistion = null;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                AppContext.AppContext.CurrentLocation = null;
            }
            return _posistion;
        }

        #endregion



        #region Tapped the location image

        private async void Tap_Location_Tapped(object sender, EventArgs e)
        {
            try
            {
                await ShowUserLocation();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        public async Task ShowUserLocation()
        {
            try
            {
                if (AppContext.AppContext.CurrentLocation == null)
                {
                    AppContext.AppContext.CurrentLocation = new CustomPin
                    {
                        Pin = new Pin
                        {
                            Address = "Current Location",
                            Label = "",
                        },
                        ImageUrl = "locCurrentPin.png"
                    };
                }


                if (!IsLocationVisible)
                {
                    await GetCurrentLocation();
                    if (AppContext.AppContext.CurrentLocation != null)
                    {
                        AppContext.AppContext.CurrentLocation.Pin.Position = new Position(_posistion.Latitude, _posistion.Longitude);
                        AppContext.AppContext.LstCustomPin.Remove(AppContext.AppContext.CurrentLocation);
                        AppContext.AppContext.LstCustomPin.Add(AppContext.AppContext.CurrentLocation);
                        if (Device.OS == TargetPlatform.Android)
                        {
                            var formsMap = (AndroidMapView)SplitView.MapView;
                            SplitView.Fullmapview.FindByName<Grid>("GlMapView").Children.Clear();
                            SplitView.Fullmapview.FindByName<Grid>("GlMapView").Children.Add(formsMap, 0, 0);
                        }
                        SplitView.MapView.LoadPins(AppContext.AppContext.LstCustomPin);
                        SplitView.MapView.MoveToRegion(AppContext.AppContext.CurrentLocation.Pin.Position,
                            Distance.FromKilometers(0.3));
                    }
                    IsLocationVisible = true;
                }
                else
                {
                    IsLocationVisible = false;
                    SplitView.MapView.ClearPin();
                    AppContext.AppContext.LstCustomPin.Remove(AppContext.AppContext.CurrentLocation);
                    if (AppContext.AppContext.LstCustomPin.Count > 0)
                    {
                        if (Device.OS == TargetPlatform.Android)
                        {
                            var formsMap = (AndroidMapView)SplitView.MapView;
                            SplitView.Fullmapview.FindByName<Grid>("GlMapView").Children.Clear();
                            SplitView.Fullmapview.FindByName<Grid>("GlMapView").Children.Add(formsMap, 0, 0);
                        }
                        SplitView.MapView.LoadPins(AppContext.AppContext.LstCustomPin);
                    }
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        #endregion

        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
