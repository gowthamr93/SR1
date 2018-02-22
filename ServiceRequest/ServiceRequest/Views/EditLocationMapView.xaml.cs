using ServiceRequest.DependencyInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GeoLocation = Plugin.Geolocator.Abstractions;
using Xamarin.Forms;
using XamarinMaps = Xamarin.Forms.Maps;
using GoogleMap = Xamarin.Forms.GoogleMaps;
using ServiceRequest.Pages;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.Custom;
using ServiceRequest.AppContext;

namespace ServiceRequest.Views
{
    public partial class EditLocationMapView : ContentView
    {
        public GeoLocation.Position _casePosition { get; set; }
        public IMapView _mapview { get; set; }
        private XamarinMaps.Position UK_XFPosition = new XamarinMaps.Position(55.378051, -3.435973);
        private GoogleMap.Position UK_GMPosition = new GoogleMap.Position(55.378051, -3.435973);
        private readonly Action _slowmethod;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private CancellationToken _token;
        private double SelectedLatitude { get; set; }
        private double SelectedLongitude { get; set; }
        public EditLocationMapView(IMapView mapview, GeoLocation.Position casePosition)
        {
            try
            {
                InitializeComponent();
                AppContext.AppContext.NewRecordInProgress = true;
                _mapview = mapview;
                _casePosition = casePosition;
                Tapgestures();
                if (Device.OS == TargetPlatform.Android)
                {
                    var formsMap = (AndroidMapView)SplitView.MapView;
                    GlMapView.Children.Clear();
                    GlMapView.Children.Add(formsMap, 0, 0);
                    SplitView.MapView?.ClearPin();
                    MoveMapPosition();
                    AndroidMapView.HideLocator();
                }
                else
                {
                    var formsMap = (WindowsMapView)mapview;
                    GlMapView.Children.Add(formsMap, 0, 0);
                }
                if (Device.OS != TargetPlatform.Android)
                {
                    var t = Task.Run(async delegate
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1));
                        MoveMapPosition();
                        return 42;
                    });
                }
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
                if (currentPage.GetType() == typeof(EditLocationMapView))
                    await SplitView.DisplayAlert("Slow Connection", "If your map is taking a while to load it could be due to a slow internet connection. \nPlease click 'Cancel' in the top left of the screen to set the location later or wait for the map to appear.", "Ok", null);
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
        }

        private async void OnCancelTapped(object sender, EventArgs e)
        {
            try
            {
                await CloseView();
                if (Device.OS == TargetPlatform.Android)
                {
                    await SplitView.Instace().PushRightContent(FullMapView.NewCaseAddView = new AddNewCaseView());
                    //  AppContext.AppContext.LoadMapPin(AppContext.AppContext.SelectedNewLatitude, AppContext.AppContext.SelectedNewLongitude);
                }
                _cts.Cancel();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        public void MoveMapPosition()
        {
            try
            {
                double ZoomOnData = 1;
                double ZoomOffData = 300;
                double? latitude = null;
                double? longitude = null;

                if (AppContext.AppContext.SelectedNewLatitude != 0 && AppContext.AppContext.SelectedNewLongitude != 0)
                {
                    latitude = AppContext.AppContext.SelectedNewLatitude;
                    longitude = AppContext.AppContext.SelectedNewLongitude;
                }
                else if (FullMapView.NewCaseAddView.SelectedLatitude != 0 &&
                    FullMapView.NewCaseAddView.SelectedLongitude != 0)
                {
                    latitude = FullMapView.NewCaseAddView.SelectedLatitude;
                    longitude = FullMapView.NewCaseAddView.SelectedLongitude;
                }
                else if (AppData.PropertyModel.SelectedProperty != null)
                {
                    latitude = AppData.PropertyModel.SelectedProperty.Latitude.Value;
                    longitude = AppData.PropertyModel.SelectedProperty.Longitude.Value;
                }
                switch (Device.OS)
                {
                    case TargetPlatform.Android:
                        //
                        if (latitude != null && longitude != null)
                        {
                            SplitView.MapView?.LoadPin(new GoogleMap.Pin()
                            {
                                Type = GoogleMap.PinType.Place,
                                Position = new GoogleMap.Position(latitude.Value, longitude.Value),
                                //Address = selectedAddress.FormattedAddress(),
                                Label = AppData.PropertyModel.AlphaIndex(AppData.PropertyModel.SelectedProperty) == "?" ? "A" : AppData.PropertyModel.AlphaIndex(AppData.PropertyModel.SelectedProperty),
                                Icon = GoogleMap.BitmapDescriptorFactory.FromView(new Views.PinView(AppData.PropertyModel.AlphaIndex(AppData.PropertyModel.SelectedProperty) == "?" ? "A" : AppData.PropertyModel.AlphaIndex(AppData.PropertyModel.SelectedProperty)))
                            });
                            SplitView.MapView?.MoveToRegion(new GoogleMap.Position(latitude.Value, longitude.Value), GoogleMap.Distance.FromKilometers(ZoomOnData));
						}
						else if (_casePosition != null)
                            SplitView.MapView?.MoveToRegion(new GoogleMap.Position(_casePosition.Latitude, _casePosition.Longitude), GoogleMap.Distance.FromKilometers(ZoomOnData));
						else if (AppContext.AppContext.LstGooglePin.Count > 0)
                            SplitView.MapView?.MoveToRegion(AppContext.AppContext.LstGooglePin.FirstOrDefault().Position, GoogleMap.Distance.FromKilometers(ZoomOnData));
						else
                            SplitView.MapView?.MoveToRegion(UK_GMPosition, GoogleMap.Distance.FromKilometers(ZoomOffData));
						//
						break;
					case TargetPlatform.Windows:
					case TargetPlatform.iOS:
						//
						if (latitude != null && longitude != null)
						{
							Device.BeginInvokeOnMainThread(() =>
							{
								_mapview?.ClearPin();
								_mapview?.LoadPin(new CustomPin()
								{
									Pin = new XamarinMaps.Pin()
									{
										Position = new XamarinMaps.Position(latitude.Value, longitude.Value),
										Label = AppData.PropertyModel.AlphaIndex(AppData.PropertyModel.SelectedProperty) == "?" ? "A" : AppData.PropertyModel.AlphaIndex(AppData.PropertyModel.SelectedProperty)
									}
								});
								_mapview.MoveToRegion(new XamarinMaps.Position(latitude.Value, longitude.Value), XamarinMaps.Distance.FromKilometers(ZoomOnData));
							});
						}
						else if (_casePosition != null)
							Device.BeginInvokeOnMainThread(() =>
							{
								_mapview.MoveToRegion(new XamarinMaps.Position(_casePosition.Latitude, _casePosition.Longitude), XamarinMaps.Distance.FromKilometers(ZoomOnData));
							});
						else if (AppContext.AppContext.LstCustomPin.Count > 0)
							Device.BeginInvokeOnMainThread(() =>
							{
								_mapview.MoveToRegion(AppContext.AppContext.LstCustomPin.FirstOrDefault().Pin.Position, XamarinMaps.Distance.FromKilometers(ZoomOnData));
							});
						else
							Device.BeginInvokeOnMainThread(() =>
							{
								_mapview.MoveToRegion(UK_XFPosition, XamarinMaps.Distance.FromKilometers(ZoomOffData));
							});
						//
						break;
				}
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
				AddNewCaseView.EditLocInstance = null;
				//AppContext.AppContext.NewRecordInProgress = false;
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
                //API Call
                await AppData.API.UtilityByCoordinates(selectedLatitude, selectedLongitude);
                if (AddNewCaseView.UtilityAddressesByCoord == null) return;
                //Close Map Page
                await CloseView();
                //Open Add Case View
                AppContext.AppContext.SelectedNewLatitude = selectedLatitude;
                AppContext.AppContext.SelectedNewLongitude = selectedLongitude;
                AddNewCaseView.isAddressChanged = true;
                if (Device.OS == TargetPlatform.Android)
                {
                    await SplitView.Instace().PushRightContent(FullMapView.NewCaseAddView = new AddNewCaseView(selectedLatitude, selectedLatitude));
                }
                else
                {
                    FullMapView.NewCaseAddView.UpdateLatLong(selectedLatitude, selectedLongitude);
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
    }
}
