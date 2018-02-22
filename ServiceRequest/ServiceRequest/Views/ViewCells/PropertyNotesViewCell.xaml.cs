using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.AppContext;
using System;

namespace ServiceRequest.Views.ViewCells
{
    public partial class PropertyNotesViewCell
    {
        ///-------------------------------------------------------------------------------------------------
        #region Constructor
        ///-------------------------------------------------------------------------------------------------
        ///
        public PropertyNotesViewCell(int index)
        {
            try
            {
                InitializeComponent();
                var propertyNotes = AppData.PropertyModel.SelectedProperty.GetPropertyNotes(index);
                if (propertyNotes != null)
                {
                    Lbl_Summary.Text = propertyNotes.Summary;
                    Lbl_Text.Text = propertyNotes.Text;
                    Sl_Main.Children.Add(Sl_Notes);
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
