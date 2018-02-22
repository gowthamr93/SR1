using System;
using System.Threading.Tasks;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.Pages;
using Xamarin.Forms;
using ServiceRequest.AppContext;
using ServiceRequest.Views.PopUp;

namespace ServiceRequest.Views.ViewCells
{
    public partial class ParagraphCustomViewCell
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables

        private SRiActionParagraph _sRiActionParagraph;
        public static EditorPopup CustomTextPopup { get; set; }

        #endregion
        /// ------------------------------------------------------------------------------------------------
        
        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        public ParagraphCustomViewCell()
        {
            try
            {
                InitializeComponent();
                TapGestureRecognizer imgDeleteTap = new TapGestureRecognizer();
                Img_Delete.GestureRecognizers.Add(imgDeleteTap);
                imgDeleteTap.Tapped += ImgDeleteTap_Tapped;

                TapGestureRecognizer tapCustomText = new TapGestureRecognizer();
                LblCustomText.GestureRecognizers.Add(tapCustomText);
                tapCustomText.Tapped += LblCustomText_Tapped;
                BindingContextChanged += OnBindingContextChanged;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Private Function
        ///
        /// ------------------------------------------------------------------------------------------------
        /// Name		OnBindingContextChanged
        /// 
        /// <summary>
        ///             Bind the value
        /// </summary>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            try
            {
                var objsender = (SRiActionParagraph)BindingContext;
                if (objsender != null)
                    _sRiActionParagraph = objsender;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
        ///
        /// ------------------------------------------------------------------------------------------------
        /// Name		LblCustomText_Tapped
        /// 
        /// <summary>
        ///             Handles on Tap event of Custom Text.
        /// </summary>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async void LblCustomText_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (_sRiActionParagraph.CellType == CellTypes.Custom)
                {
                   // await Task.Delay(250);
                    VisitActionDetailsPage.CenterPopup.ShowPopupCenter(CustomTextPopup = new EditorPopup(LblCustomText.Text), 0.5, "Edit Custom Text");
                    CustomTextPopup.CustomText = SaveCustomText;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SaveCustomText
        /// 
        /// <summary>
        /// To save the text entered
        /// </summary>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void SaveCustomText(string editorText)
        {
            try
            {
                _sRiActionParagraph.Text = editorText;
                VisitActionDetailsPage.CurrentInstance.RefreshList();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }

        }

        ///
        /// ------------------------------------------------------------------------------------------------
        /// Name		ImgDeleteTap_Tapped
        /// 
        /// <summary>
        /// Delete the viewcell
        /// </summary>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        ///
        private void ImgDeleteTap_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (_sRiActionParagraph != null)
                {
                    VisitActionDetailsPage.CurrentInstance.OnDelete(_sRiActionParagraph);
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
