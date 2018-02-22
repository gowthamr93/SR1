using System;
using ServiceRequest.Custom;
using Xamarin.Forms;
using ServiceRequest.Droid.CustomRenderer;

using Xamarin.Forms.Platform.Android;
using ServiceRequest.AppContext;
using System.ComponentModel;
using Android.Views;

[assembly: ExportRenderer(typeof(SrEditor), typeof(CustomEditorRenderer))]
namespace ServiceRequest.Droid.CustomRenderer
{
    public class CustomEditorRenderer : EditorRenderer
    {
        /// ------------------------------------------------------------------------------------------------
        #region Protected Functions
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            try
            {
                base.OnElementChanged(e);

                if (e.NewElement != null)
                {
                    var element = e.NewElement as SrEditor;
                    if (element != null) Control.Hint = element.Placeholder;
                    Control.SetHintTextColor(Android.Graphics.Color.Gray);
                    Control.Gravity = GravityFlags.Start;            
                    Control.SetSelection(0);
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
                    if (element != null) Control.Hint = element.Placeholder;
                    Control.SetHintTextColor(Android.Graphics.Color.Gray);
                    Control.Gravity = GravityFlags.Start;
                    Control.SetSelection(0);
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}