using System;
using Xamarin.Forms;
using ServiceRequest.Droid.CustomRenderer;
using Xamarin.Forms.Platform.Android;
using ServiceRequest.Custom;
using ServiceRequest.AppContext;

[assembly: ExportRenderer(typeof(SRiButton), typeof(CustomButtonRenderer))]

namespace ServiceRequest.Droid.CustomRenderer
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            try
            {
                base.OnElementChanged(e);
                var button = Control;
                button.SetAllCaps(false);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
    }
}