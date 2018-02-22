using ServiceRequest.Custom;
using ServiceRequest.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SrTimePicker), typeof(CustomTimePickerRenderer))]
namespace ServiceRequest.iOS
{
	public class CustomTimePickerRenderer:TimePickerRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
		{
			base.OnElementChanged(e);
			if (Control == null)
				return;
			Control.InputAssistantItem.LeadingBarButtonGroups = null;
			Control.InputAssistantItem.TrailingBarButtonGroups = null;
		}
	}
}
