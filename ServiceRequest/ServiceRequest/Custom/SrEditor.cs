using System;
using Xamarin.Forms;

namespace ServiceRequest.Custom
{
    /// <summary>  Custom Controller for the Editor.
    /// </summary>
    /// ------------------------------------------------------------------------------------------------
    /// 
    public class SrEditor : Editor
    {
        public static readonly BindableProperty PlaceholderProperty =
       BindableProperty.Create<SrEditor, string>(view => view.Placeholder, String.Empty);

        public string Placeholder
        {
            get
            {
                return (string)GetValue(PlaceholderProperty);
            }

            set
            {
                SetValue(PlaceholderProperty, value);
            }
        }
    }
}
