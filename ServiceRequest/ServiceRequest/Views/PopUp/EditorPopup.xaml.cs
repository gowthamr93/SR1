using ServiceRequest.AppContext;
using ServiceRequest.Pages;
using ServiceRequest.ViewModels;
using System;

namespace ServiceRequest.Views.PopUp
{
    public partial class EditorPopup
    {
        public Action<string> CustomText;

        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        public EditorPopup(string text)
        {
            try
            {
                InitializeComponent();
                if (text != ParagraphViewModel.TAP_CUSTOM_PLACEHOLDER)
                {
                    Edt_CustomText.Text = text;
                }
                else
                {
                    Edt_CustomText.Placeholder = ParagraphViewModel.CUSTOM_PLACEHOLDER;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Private Functions

        private void SaveCustomText()
        {
            try
            {
                if (!string.IsNullOrEmpty(Edt_CustomText.Text))
                    CustomText.Invoke(Edt_CustomText.Text);
                else
                    CustomText.Invoke(ParagraphViewModel.TAP_CUSTOM_PLACEHOLDER);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name		OnCancelTapped
        /// 
        /// <summary>	Dismisses the popup on clicking Cancel button.
        /// </summary>
        /// <param name="sender"> </param>
        ///   /// <param name="e"> event arguments</param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void OnCancelTapped(object sender, EventArgs e)
        {
            try
            {
                VisitActionDetailsPage.CenterPopup.DismisPopup();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }


        /// ------------------------------------------------------------------------------------------------
        /// Name		OnSaveTapped
        /// 
        /// <summary>	Saves the data and Dismisses the popup on clicking Cancel button.
        /// </summary>
        /// <param name="sender"> </param>
        ///   /// <param name="e"> event arguments</param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void OnSaveTapped(object sender, EventArgs e)
        {
            try
            {
                SaveCustomText();
                VisitActionDetailsPage.CenterPopup.DismisPopup();
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
