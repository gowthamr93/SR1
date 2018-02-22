using System;
using System.Collections.Generic;
using System.Linq;
using ServiceRequest.AppContext;
using ServiceRequest.Custom;
using ServiceRequest.DependencyInterfaces;
using ServiceRequest.Views.PopUp;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using XamarinMaps = Xamarin.Forms.Maps;

namespace ServiceRequest.Views
{
    public partial class WindowsMapView : IMapView
    {
        public static Action ChangeMapType;
        public WindowsMapView()
        {
            try
            {
                InitializeComponent();
                _CustomMap.MoveToRegion(new MapSpan(new Position(0, 0), 360, 360));
                BtnMap.TextColor = Color.White;
                BtnMap.BackgroundColor = Styles.MainAccent;
                BtnSatellite.TextColor = Styles.MainAccent;
                BtnSatellite.BackgroundColor = Color.White;
                BtnMap.Clicked += HandleClicked;
                BtnSatellite.Clicked += HandleClicked;
                ChangeMapType += OnMapChange;
                OnMapChange();
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
                        _CustomMap.MapType = MapType.Street;
                        _CustomMap.MapTypeStyle = type;
                        BtnMap.TextColor = Color.White;
                        BtnMap.BackgroundColor = Styles.MainAccent;
                        BtnSatellite.TextColor = Styles.MainAccent;
                        BtnSatellite.BackgroundColor = Color.White;


                        break;
                    case "Satellite":
                        _CustomMap.MapType = MapType.Hybrid;
                        _CustomMap.MapTypeStyle = type;
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
        public void LoadPin(object pin)
        {
            try
            {
                _CustomMap.LoadPins?.Invoke(new List<CustomPin> { (CustomPin)pin });
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        public void LoadPins<T>(List<T> pins)
        {
            try
            {
                List<CustomPin> lstPins = new List<CustomPin>();
                var lstCustomPins = (IEnumerable<CustomPin>)pins;
                lstPins.AddRange(lstCustomPins);

                foreach (var item in lstCustomPins)
                {
                    if (lstPins.FirstOrDefault(x => x.Pin.Position.Equals(item.Pin.Position)) == null)
                        lstPins.Add(item);
                }
                if (lstPins.Count > 0)
                {
                    _CustomMap.LoadPins?.Invoke(lstPins);
                    MoveToRegion(lstCustomPins.FirstOrDefault().Pin.Position, Distance.FromKilometers(0.5));
                }
				//Added for IDXSR-260 
                else
                {
                    _CustomMap.MoveToRegion(new MapSpan(new Position(0, 0), 360, 360));
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        public bool ClearPin()
        {
            try
            {
                _CustomMap.ClearPins?.Invoke();
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
                _CustomMap.MoveToRegion(MapSpan.FromCenterAndRadius((Position)position, (Distance)distance));
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

		public void DisposeMap()
		{
			_CustomMap.DisposeMapControl?.Invoke();
		}
	}
}
