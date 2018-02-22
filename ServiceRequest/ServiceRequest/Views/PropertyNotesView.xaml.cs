using System;
using ServiceRequest.Pages;
using ServiceRequest.Views.CaseListViewControl;
using ServiceRequest.Views.ViewCells;
using Xamarin.Forms;
using ServiceRequest.AppContext;

namespace ServiceRequest.Views
{
    public partial class PropertyNotesView : ContentView
    {
        ///-------------------------------------------------------------------------------------------------
        #region Constructor
        ///-------------------------------------------------------------------------------------------------
        ///
        public PropertyNotesView()
        {
            try
            {
                InitializeComponent();

                TapGestureRecognizer tapback = new TapGestureRecognizer();
                tapback.Tapped += BackImageTapped;
                Img_Back.GestureRecognizers.Add(tapback);
                Lbl_CaseName.GestureRecognizers.Add(tapback);

                for (int index = 0; index < AppContext.AppContext.LstSriproperty[CaseListControl.SectionIndex].PropertyNotesCount; index++)
                {
                    Sl_PropertyNotes.Children.Add(new PropertyNotesViewCell(index));
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
        /// ------------------------------------------------------------------------------------------------
    }
}
