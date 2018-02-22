using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.AppContext;
using ServiceRequest.Custom;
using System;
using Xamarin.Forms.GoogleMaps;

namespace ServiceRequest.ViewModels
{
    public class MapViewModel
    {
        /// ------------------------------------------------------------------------------------------------               
        #region Public Functions
        /// ------------------------------------------------------------------------------------------------        
        /// 
        public static CustomPin GetCurrentPin()
        {
            try
            {
                if (AppData.PropertyModel.SelectedProperty != null && (AppData.PropertyModel.SelectedProperty.Latitude != null && AppData.PropertyModel.SelectedProperty.Longitude != null && AppData.PropertyModel.SelectedProperty.HasValidCoords))
                {

                    var pin = new CustomPin
                    {
                        Pin = new Xamarin.Forms.Maps.Pin
                        {
                            Type = Xamarin.Forms.Maps.PinType.Place,
                            Position =
                                new Xamarin.Forms.Maps.Position((double)AppData.PropertyModel.SelectedProperty.Latitude,
                                    (double)AppData.PropertyModel.SelectedProperty.Longitude),
                            Address = AppData.PropertyModel.SelectedProperty.Address.LongAddress,
                            Label = AppData.PropertyModel.AlphaIndex(AppData.PropertyModel.SelectedProperty),
                        }
                    };

                    return pin;
                }

                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }
        public static Pin GetCustomePin()
        {
            try
            {
                if (AppData.PropertyModel.SelectedProperty.Latitude != null && AppData.PropertyModel.SelectedProperty.Longitude != null && AppData.PropertyModel.SelectedProperty.HasValidCoords)
                {
                    var pin = new Pin
                    {
                        Type = PinType.Place,
                        Position = new Position((double)AppData.PropertyModel.SelectedProperty.Latitude,
                               (double)AppData.PropertyModel.SelectedProperty.Longitude),
                        Address = AppData.PropertyModel.SelectedProperty.Address.LongAddress,
                        Label = AppData.PropertyModel.AlphaIndex(AppData.PropertyModel.SelectedProperty),
                        Icon = BitmapDescriptorFactory.FromView(new Views.PinView(AppData.PropertyModel.AlphaIndex(AppData.PropertyModel.SelectedProperty)))
                    };
                    return pin;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }

        }
        #endregion
        /// ------------------------------------------------------------------------------------------------

    }
}
