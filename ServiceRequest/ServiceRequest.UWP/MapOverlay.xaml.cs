using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ServiceRequest.Custom;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ServiceRequest.UWP
{
    public sealed partial class MapOverlay : UserControl
    {
        CustomPin customPin;
        public MapOverlay(CustomPin pin)
        {
            this.InitializeComponent();
            customPin = pin;
            SetupData();
        }

        void SetupData()
        {
            Label.Text = customPin.Pin.Label;
        }

        private async void OnInfoButtonTapped(object sender, TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(customPin.Url));
        }
    }
}
