using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.AppContext;
using ServiceRequest.DependencyInterfaces;
using ServiceRequest.Pages;
using ServiceRequest.Views.PopUp;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace ServiceRequest.Views
{
    public partial class AndroidMapView : IMapView
    {
        /// ------------------------------------------------------------------------------------------------
        #region publicVariables

        public static Action HideLocator;
        public static Action ShowLocator;
        public static Action ChangeMapType;

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public constructor
        public AndroidMapView()
        {
            try
            {
                InitializeComponent();
                googleMap.MoveToRegion(new MapSpan(new Position(0, 0), 360, 360));
                googleMap.IsShowingUser = true;
                BtnMap.TextColor = Color.White;
                BtnMap.BackgroundColor = Styles.MainAccent;
                BtnSatellite.TextColor = Styles.MainAccent;
                BtnSatellite.BackgroundColor = Color.White;
                BtnMap.Clicked += HandleClicked;
                BtnSatellite.Clicked += HandleClicked;
                HideLocator += HideGeoLocator;
                ShowLocator += ShowGeoLocator;
                ChangeMapType += OnMapChange;
                OnMapChange();
                googleMap.MapClicked += GoogleMap_MapClicked;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        private async void GoogleMap_MapClicked(object sender, MapClickedEventArgs e)
        {
            try
            {
                var location = e.Point;
                if (FullMapView.AddNewCasePointOnMapView != null && FullMapView.AddNewCasePointOnMapView.NewCaseAddView == null && AppData.PropertyModel.SelectedProperty == null)
                {
                    if (!AppContext.AppContext.LocationSelected)
                    {
                        AppContext.AppContext.LocationSelected = true;
                        if (await SplitView.DisplayAlert("Location Selected", "Do you want to create a new Service Record in this location?", "Yes", "No"))
                        {
                            FullMapView.AddNewCasePointOnMapView.PostionSelected(location.Latitude, location.Longitude);
                        }
                        else
                        {
                            AppContext.AppContext.LocationSelected = false;
                            AppContext.AppContext.MapView?.ClearPin();
                        }
                    }
                }
                else if (AddNewCaseView.EditLocInstance != null)
                {
                    if (!AppContext.AppContext.LocationSelected)
                    {
                        AppContext.AppContext.LocationSelected = true;
                        if (await SplitView.DisplayAlert("Location Selected", "Do you want to move the Service Record to this location?", "Yes", "No"))
                        {
                            AddNewCaseView.EditLocInstance.PostionSelected(location.Latitude, location.Longitude);
                        }
                        else
                        {
                            AppContext.AppContext.LocationSelected = false;
                            AppContext.AppContext.MapView?.ClearPin();
                        }
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Function
        public async void LoadPins<T>(List<T> pinList)
        {
            try
            {
                List<Pin> pins = ((IEnumerable<Pin>)pinList).ToList();
                var lstPins = new List<Pin>();

                if (pins.Count > 0)
                {
                    // customMap.LoadPins?.Invoke(lstPins);
                    var pin = pins.FirstOrDefault();
                    if (pin != null)
                        MoveToRegion(pin.Position, Distance.FromKilometers(0.5));
                }

                foreach (var item in pins)
                {
                    if (lstPins.FirstOrDefault(x => x.Position.Equals(item.Position)) == null)
                    {
                        lstPins.Add(item);
                        await Task.Run(() => SlowMethod(100));

                        if (!googleMap.Pins.Contains(item))
                            googleMap.Pins.Add(item);
                    }
                }
                //Added for IDXSR-260
                if (lstPins.Count <= 0)
                    googleMap.MoveToRegion(new MapSpan(new Position(0, 0), 360, 360));

            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public bool ClearPin()
        {
            try
            {
                googleMap.Pins.Clear();
                return true;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return false;
            }

        }

        public void MoveToRegion(object position, object distance)
        {
            try
            {
                googleMap.MoveToRegion(MapSpan.FromCenterAndRadius((Position)position, (Distance)distance));
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        public void DestroyMapView()
        {
            
        }

        public void LoadPin(object mapPin)
        {
            try
            {
                var pin = (Pin)mapPin;
                ClearPin();

                if (pin != null)
                {
                    googleMap.Pins.Add(pin);
                    //MoveToRegion(pin.Position, Distance.FromKilometers(0.5));
                    var Lat_pos = pin.Position.Latitude - 00.000991;
                    var Lon_pos = pin.Position.Longitude;
                    googleMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(Lat_pos, Lon_pos), Distance.FromKilometers(0.2)));
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                System.Diagnostics.Debug.WriteLine("Exception {0}", ex.Message);
            }
        }

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Private Function
        private void HandleClicked(object sender, EventArgs e)
        {
            try
            {
                var obj = sender as Button;
                if (obj != null)
                {
                    LoadMapType(obj.Text);
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        private void LoadMapType(string type)
        {
            try
            {
                switch (type)
                {
                    case "Map":
                        googleMap.MapType = MapType.Street;
                        BtnMap.TextColor = Color.White;
                        BtnMap.BackgroundColor = Styles.MainAccent;
                        BtnSatellite.TextColor = Styles.MainAccent;
                        BtnSatellite.BackgroundColor = Color.White;


                        break;
                    case "Satellite":
                        googleMap.MapType = MapType.Hybrid;
                        BtnSatellite.TextColor = Color.White;
                        BtnSatellite.BackgroundColor = Styles.MainAccent;
                        BtnMap.TextColor = Styles.MainAccent;
                        BtnMap.BackgroundColor = Color.White;
                        break;
                }
                AppContext.AppContext.MapType = type;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        private void HideGeoLocator()
        {
            try
            {
                googleMap.IsShowingUser = false;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
        private void ShowGeoLocator()
        {
            try
            {
                googleMap.IsShowingUser = true;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        private void OnMapChange()
        {
            try
            {
                if (AppContext.AppContext.MapType != null)
                {
                    LoadMapType(AppContext.AppContext.MapType);
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }





        private async Task SlowMethod(int watingTime)
        {
            try
            {
                Task.Run(() => Task.Delay(watingTime)).Wait();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

		public void DisposeMap()
		{
			
		}

		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}
