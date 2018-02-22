using ServiceRequest.Custom;
using ServiceRequest.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(SrDatePicker), typeof(CustomDatePickerRenderer))]

namespace ServiceRequest.iOS
{
	public class CustomDatePickerRenderer:DatePickerRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
		{
			base.OnElementChanged(e);
			if (Control == null)
				return;
			Control.InputAssistantItem.LeadingBarButtonGroups = null;
			Control.InputAssistantItem.TrailingBarButtonGroups = null;
		}
	}
}
