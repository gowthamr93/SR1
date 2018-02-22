using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ServiceRequest.Pages;
using Idox.LGDP.Apps.Common.OnSite;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.Models;
using Xamarin.Forms;
using ServiceRequest.AppContext;

namespace ServiceRequest.Views
{
    public partial class InspectionCellView : StackLayout
    {
        /// ------------------------------------------------------------------------------------------------
        #region private variables and properties

        private GroupedRequestGroupModel SriRequestGrp { get; set; }
        private ObservableCollection<GroupedRequestGroupModel> Grouped { get; set; }
        private List<SRiRequestGroup> LstRequestGroup { get; set; }
        private List<int> _lstReqIndex;
        private List<Tuple<int, int, int>> _index;
        private int _initialSectionIndex;
        private int _cellHeight;
        private int _recordIndex;
        private int _propertyIndex;
        private int _requestIndex;
        private static int _refreshCellIndex;
        #endregion
        /// ------------------------------------------------------------------------------------------------

        public static SRiRequestGroupModel CurrentUnifiedItem { get; set; }

        /// ------------------------------------------------------------------------------------------------
        #region Public constructor
        public InspectionCellView(int caseIndex)
        {
            try
            {
                InitializeComponent();
                _initialSectionIndex = caseIndex;
                Orientation = StackOrientation.Vertical;
                HorizontalOptions = LayoutOptions.Fill;
                BackgroundColor = Color.White;
                LstRequestGroup = LstRequestGroup ?? new List<SRiRequestGroup>();
                LstRequestGroup.AddRange(SplitView.HubMaster.LstSRiRequestGroups);
                Grouped = Grouped ?? new ObservableCollection<GroupedRequestGroupModel>();
                Load_Case();
                Lstvw_Inspections.ItemsSource = Grouped;
                Lstvw_Inspections.ItemTapped += Inspection_Tapped;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Private functions

        /// <summary>
        /// Load Request list view
        /// </summary>
        private void Load_Case()
        {
            try
            {
                _cellHeight = 0;
                _recordIndex = 0;
                int reqindcheck = 0;


                _index = new List<Tuple<int, int, int>>();
                _lstReqIndex = new List<int>();
                PropertyMapping temp = PropertyMappingFromIndex(_initialSectionIndex); //To get the property mapping   

                for (int i = 1; i < temp.ActiveExpanded.Count; i++)
                {
                    _propertyIndex = temp.ActiveExpanded[1].Property;
                    var requestGroup = temp.ActiveExpanded[i].RequestGroup;
                    if (requestGroup != null)
                        _requestIndex = requestGroup.Value;

                    if (temp.ActiveExpanded[i].Record != null)
                    {
                        _recordIndex = temp.ActiveExpanded[i].Record.Value;
                        if (!_lstReqIndex.Contains(_requestIndex))
                            _lstReqIndex.Add(_requestIndex);//Request Group index list
                        _index.Add(Tuple.Create(temp.ActiveExpanded[i].Property, _requestIndex, _recordIndex));  //To get the index mapping of request group
                    }
                }

                foreach (var record in LstRequestGroup)
                {
                    int recordHeight = 0;
                    int reqGrpIndex = _lstReqIndex[reqindcheck];
                    SriRequestGrp = new GroupedRequestGroupModel() { GroupName = record.Name };

                    if (record.Records.Count > 0)
                    {
                        //filter mode is ALL
                        if (AppData.PropertyModel.Filter == FilterMode.All || AppData.PropertyModel.Filter == FilterMode.Complete || AppData.PropertyModel.Filter == FilterMode.Incomplete)
                        {
                            List<int> lstRecIndex = new List<int>();
                            foreach (Tuple<int, int, int> tupleindex in _index)
                            {
                                if (tupleindex.Item1 == _propertyIndex && tupleindex.Item2 == reqGrpIndex)
                                {
                                    lstRecIndex.Add(tupleindex.Item3);//Records index list
                                }
                            }

                            for (int i = 0; i < (record.Records.Count > 1 ? record.TempRecords.Count : record.Records.Count); i++)
                            {
                                SriRequestGrp.Add(new SRiRequestGroupModel()
                                {
                                    GroupDetails = record.Records.Count > 1 ? record.TempRecords[i].Record.Details : record.Records[i].Record.Details ?? "No details given",
                                    TargetResponseDate = record.Records.Count > 1 ? record.TempRecords[i].Record.TargetResponseDate.ToDateString() : record.Records[i].Record.TargetResponseDate.ToDateString(),
                                    ShortIndex = new ShortIndexMapping(_initialSectionIndex, reqGrpIndex, lstRecIndex[i], _propertyIndex),
                                    GrayLine = (record.Records.Count > 1 ? record.TempRecords.Count - 1 : record.Records.Count - 1) != i, // To set the gray line between records of same groupname,
                                    UploadStatusImage = GetSyncStatus(record.Records.Count > 1 ? record.TempRecords[i].Record.Status : record.Records[i].Record.Status)
                                });
                                recordHeight++;
                            }
                            _cellHeight++;
                            Grouped.Add(SriRequestGrp);
                        }
                        #region future check
                        //fileter mode is Complete
                        else if (AppData.PropertyModel.Filter == FilterMode.Complete)
                        {
                            List<int> recIndex = new List<int>();
                            for (int s = 0; s < _index.Count; s++)
                            {

                                if (_index[s].Item1 == _propertyIndex && _index[s].Item2 == reqGrpIndex)
                                {
                                    recIndex.Add(_index[s].Item3);
                                }
                            }

                            for (int i = 0; i < record.Records.Count; i++)
                            {

                                SriRequestGrp.Add(new SRiRequestGroupModel()
                                {
                                    GroupDetails = record.Records[i].Record.Details ?? "No details given",
                                    TargetResponseDate = record.Records[i].Record.TargetResponseDate.ToDateString(),
                                    ShortIndex = new ShortIndexMapping(_initialSectionIndex, reqGrpIndex, recIndex[i], _propertyIndex),
                                    GrayLine = (record.Records.Count - 1) != i // To set the gray line between records of same groupname
                                });
                            }
                            _cellHeight++;
                            Grouped.Add(SriRequestGrp);
                        }
                        //fileter mode is Incomplete
                        else
                        {
                            List<int> recIndex = new List<int>();
                            for (int s = 0; s < _index.Count; s++)
                            {
                                if (_index[s].Item1 == _propertyIndex && _index[s].Item2 == reqGrpIndex)
                                {
                                    recIndex.Add(_index[s].Item3);
                                }
                            }
                            for (int i = 0; i < record.Records.Count; i++)
                            {
                                SriRequestGrp.Add(new SRiRequestGroupModel()
                                {
                                    GroupDetails = record.Records[i].Record.Details ?? "No details given",
                                    TargetResponseDate = record.Records[i].Record.TargetResponseDate.ToDateString(),
                                    ShortIndex = new ShortIndexMapping(_initialSectionIndex, reqGrpIndex, recIndex[i], _propertyIndex),
                                    GrayLine = (record.Records.Count - 1) != i // To set the gray line between records of same groupname
                                });
                            }
                            _cellHeight++;
                            Grouped.Add(SriRequestGrp);
                        }
                        #endregion
                        var height = recordHeight == 2 ? recordHeight * Device.OnPlatform<double>(38, 38, 36) : 0;

                        HeightRequest = (_cellHeight * Device.OnPlatform<double>(108, 108, 124)) + height;
                        //To Fix the height of listgroup
                    }
                    reqindcheck++;
                }
                GC.Collect();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        public static void DeSelectItem()
        {
            try
            {
                CurrentUnifiedItem = null;
                SplitView.HubMaster.MakeDeleteVisible(false);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }

        }

        /// <summary>
        /// To get the sync status indication
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private string GetSyncStatus(SyncStatus status)
        {
            try
            {
                string image;
                switch (status)
                {
                    case SyncStatus.New:
                        image = "new_upload.png";
                        break;
                    case SyncStatus.Changed:
                        image = "pending_upload.png";
                        break;
                    case SyncStatus.Saved:
                        image = "uploaded.png";
                        break;
                    default:
                        image = "";
                        break;
                }
                return image;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }

        /// <summary>
        ///  Get the property mapping from selected index using ShortIndexMapping class
        /// </summary>
        /// <param name="shortIndex"></param>
        /// <returns></returns>
        public PropertyMapping PropertyMappingFromIndex(ShortIndexMapping shortIndex)
        {
            try
            {
                PropertyMapping pMap;
                if (shortIndex.Section >= 0 && (AppData.PropertyModel.Filter == FilterMode.All || AppData.PropertyModel.Filter == FilterMode.Complete || AppData.PropertyModel.Filter == FilterMode.Incomplete))
                {
                    pMap = AppData.PropertyModel.Mappings[shortIndex.Section];
                }
                else
                    throw new ArgumentException(
                        string.Format("The section index ({0}) is out of bounds for the PropertyModel.Mappings collection (count={1}).",
                                      shortIndex.Section, AppData.PropertyModel.Mappings.Count));
                return pMap;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Getting Property mapping from index
        /// </summary>
        /// <param name="shortIndex"></param>
        /// <returns></returns>
        private PropertyMapping PropertyMappingFromIndex(int shortIndex)
        {
            try
            {
                PropertyMapping pMap;
                if (shortIndex >= 0 && shortIndex < AppData.PropertyModel.Mappings.Count)
                {
                    pMap = AppData.PropertyModel.Mappings[shortIndex];
                }
                else
                    throw new ArgumentException(
                        string.Format("The section index ({0}) is out of bounds for the PropertyModel.Mappings collection (count={1}).",
                                      shortIndex, AppData.PropertyModel.Mappings.Count));

                return pMap;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region public functions


        /// <summary>
        /// To Refresh the Requestgroup list
        /// </summary>
        public void RefreshList()
        {
            try
            {
                Grouped = new ObservableCollection<GroupedRequestGroupModel>();
                Lstvw_Inspections.BeginRefresh();
                _initialSectionIndex = _refreshCellIndex;
                SplitView.HubMaster.LstSRiRequestGroups = AppData.PropertyModel.SelectedProperty.RequestGroups;
                var lstView = HubMasterView.CaseListView.MEntries[_initialSectionIndex]?.View.FindByName<ListView>("Lstvw_Inspections");
                SplitView.HubMaster.LoadRequestGrp();
                LstRequestGroup = SplitView.HubMaster.LstSRiRequestGroups;
                Load_Case();
                if (lstView != null) lstView.ItemsSource = Grouped;
                Lstvw_Inspections.EndRefresh();

                AppContext.AppContext.CaseCell.RefreshList(_initialSectionIndex);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }



        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Private functions

        /// Name		Inspection_Tapped
        /// 
        /// <summary>
        /// Catches the item selected in the listview.
        /// </summary>
        /// 
        /// <param name="sender">			The Inspection_RowSeleted instance.</param>
        /// <param name="e">				The event arguments of the row being selected.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async void Inspection_Tapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var item = e.Item as SRiRequestGroupModel;
                CurrentUnifiedItem = (SRiRequestGroupModel)e.Item;
                if (item != null)
                {
                    var pMap = PropertyMappingFromIndex(item.ShortIndex); //get the property mapping from shortindex
                    _refreshCellIndex = item.ShortIndex.Section;
                    int index = 0;
                    foreach (var indexMap in pMap.ActiveExpanded)
                    {
                        if (indexMap.Property == item.ShortIndex.PropertyIndex && indexMap.RequestGroup == item.ShortIndex.RequestGroupIndex &&
                            indexMap.Record == item.ShortIndex.RecordIndex)
                        {
                            var iMap = pMap.ActiveExpanded[index];
                            //check the indexmapping type
                            if (iMap.Type == MappingType.Record)
                            {
                                var record = AppData.PropertyModel.EntityFromMapping(iMap) as SRiRecordMeta;
                                if (record != null && (AppData.PropertyModel.SelectedRecord == null || !AppData.PropertyModel.SelectedRecord.Record.Record.KeyVal.Equals(record.Record?.KeyVal)))
                                {
                                    if (AppData.PropertyModel.SelectedRecord != null && AppData.PropertyModel.SelectedRecord.Edited)
                                    {
                                        if (await SplitView.DisplayAlert("Unsaved changes", "Not all changes have been saved, what would like to do?", "Save", "Discard"))
                                        {
                                            Exception error;
                                            AppData.PropertyModel.SaveRecord(out error);
                                            if (error != null)
                                                await SplitView.DisplayAlert("Save failed", error.Message, "OK", null);

                                            AppData.PropertyModel.SelectedRecord = new SelectedRecord(record, iMap, false);
                                            await SplitView.Instace().PushRightContent(new RecordSummaryView());
                                        }
                                        else
                                        {
                                            AppData.PropertyModel.SelectedRecord = new SelectedRecord(record, iMap, false);
                                            await SplitView.Instace().PushRightContent(new RecordSummaryView());
                                        }
                                    }
                                    else
                                    {
                                        AppData.PropertyModel.SelectedRecord = new SelectedRecord(record, iMap, GetSyncStatus(record.Record.Status) == "new_upload.png" ? true : false);
                                        await SplitView.Instace().PushRightContent(new RecordSummaryView());
                                    }
                                }
                            }
                            break;
                        }
                        index++;
                    }
                }
                if (Device.OS == TargetPlatform.Android)
                    SplitView.HubMaster.MakeDeleteVisible(CurrentUnifiedItem.UploadStatusImage == "new_upload.png");
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
