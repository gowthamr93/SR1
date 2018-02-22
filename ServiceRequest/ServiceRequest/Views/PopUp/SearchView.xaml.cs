using System;
using System.Collections.ObjectModel;
using System.Text;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.Pages;
using Xamarin.Forms;
using ServiceRequest.AppContext;


namespace ServiceRequest.Views.PopUp
{
    public partial class SearchView
    {
        /// ------------------------------------------------------------------------------------------------

        #region Private Variables
        /// ------------------------------------------------------------------------------------------------
        /// 
        private ObservableCollection<SRiSearchResult> _sriSearchResult;


        private bool _isSelected;
        #endregion

        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------

        #region Constructor

        /// ------------------------------------------------------------------------------------------------
        /// 
        public SearchView()
        {
            try
            {
                InitializeComponent();
                TapGestureRecognizer tapCancel = new TapGestureRecognizer();
                tapCancel.Tapped += OnCancel;
                Lbl_SearchCancel.GestureRecognizers.Add(tapCancel);
                SrchBar.SearchButtonPressed += Search;
                Lst_Search.ItemTapped += OnItemTapped;
                TapGestureRecognizer tapSave = new TapGestureRecognizer();
                tapSave.Tapped += OnSave;
                Lbl_SearchSave.GestureRecognizers.Add(tapSave);
                SrchBar.TextChanged += SearchTextEntered;
                SrchBar.SearchButtonPressed += SearchEntered;
                Lbl_SearchSave.TextColor = Color.Gray;
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

        /// ------------------------------------------------------------------------------------------------
        /// ------------------------------------------------------------------------------------------------
        /// Name		OnCancel
        /// 
        /// <summary>	Dismisses the popup on clicking Cancel button.
        /// </summary>
        /// <param name="sender"> </param>
        ///   /// <param name="e"> event arguments</param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void OnCancel(object sender, EventArgs e)
        {
            try
            {
                AppData.PropertyModel.SearchResults.Clear();
                SplitView.CenterPopupContent.DismisPopup();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        /// ------------------------------------------------------------------------------------------------
        /// ------------------------------------------------------------------------------------------------
        /// Name		OnSave
        /// 
        /// <summary>	Saves the search results and dismisses the popup on clicking Save button.
        /// </summary>
        /// <param name="sender"> </param>
        ///   /// <param name="e"> event arguments</param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        ///
        private void OnSave(object sender, EventArgs e)
        {
            try
            {
                if (!Lbl_SearchSave.IsEnabled) return;
                if (!_isSelected) SplitView.CenterPopupContent.DismisPopup();
                else
                {
                    SplitView.CenterPopupContent.DismisPopup();
                    SplitView.Instace().FilterCheckAvailable(true);
                    AppData.PropertyModel.SaveSearchResults();
                    SplitView.HubMaster.FindByName<Label>("LblSearchHint").IsVisible = false;
                    AppData.PropertyModel.SearchResults.Clear();
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        /// ------------------------------------------------------------------------------------------------
        /// ------------------------------------------------------------------------------------------------
        /// Name		OnItemTapped
        /// 
        /// <summary>	Executes when the item is being tapped.
        /// </summary>
        /// <param name="sender"> </param>
        ///   /// <param name="e">Item tapped event arguments</param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                SRiSearchResult model = (SRiSearchResult)Lst_Search.SelectedItem;

                if (model != null)
                {
                    var selectedIndex = _sriSearchResult.IndexOf(model);
                    model.Selected = !model.Selected;
                    _sriSearchResult[selectedIndex] = model;
                    CheckSaveAvailable();
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        /// ------------------------------------------------------------------------------------------------
        /// ------------------------------------------------------------------------------------------------
        /// Name		Search
        /// 
        /// <summary>	Executes when the search bar is pressed.
        /// </summary>
        /// <param name="sender"> </param>
        ///   /// <param name="e"> event arguments</param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async void Search(object sender, EventArgs e)

        {
            try
            {
                _sriSearchResult = new ObservableCollection<SRiSearchResult>();

                if (string.IsNullOrEmpty(SrchBar.Text))
                    return;

                // Clear the current search results.

                if (AppData.PropertyModel.SearchResults != null)
                {
                    AppData.PropertyModel.SearchResults.Clear();
                }

                // Load demo datas in SearchResults.

                await AppData.API.Search(SrchBar.Text);

                if (AppData.PropertyModel.SearchResults != null && AppData.PropertyModel.SearchResults.Count != 0)
                {
                    Lst_Search.IsVisible = true;
                    foreach (SRiSearchResult demoData in AppData.PropertyModel.SearchResults)
                    {
                        _sriSearchResult.Add(demoData);
                    }
                    Lst_Search.ItemsSource = _sriSearchResult;
                }
                else
                    Lst_Search.IsVisible = false;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }




        /// ------------------------------------------------------------------------------------------------
        /// ------------------------------------------------------------------------------------------------
        /// Name		CheckSaveAvailable
        /// 
        /// <summary>	Enables the search button by checking condition
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void CheckSaveAvailable()
        {

            foreach (SRiSearchResult result in _sriSearchResult)
                if (result.Selected)
                {
                    Lbl_SearchSave.IsEnabled = true;
                    _isSelected = true;
                    Lbl_SearchSave.TextColor = Styles.MainAccent;
                    break;
                }
                else
                {
                    Lbl_SearchSave.IsEnabled = false;
                    _isSelected = false;
                    Lbl_SearchSave.TextColor = Color.Gray;
                }
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name		SearchTextEntered
        /// 
        /// <summary>	Handles the OnText entered event in the the search bar.
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> event arguments</param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        ///
        private void SearchTextEntered(object sender, EventArgs e)
        {
            try
            {
                var sb = new StringBuilder();
                foreach (char text in SrchBar.Text)
                {
                    if(!char.IsWhiteSpace(text))
                        sb.Append(text);
                }
                SrchBar.Text = sb.ToString().ToUpper();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name		SearchEntered
        /// 
        /// <summary>	Used for changing the Save button colour and IsEnabled Property
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> event arguments</param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        ///
        private void SearchEntered(object sender, EventArgs e)
        {
            try
            {
                Lbl_SearchSave.TextColor = Color.Gray;
                Lbl_SearchSave.IsEnabled = false;
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
