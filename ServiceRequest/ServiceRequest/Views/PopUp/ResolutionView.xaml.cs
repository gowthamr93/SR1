using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.AppContext;
using ServiceRequest.Pages;
using Xamarin.Forms;

namespace ServiceRequest.Views.PopUp
{
    public partial class ResolutionView : ContentView
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables
        /// ------------------------------------------------------------------------------------------------
        ///
        private readonly ObservableCollection<SRiResolution> _mConflictResolution = new ObservableCollection<SRiResolution>();
        private readonly ObservableCollection<SRiResolution> _mDeleteResolution = new ObservableCollection<SRiResolution>();       
        private readonly ScrollView _mScrollView;
        ///
        #endregion


        #region Constructor

        /// ------------------------------------------------------------------------------------------------
        /// 
        public ResolutionView(List<SRiResolution> resolutions)
        {
			try
			{
				InitializeComponent();
				//
				foreach (var res in resolutions)
				{
					if (res.Issue== SRiResolutionIssue.Conflict)
						_mConflictResolution.Add(res);
					else if(res.Issue== SRiResolutionIssue.Deletion)
						_mDeleteResolution.Add(res);
				}
               
                _mScrollView = new ScrollView
                {
                    IsClippedToBounds = true,
                    Orientation = ScrollOrientation.Vertical,
                    Content = S_RecordSummary,
                };
                Gl_Main.Children.Add(_mScrollView,0,2);
                Load();
			    TapGestures();
                Lst_Conflict.ItemTapped += OnConflictItemTapped;
				Lst_Delete.ItemTapped += OnDeletionItemTapped;
			}
				catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());
			}
        }

        #endregion

        /// ------------------------------------------------------------------------------------------------

        #region Private Functions


        /// ------------------------------------------------------------------------------------------------
        /// Name		OnDone
        /// 
        /// <summary>	Resolves the resolution that is selected and close the Popup.
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> event arguments</param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        ///
        private void OnDone(object sender, EventArgs e)
        {
            try
            {              
                foreach (var res in _mConflictResolution)
                {
                    res.Resolve();
                }
                foreach (var res in _mDeleteResolution)
                {
                    res.Resolve();
                }

                _mConflictResolution.Clear();
                _mDeleteResolution.Clear();
                               
                SplitView.CenterPopupContent.DismisPopup();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private void TapGestures()
        {
            TapGestureRecognizer tapDone = new TapGestureRecognizer();
            tapDone.Tapped += OnDone;
            Lbl_Done.GestureRecognizers.Add(tapDone);

            TapGestureRecognizer tapConflictHelp = new TapGestureRecognizer();
            tapConflictHelp.Tapped += ConflictHelp;
            Help_Conflict.GestureRecognizers.Add(tapConflictHelp);

            TapGestureRecognizer tapDeleteHelp = new TapGestureRecognizer();
            tapDeleteHelp.Tapped += DeletionHelp;
            Help_Delete.GestureRecognizers.Add(tapDeleteHelp);
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name		DeletionHelp
        /// 
        /// <summary>	Opens an display alert about the Deletion Description.
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> event arguments</param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        ///
        private async void DeletionHelp(object sender, EventArgs e)
        {
            try
            {
				if (Device.OS == TargetPlatform.iOS)
				{
					DependencyService.Get<IDisplayAlertPopup>().DeletionHelp();
				}
				else
                await SplitView.DisplayAlert("Deletion", "Not all items were recognised by the cloud, these items are either deleted from the cloud or referenced under a new ID. \n\nYou can either mark these items for deletion now, or make a note of the changes you have made and delete them at a later date. ", "Ok", null);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
           
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name		ConflictHelp
        /// 
        /// <summary>	Opens an display alert about the Conflict Description.
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> event arguments</param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        ///
        private async void ConflictHelp(object sender, EventArgs e)
        {
            try
            {
				if (Device.OS == TargetPlatform.iOS)
				{
					DependencyService.Get<IDisplayAlertPopup>().ConflictHelp();
				}
				else
                await SplitView.DisplayAlert("Conflicts", "These items have conflicted with items in the cloud, you could be overwriting changes made by a Uniform user. \nSelect any items you wish to continue to upload and the cloud version will be overwritten. \n\nA record of any conflict will be available in the task log.", "Ok", null);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
           
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name		Load
        /// 
        /// <summary>	Loads the data to the view while appearing.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------

        private void Load()
        {
            
            try
            {               
                if (_mConflictResolution.Count > 0)
                {
                    Lst_Conflict.ItemsSource = _mConflictResolution;
                    Lst_Conflict.HeightRequest = _mConflictResolution.Count*200;
                }
                else
                {
                    Lst_Conflict.IsVisible = false;
                    G_Conflict.IsVisible = false;
                }
                if (_mDeleteResolution.Count > 0)
                {               
                   Lst_Delete.ItemsSource = _mDeleteResolution;
                   Lst_Delete.HeightRequest = _mDeleteResolution.Count*200;                                  
                }
                else
                {
                    Lst_Delete.IsVisible = false;
                    G_Delete.IsVisible = false;
                }

            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }

        }

        /// ------------------------------------------------------------------------------------------------
        /// ------------------------------------------------------------------------------------------------
        /// Name		OnConflictItemTapped
        /// 
        /// <summary>	Executes when the Conflict item is being tapped.
        /// </summary>
        /// <param name="sender"> </param>
        ///   /// <param name="e">Item tapped event arguments</param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void OnConflictItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                SRiResolution model = (SRiResolution) Lst_Conflict.SelectedItem;

                if (model != null)
                {
                    var selectedIndex = _mConflictResolution.IndexOf(model);
                    model.Selected = !model.Selected;
                    _mConflictResolution[selectedIndex] = model;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        /// ------------------------------------------------------------------------------------------------
        /// ------------------------------------------------------------------------------------------------
        /// Name		OnDeletionItemTapped
        /// 
        /// <summary>	Executes when the Deletion item is being tapped.
        /// </summary>
        /// <param name="sender"> </param>
        ///   /// <param name="e">Item tapped event arguments</param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void OnDeletionItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                SRiResolution model = (SRiResolution)Lst_Delete.SelectedItem;

                if (model != null)
                {
                    var selectedIndex = _mDeleteResolution.IndexOf(model);
                    model.Selected = !model.Selected;
                    _mDeleteResolution[selectedIndex] = model;
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
