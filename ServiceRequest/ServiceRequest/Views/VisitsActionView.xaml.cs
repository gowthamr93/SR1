using System;
using System.Collections.Generic;
using System.Linq;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.Pages;
using Xamarin.Forms;
using Idox.LGDP.Apps.Common.OnSite;
using ServiceRequest.ViewModels;
using ServiceRequest.Views.PopUp;
using PopUpSample;
using ServiceRequest.AppContext;
using System.Collections.ObjectModel;

namespace ServiceRequest.Views
{
    public partial class VisitsActionView : ContentPage
    {
        /// -------------------------------------------------------------------------------------------------
        #region Private Properties
        ///--------------------------------------------------------------------------------------------------
        ///
        private ObservableCollection<VisitsActionData> _lstVisitsAction;
        private VisitsActionData _visitsActionData;
        private TapGestureRecognizer _tapCancel, _onAddTapped, _tapSave, _tapAddAction;
        private PopupLayouts PopupContent { get; set; }
        private ScrollView PageScroll { get; set; }
        ///
        #endregion
        /// -------------------------------------------------------------------------------------------------
        #region public Properties
        /// -------------------------------------------------------------------------------------------------
        /// 
        public PopupLayouts CenterPopupContent { get; set; }
        ///
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        /// ------------------------------------------------------------------------------------------------
        /// 
        public VisitsActionView()
        {
            InitializeComponent();
        AppContext.AppContext.RefreshList = (sender, args) => OnRefreshList();
            LoadPickerData();
            var details = AppData.PropertyModel.SelectedVisit.Visit;
            Lbl_TitleName.Text = details.VisitTypeDescription;
            OnLoad(details.Visit);
            LoadActionList(details.Visit);
            TapGestureRecognizer();
            Events();
         }
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Protected Functions
        /// <summary>
        /// Event of OnAppering to VisitActionView 
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            DrawScreen();
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Private Functions

        /// ------------------------------------------------------------------------------------------------
        /// ------------------------------------------------------------------------------------------------
        /// Name		DrawScreen
        /// <summary>
        /// Initializes a new instance of the PopupLayouts
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        ///
        private void DrawScreen()
        {
            if (PopupContent == null)
            {
                PopupContent = new PopupLayouts(Content, this, PageScroll);
                Content = PopupContent;
            }
            if (CenterPopupContent == null)
            {
                CenterPopupContent = new PopupLayouts(Content, this, PageScroll);
                Content = CenterPopupContent;
            }
        }

        ///
        /// ------------------------------------------------------------------------------------------------
        /// Name            RefreshList
        /// ------------------------------------------------------------------------------------------------
        ///
        /// <summary>       Refresh the list after the action is added
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void OnRefreshList()
        {
            _lstVisitsAction.Clear();
            Lstvw_Main.BeginRefresh();
            var details = AppData.PropertyModel.SelectedVisit.Visit.Visit;
            LoadActionList(details);
            Lstvw_Main.EndRefresh();
        }

        /// ------------------------------------------------------------------------------------------------
        /// ------------------------------------------------------------------------------------------------
        /// Name		OnCancelClick
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// <summary>	Back to previous page on clicking Cancel button.
        /// </summary>
        /// <param name="sender"> object</param>
        /// <param name="e"> event arguments</param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async void OnCancelClick(object sender, EventArgs e)
        {
            if (AppData.PropertyModel.SelectedVisit.Edited)
            {
                if (await DisplayAlert("Cancel", "Are you sure you want to discard changes for this visit?", "Discard changes", "Cancel"))
                    Cancel();
            }
            else
                Cancel();
        }
        ///
        /// ------------------------------------------------------------------------------------------------
        /// Name        Cancel
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// <summary>   Set selected Action in property model to null
        ///             Back to previous page
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async void Cancel()
        {
            await SplitView.Instace().Navigation.PopModalAsync();
        }
        ///
        /// ------------------------------------------------------------------------------------------------
        /// Name        OnSaveClicked
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// <summary>   Handles a click on the save button.
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        ///         
        private void OnSaveClicked(object sender, EventArgs e)
        {
            Exception error;
            SaveVisit();
            AppData.PropertyModel.SaveVisit(out error);
            if (error == null)
            {
                Cancel();
                AppContext.AppContext.RefreshVistsList.Invoke(sender, e);
               AppContext.AppContext.InspectionCell.RefreshList();
            }
            else
                DisplayAlert("Save Failed", error.Message, "Ok");
        }
        ///
        /// ------------------------------------------------------------------------------------------------
        /// Name        SaveVisit
        /// ------------------------------------------------------------------------------------------------
        /// <summary>   Save the changed visit data
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void SaveVisit()
        {
            SRiVisit sriVisit = AppData.PropertyModel.SelectedVisit.Visit.Visit;
            sriVisit.DateScheduled = Dtp_ScheduledDate.Date + Tmp_ScheduledDate.Time;
            sriVisit.DateVisit = Dtp_CompletedDate.Date + Tmp_CompletedDate.Time;
            sriVisit.Hours = AppContext.AppContext.RevertHt(Pkr_TimeTakenHours.Items[Pkr_TimeTakenHours.SelectedIndex], Pkr_TimeTakenMinutes.Items[Pkr_TimeTakenMinutes.SelectedIndex]);
            sriVisit.Miles = AppContext.AppContext.RevertMt(Pkr_Miles.Items[Pkr_Miles.SelectedIndex], Pkr_DecimalMiles.Items[Pkr_DecimalMiles.SelectedIndex]);
            sriVisit.Comments = Ed_Notes.Text;
        }

