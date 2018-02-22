using PopUpSample;
using ServiceRequest.AppContext;
using ServiceRequest.Pages;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ServiceRequest.Views.PopUp
{
    public partial class ActionView
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ActionView{T}"/> class.
        /// </summary>
        public ActionView(PopupLayouts parentPopup)
        {
            try
            {
                if (parentPopup == null) return;
                InitializeComponent();
                // Tap Search
                var tapSearch = new TapGestureRecognizer();
                tapSearch.Tapped +=  (s, e) =>
                {
                    parentPopup.DismisPopup();
                    //await Task.Delay(250);
                    SplitView.CenterPopupContent.ShowPopupCenter(new SearchView(), 0.5);
                };
                // Tap Sync
                var tapSync = new TapGestureRecognizer();
                tapSync.Tapped += (s, e) =>
                {
                    parentPopup.DismisPopup();
                    SplitView.HubMaster.SyncCheck();
                };
                Gl_Search.GestureRecognizers.Add(tapSearch);
                Gl_Sync.GestureRecognizers.Add(tapSync);
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
