using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Idox.LGDP.Apps.Common.OnSite;
using Idox.LGDP.Apps.ServiceRequest.Client;
using PopUpSample;
using ServiceRequest.Pages;
using ServiceRequest.ViewModels;
using ServiceRequest.Views.PopUp;
using Xamarin.Forms;
using Idox.LGDP.Apps.ServiceRequest.Client;

namespace ServiceRequest.Views.ViewCells
{
    public partial class VisitsViewCell : StackLayout
    {
        /// ------------------------------------------------------------------------------------------------
        #region public construtor
        public VisitsViewCell(List<CreateVisitsList> data)
        {
            InitializeComponent();
            Lstvw_Main.ItemsSource = data;
            Lstvw_Main.HeightRequest = (data.Count * 110) + 40;
            TapGestureRecognizer _actionTapped = new TapGestureRecognizer();
            _actionTapped.Tapped += AddVisits;
            Lbl_AddVisis.GestureRecognizers.Add(_actionTapped);
            Lstvw_Main.ItemTapped += Visits_Tapped;
            AppContext.AppContext.RefreshVistsList = (sender, args) => OnRefreshList();

        }
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region public functions

        /// <summary>
        /// Refresh the list of vists data
        /// </summary>
        public void OnRefreshList()
        {
            List<CreateVisitsList> VisitsList;
            CreateVisitsList VisitsData;
            Lstvw_Main.BeginRefresh();
            IndexMapping iMap;
            iMap = AppData.PropertyModel.SelectedRecord.IndexMap;
            iMap = new IndexMapping(iMap.Property, iMap.RequestGroup, iMap.Record, 0);
            var InspectionList = AppData.PropertyModel.EntityFromMapping(iMap) as SRiInspection;
            VisitsList = new List<CreateVisitsList>();
            var visitcount = InspectionList.Visits.Count;
            for (int j = 0; j < visitcount; j++)
            {
                var data = InspectionList.Visits[j];
                VisitsData = new CreateVisitsList()
                {
                    VisitTypeDescription = data.VisitTypeDescription,
                    ScheduleDate = data.Visit.DateScheduled.ToString("dd MMM yyyy", "Scheduled for ", "Not scheduled"),
                    CompletedDate = data.Visit.DateVisit.ToString("dd MMM yyyy", "Completed on ", "Outstanding"),
                    VisitListIndex = j
                };
                VisitsList.Add(VisitsData);
            }
            Lstvw_Main.ItemsSource = VisitsList;
            Lstvw_Main.HeightRequest = (VisitsList.Count * 110) + 40;
            Lstvw_Main.EndRefresh();
            GC.Collect();
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region private functions

        private void UnSelectList(object sender, EventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        /// <summary>
        /// On tapped to Add visits
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private async void AddVisits(object s, EventArgs e)
        {

            SRiRecordMeta record;
            record = AppData.PropertyModel.SelectedRecord.Record;
            var orderedVisits = AppData.ConfigModel.Visits(record.Organisation).OrderBy(a => a.Code + a.Description).ToList();
            var stringVisits = orderedVisits.Select(a => string.Format("{0} - {1}", a.Code, a.Description)).ToList();
            if (!AppData.ConfigModel.Visits(record.Organisation).Any())
            {
                await
                    SplitView.Instace()
                        .DisplayAlert("Cannot add Visits",
                            "No visit types set up in Uniform for this visit type. Please contact your Uniform administrator for assistance.",
                            "Ok");
            }
            else
            {
                SplitView.PopupContent.ShowPopupRelative(new AddVisitsPopUp(SplitView.PopupContent), Lbl_AddVisis, this.Width * 0.50, GetHeight(stringVisits.Count), true, "");
                SplitView.PopupContent._triangleImage.Source = "";
            }    

        }
        /// <summary>
        /// On tapped in visits data
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private async void Visits_Tapped(object s, ItemTappedEventArgs e)
        {
            var data = e.Item as CreateVisitsList;
            AppData.PropertyModel.SelectedVisit = null;
            IndexMapping iMap;
            iMap = AppData.PropertyModel.SelectedRecord.IndexMap;
            iMap = new IndexMapping(iMap.Property, iMap.RequestGroup, iMap.Record, 0, data.VisitListIndex);

            var Details = AppData.PropertyModel.SelectedRecord.Record.Record.Inspections[0].Visits[data.VisitListIndex];
            AppData.PropertyModel.SelectedVisit = new SelectedVisit(Details, iMap, false);
            await SplitView.Instace().Navigation.PushModalAsync(new VisitActionPage());
        }

       /// <summary>
       /// To set the height of popup height
       /// </summary>
       /// <param name="count"></param>
       /// <returns></returns>
        private double GetHeight(int count)
        {
            var minHeight = count * GroupedListView.GroupedListViewCellHeight + GroupedListView.GroupedListViewPadding;
            double maxHeight = SplitView.Instace().Height * 0.55;
            return maxHeight < minHeight ? maxHeight : minHeight;
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
