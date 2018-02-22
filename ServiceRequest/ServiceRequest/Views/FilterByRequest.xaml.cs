using System;
using System.Linq;
using Idox.LGDP.Apps.ServiceRequest.Client;
using PopUpSample;
using ServiceRequest.Models;
using Xamarin.Forms;
using ServiceRequest.AppContext;

namespace ServiceRequest.Views
{
    public partial class FilterByRequest
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variable
        /// ------------------------------------------------------------------------------------------------
        private PopupLayouts PopupLayout { get; }
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Constructor
        public FilterByRequest(FilterByRequestType data, PopupLayouts popupLayouts)
        {
            try
            {
                InitializeComponent();
                PopupLayout = popupLayouts;
                BindingContext = data;
                var itemTapped = new TapGestureRecognizer();
                itemTapped.Tapped += ImageShow;
                Gl_RequestType.GestureRecognizers.Add(itemTapped);
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
        private void ImageShow(object sender, EventArgs e)
        {
            try

            {
                var tappedData = (Grid)sender;
                FilterByRequestType data = (FilterByRequestType)tappedData.BindingContext;
                var filterControls = tappedData.Children.ToList();
                var filterByType = (Label)filterControls[1];
                if (AppContext.AppContext.GetListIndex == data.index && AppContext.AppContext.CheckFilterTickImage == false)
                {
                    AppData.PropertyModel.FilterByType = null;
                    AppContext.AppContext.GetListIndex = null;
                    AppContext.AppContext.CheckFilterTickImage = true;
                }
                else
                {
                    AppData.PropertyModel.FilterByType = filterByType.Text;
                    AppContext.AppContext.CheckFilterTickImage = false;
                }
                Gl_RequestType.BindingContext = new
                {
                    filterByType.Text,
                    IsVisible =
                        AppContext.AppContext.GetListIndex != data.index &&
                        AppContext.AppContext.CheckFilterTickImage
                };
                AppContext.AppContext.GetListIndex = data.index;
                PopupLayout.DismisPopup();
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
