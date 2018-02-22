using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Idox.LGDP.Apps.Common.OnSite;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.Pages;
using ServiceRequest.ViewModels;
using ServiceRequest.Views.PopUp;
using Xamarin.Forms;
using ServiceRequest.AppContext;

namespace ServiceRequest.Views
{
    public partial class VisitsView
    {

        private bool _isExecute;        
        /// ------------------------------------------------------------------------------------------------
        #region public construtor
        public VisitsView(List<CreateVisitsList> data)
        {
            try
            {
                InitializeComponent();
                Lstvw_Main.ItemsSource = data;
                Lstvw_Main.HeightRequest = data.Count * 100; 
                TapGestureRecognizer actionTapped = new TapGestureRecognizer();
                actionTapped.Tapped += AddVisits;
                Lbl_AddVisis.GestureRecognizers.Add(actionTapped);
                Lstvw_Main.ItemTapped += Visits_Tapped;
                AppContext.AppContext.RefreshVistsList = (sender, args) => OnRefreshList();
                _isExecute = true;
                if (data.Count == 1)
                    Bx_Line.IsVisible = false;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region private functions
        /// <summary>
        /// Refresh the list of vists data
        /// </summary>
        private void OnRefreshList()
        {
            try
            {
                Lstvw_Main.BeginRefresh();
                var iMap = AppData.PropertyModel.SelectedRecord.IndexMap;
                iMap = new IndexMapping(iMap.Property, iMap.RequestGroup, iMap.Record, 0);
                var inspectionList = AppData.PropertyModel.EntityFromMapping(iMap) as SRiInspection;
                var visitsList = new List<CreateVisitsList>();
                if (inspectionList != null)
                {
                    var visitcount = inspectionList.Visits.Count;
                    for (int j = 0; j < visitcount; j++)
                    {
                        var data = inspectionList.Visits[j];
                        var visitsData = new CreateVisitsList()
                        {
                            VisitTypeDescription = data.VisitTypeDescription,
                            ScheduleDate = data.Visit.DateScheduled.ToString("dd MMM yyyy", "Scheduled for ", "Not scheduled"),
                            CompletedDate = data.Visit.DateVisit.ToString("dd MMM yyyy", "Completed on ", "Outstanding"),
                            VisitListIndex = j
                        };
                        visitsList.Add(visitsData);
                    }
                }
                Lstvw_Main.ItemsSource = visitsList;
                Lstvw_Main.HeightRequest = visitsList.Count * 100;
                Lstvw_Main.EndRefresh();
                GC.Collect();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }


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

        /// <summary>
        /// On tapped to Add visits
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private async void AddVisits(object s, EventArgs e)
        {
            try
            {
                SRiRecordMeta record;
                record = AppData.PropertyModel.SelectedRecord.Record;
                var orderedVisits = AppData.ConfigModel.Visits(record.Organisation).OrderBy(a => a.Code + a.Description).ToList();
                var groupedCount =
                    orderedVisits.Select(x => new KeyValuePair<string, string>(x.Code, x.Description))
                        .ToList()
                        .ToGroupedList().Count;
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
                   await Task.Delay(300);
					if (Device.OS == TargetPlatform.iOS)
			      SplitView.PopupContent.ShowPopupRelative(new AddVisitsPopUp(SplitView.PopupContent), Lbl_AddVisis, Width *0.7,GetHeight(stringVisits.Count, groupedCount * GroupedListView.GroupedListViewCellHeight), true, "");
					else
					  SplitView.PopupContent.ShowPopupRelative(new AddVisitsPopUp(SplitView.PopupContent), Lbl_AddVisis, Width * 0.5, GetHeight(stringVisits.Count, groupedCount * GroupedListView.GroupedListViewCellHeight), true, "");	
				SplitView.PopupContent._triangleImage.Source = "";
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
        /// <summary>
        /// On tapped in visits data
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private async void Visits_Tapped(object s, ItemTappedEventArgs e)
        {
            try
            {
                if (_isExecute)
                {
                    _isExecute = false;
                    var data = e.Item as CreateVisitsList;
                    AppData.PropertyModel.SelectedVisit = null;
                    var iMap = AppData.PropertyModel.SelectedRecord.IndexMap;
                    if (data != null)
                    {
                        iMap = new IndexMapping(iMap.Property, iMap.RequestGroup, iMap.Record, 0, data.VisitListIndex);

                        var details = AppData.PropertyModel.SelectedRecord.Record.Record.Inspections[0].Visits[data.VisitListIndex];
                        //Details.Status = SyncStatus.New;
                        AppData.PropertyModel.SelectedVisit = new SelectedVisit(details, iMap, false);
                    }
					//await SplitView.Instace().Navigation.PushModalAsync(new VisitActionPage(),true);
					PageNavigation.PushMainPage(new VisitActionPage());
                    _isExecute = true;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        /// <summary>
        /// To set the height of popup height
        /// </summary>
        /// <param name="count"></param>
        /// <param name="groupedCount"></param>
        /// <returns></returns>
        private double GetHeight(int count, int groupedCount)
        {
				int minHeight;
				double maxHeight;
				minHeight = count * GroupedListView.GroupedListViewCellHeight + groupedCount;
				if (Device.OS == TargetPlatform.iOS)
					maxHeight = SplitView.Instace().Height * 0.7;
				else
					maxHeight = SplitView.Instace().Height * 0.5;

				return maxHeight < minHeight ? maxHeight : minHeight;

        }
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
