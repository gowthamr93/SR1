using System;
using ServiceRequest.Custom;

using ServiceRequest.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SrPicker), typeof(CustomPickerRender))]
namespace ServiceRequest.iOS
{
	public class CustomPickerRender:PickerRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
				return;
			Control.InputAssistantItem.LeadingBarButtonGroups = null;
			Control.InputAssistantItem.TrailingBarButtonGroups = null;
		}
	}
}
