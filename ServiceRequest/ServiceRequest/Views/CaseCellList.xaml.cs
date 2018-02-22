using System;
using System.Collections.Generic;
using Idox.LGDP.Apps.Common.OnSite;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.AppContext;
using ServiceRequest.Pages;
using ServiceRequest.ViewModels;
using Xamarin.Forms;

namespace ServiceRequest.Views
{
    public partial class CaseCellList : ContentView
    {
        /// ------------------------------------------------------------------------------------------------
        #region private properties and variables
        private List<RequestGroupViewModel> LstReqGrpData
        {
            get { return _sRiRequestGroup ?? (_sRiRequestGroup = new List<RequestGroupViewModel>()); }
            set { _sRiRequestGroup = value; }
        }
        private List<RequestGroupViewModel> _sRiRequestGroup;

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public constructor

        public CaseCellList(SRiProperty data, int alphabets)
        {
            try
            {
                InitializeComponent();
                SetSyncStatus(data.Status);
                Lstvw_RequestGroup.ItemsSource = Lst_Load();
                BindingContext = new
                {
                    Alphabet = alphabets.IndexToAlphabet(),
                    //TradeName = data.TradeName ?? data.Address.Lines[0],
                    TradeName = string.IsNullOrWhiteSpace(data.TradeName) ? data.Address.ShortAddress : data.TradeName,
                    Address = data.Address.ShortAddress.Split(Environment.NewLine.ToCharArray())[0].Trim(),
                };
                TapGestures(alphabets);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Private Functions

        /// <summary>   
        /// Get the sync status
        /// </summary>
        private void SetSyncStatus(SyncStatus status)
        {
            try
            {
                switch (status)
                {
                    case SyncStatus.Changed:
                        //Img_NewStatus.IsVisible = false;
                        Img_DoneUpload.IsVisible = false;
                        Img_PendingUpload.IsVisible = true;
                        Img_NewCase.IsVisible = false;
                        break;
                    case SyncStatus.New:
                        // Img_NewStatus.IsVisible = true;
                        Img_DoneUpload.IsVisible = false;
                        Img_PendingUpload.IsVisible = false;
                        Img_NewCase.IsVisible = true;
                        break;
                    case SyncStatus.Saved:
                        //Img_NewStatus.IsVisible = false;
                        Img_DoneUpload.IsVisible = true;
                        Img_PendingUpload.IsVisible = false;
                        Img_NewCase.IsVisible = false;
                        break;
                    default:
                        //Img_NewStatus.IsVisible = false;
                        Img_DoneUpload.IsVisible = false;
                        Img_PendingUpload.IsVisible = false;
                        Img_NewCase.IsVisible = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        /// <summary>
        /// On tapped the caselistview layout
        /// </summary>
        /// <param name="caseIndex"></param>
        private void TapGestures(int caseIndex)
        {
            try
            {
                var tapCaseCell = new TapGestureRecognizer();
                if (Device.OS == TargetPlatform.Android)
                {
                    Lstvw_RequestGroup.ItemTapped += (s, e) => HubMasterView.CaseListView.OnCellTouchUpInside(caseIndex);
                }
                Lstvw_RequestGroup.ItemTapped += UnSelectList;
                tapCaseCell.Tapped += (s, e) => HubMasterView.CaseListView.OnCellTouchUpInside(caseIndex);
                Main_Layout.GestureRecognizers.Add(tapCaseCell);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        /// <summary>
        /// List data loaded from Sriproperty 
        /// </summary>
        /// <returns></returns>
        private List<RequestGroupViewModel> Lst_Load()
        {
            try
            {
                //List<RequestGroupViewModel> Lst_ReqGrpData = new List<RequestGroupViewModel>();
                foreach (SRiRequestGroup sriReqGrp in SplitView.HubMaster.LstSRiRequestGroups)
                {
                    var customHeight = SplitView.HubMaster.LstSRiRequestGroups.Count * Device.OnPlatform(65, 95, 60);
                    Main_Layout.HeightRequest = customHeight + Device.OnPlatform(70, 70, 70);
                    Main_Grid.HeightRequest = Main_Layout.HeightRequest + 1;

                    LstReqGrpData.Add(new RequestGroupViewModel
                    {
                        GroupName = sriReqGrp.Name,
                        TargetResponse = sriReqGrp.EarliestTargetDate.ToString("dddd dd MMM HH:mm", "", "No target date"),
                        ScheduledDate = Schedule(sriReqGrp).ToString("dddd dd MMM HH:mm", "", "No Scheduled date"),
                        RecordCount = sriReqGrp.Records.Count
                    });

                }
                return LstReqGrpData;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Returns the scheduled date to sriReqGrp
        /// </summary>
        private DateTime? Schedule(SRiRequestGroup sriReqGrp)
        {
            try
            {
                DateTime? dt = null;
                foreach (var rec in sriReqGrp.Records)
                {

                    foreach (var res in rec.Record.Inspections)
                    {
                        if (!dt.HasValue ||
                            res.EarliestScheduledDate.HasValue && res.EarliestScheduledDate.Value < dt.Value)
                            dt = res.EarliestScheduledDate;
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// Handles the background highlight 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region public functions
        /// <summary>
        /// To Refresh the Requestgroup list
        /// </summary>
        public void RefreshList(int index)
        {
            try
            {
                HubMasterView.CaseListView.SetSyncStatusManual(AppData.PropertyModel.SelectedProperty.Status, index);
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
