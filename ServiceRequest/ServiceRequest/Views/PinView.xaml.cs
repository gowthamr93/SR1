using ServiceRequest.AppContext;
using System;
using Xamarin.Forms;

namespace ServiceRequest.Views
{
    public partial class PinView: StackLayout
    {
       

        /// ------------------------------------------------------------------------------------------------
        #region Constructor
        public PinView(string display)
        {
            try
            {
                InitializeComponent();
                _display = display;
                BindingContext = this;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Properties
        public string Display
        {
            get { return _display; }
        }
        private string _display;
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