        ///
        /// ------------------------------------------------------------------------------------------------
        /// Name        TapGestureRecognizer
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// <summary>   Handles a TapGestureRecognizer for the labels.
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void TapGestureRecognizer()
        {
            //Cancel Tapped
            _tapCancel = new TapGestureRecognizer();
            _tapCancel.Tapped += OnCancelClick;
            Lbl_VisitCancel.GestureRecognizers.Add(_tapCancel);

            // + Tapped
            _onAddTapped = new TapGestureRecognizer();
            Img_Add.GestureRecognizers.Add(_onAddTapped);
            _onAddTapped.Tapped += (s, e) =>
            {
                PopupContent.ShowPopupRelative(new AddVisitPopupView(PopupContent,this), Img_Add, 150, 100, true, "");
            };

            // Save Tapped
            _tapSave = new TapGestureRecognizer();
            _tapSave.Tapped += OnSaveClicked;
            Lbl_VisitSave.GestureRecognizers.Add(_tapSave);

            //Lbl_AddActions tapped.
            _tapAddAction = new TapGestureRecognizer();
            _tapAddAction.Tapped += AddActionClick;
            Lbl_AddActions.GestureRecognizers.Add(_tapAddAction);

        }

        ///
        /// ------------------------------------------------------------------------------------------------
        /// Name        AddActionClick
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// <summary> Handles a click on add action
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void AddActionClick(object sender, EventArgs e)
        {
            SRiVisitMeta visit = AppData.PropertyModel.SelectedVisit.Visit;
            if (!AppData.ConfigModel.Actions(visit.Organisation, visit.Visit.VisitType).Any())
            {
                DisplayAlert("Cannot add Action", "No action types set up in Uniform for this visit type. Please contact your Uniform administrator for assistance.", "Ok");
            }
            else
            {
                PopupContent.ShowPopupRelative(new AddActionView(this, PopupContent), Lbl_AddActions, Width * 0.40, GetHeight(GroupedListViewModel.Count), true, "");
                PopupContent._triangleImage.Source = "";

                AppData.PropertyModel.SelectedAction = new SelectedAction(new SRiAction()
                {
                    DueDate = DateTime.Now,
                    VisitKeyVal = visit.Visit.KeyVal,
                    MDKeyVal = visit.Visit.MDKeyVal,
                    MDSubSys = visit.Visit.MDSubSys,
                    Paragraphs = new List<SRiActionParagraph>()
                }, AppData.PropertyModel.SelectedVisit.NewActionMapping, true);

            }
        }
        ///
        /// ------------------------------------------------------------------------------------------------
        /// Name        LoadActionForm
        /// ------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sets the selected action and displays the actions edit form.
        /// </summary>
        /// <param name="selectedAction">Selected action.</param>
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void LoadActionsForm(SelectedAction selectedAction)
        {
            AppData.PropertyModel.SelectedAction = selectedAction;
            // Create the action edit view with a dismiss action
            Navigation.PushModalAsync(new VisitActionDetailsView());
        }
        private double GetHeight(int count)
        {
            var minHeight = count * GroupedListView.GroupedListViewCellHeight + GroupedListView.GroupedListViewPadding;
            double maxHeight = SplitView.Instace().Height * 0.8;
            return maxHeight < minHeight ? maxHeight : minHeight;
        }
        ///
        /// ------------------------------------------------------------------------------------------------
        /// Name            Events
        /// ------------------------------------------------------------------------------------------------
        /// <summary>       Handles the events change.
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void Events()
        {
            Pkr_TimeTakenHours.SelectedIndexChanged += EventsChanged;
            Pkr_TimeTakenMinutes.SelectedIndexChanged += EventsChanged;
            Pkr_Miles.SelectedIndexChanged += EventsChanged;
            Pkr_DecimalMiles.SelectedIndexChanged += EventsChanged;
            Ed_Notes.TextChanged += NotesTextChanged;
            Dtp_CompletedDate.DateSelected += CompletedDateChanged;
        }
        private void CompletedDateChanged(object sender, DateChangedEventArgs e)
        {
            AppData.PropertyModel.SelectedVisit.Edited = true;
        }
        private void EventsChanged(object sender, EventArgs e)
        {
            AppData.PropertyModel.SelectedVisit.Edited = true;
        }
        private void NotesTextChanged(object sender, TextChangedEventArgs e)
        {
            AppData.PropertyModel.SelectedVisit.Edited = true;
        }
        ///
        /// ------------------------------------------------------------------------------------------------
        /// Name        ActionItemTapped
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// <summary>   Handles the event when action item is tapped
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void AddActionsItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedItem = ((VisitsActionData)e.Item);
            var selectedIndex = selectedItem.ActionIndex;
            AppData.PropertyModel.SelectedAction =
                                   new SelectedAction(AppData.PropertyModel.SelectedVisit.Visit.Visit.Actions[selectedIndex],
                                                      AppData.PropertyModel.SelectedVisit.MappingForAction(selectedIndex), false);
            Navigation.PushModalAsync(new VisitActionDetailsView(), true);
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name        OnLoad
        /// ------------------------------------------------------------------------------------------------
        /// <summary>
        /// Load data when the page is loaded.
        /// </summary>
        /// <param name="details">SRiVisit</param>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void OnLoad(SRiVisit details)
        {
            if (details.DateScheduled != null)
            {
                Lbl_ScheduledDate.Text = details.DateScheduled.ToString("dd MMM yyyy", "", "");
            }
            else
            {
                Dtp_ScheduledDate.IsVisible = true;
                Tmp_ScheduledDate.IsVisible = true;
            }
            if (details.DateVisit != null)
            {
                var completeDate = details.DateVisit.ToString("ddd dd MMM yyyy", "", "");
                Dtp_CompletedDate.Date = Convert.ToDateTime(completeDate.Replace("\"", ""));
                Tmp_CompletedDate.Time = details.DateVisit.Value.TimeOfDay;
            }
            if (details.Hours != null)
            {
                Pkr_TimeTakenHours.SelectedIndex = Pkr_TimeTakenHours.Items.IndexOf(details.Hours.ToConvertHt().Item1);
                Pkr_TimeTakenMinutes.SelectedIndex = Pkr_TimeTakenMinutes.Items.IndexOf(details.Hours.ToConvertHt().Item2);
            }
            if(details.Miles != null)
            {
                Pkr_Miles.SelectedIndex = Pkr_Miles.Items.IndexOf(details.Miles.ToConvertMt().Item1);
                Pkr_DecimalMiles.SelectedIndex = Pkr_DecimalMiles.Items.IndexOf(details.Miles.ToConvertMt().Item2 ?? Pkr_DecimalMiles.Items[0]);
            }
            if(!string.IsNullOrEmpty(details.Comments))
            {
                Ed_Notes.Text = details.Comments;
            }
            
        }

