using System;
using System.Collections.Generic;
using System.Linq;
using Idox.LGDP.Apps.Common.OnSite;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.AppContext;
using ServiceRequest.DependencyInterfaces;
using ServiceRequest.Pages;
using Xamarin.Forms;

namespace ServiceRequest.Views.CaseListViewControl
{
    public class CaseListControl : Grid
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public Variables and Properties     

        /// <summary>
        /// Gets the number of elements in the Case List.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return MEntries.Count; }
        }

        public readonly List<CaseListModel> MEntries = new List<CaseListModel>();

        private CaseListModel CaseEntry { get; set; }

        public PropertySummary PropertySummary { get; set; }

        public static int SectionIndex { get; set; }

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Private Variables and Properties

        /// ------------------------------------------------------------------------------------------------        
        ///
        private readonly ScrollView _mScrollView;
        private StackLayout _mCellStackLayout;
        private readonly Image _mShadowImage;
        /// <summary>
        /// Gets or sets the default color of the button background.
        /// </summary>
        /// <value>The default color of the button background.</value>
        private Color DefaultButtonBackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the default color of the button text.
        /// </summary>
        /// <value>The default color of the button text.</value>
        private Color DefaultButtonTextColor { get; set; }

        /// <summary>
        /// Gets or sets the duration of the animation.
        /// </summary>
        /// <value>The duration of the animation.</value>
        private uint AnimationDuration { get; set; }

        #endregion
        /// ------------------------------------------------------------------------------------------------       

        /// ------------------------------------------------------------------------------------------------
        #region Constructor

        /// ------------------------------------------------------------------------------------------------        
        ///        
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CaseListView"/> class.
        /// </summary>
        /// <param name="header">Defines a header for the Inspection.</param>
        public CaseListControl(View header = null)
        {
            try
            {
                AnimationDuration = 400;
                BackgroundColor = DefaultButtonBackgroundColor;
                DefaultButtonTextColor = Color.Black;
                Padding = new Thickness(0, 0, 0, 0);

                RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                RowSpacing = 0;
                ColumnSpacing = 0;
                CreateStacklayout();

                _mShadowImage = new Image()
                {
                    Source = "HeaderShadow.png",
                    InputTransparent = true,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Aspect = Aspect.Fill
                };

                _mScrollView = new ScrollView()
                {
                    BackgroundColor = DefaultButtonBackgroundColor,
                    Content = _mCellStackLayout,
                    Orientation = ScrollOrientation.Vertical,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                };

                Children.Add(_mScrollView, 0, 1);

                if (header != null)
                {
                    Children.Add(header, 0, 0);
                    Children.Add(_mShadowImage, 0, 1);
                }
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
        /// 
        private void CreateStacklayout()
        {
            try
            {
                _mCellStackLayout = new StackLayout()
                {
                    BackgroundColor = DefaultButtonBackgroundColor,
                    Orientation = StackOrientation.Vertical,
                    Spacing = 0
                };
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        private void OpenCase(CaseListModel entry)
        {
            try
            {
                // Get the element (Case) touched
				var element = _mCellStackLayout.Children.FirstOrDefault(x => x == entry.Cell);
                if (element == null)
                    return;               
                entry.View.Animate("expand",
                    x =>
                    {
                       entry.View.IsVisible = true;
					   entry.View.HeightRequest = entry.OriginalSize.Height ;						
					},16, AnimationDuration, Easing.SpringOut, (d, b) =>
                    {	
					//Added by Gowtham to scroll the Caselist at the desired position in IOS
					   if(Device.OS== TargetPlatform.iOS)
						_mScrollView.ScrollToAsync(element, ScrollToPosition.MakeVisible, true);
					//
                        entry.View.IsVisible = true;
                    });
                if (Device.OS == TargetPlatform.Android)
                    SplitView.MapView?.ClearPin();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        private async void CloseCase(CaseListModel entry)
        {
            try
            {
                entry.View.Animate("colapse",
                    x =>
                    {
                        //var change = entry.OriginalSize.Height * x;
                        //entry.View.HeightRequest = entry.OriginalSize.Height - change;
                        entry.View.HeightRequest = 0;
                    }, 0, AnimationDuration, Easing.SpringIn, (d, b) =>
                    {
                        entry.View.IsVisible = false;
                    });
                await SplitView.Instace().Clear();

                if (Device.OS == TargetPlatform.Android)
                {
                    var formsMap = (AndroidMapView)SplitView.MapView;
                    SplitView.Fullmapview.FindByName<Grid>("GlMapView").Children.Clear();
                    SplitView.Fullmapview.FindByName<Grid>("GlMapView").Children.Add(formsMap, 0, 0);
                    SplitView.MapView.LoadPins(AppContext.AppContext.LstGooglePin);
                    AndroidMapView.ChangeMapType();

                }
                else
                {
                    //Added to reload the Map pins
                    WindowsMapView.ChangeMapType();
                    SplitView.MapView.ClearPin();
                    SplitView.MapView.LoadPins(AppContext.AppContext.LstCustomPin);
                    SplitView.MapView.MoveToRegion(AppContext.AppContext.LstCustomPin?.FirstOrDefault().Pin.Position, Xamarin.Forms.Maps.Distance.FromKilometers(0.5));
                }
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

        /// ------------------------------------------------------------------------------------------------        
        /// 

        /// <summary>
        /// Add the specified Case and Inspection.
        /// </summary>
        /// <param name="Case">Case.</param>
        /// <param name="inspection">View.</param>
        public void Add(View Case, View inspection)
        {
            try
            {
                if (Case == null)
                    throw new ArgumentNullException(nameof(Case));

                if (inspection == null)
                    throw new ArgumentNullException(nameof(inspection));

                var entry = CaseEntry = new CaseListModel()
                {
                    Cell = Case,
                    View = inspection,
                    OriginalSize = new Size(inspection.WidthRequest, inspection.HeightRequest)
                };

                MEntries.Add(CaseEntry);

                _mCellStackLayout.Children.Add(CaseEntry.Cell);
                _mCellStackLayout.Children.Add(CaseEntry.View);

                var cellIndex = CaseEntry.Index = MEntries.Count - 1;

                // Commented for Issue in Android

                //var tapGestureRecognizer = new TapGestureRecognizer();
                //Case.GestureRecognizers.Remove(tapGestureRecognizer);
                //if (CellTouched == null)
                //{
                //    tapGestureRecognizer.Tapped += (object sender, EventArgs e) =>
                //    {
                //        OnCellTouchUpInside(cellIndex);
                //    };
                //}
                //else
                //    tapGestureRecognizer.Tapped += (object sender, EventArgs e) => CellTouched(cellIndex);

                //Case.GestureRecognizers.Add(tapGestureRecognizer);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        public void Clear()
        {
            try
            {
                MEntries.Clear();
                _mCellStackLayout.Children.Clear();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        /// <summary>
        /// Closes all entries.
        /// </summary>
        public void CloseAllEntries()
        {
            try
            {
                foreach (var entry in MEntries)
                {
                    AppData.PropertyModel.SelectedProperty = null;
                    AppData.PropertyModel.SelectedRecord = null;
                    entry.View.HeightRequest = 0;
                    entry.IsOpen = false;
                    entry.View.IsVisible = false;
                }
                AppData.PropertyModel.SelectedProperty = null;
                AppData.PropertyModel.SelectedRecord = null;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        /// <summary>
        /// Raises the Case touch up inside event.
        /// </summary>
        /// <param name="cellIndex">Case index.</param>
        public async void OnCellTouchUpInside(int cellIndex)
        {
            try
            {
                if (AppData.SyncInProgress)
                {
                    await
                        SplitView.DisplayAlert("Sync In Progress",
                            "Please wait for the sync to finish before taking further actions.", "OK", null);
                }
                else
                {
                    if (AppContext.AppContext.NewRecordInProgress)
                        await SplitView.DisplayAlert("New Record creation is in progress","Please save or cancel new sevice record to proceed", "OK", null);
                    else
                    {
                        var touchedEntry = MEntries[cellIndex];
                        bool isTouchingToClose = touchedEntry.IsOpen;
                        var entriesToClose = MEntries.Where(e => e.IsOpen);
                        foreach (var entry in entriesToClose)
                        {
                            InspectionCellView.CurrentUnifiedItem = null;
                            InspectionCellView.DeSelectItem();
                            CloseCase(entry);
                            // CloseAllEntries();
                            entry.IsOpen = false;
                            Update(entry.Index, entry.IsOpen);
                        }

                        if (!isTouchingToClose)
                        {
                            await SplitView.Instace().Clear();
                            OpenCase(touchedEntry);
                            touchedEntry.IsOpen = true;
                            Update(touchedEntry.Index, touchedEntry.IsOpen);
                            if (Device.OS == TargetPlatform.Android)
                                await
                                    SplitView.Instace()
                                        .PushRightContent(SplitView.PropertySummary = new PropertySummary());
                            else
                            {
                                await SplitView.Instace().PushRightContent(new PropertySummary());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }



        public void Update(int cellIndex, bool selected)
        {
            try
            {
                SectionIndex = cellIndex;
                SetExpandCollapse(selected, cellIndex);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }


        public async void SetExpandCollapse(bool selected, int presentCaseIndex)
        {
            try
            {
                if (MEntries.Count != 0)
                {
                    var imgCollapse = MEntries[presentCaseIndex]?.Cell.FindByName<Image>("Img_Collapse");
                    var imgExpand = MEntries[presentCaseIndex]?.Cell.FindByName<Image>("Img_Expand");
                    var glInspectionCell = MEntries[presentCaseIndex]?.Cell.FindByName<StackLayout>("Main_Layout");
                    var mainGridCaselist = MEntries[presentCaseIndex]?.Cell.FindByName<Grid>("Main_Grid");
                    var caseCell = MEntries[presentCaseIndex]?.Cell;
                    var inspectionlist = MEntries[presentCaseIndex]?.View.FindByName<ListView>("Lstvw_Inspections");

                    if (mainGridCaselist != null)
                    {
                        var mainLayoutHeight = mainGridCaselist.HeightRequest;

                        if (imgExpand != null && imgCollapse != null && glInspectionCell != null)
                        {
                            imgExpand.IsVisible = selected;
                            if (imgExpand.IsVisible)
                            {
                                caseCell.HeightRequest = 55;//Set the casecelllist view height 

                                SplitView.Instace().FilterCheckAvailable(false);//Toenable and Disable the filter                      
                            }
                            else
                            {
                                await SplitView.Instace().Clear();
                                //Disposing Map instance                               
                                AppContext.AppContext.MapView?.DisposeMap();
                                GC.Collect(0, GCCollectionMode.Forced);
                                //
                                AppData.PropertyModel.SelectedRecord = null;// To set the selected record summary details as null
                                AppData.PropertyModel.SelectedProperty = null;
                                caseCell.HeightRequest = mainLayoutHeight; //Set the casecelllist view height 
                                inspectionlist.SelectedItem = null;
                                SplitView.Instace().FilterCheckAvailable(true);
                            }
                            imgCollapse.IsVisible = !selected;
                            glInspectionCell.BackgroundColor = selected ? Color.White : Styles.WindowBackground;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        public void SetSyncStatusManual(SyncStatus status, int presentCaseIndex)
        {
            try
            {
                if (MEntries.Count != 0)
                {
                    // var imgNewStatus = m_entries[presentCaseIndex]?.Cell.FindByName<Image>("Img_NewStatus");
                    var imgDoneUpload = MEntries[presentCaseIndex]?.Cell.FindByName<Image>("Img_DoneUpload");
                    var imgPendingUpload = MEntries[presentCaseIndex]?.Cell.FindByName<Image>("Img_PendingUpload");

                    if (imgDoneUpload != null && imgPendingUpload != null)
                    {
                        switch (status)
                        {
                            case SyncStatus.Changed:
                                //imgNewStatus.IsVisible = false;
                                imgDoneUpload.IsVisible = false;
                                imgPendingUpload.IsVisible = true;
                                break;
                            case SyncStatus.New:
                                //imgNewStatus.IsVisible = true;
                                imgDoneUpload.IsVisible = false;
                                imgPendingUpload.IsVisible = false;
                                break;
                            case SyncStatus.Saved:
                                //imgNewStatus.IsVisible = false;
                                imgDoneUpload.IsVisible = true;
                                imgPendingUpload.IsVisible = false;
                                break;
                            default:
                                // imgNewStatus.IsVisible = false;
                                imgDoneUpload.IsVisible = false;
                                imgPendingUpload.IsVisible = false;
                                break;
                        }
                    }
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