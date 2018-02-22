using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.Custom;
using ServiceRequest.DependencyInterfaces;
using ServiceRequest.Views;
using ServiceRequest.Pages;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms;

namespace ServiceRequest.AppContext
{
	///
	/// ------------------------------------------------------------------------------------------------
	/// <summary>  It contains all the static fields that are used inside the application.
	/// </summary>
	/// ------------------------------------------------------------------------------------------------
	/// 
	public static class AppContext
	{

		#region Private Properties and functions
		/// ------------------------------------------------------------------------------------------------
		/// 
		private static List<CustomPin> _lstCustomPins;
		private static List<Pin> _lstpin;
		private static List<SRiProperty> _lstProperties;
		private static LoginPage _vwLoginPage;
		private static IMapView _mapview;
		public static LoginPage LoginPage;
		private static decimal GetValueFromMinutes(string minutes)
		{
			return Math.Round(Convert.ToDecimal(Convert.ToInt32(minutes)) / 60, 2);
		}

		private static decimal GetMinutesFromValue(string[] splitHoursTaken)
		{
			return Math.Round(Convert.ToDecimal("." + splitHoursTaken[1]) % 1 * 60);
		}

		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Properties
		/// ------------------------------------------------------------------------------------------------
		/// 
		public static List<CustomPin> LstCustomPin
		{
			get
			{
				if (_lstCustomPins == null)
					_lstCustomPins = new List<CustomPin>();
				return _lstCustomPins;
			}
			set { _lstCustomPins = value; }
		}

		public static List<SRiProperty> LstSriproperty
		{
			get
			{
				if (_lstProperties == null)
					_lstProperties = new List<SRiProperty>();

				return _lstProperties;
			}
			set
			{
				_lstProperties = value;
			}
		}




		public static List<Pin> LstGooglePin
		{
			get
			{
				if (_lstpin == null)
					_lstpin = new List<Pin>();
				return _lstpin;
			}
			set { _lstpin = value; }
		}

		public static CustomPin CurrentLocation { get; set; }

		public static IMapView MapView
		{
			get
			{
				if (_mapview == null)
				{
					if (Device.OS == TargetPlatform.Android)
						_mapview = new AndroidMapView();
					else
						_mapview = new WindowsMapView();
				}
				return _mapview;
			}
		}

		public static bool IsForLockScreen { get; set; }

		public static CaseCellList CaseCell { get; set; }

		public static InspectionCellView InspectionCell { get; set; }

		public static bool IsTypeList { get; set; }

        public static bool IsParalist { get; set; }

        public static Stream ImageSource { get; set; }

		public static HelpPage HelpPageInstance { get; set; }

		public static float? RevertHt(string hours, string minutes)
		{
			float? value =
				Convert.ToSingle(Convert.ToInt32(hours.Remove(hours.Length - 1)) +
								 GetValueFromMinutes(minutes.Remove(minutes.Length - 1)));
			return value == 0 ? null : value;
		}

		public static Tuple<string, string> ToConvertHt(this float? value)
		{
			try
			{
				Tuple<string, string> htValue;

				var splitValue = String.Format("{0:.00}", value).Split('.');
				var htHours = (!string.IsNullOrWhiteSpace(splitValue[0]) ? splitValue[0] + "h" : null);
				var htMin = splitValue.Length > 1 ? GetMinutesFromValue(splitValue) + "m" : null;

				htValue = new Tuple<string, string>(htHours, htMin);

				return htValue;
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());
				return null;
			}
		}

		public static string[] CpiHours = { "0h", "1h", "2h", "3h", "4h", "5h", "6h", "7h", "8h" };

		public static string[] CpiMinutes = { "0m", "15m", "30m", "45m" };

		public static float? RevertMt(string miles, string decMiles)
		{
			try
			{
				float? value = Convert.ToInt32(miles) + Convert.ToSingle(decMiles);
				return value == 0 ? null : value;
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());
				return null;
			}
		}

		public static string MapType { get; set; }

		public static Action<object, EventArgs> RefreshList { get; set; }

		public static Action<object, EventArgs> RefreshVistsList { get; set; }

		public static Tuple<string, string> ToConvertMt(this float? value)
		{
			try
			{
				Tuple<string, string> mtValue;

				var splitValue = String.Format("{0:.00}", value).Split('.');
				var mtMiles = (!string.IsNullOrWhiteSpace(splitValue[0]) ? splitValue[0] : null);
				var mtDec = splitValue.Length > 1 ? "." + splitValue[1] : null;

				mtValue = new Tuple<string, string>(mtMiles, mtDec);

				return mtValue;
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());
				return null;
			}
		}

		public static void LoadMapPin(double? latitude, double? longitude)
		{
			try
			{
				if (Device.OS == TargetPlatform.Android)
				{
					SplitView.MapView?.LoadPin(new Pin()
					{
						Type = PinType.Place,
						Position = new Position(latitude.Value, longitude.Value),
						//Address = selectedAddress.FormattedAddress(),
						Label = AppData.PropertyModel.AlphaIndex(AppData.PropertyModel.SelectedProperty) == "?" ? "A" : AppData.PropertyModel.AlphaIndex(AppData.PropertyModel.SelectedProperty),
						Icon = BitmapDescriptorFactory.FromView(new Views.PinView(AppData.PropertyModel.AlphaIndex(AppData.PropertyModel.SelectedProperty) == "?" ? "A" : AppData.PropertyModel.AlphaIndex(AppData.PropertyModel.SelectedProperty)))
					});
				}
			}
			catch (Exception)
			{
				// ignored
			}
		}

		public static int? GetListIndex { get; set; }

		public static bool CheckFilterTickImage { get; set; }

		public static StringBuilder LogDetails { get; set; }

		//public static Action ShowInput { get; set; }

		public static bool IsAlertOnDisplay { get; set; }
		public static bool NewRecordInProgress { get; set; }
		public static bool LocationSelected { get; set; } = false;

		public static double SelectedNewLatitude;

		public static double SelectedNewLongitude;
		public static bool isexecute { get; set; } = false;
		#endregion
	}
}

