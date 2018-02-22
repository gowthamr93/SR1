using ServiceRequest.AppContext;
using System;

namespace ServiceRequest.Pages
{
    public partial class AboutPage
    {
        /// ------------------------------------------------------------------------------------------------
        #region Constructor
        ///-------------------------------------------------------------------------------------------------
        ///
        public AboutPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        #endregion
        ///-------------------------------------------------------------------------------------------------
       
        ///
        #region Private Functions
        ///-------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Btn_Done
        /// 
        /// <summary>	handles a click on the done button, it navigates to its previous page.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void Btn_Done(object sender, EventArgs e)
        {
            try
            {
                SplitView.Instace().Navigation.PopModalAsync();
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
