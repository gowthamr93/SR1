using System;
using Xamarin.Forms;

namespace ServiceRequest
{
	public interface INativePopup
	{
		void NativeCenterPopup(View view, double width, double height);
		void NativeRelativePopup(View view, Point point, double width, double height);
	}
}