        ///
        /// ------------------------------------------------------------------------------------------------
        /// NAme            LoadActionList
        /// ------------------------------------------------------------------------------------------------
        /// <summary>       Load the actionlist
        /// </summary>
        /// <param name="details"></param>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void LoadActionList(SRiVisit details)
        {
            var visit = details.Actions;
            _lstVisitsAction = new ObservableCollection<VisitsActionData>();
            for (var i = 0; i < visit.Count; i++)
            {
                _visitsActionData = new VisitsActionData()
                {
                    ActionTypeDescription = visit[i].ActionTypeDescription,
                    CompletedDate = visit[i].ActualDate.ToString("dd MMM yyyy", "Completed ", "No actual data set"),
                    ActionIndex = i
                };
                _lstVisitsAction.Add(_visitsActionData);
            }
            Lstvw_Main.ItemsSource = _lstVisitsAction;
            Lstvw_Main.HeightRequest = (_lstVisitsAction.Count * 60) + 50;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		LoadPickerData
        /// 
        /// <summary>
        /// Loads the data for the pickers in the page.
        /// </summary>
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void LoadPickerData()
        {
            var decMiles = new[] { ".00", ".25", ".50", ".75" };
            foreach (var item in decMiles)
            {
                Pkr_DecimalMiles.Items.Add(item);
            }
            for (var i = 0; i <= 100; i++)
            {
                Pkr_Miles.Items.Add(i.ToString());
            }
            foreach(var minutes in AppContext.AppContext.CpiMinutes)
            {
                Pkr_TimeTakenMinutes.Items.Add(minutes);
            }
            foreach (var hour in AppContext.AppContext.CpiHours)
            {
                Pkr_TimeTakenHours.Items.Add(hour);
            }
            Pkr_Miles.SelectedIndex = Pkr_Miles.Items.IndexOf("0");
            Pkr_DecimalMiles.SelectedIndex = Pkr_DecimalMiles.Items.IndexOf(".00");
            Pkr_TimeTakenHours.SelectedIndex = Pkr_TimeTakenHours.Items.IndexOf("0h");
            Pkr_TimeTakenMinutes.SelectedIndex = Pkr_TimeTakenMinutes.Items.IndexOf("0m");
        }

        #endregion
        /// ------------------------------------------------------------------------------------------------

    }
}
