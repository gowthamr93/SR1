using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using Windows.UI.Core;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.Custom;
using ServiceRequest.DependencyInterfaces;
using ServiceRequest.Pages;
using ServiceRequest.ViewModels;
using ServiceRequest.Views.PopUp;
using Xamarin.Forms;
using XamarinMaps = Xamarin.Forms.Maps;
using GoogleMap = Xamarin.Forms.GoogleMaps;
using GeoLocation = Plugin.Geolocator.Abstractions;
using Idox.LGDP.Apps.Common.OnSite;
using Plugin.Geolocator;
using ServiceRequest.AppContext;

namespace ServiceRequest.Views
{
    public partial class AddNewCasePointOnMapView : ContentView
    {
        private readonly IMapView _mapview;
        private readonly Action _slowmethod;
        private readonly GeoLocation.Position _casePosition;
        private XamarinMaps.Position UK_XFPosition = new XamarinMaps.Position(53.800651, -4.064941);
        private GoogleMap.Position UK_GMPosition = new GoogleMap.Position(53.800651, -4.064941);
        public AddNewCaseView NewCaseAddView { get; set; }
        public static Action MoveToPosition;
        public Plugin.Geolocator.Abstractions.Position _posistion;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private CancellationToken _token;
        public AddNewCasePointOnMapView(IMapView mapview, GeoLocation.Position casePosition)
        {
            try
            {
                _mapview = mapview;
                _casePosition = casePosition;
                InitializeComponent();
                AppContext.AppContext.NewRecordInProgress = true;
                Tapgestures();
                if (Device.OS == TargetPlatform.Android)
                {
                    SplitView.MapView?.ClearPin();
                    MoveMapPosition();
                    AndroidMapView.HideLocator();
                    var googleMap = (AndroidMapView)SplitView.MapView;
                    GlMapView.Children.Clear();
                    GlMapView.Children.Add(googleMap, 0, 0);
                }
                else
                {
                    var formsMap = (WindowsMapView)mapview;
                    GlMapView.Children.Add(formsMap, 0, 0);
                }
                MoveToPosition += MoveMapPosition;
                _slowmethod = SlowConnectionAlert;
                _token = CancellationToken.None;
                SlowConnection();

            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private void SlowConnection()
        {
            try
            {
                var uiSyncContext = TaskScheduler.FromCurrentSynchronizationContext();
                Task.Run(async () =>
                {
                    _token = _cts.Token;
                    await Task.Delay(TimeSpan.FromSeconds(30));
                    if (!_token.IsCancellationRequested)
                        await Task.Factory.StartNew(() => { _slowmethod.Invoke(); }, _token, TaskCreationOptions.PreferFairness, uiSyncContext);
                }, _token);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }

        }


        private async void SlowConnectionAlert()
        {
            try
            {
                var currentPage = SplitView.Instace().GetCurrentView();
                if (currentPage.GetType() == typeof(AddNewCasePointOnMapView))
                    await SplitView.DisplayAlert("Slow Connection", "If your map is taking a while to load it could be due to a slow internet connection. \nPlease click 'Skip' in the bottom right of the screen to set the location later or wait for the map to appear.", "Ok", null);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }

        }

        private void Tapgestures()
        {
            var tapCancel = new TapGestureRecognizer();
            tapCancel.Tapped += OnCancelTapped;
            LblCancel.GestureRecognizers.Add(tapCancel);
            BxVw_Cancel.GestureRecognizers.Add(tapCancel);
            BtnSkipLocation.Clicked += SkipLocationClicked;
        }

        private async void SkipLocationClicked(object sender, EventArgs e)
        {
            try
            {
                await CloseView();
                await SplitView.Instace().PushRightContent(FullMapView.NewCaseAddView = new AddNewCaseView());
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private void MoveMapPosition()
        {
            try
            {
                double ZoomOnData = 1;
                double ZoomOffData = 400;
                switch (Device.OS)
                {
                    case TargetPlatform.Android:
                        //
                        if (_casePosition != null)
                            SplitView.MapView.MoveToRegion(new GoogleMap.Position(_casePosition.Latitude, _casePosition.Longitude), GoogleMap.Distance.FromKilometers(ZoomOnData));
                        else if (AppContext.AppContext.LstGooglePin.Count > 0)
                            SplitView.MapView.MoveToRegion(AppContext.AppContext.LstGooglePin.FirstOrDefault().Position, GoogleMap.Distance.FromKilometers(ZoomOnData));
                        else
                            SplitView.MapView.MoveToRegion(UK_GMPosition, GoogleMap.Distance.FromKilometers(ZoomOffData));
                        //
                        break;
                    case TargetPlatform.Windows:
                    case TargetPlatform.iOS:
                        //
                        if (_casePosition != null)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                _mapview?.ClearPin();
                                _mapview?.LoadPin(new CustomPin()
                                {
                                    ImageUrl = "locCurrentPin.png",
                                    Pin = new XamarinMaps.Pin()
                                    {
                                        //To show the Location icon
                                        Address = "Current Location",
                                        Position = new XamarinMaps.Position(_casePosition.Latitude, _casePosition.Longitude),
                                        Label = "",
                                        //
                                    }

                                });
                                _mapview?.MoveToRegion(new XamarinMaps.Position(_casePosition.Latitude, _casePosition.Longitude), XamarinMaps.Distance.FromKilometers(ZoomOnData));
                            });

                            //Device.BeginInvokeOnMainThread(() =>
                            //	{
                            //		_mapview?.ClearPin();
                            //		AppContext.AppContext.CurrentLocation = new CustomPin
                            //		{
                            //		ImageUrl = "locCurrentPin.png",
                            //			Pin = new XamarinMaps.Pin()
                            //			{
                            //				Position = new XamarinMaps.Position(_casePosition.Latitude, _casePosition.Longitude),
                            //				Label = "",
                            //			}

                            //		};
                            //		_mapview.MoveToRegion(new XamarinMaps.Position(_casePosition.Latitude, _casePosition.Longitude), XamarinMaps.Distance.FromKilometers(ZoomOnData));
                            //	});

                        }
                        else if (AppContext.AppContext.LstCustomPin.Count > 0)
                            _mapview.MoveToRegion(AppContext.AppContext.LstCustomPin.FirstOrDefault().Pin.Position, XamarinMaps.Distance.FromKilometers(ZoomOnData));
                        else
                            _mapview.MoveToRegion(UK_XFPosition, XamarinMaps.Distance.FromKilometers(ZoomOffData));
                        //
                        break;
                        //case TargetPlatform.iOS:
                        //
                        //if (_casePosition != null)
                        //    _mapview.MoveToRegion(new XamarinMaps.Position(_casePosition.Latitude, _casePosition.Longitude), XamarinMaps.Distance.FromKilometers(ZoomOnData));
                        // else if (AppContext.AppContext.LstCustomPin.Count > 0)
                        //     _mapview.MoveToRegion(AppContext.AppContext.LstCustomPin.FirstOrDefault().Pin.Position, XamarinMaps.Distance.FromKilometers(ZoomOnData));
                        // else
                        //    _mapview.MoveToRegion(UK_XFPosition, XamarinMaps.Distance.FromKilometers(ZoomOffData));
                        //
                        // break;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private async void OnCancelTapped(object sender, EventArgs e)
        {
            try
            {
                await CloseView();
                if (Device.OS == TargetPlatform.Android)
                {
                    AndroidMapView.ShowLocator();
                    var formsMap = (AndroidMapView)SplitView.MapView;
                    SplitView.Fullmapview.FindByName<Grid>("GlMapView").Children.Add(formsMap, 0, 0);
                    SplitView.MapView.LoadPins(AppContext.AppContext.LstGooglePin);
                }
                _cts.Cancel();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private async Task CloseView()
        {
            try
            {
                await SplitView.Instace().PopRightContent();
				_mapview.DisposeMap();
				GC.Collect(0, GCCollectionMode.Forced);
                AddVisitPopupView.AddNewCasePointOnMapView = null;
                FullMapView.AddNewCasePointOnMapView = null;
                AppContext.AppContext.NewRecordInProgress = false;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        public async void PostionSelected(double selectedLatitude, double selectedLongitude)
        {
            try
            {
                if (AppData.MainModel.Environment == OnSiteEnvironments.Sales)
                {
                    if (await SplitView.DisplayAlert("Demo Mode", "For security reasons in demo mode the address is not checked. When live you will be able to set to a validated addresses", "Ok", null))
                    {
                        await CloseView();
                        AppContext.AppContext.SelectedNewLatitude = selectedLatitude;
                        AppContext.AppContext.SelectedNewLongitude = selectedLongitude;
                        await SplitView.Instace().PushRightContent(FullMapView.NewCaseAddView = new AddNewCaseView(selectedLatitude, selectedLatitude));
                        if (Device.OS == TargetPlatform.Android)
                            FullMapView.NewCaseAddView.UpdateLatLong(selectedLatitude, selectedLongitude);
                    }
                }
                else
                {
                    //API Call
                    await AppData.API.UtilityByCoordinates(selectedLatitude, selectedLongitude);
                    if (AddNewCaseView.UtilityAddressesByCoord == null) return;
                    //Close Map Page
                    await CloseView();
                    AppContext.AppContext.SelectedNewLatitude = selectedLatitude;
                    AppContext.AppContext.SelectedNewLongitude = selectedLongitude;
                    //Open Add Case View
                    await SplitView.Instace().PushRightContent(FullMapView.NewCaseAddView = new AddNewCaseView(selectedLatitude, selectedLongitude));
                    //if (Device.OS == TargetPlatform.Android)
                    //    AppContext.AppContext.LoadMapPin(selectedLatitude, selectedLongitude);
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
    }
}
