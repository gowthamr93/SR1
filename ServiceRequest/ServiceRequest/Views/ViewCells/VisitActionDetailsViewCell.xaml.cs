using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.Pages;
using ServiceRequest.Views.PopUp;
using Xamarin.Forms;
using ServiceRequest.AppContext;

namespace ServiceRequest.Views.ViewCells
{
    public partial class VisitActionDetailsViewCell
    {
        /// ------------------------------------------------------------------------------------------------
        #region publicVariables
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// 
        public static GroupedListView GroupedLegislation;

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        /// ------------------------------------------------------------------------------------------------
        /// 
        public VisitActionDetailsViewCell()
        {
            try
            {
                InitializeComponent();
                OnLoad();

                //LegislationTapped
                var tapLegislation = new TapGestureRecognizer();
                tapLegislation.Tapped += OntapLegislationTapped;
                Lbl_legislation.GestureRecognizers.Add(tapLegislation);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region private Functions

        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// ------------------------------------------------------------------------------------------------
        /// Name		OnLoad
        /// 
        /// <summary>	Loads the data while appearing
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void OnLoad()
        {
            try
            {
                int index;

                var action = AppData.PropertyModel.SelectedAction.Action;

                var orderedLegs = AppData.ConfigModel.Legislations(AppData.PropertyModel.SelectedRecord.Record.Organisation,
                                                          AppData.PropertyModel.SelectedVisit.Visit.Visit.VisitType)
                                .OrderBy(l => l.Code + l.Description).ToList();

                GroupedLegislation = new GroupedListView(AppData.ConfigModel.Legislations(AppData.PropertyModel.SelectedRecord.Record.Organisation,
                                                          AppData.PropertyModel.SelectedVisit.Visit.Visit.VisitType)
                                .OrderBy(l => l.Code + l.Description).Select(a => new KeyValuePair<string, string>(a.Code, a.Description)).ToList(), Lbl_legislation);
                index = -1;
                if (!string.IsNullOrEmpty(action.LegislationType))
                {
                    for (int i = 0; i < orderedLegs.Count; i++)
                        if (orderedLegs[i].Code.Equals(action.LegislationType))
                        {
                            index = i;
                            break;
                        }

                    Lbl_legislation.Text = $"{orderedLegs[index].Code} - {orderedLegs[index].Description}";
                }
                else
                {
                    Lbl_legislation.Text = "Value";
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        /// ------------------------------------------------------------------------------------------------
        /// ------------------------------------------------------------------------------------------------
        /// Name		OntapLegislationTapped
        /// 
        /// <summary>	Opens the Popup on touching the Legislation label
        /// </summary>
        /// <param name="sender"> </param>
        ///   /// <param name="e"> event arguments</param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async void OntapLegislationTapped(object sender, EventArgs e)
        {
            try
            {
                if (GroupedLegislation != null && GroupedLegislation.listData.Count != 0)
                {
                    int height = GroupedLegislation.listData.Count * 90;

                    if (height > 400)
                    {
                        height = 400;
                    }                
                   VisitActionDetailsPage.RelativePopup.ShowPopupRelative(GroupedLegislation, BX_Legislation, 400, height, true, "");
                   VisitActionDetailsPage.RelativePopup._triangleImage.Source = "";
                }
                else
                {
                    await VisitActionDetailsPage.CurrentInstance.DisplayAlert("No legislation", "No legislation types set up in Uniform for this visit type. Please contact your Uniform administrator for assistance.", "Ok");
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
