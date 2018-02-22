using System;
using Idox.LGDP.Apps.Common.OnSite;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.ViewModels;
using Xamarin.Forms;
using ServiceRequest.AppContext;

namespace ServiceRequest.Pages
{
    public partial class HelpPage
    {
        /// ------------------------------------------------------------------------------------------------
        #region Constructor
        ///-------------------------------------------------------------------------------------------------
        ///

        public HelpPage()
        {
            try
            {
                InitializeComponent();
                ItemsSource = HelpViewModel.All;
                if (Device.OS == TargetPlatform.Android)
                {
                    BackgroundColor = Color.FromHex("#C0000000");
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------
        
        ///-------------------------------------------------------------------------------------------------
        #region Private Functions
        ///-------------------------------------------------------------------------------------------------
        ///

        ///-------------------------------------------------------------------------------------------------
        /// Name		Btn_Done
        /// 
        /// <summary>	handles a click on the done button, it navigates to its previous page.
        /// </summary>
        /// <remarks>
        /// </remarks>
        private void Btn_Done(object sender, EventArgs e)
        {
            try
            {
               SplitView.Instace().Navigation.PopModalAsync();
				AppContext.AppContext.HelpPageInstance = null;
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
