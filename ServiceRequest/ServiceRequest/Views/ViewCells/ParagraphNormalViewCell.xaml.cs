using System;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.Pages;
using Xamarin.Forms;
using ServiceRequest.AppContext;

namespace ServiceRequest.Views.ViewCells
{
    public partial class ParagraphNormalViewCell
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables
        ///
        private SRiActionParagraph _sRiActionParagraph;
        ///
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        /// 
        public ParagraphNormalViewCell()
        {
            try
            {
                InitializeComponent();
                TapGestureRecognizer tapDelete = new TapGestureRecognizer();
                Img_Delete.GestureRecognizers.Add(tapDelete);
                tapDelete.Tapped += OnDeleteImageTapped;
                BindingContextChanged += OnBindingContextChanged;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
        ///
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Private Functions
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		    OnBindingContextChanged
        /// 
        /// <summary>       Bind the values 
        /// </summary>
        /// 
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
        /// Name            OnDeleteImageTapped
        /// 
        /// <summary>       Delete the view cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        /// ------------------------------------------------------------------------------------------------
        ///
        private void OnDeleteImageTapped(object sender, EventArgs e)
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
