using System;
using PopUpSample;
using ServiceRequest.Classes;
using ServiceRequest.Pages;
using Xamarin.Forms;

namespace ServiceRequest.Views.PopUp
{
    public partial class AddView : Grid
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ActionView{T}"/> class.
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public AddView(PopupLayouts parentPopup)
        {
            InitializeComponent();
            if (parentPopup == null) return;
            //Add new image
            var tapAddImage = new TapGestureRecognizer();
            tapAddImage.Tapped += (sender, e) =>
            {
                parentPopup.DismisPopup();
                OnCameraCapture();
            };
            Gl_Add.GestureRecognizers.Add(tapAddImage);
        }
        ///
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Private Functions
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		OnCameraCapture
        /// 
        /// <summary>	handles the camera on click functionality.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async void OnCameraCapture()
        {
            await new CameraCapture().TakePhoto();
            SplitView.CenterPopupContent.ShowPopupCenter(new AddNewImageView(AppContext.PageNames.SplitView), 0.5);
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
