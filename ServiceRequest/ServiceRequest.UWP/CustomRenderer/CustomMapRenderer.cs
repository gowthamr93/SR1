using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Idox.LGDP.Apps.ServiceRequest.Client;
using Plugin.Geolocator;
using ServiceRequest.Custom;
using ServiceRequest.UWP.CustomRenderer;
using ServiceRequest.ViewModels;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.UWP;
using Xamarin.Forms.Platform.UWP;
using ServiceRequest.AppContext;
using ServiceRequest.Views.CaseListViewControl;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace ServiceRequest.UWP.CustomRenderer
{
    public class CustomMapRenderer : ViewRenderer<Map, MapControl>
    {
        private static MapControl _nativeMap;
        private static CustomMap _formsMap;
        List<CustomPin> customPins;
        private MapOverlay _mapOverlay;
        bool xamarinOverlayShown = false;

        private CustomMap FormsMap { get; set; }

        private MapControl NativeMap
        {
            get
            {
                if (_nativeMap == null)
                {
                    _nativeMap = Control;
                }
                return _nativeMap;
            }
        }

        protected override async void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            try
            {
                base.OnElementChanged(e);
                if (e.OldElement != null)
                {
                    if (NativeMap != null)
                        NativeMap.Children.Clear();

                    _mapOverlay = null;
                    MessagingCenter.Unsubscribe<Map, MapSpan>(this, "MapMoveToRegion");
                }

                if (e.NewElement != null)
                {
                    FormsMap = (CustomMap)e.NewElement;

                    if (Control == null)
                    {
                        SetNativeControl(new MapControl());
                        Control.MapServiceToken =
                            "EBluhremUIEFIJHxuhLu~p13Cqk8HmULBachuX56Htw~Aq7mhwQ6IBoBm31rHk7hHALJMJ78AkfDOdFpcNy1VpycneXTMQ8mro4XVC8cy5Fd";
                        _nativeMap = Control;

                        //Take Location on Tapped
                        Control.MapTapped += Control_MapTapped;
                        //
                    }
                    NativeMap.Loading += (sender, args) =>
                    {
                        var pin = MapViewModel.GetCurrentPin();
                        if (pin != null)
                        {
                            SetPins(pin);

                        }
                        MapType mapType = FormsMap.MapType;
                        UpdateMapType(mapType);
                    };

                    FormsMap.ClearPins = () =>
                    {
                        if (NativeMap != null)
                        {
                            NativeMap.Children.Clear();
                            NativeMap.MapElements.Clear();
                        }
                    };

                    FormsMap.LoadPins = Pins =>
                  {

                      if (Pins != null)

                          foreach (var pin in Pins)
                          {
                              SetPins(pin);
                          }
                  };

                    FormsMap.DisposeMapControl = () =>
                    {
                        //
                        _nativeMap = null;
                        _mapOverlay = null;
                        //
                    };
                    MessagingCenter.Subscribe<Map, MapSpan>(this, "MapMoveToRegion", async (s, a) => await boundingMap(a), FormsMap);
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }


        private void Control_MapTapped(MapControl sender, MapInputEventArgs args)
        {
            var custMap = (CustomMap)Element;
            custMap.GetPositionOnClick(args.Location.Position.Latitude, args.Location.Position.Longitude);
        }

        private async void SetPins(CustomPin pin)
        {
            try
            {
                var snPosition = new BasicGeoposition { Latitude = pin.Pin.Position.Latitude, Longitude = pin.Pin.Position.Longitude };
                var snPoint = new Geopoint(snPosition);

                if (AppData.PropertyModel.SelectedProperty != null)
                {
                    NativeMap.Center = snPoint;
                    NativeMap.ZoomLevel = 15;
                }
                var mapIcon = new MapIcon();

                pin.ImageUrl = (pin.ImageUrl == null || pin.ImageUrl == string.Empty) ? "mapPin.png" : pin.ImageUrl;

                mapIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri(string.Format("ms-appx:///{0}", pin.ImageUrl)));
                mapIcon.CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible;
                mapIcon.Location = snPoint;
                mapIcon.NormalizedAnchorPoint = new Windows.Foundation.Point(0.5, 1.0);

                _mapOverlay = new MapOverlay(pin);
                MapControl.SetLocation(_mapOverlay, snPoint);
                MapControl.SetNormalizedAnchorPoint(_mapOverlay, new Windows.Foundation.Point(0.5, 1.0));
                xamarinOverlayShown = true;

                if (NativeMap != null)
                {
                    NativeMap.Children.Add(_mapOverlay);
                    NativeMap.MapElements.Add(mapIcon);
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }


        async Task boundingMap(MapSpan span, MapAnimationKind animation = MapAnimationKind.Bow)

        {
            var nw = new BasicGeoposition
            {
                Latitude = span.Center.Latitude + span.LatitudeDegrees / 2,
                Longitude = span.Center.Longitude - span.LongitudeDegrees / 2

            };
            var se = new BasicGeoposition
            {
                Latitude = span.Center.Latitude - span.LatitudeDegrees / 2,
                Longitude = span.Center.Longitude + span.LongitudeDegrees / 2
            };
            var boundingBox = new GeoboundingBox(nw, se);
            await Control.TrySetViewBoundsAsync(boundingBox, null, animation);

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var formsMap = sender as CustomMap;
            if (e.PropertyName == "MapType")
            {
                MapType mapType = formsMap.MapType;
                UpdateMapType(mapType);
            }
        }


        void UpdateMapType(MapType type)

        {
            switch (type)

            {
                case MapType.Street:

                    Control.Style = MapStyle.Road;

                    break;

                case MapType.Satellite:

                    Control.Style = MapStyle.Aerial;

                    break;

                case MapType.Hybrid:

                    Control.Style = MapStyle.AerialWithRoads;

                    break;
            }
        }
    }
}
