using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ServiceRequest.Droid.CustomRenderer;
using ServiceRequest.Custom;
using ServiceRequest.AppContext;
using System;
using TextAlignment = Android.Views.TextAlignment;

[assembly: ExportRenderer(typeof(SrPicker), typeof(CustomPickerRenderer))]
namespace ServiceRequest.Droid.CustomRenderer
{
    public class CustomPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            try
            {
                base.OnElementChanged(e);
                if (Control != null)
                {
                    Control.Gravity = GravityFlags.Start;
                    Control.TextAlignment = TextAlignment.TextStart;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
    }
}