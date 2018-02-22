using Idox.LGDP.Apps.ServiceRequest.Client;
using PopUpSample;
using ServiceRequest.Models;
using ServiceRequest.ViewModels;
using System.Collections.Generic;
using System.Linq;
using ServiceRequest.Pages;
using Xamarin.Forms;
using System;
using ServiceRequest.AppContext;

namespace ServiceRequest.Views.PopUp
{
    public partial class AddActionView
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables
        /// 
        private readonly PopupLayouts _popupContent;

        private static bool _isExecute;
        private VisitActionPage VisitInstance { get; set; }

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        /// 
        ///  ------------------------------------------------------------------------------------------------
        ///  <summary>
        ///   Initializes a new instance of the class
        ///  </summary>
        ///  <param name="visitActionView"></param>
        /// <param name="parentPopup"></param>
        public AddActionView(VisitActionPage visitActionView, PopupLayouts parentPopup)
        {
            try
            {
                InitializeComponent();
                _popupContent = parentPopup;
                VisitInstance = visitActionView;
                var sriVisit = AppData.PropertyModel.SelectedVisit.Visit;
                Lstvw_Actions.ItemsSource = AppData.ConfigModel.Actions(sriVisit.Organisation, sriVisit.Visit.VisitType)
                    .OrderBy(a => a.Historic + a.Description)
                    .Select(x => new KeyValuePair<string, string>(x.Code, x.Description))
                    .ToList().ToGroupedList();
                _isExecute = true;
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
        /// Item tapped in the list
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
                    _popupContent.DismisPopup();
                    var selectedItem = (GroupedListModel)e.Item;

                    var visitMap = AppData.PropertyModel.SelectedAction.IndexMap;

                    var newAction = new SelectedAction(AppData.PropertyModel.SelectedAction.Action, visitMap, true)
                    {
                        Action = { ActionType = selectedItem.Code }
                    };

                    await VisitInstance.LoadActionsForm(newAction);
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
