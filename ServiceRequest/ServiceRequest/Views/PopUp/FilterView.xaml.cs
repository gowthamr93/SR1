using System;
using PopUpSample;
using Xamarin.Forms;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.Models;
using ServiceRequest.Pages;
using ServiceRequest.AppContext;

namespace ServiceRequest.Views.PopUp
{
    public partial class FilterView
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Properties

        private FilterByRequestType _filterByRequestType;
        private bool _isExecute;
        private PopupLayouts PopupLayout { get; }
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="T:FilterView{T}"/> class.
        /// </summary>
        public FilterView(PopupLayouts popupLayouts)
        {
            try
            {
                InitializeComponent();
                PopupLayout = popupLayouts;
                ChangeButton(AppData.PropertyModel.Filter);
                ToggleSort(AppData.PropertyModel.Sort);
                int index = 0;
                btn_Incomplete.FontSize = FontSizeView.CustomFontSizeSmSmMi;
                btn_Complete.FontSize = FontSizeView.CustomFontSizeSmSmMi;
                btn_All.FontSize = FontSizeView.CustomFontSizeSmSmMi;
                foreach (var item in AppData.PropertyModel.FilterTypes)
                {
                    if (!String.IsNullOrEmpty(item.Value))
                    {
                        _filterByRequestType = new FilterByRequestType()
                        {
                            Text = item.Value,
                            IsVisible = AppContext.AppContext.GetListIndex != null && AppContext.AppContext.GetListIndex.Value == index
                                        && !AppContext.AppContext.CheckFilterTickImage,
                            index = index,
                            Key = item.Key
                        };
                        Sl_RequestTypeList.Children.Add(new FilterByRequest(_filterByRequestType, PopupLayout));
                    }
                    index++;
                }
                _isExecute = true;
                TapGestures();
               
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
        ///Fire at All Button Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAll_OnClicked(object sender, EventArgs e)
        {
            try
            {
                ChangeButton(AppData.PropertyModel.Filter = FilterMode.All);
                PopupLayout.DismisPopup();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		BtnComplete_OnClicked
        /// 
        /// <summary>
        /// Handles operation for clicking on Complete button.
        /// </summary>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        ///
        private async void BtnComplete_OnClicked(object sender, EventArgs e)
        {
            try
            {
                ChangeButton(AppData.PropertyModel.Filter = FilterMode.Complete);
                PopupLayout.DismisPopup();
                await SplitView.Instace().Clear();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		BtnIncomplete_OnClicked
        /// 
        /// <summary>
        /// Handles operation for clicking on InComplete button.
        /// </summary>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        ///
        private async void BtnIncomplete_OnClicked(object sender, EventArgs e)
        {
            try
            {
                ChangeButton(AppData.PropertyModel.Filter = FilterMode.Incomplete);
                PopupLayout.DismisPopup();
                await SplitView.Instace().Clear();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        /// <summary>
        /// Change the button color based on selection of mode
        /// </summary>
        /// <param name="filterMode"></param>
        private void ChangeButton(FilterMode filterMode)
        {
            try
            {
                switch (filterMode)
                {
                    case FilterMode.All:
                        btn_All.TextColor = Color.White;
                        btn_All.BackgroundColor = Styles.MainAccent;
                        btn_Complete.TextColor = Styles.MainAccent;
                        btn_Complete.BackgroundColor = Color.White;
                        btn_Incomplete.TextColor = Styles.MainAccent;
                        btn_Incomplete.BackgroundColor = Color.White;
                        break;
                    case FilterMode.Complete:
                        btn_All.TextColor = Styles.MainAccent;
                        btn_All.BackgroundColor = Color.White;
                        btn_Complete.TextColor = Color.White;
                        btn_Complete.BackgroundColor = Styles.MainAccent;
                        btn_Incomplete.TextColor = Styles.MainAccent;
                        btn_Incomplete.BackgroundColor = Color.White;
                        break;
                    case FilterMode.Incomplete:
                        btn_All.TextColor = Styles.MainAccent;
                        btn_All.BackgroundColor = Color.White;
                        btn_Complete.TextColor = Styles.MainAccent;
                        btn_Complete.BackgroundColor = Color.White;
                        btn_Incomplete.TextColor = Color.White;
                        btn_Incomplete.BackgroundColor = Styles.MainAccent;
                        break;
                    default:
                        btn_All.TextColor = Color.White;
                        btn_All.BackgroundColor = Styles.MainAccent;
                        btn_Complete.TextColor = Styles.MainAccent;
                        btn_Complete.BackgroundColor = Color.White;
                        btn_Incomplete.TextColor = Styles.MainAccent;
                        btn_Incomplete.BackgroundColor = Color.White;
                        break;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        /// Name		ToggleSort
        /// 
        /// <summary>
        /// Handle all tapgestures for this view.
        /// </summary>
        /// 
        /// <param name="sortMode"> The active sort mode to change</param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void ToggleSort(SortMode sortMode)
        {
            try
            {
                switch (sortMode)
                {
                    case SortMode.Date:
                        Img_ForDate.IsVisible = true;
                        Img_ForName.IsVisible = false;
                        Img_ForDistance.IsVisible = false;
                        break;
                    case SortMode.Distance:
                        Img_ForDate.IsVisible = false;
                        Img_ForName.IsVisible = false;
                        Img_ForDistance.IsVisible = true;
                        break;
                    case SortMode.Alphabetical:
                        Img_ForDate.IsVisible = false;
                        Img_ForName.IsVisible = true;
                        Img_ForDistance.IsVisible = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        /// <summary>
        /// Set the sort mode
        /// </summary>
        private void TapGestures()
        {
            try
            {
                var tapDueDate = new TapGestureRecognizer();
                tapDueDate.Tapped +=  (s, e) =>
                {
                    if (_isExecute)
                    {
                        _isExecute = false;
                        ToggleSort(AppData.PropertyModel.Sort = SortMode.Date);
                        PopupLayout.DismisPopup();
                    }
                };
                Gl_Date.GestureRecognizers.Add(tapDueDate);

                var tapCase = new TapGestureRecognizer();
                tapCase.Tapped +=  (s, e) =>
                {
                    if(_isExecute)
                    {
                        _isExecute = false;
                        ToggleSort(AppData.PropertyModel.Sort = SortMode.Alphabetical);
                        PopupLayout.DismisPopup();
                    }
                };
                Gl_ForName.GestureRecognizers.Add(tapCase);

                var tapClosest = new TapGestureRecognizer();
                tapClosest.Tapped +=  (s, e) =>
                {
                    if (_isExecute)
                    {
                        _isExecute = false;
                       // await SplitView.Fullmapview.GetCurrenLocation();
                        if (SplitView.Fullmapview._posistion != null)
                            AppData.PropertyModel.UpdateUserLocation(SplitView.Fullmapview._posistion.Latitude,
                                SplitView.Fullmapview._posistion.Longitude);
                        ToggleSort(AppData.PropertyModel.Sort = SortMode.Distance);
                        PopupLayout.DismisPopup();
                    }
                };
                Gl_ForDistance.GestureRecognizers.Add(tapClosest);
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
