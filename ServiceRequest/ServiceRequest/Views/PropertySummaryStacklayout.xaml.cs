using System;
using Idox.LGDP.Apps.ServiceRequest.Client;
using Xamarin.Forms;
using ServiceRequest.AppContext;

namespace ServiceRequest.Views
{
    public partial class PropertySummaryStacklayout
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        /// ------------------------------------------------------------------------------------------------
        /// 
        public PropertySummaryStacklayout(SRiPropertyDetail propertyDetail)
        {
            try
            {
                InitializeComponent();
                ShowDetails(propertyDetail);
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
        /// ------------------------------------------------------------------------------------------------
        /// ------------------------------------------------------------------------------------------------
        /// Name		UnSelectList
        /// 
        /// <summary>	To Unselect the list being selected
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void UnSelectList(object sender, EventArgs e)
        {
            try
            {
                ((ListView)sender).SelectedItem = null;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        /// ------------------------------------------------------------------------------------------------
        /// ------------------------------------------------------------------------------------------------
        /// Name		ShowDetails
        /// 
        /// <summary>	To bind the values in view and to set height Request
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 

        private void ShowDetails(SRiPropertyDetail propertyDetails)
        {
            try
            {
                Lstvw_Main.HeightRequest = propertyDetails.Details.Count * Device.OnPlatform<double>(45, 45, 35);
                BindingContext = propertyDetails;
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
