using System;
using System.Collections.Generic;
using System.Linq;
using Idox.LGDP.Apps.Common.OnSite;
using Idox.LGDP.Apps.ServiceRequest.Client;
using PopUpSample;
using ServiceRequest.Models;
using ServiceRequest.Pages;
using ServiceRequest.ViewModels;
using Xamarin.Forms;
using ServiceRequest.AppContext;

namespace ServiceRequest.Views.PopUp
{

    public partial class AddVisitsPopUp
    {
        /// ------------------------------------------------------------------------------------------------
        #region private variable and properties

        private PopupLayouts PopupContent;
        private bool _isExecute;

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region public constructor
        public AddVisitsPopUp(PopupLayouts parentPopup)
        {
            try
            {
                InitializeComponent();
                _isExecute = true;
                PopupContent = parentPopup;
                SRiRecordMeta record;
                record = AppData.PropertyModel.SelectedRecord.Record;
                Lstvw_Actions.ItemsSource = AppData.ConfigModel.Visits(record.Organisation).OrderBy(a => a.Code + a.Description).Select(x => new KeyValuePair<string, string>(x.Code, x.Description)).ToList().ToGroupedList();
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
        /// Item tapped in visits list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ActionsItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                if (_isExecute)
                {
                    _isExecute = false;
                    PopupContent.DismisPopup();
                    var items = (GroupedListModel)e.Item;
                    SRiRecordMeta record;
                    record = AppData.PropertyModel.SelectedRecord.Record;
                    var entityKey = Guid.NewGuid().ToString();
                    AppData.PropertyModel.SelectedVisit = new SelectedVisit(new SRiVisitMeta()
                    {
                        Organisation = record.Organisation,
                        EntityMeta = new SRiEntityMeta("entitykey", entityKey),
                        Received = "2",
                        ID = entityKey,
                        Version = "",
                        Visit = new SRiVisit()
                        {
                            EntityKey = entityKey,
                            KeyVal = OnSiteSettings.NewTempKey,
                            UVersion = "",
                            InspectionKeyVal = record.Record.Inspections[0].KeyVal,
                            MDKeyVal = record.Record.Inspections[0].MdKeyVal,
                            MDSubSys = record.Record.Inspections[0].MdSubSys,
                            IdoxID = AppData.MainModel.CurrentUser.IdoxId,
                            RecordEntityKey = record.Record.EntityKey,
                            VisitType = items.Code,
							//Officer= AppData.MainModel.Environment!=OnSiteEnvironments.Sales? AppData.MainModel.CurrentUser.OfficerCode(record.Organisation):"Demo",
                        }
                    }, AppData.PropertyModel.SelectedRecord.NewVisitMapping(0), true);
                    AppData.PropertyModel.SelectedVisit.Visit.EntityMeta.StringFields.Add("str_idoxid", AppData.MainModel.CurrentUser.IdoxId);
                    AppData.PropertyModel.SelectedVisit.Visit.Visit.Status = SyncStatus.New;
					//await SplitView.Instace().Navigation.PushModalAsync(new VisitActionPage(),true);
					//App.Current.MainPage = new VisitActionPage();
                    PageNavigation.PushMainPage(new VisitActionPage());
                   _isExecute = true;
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
