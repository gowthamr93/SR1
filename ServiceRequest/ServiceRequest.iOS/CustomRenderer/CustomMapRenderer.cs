using System;
using Xamarin.Forms;
using ServiceRequest.iOS;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using ServiceRequest.Views;
using ServiceRequest.Custom;
using ServiceRequest.ViewModels;
using MapKit;
using UIKit;
using System.Collections.Generic;
using CoreGraphics;
using System.Threading.Tasks;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace ServiceRequest.iOS
{
	public class CustomMapRenderer : MapRenderer
	{
		/// ------------------------------------------------------------------------------------------------
		#region Private Variables
		/// ------------------------------------------------------------------------------------------------

		CustomMap formsMap;
		MKMapView _nativeMap;
		List<CustomPin> customPins;
		private readonly UITapGestureRecognizer _tapRecogniser;
		MKAnnotationView annotationView;
		#endregion
		/// ------------------------------------------------------------------------------------------------

		public CustomMapRenderer()
		{
			_tapRecogniser = new UITapGestureRecognizer(OnTap)
			{
				NumberOfTapsRequired = 1,
				NumberOfTouchesRequired = 1
			};
		}

		#region Event Functions
		/// ------------------------------------------------------------------------------------------------
		/// 
		protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<View> e)
		{
			try
			{
				base.OnElementChanged(e);
				if (e.OldElement != null)
				{
					if (formsMap == null)
						formsMap = (CustomMap)e.NewElement;
					this.MapViewInstance().GetViewForAnnotation = null;
					customPins.Clear();
				}
				if (e.NewElement != null)
				{
					if (formsMap == null)
						formsMap = (CustomMap)e.NewElement;
					var customView = Control as CustomMKAnnotationView;

					if (Control != null)
						Control.RemoveGestureRecognizer(_tapRecogniser);
					base.OnElementChanged(e);
					if (Control != null)
						Control.AddGestureRecognizer(_tapRecogniser);

					if (customPins == null)
					{
						customPins = new List<CustomPin>();
					}
					else if (customPins != null && customPins.Count != 0)
					{
						customPins.Clear();
					}

					var singlePin = MapViewModel.GetCurrentPin();
					if (singlePin != null)
					{
						formsMap.Pins.Add(singlePin.Pin);
						customPins.Add(singlePin);
					}
					formsMap.ClearPins = () =>
					{
						if (customPins != null)
						{
							formsMap?.Pins.Clear();
							customPins.Clear();
						}
					};
					formsMap.LoadPins = Pins =>
					{
						if (Pins != null)
						{
							customPins.Clear();
							formsMap.Pins.Clear();
							foreach (var pin in Pins)
							{
								formsMap.CustomPins = Pins;
								formsMap.Pins.Add(pin.Pin);
								customPins.Add(pin);
							}
						}
					};
					this.MapViewInstance().GetViewForAnnotation = GetViewForAnnotation;
					formsMap.DisposeMapControl = () =>
					{
						try
						{
							//
							annotationView?.RemoveFromSuperview();
							annotationView?.Dispose();
							//
							_nativeMap?.RemoveFromSuperview();
							_nativeMap?.Delegate?.Dispose();
							_nativeMap.Delegate = null;
							_nativeMap?.Dispose();
							//
							NativeView?.RemoveFromSuperview();
							NativeView?.Dispose();
							//
							formsMap = null;
							customPins = null;
							singlePin = null;
							//
							customView?.RemoveFromSuperview();
							customView?.Dispose();
							//
							Control?.RemoveFromSuperview();
							Control?.Dispose();
							//
						}
						catch (ObjectDisposedException ex)
						{

						}
						catch (Exception ex)
						{

						}
					};
				}
			}
			catch (Exception ex)
			{

			}
		}

		#endregion
		/// ------------------------------------------------------------------------------------------------

		/// ------------------------------------------------------------------------------------------------
		#region MKAnnotationView Method
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		   GetViewForAnnotation
		/// 
		/// Description    Get the view for annotation in map
		/// ------------------------------------------------------------------------------------------------
		/// 
		MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
		{
			try
			{
				annotationView = new MKAnnotationView();

				if (customPins == null)
				{
					throw new Exception("Custom pin not found");
				}

				var position = new Position(annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);
				foreach (var pin in customPins)
				{
					if (pin.Pin.Position == position)
					{
						annotationView = new CustomMKAnnotationView(annotation, pin.Pin.Label);


						if (annotation != null)
						{

							if (pin.Pin.Address == "Current Location")
							{
								annotationView.Image = UIImage.FromFile("locCurrentPin.png");

								annotationView.AddSubview(new UILabel(new CGRect(0, 0, 15, 15))
								{
									BackgroundColor = UIColor.Clear,
									TextColor = UIColor.White,
									Font = UIFont.SystemFontOfSize(15, UIFontWeight.Semibold),
									TextAlignment = UITextAlignment.Center,
									Text = pin.Pin.Label
								});
							}
							else
							{
								annotationView.Image = UIImage.FromFile("map-marker@2x.png");

								annotationView.AddSubview(new UILabel(new CGRect(0, 0, annotationView.Image.Size.Width, annotationView.Image.Size.Width))
								{
									BackgroundColor = UIColor.Clear,
									TextColor = UIColor.White,
									Font = UIFont.SystemFontOfSize(15, UIFontWeight.Semibold),
									TextAlignment = UITextAlignment.Center,
									Text = pin.Pin.Label
								});
							}
							annotationView.CalloutOffset = new CGPoint(0, 0);
							((CustomMKAnnotationView)annotationView).Id = pin.Pin.Label;
							((CustomMKAnnotationView)annotationView).Url = "url.com";

						}
						annotationView.CanShowCallout = true;

					}
				}

				return annotationView;

			}
			catch (Exception ex)
			{
				return null;
			}

		}
		#endregion
		/// ------------------------------------------------------------------------------------------------

		/// ------------------------------------------------------------------------------------------------
		#region OnTap Method
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		   OnTap the Map
		/// Description    To Get Coordinates of the tapped location in the map
		/// ------------------------------------------------------------------------------------------------
		/// 
		private void OnTap(UITapGestureRecognizer recognizer)
		{
			var cgPoint = recognizer.LocationInView(Control);
			var location = ((MKMapView)Control).ConvertPoint(cgPoint, Control);
			formsMap.GetPositionOnClick(location.Latitude, location.Longitude);
		}

		private MKMapView MapViewInstance()
		{
			if (_nativeMap == null)
			{
				_nativeMap = Control as MKMapView;
			}
			return _nativeMap;
		}

		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}
