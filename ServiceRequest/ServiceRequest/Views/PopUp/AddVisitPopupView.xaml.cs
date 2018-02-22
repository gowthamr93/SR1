using PopUpSample;
using ServiceRequest.Classes;
using System;
using System.Threading.Tasks;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.DependencyInterfaces;
using ServiceRequest.Pages;
using Xamarin.Forms;
using ServiceRequest.AppContext;
using ServiceRequest.Custom;
using XamarinMaps = Xamarin.Forms.Maps;
using GoogleMap = Xamarin.Forms.GoogleMaps;
using GeoLocation = Plugin.Geolocator.Abstractions;

namespace ServiceRequest.Views.PopUp
{
    public partial class AddVisitPopupView
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables
        ///
     //   private ContentPage _conentPage;
        public static AddNewCaseView NewCaseAddView { get; set; }
        public static IMapView PointOnMapView { get; set; }
        public static AddNewCasePointOnMapView AddNewCasePointOnMapView { get; set; }
        public static GeoLocation.Position UserPosition { get; set; }
        ///
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Constructor
        /// ------------------------------------------------------------------------------------------------
        /// 
        public AddVisitPopupView(PopupLayouts parentPopup, bool isFromMap = false)
        {
            try
            {
                InitializeComponent();

                if (isFromMap)
                {
                    NewSrUi();
                }
                else
                {
                    if (parentPopup == null) return;
                    //Add new image
                    var tapAddImage = new TapGestureRecognizer();
                    tapAddImage.Tapped += (sender, e) =>
                    {
                        parentPopup.DismisPopup();
                        OnCameraCapture();
                    };
                    Gl_Add.GestureRecognizers.Add(tapAddImage);
                    //Add audio
                    var tapAddAudio = new TapGestureRecognizer();
                    tapAddAudio.Tapped += async (sender, e) =>
                    {
                        parentPopup.DismisPopup();
                        var isMicrophoneAvailable = DependencyService.Get<IRecorder>().IsMicrophoneAvailable();
                        if (isMicrophoneAvailable)
                        {
                            //if (Device.OS.Equals(TargetPlatform.WinPhone) || Device.OS.Equals(TargetPlatform.Windows))
                            //{
                            //    await Task.Delay(600);
                            //}
                            SplitView.CenterPopupContent.ShowPopupCenter(new AddNewAudioView(), 0.5);
                        }
                        else
                        {
                            await SplitView.DisplayAlert("No Microphone",
                            "The Microphone is not available for this device. Please check in the device settings to see if the Microphone has been disabled.",
                            "OK", null);
                        }
                    };
                    Gl_AddAudio.GestureRecognizers.Add(tapAddAudio);
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        #region New Service Request

        private void NewSrUi()
        {
            var tapNewSr = new TapGestureRecognizer();
            //
            this.Children.Clear();
            this.RowDefinitions.Clear();
            this.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100, GridUnitType.Star) });
            var gridSr = new Grid()
            {
                RowDefinitions =
                    new RowDefinitionCollection()
                    {
                        new RowDefinition() {Height = new GridLength(100, GridUnitType.Star)}
                    }
            };
            var lblOption = new Label()
            {
                Text = "New Service Request",
                TextColor = Styles.MainAccent,
                FontSize = FontSizeView.CustomFontSizeSmSmMi,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
            };

            //tapNewSr.Tapped += (s, e) =>
            //{
            //    OnNewSrTapped();
            //    var t = Task.Run(async delegate
            //    {
            //        await Task.Delay(TimeSpan.FromSeconds(0.5));
            //        AddNewCasePointOnMapView.MoveToPosition?.Invoke();
            //        return 42;
            //    });
            //};
            //lblOption.GestureRecognizers.Add(tapNewSr);

            gridSr.Children.Add(lblOption);
            this.Children.Add(gridSr);
        }

        //private async void OnNewSrTapped()
        //{
        //    SplitView.PopupContent.DismisPopup();
        //    if (Device.OS != TargetPlatform.Android)
        //        UserPosition = await SplitView.Fullmapview.GetCurrentLocation();
        //    if (Reachability.InternetConnectionStatus() == ReachabilityNetworkStatus.NotReachable)
        //    {
        //        await SplitView.Instace().PushRightContent(NewCaseAddView = new AddNewCaseView());
        //    }
        //    else
        //    {
        //        if (Device.OS == TargetPlatform.Android)
        //            PointOnMapView = new AndroidMapView();
        //        else
        //            PointOnMapView = new WindowsMapView();

        //        await SplitView.Instace().PushRightContent(AddNewCasePointOnMapView = new AddNewCasePointOnMapView(PointOnMapView, UserPosition));
        //    }
        //}



        #endregion

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Private Functions

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		OnCameraCapture
        /// 
        /// <summary>	handles the camera on click functionality.
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async void OnCameraCapture()
        {
            try
            {
                await CameraCapture.AssistTakePhoto();

                if (AppContext.AppContext.ImageSource != null)
                {
                   // await Task.Delay(500);
                    SplitView.CenterPopupContent.ShowPopupCenter(new AddNewImageView(), 0.5);
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
