using ServiceRequest.Models;
using ServiceRequest.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ServiceRequest.Pages;
using Xamarin.Forms;
using ServiceRequest.AppContext;

namespace ServiceRequest.Views.PopUp
{
    public partial class GroupedListView
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public Variables and Properties
        /// 
        public const int GroupedListViewPadding = 15;
        public const int GroupedListViewCellHeight = 40;
        public ObservableCollection<Grouping<string, GroupedListModel>> listData;
        public string SelectedValue { get; set; }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        public GroupedListView(List<KeyValuePair<string, string>> grouptedList, object objLabel)
        {
            try
            {
                InitializeComponent();

                listData = grouptedList.ToGroupedList();

                lstvw_GroupedList.ItemsSource = listData;

                lstvw_GroupedList.ItemTapped += (sender, args) =>
                {
                    var label = objLabel as Label;
                    if (label != null) label.Text = SelectedValue = ((GroupedListModel)lstvw_GroupedList.SelectedItem).Description;

                    if (Device.OS == TargetPlatform.iOS)
                    {
						if (VisitActionDetailsPage.RelativePopup!=null&&!VisitActionDetailsPage.RelativePopup.PopupChanged)
						{

							if (ParagraphView.RelativePopup != null)

							{
								ParagraphView.RelativePopup.DismisPopup();
								ParagraphView.RelativePopup = null;
							}

							else
							{
								if (Application.Current.MainPage.GetType() == typeof(VisitActionDetailsPage))
									VisitActionDetailsPage.RelativePopup?.DismisPopup();
								else if (Application.Current.MainPage.GetType() == typeof(VisitActionPage))
									VisitActionPage.PopupContent?.DismisPopup();
							}
						}
						else 
						{
							if (Application.Current.MainPage.GetType() == typeof(VisitActionDetailsPage))
								VisitActionDetailsPage.RelativePopup?.DismisPopup();
							else if (Application.Current.MainPage.GetType() == typeof(VisitActionPage))
								VisitActionPage.PopupContent?.DismisPopup();
						}
                    }
                    else
                    {
                        if (Application.Current.MainPage.GetType() == typeof(VisitActionDetailsPage))
                            VisitActionDetailsPage.RelativePopup?.DismisPopup();
                        else if (Application.Current.MainPage.GetType() == typeof(VisitActionPage))
                            VisitActionPage.PopupContent?.DismisPopup();
                    }
                };
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Functions

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SelectedIndex
        /// 
        /// <summary>
        /// Returns the selected Index.
        /// </summary>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        ///
        public int SelectedIndex()
        {
            try
            {
                int index = GroupedListViewModel.SelectedIndex((GroupedListModel)lstvw_GroupedList.SelectedItem);
                return index;

            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return -1;
            }
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
