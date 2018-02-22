using System;
using ServiceRequest.Views.ViewCells;
using Xamarin.Forms;
using ServiceRequest.Pages;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.AppContext;

namespace ServiceRequest.Views
{
    public partial class PropertyHistory : ContentView
    {
        ///-------------------------------------------------------------------------------------------------
        #region Constructor
        ///-------------------------------------------------------------------------------------------------
        ///
        public PropertyHistory()
        {
            try
            {
                InitializeComponent();

                var tapback = new TapGestureRecognizer();
                tapback.Tapped += BackImageTapped;
                Img_Back.GestureRecognizers.Add(tapback);
                Lbl_CaseName.GestureRecognizers.Add(tapback);

                for (var index = 0; index < AppData.PropertyModel.SelectedProperty.PropertyHistoryCount; index++)
                {
                    Sl_PropertyHistory.Children.Add(new PropertyHistoryViewCell(index));
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

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		BackImageTapped
        /// 
        /// <summary> handles the navigation to the previous page.
        /// </summary>
        /// /// <param name="sender">  </param>
        /// <param name="e">event arguments</param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async void BackImageTapped(object sender, EventArgs e)
        {
            try
            {
                await SplitView.Instace().PopRightContent();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        #endregion
    }
}
