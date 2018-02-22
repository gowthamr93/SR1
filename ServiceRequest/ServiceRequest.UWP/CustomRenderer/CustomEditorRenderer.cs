using ServiceRequest.AppContext;
using ServiceRequest.Custom;
using ServiceRequest.UWP.CustomRenderer;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(SrEditor), typeof(CustomEditorRenderer))]
namespace ServiceRequest.UWP.CustomRenderer
{
    public class CustomEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            try
            {
                base.OnElementChanged(e);

                if (e.NewElement != null)
                {
                    var element = e.NewElement as SrEditor;
                    //this.Control.MaxLength = 140;
                    if (element != null) Control.PlaceholderText = element.Placeholder;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                base.OnElementPropertyChanged(sender, e);

                if (e.PropertyName == SrEditor.PlaceholderProperty.PropertyName)
                {
                    var element = Element as SrEditor;
                    Control.PlaceholderText = element.Placeholder;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
    }
}
