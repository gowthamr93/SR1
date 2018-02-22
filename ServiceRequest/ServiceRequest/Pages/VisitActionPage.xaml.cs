using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Idox.LGDP.Apps.ServiceRequest.Client;
using Xamarin.Forms;
using Idox.LGDP.Apps.Common.OnSite;
using ServiceRequest.ViewModels;
using ServiceRequest.Views.PopUp;
using PopUpSample;
using ServiceRequest.AppContext;
using System.Collections.ObjectModel;
using ServiceRequest.Views;

namespace ServiceRequest.Pages
{
    public partial class VisitActionPage : ContentPage
    {
        /// -------------------------------------------------------------------------------------------------
        #region Private Properties
        ///--------------------------------------------------------------------------------------------------
        ///
        private ObservableCollection<VisitsActionData> _lstVisitsAction;
        private VisitsActionData _visitsActionData;
        public static PopupLayouts PopupContent { get; set; }
        private ScrollView PageScroll { get; set; }
        private static VisitActionPage _instance;
        private bool _isExecute;
        private GroupedListView _groupedOffice;
        ///
        #endregion
        /// -------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region public Properties
        /// -------------------------------------------------------------------------------------------------
        /// 
        public static PopupLayouts CenterPopupContent { get; private set; }
        ///
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        /// ------------------------------------------------------------------------------------------------
        /// 
        public VisitActionPage()
        {
            try
            {
                InitializeComponent();
                AppContext.AppContext.RefreshList = (sender, args) => OnRefreshList();
                LoadPickerData();
                OnLoad(AppData.PropertyModel.SelectedVisit.Visit);
                LoadActionList(AppData.PropertyModel.SelectedVisit.Visit.Visit);
                TapGestureRecognizer();
                Events();
                Ed_Notes.HeightRequest = 100;
                Lstvw_Main.ItemTapped += AddActionsItemTapped;
                _isExecute = true;
                Pkr_Status.SelectedIndexChanged += Pkr_StatusIndexChanged;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }

        }
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Protected Functions
        /// <summary>
        /// Event of OnAppering to VisitActionView 
        /// </summary>
        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                DrawScreen();
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
        public async Task LoadActionsForm(SelectedAction selectedAction)
        {
            try
            {
                AppData.PropertyModel.SelectedAction = selectedAction;
                // Create the action edit view with a dismiss action
                //  await Navigation.PushModalAsync(new VisitActionDetailsPage());
                PageNavigation.PushMainPage(new VisitActionDetailsPage());
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        ///
        private void Pkr_StatusIndexChanged(object sender, EventArgs e)
        {
            if (Pkr_Status.Items[Pkr_Status.SelectedIndex] == "Completed")
            {
                Dtp_CompletedDate.IsVisible = true;
                Tmp_CompletedDate.IsVisible = true;
                Tmp_CompletedDate.Time = DateTime.Now.TimeOfDay;
                Dtp_CompletedDate.Date = DateTime.Now.Date;
            }
            else
            {
                Dtp_CompletedDate.IsVisible = false;
                Tmp_CompletedDate.IsVisible = false;
            }
        }

        public static VisitActionPage Instace()
        {
            return _instance ?? (_instance = new VisitActionPage());
        }

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Private Functions
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
        private async Task OnCancelClick(object sender, EventArgs e)
        {
            try
            {
                Lbl_VisitCancel.IsVisible= Lbl_VisitCancel.IsEnabled = Boxvw_Cancel.IsVisible = Boxvw_Cancel.IsEnabled = false;
                if (AppData.PropertyModel.SelectedVisit.Edited)
                {
                    if (await LockScreen.ToDisplayAlert(this, "Cancel", "Are you sure you want to discard changes for this visit?", "Discard changes", "Cancel"))
                        Cancel();
                }
                else
                    Cancel();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

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
            try
            {
                _lstVisitsAction.Clear();
                Lstvw_Main.BeginRefresh();
                var details = AppData.PropertyModel.SelectedVisit.Visit.Visit;
                LoadActionList(details);
                Lstvw_Main.EndRefresh();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
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
        private void Cancel()
        {
            try
            {
                //await SplitView.Instace().Navigation.PopModalAsync();
                PageNavigation.PopMainPage();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                LogTracking.LogTrace(ex.ToString());
            }

        }
        /// ------------------------------------------------------------------------------------------------
        /// Name		DrawScreen
        /// <summary>
        /// Initializes a new instance of the PopupLayouts
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        ///
        private void DrawScreen()
        {
            try
            {
                if (Content.GetType() != typeof(PopupLayouts))
                {
                    PopupContent = new PopupLayouts(Content, this, PageScroll);
                    if (Device.OS != TargetPlatform.iOS)
                        Content = PopupContent;
                    //
                    CenterPopupContent = new PopupLayouts(Content, this, PageScroll);
                    if (Device.OS != TargetPlatform.iOS)
                        Content = CenterPopupContent;                 
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
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
        private async Task OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                if (_isExecute)
                {
                    Lbl_VisitSave.IsVisible = Lbl_VisitSave.IsEnabled = Boxvw_Save.IsEnabled = false;
                    _isExecute = false;
                    Exception error;
                    SaveVisit();
                    AppData.PropertyModel.SaveVisit(out error);
                    if (error == null)
                    {
                        Cancel();
                        AppContext.AppContext.RefreshVistsList.Invoke(sender, e);
                        AppContext.AppContext.InspectionCell.RefreshList();
                        if (AppData.PropertyModel.SelectedVisit.Visit.Status == SyncStatus.New)
                        {
                            AppData.PropertyModel.SelectedVisit.Visit.Visit.Status = SyncStatus.New;
                        }
                    }
                    else
                    {
                        await LockScreen.ToDisplayAlert(this, "Save Failed", error.Message, "Ok");
                    }
                    _isExecute = true;
                    Lbl_VisitSave.IsVisible = Lbl_VisitSave.IsEnabled = Boxvw_Save.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
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
            try
            {
                var visit = AppData.PropertyModel.SelectedVisit.Visit.Visit;
                if (Dtp_ScheduledDate.IsVisible || Tmp_ScheduledDate.IsVisible)
                    visit.DateScheduled = Dtp_ScheduledDate.Date + Tmp_ScheduledDate.Time;

                if (Pkr_Status.IsVisible)
                {
                    if (Pkr_Status.Items[Pkr_Status.SelectedIndex] == "Completed")
                        visit.DateVisit = Dtp_CompletedDate.Date + Tmp_CompletedDate.Time;
                    else
                        visit.DateVisit = null;
                }
                else
                {
                    visit.DateVisit = Dtp_CompletedDate.Date + Tmp_CompletedDate.Time;
                }

                visit.Hours = AppContext.AppContext.RevertHt(Pkr_TimeTakenHours.Items[Pkr_TimeTakenHours.SelectedIndex], Pkr_TimeTakenMinutes.Items[Pkr_TimeTakenMinutes.SelectedIndex]);
                visit.Miles = AppContext.AppContext.RevertMt(Pkr_Miles.Items[Pkr_Miles.SelectedIndex], Pkr_DecimalMiles.Items[Pkr_DecimalMiles.SelectedIndex]);
                // sriAction.Comments = Ed_Notes.Text;             
                visit.Notes = Ed_Notes.Text;
                var officeDetails = _groupedOffice?.SelectedValue?.Split('-');
                if (officeDetails == null)
                {
                    if (Lbl_Officer.Text != "Value")
                    {
                        var officerText = Lbl_Officer.Text?.Split('-');
                        if (officerText != null)
                        {
                            string officerCode = officerText[0].Remove(officerText[0].Length - 1);
                            visit.Officer = officerCode;
                        }
                    }
                }
                else
                {
                    var officer = officeDetails[0].Remove(officeDetails[0].Length - 1);
                    visit.Officer = officer;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
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
            try
            {
                //Cancel Tapped
                var tapCancel = new TapGestureRecognizer();
                tapCancel.Tapped += async (o, args) => await OnCancelClick(o, args);
                Lbl_VisitCancel.GestureRecognizers.Add(tapCancel);
                Boxvw_Cancel.GestureRecognizers.Add(tapCancel);

                //// + Tapped
                //var onAddTapped = new TapGestureRecognizer();               
                //Boxvw_Add.GestureRecognizers.Add(onAddTapped);
                //Img_Add.GestureRecognizers.Add(onAddTapped);
                //onAddTapped.Tapped += (s, e) =>
                //{
                //    Task.Delay(300);
                //    PopupContent.ShowPopupRelative(new AddVisitPopupView(PopupContent, this), Img_Add, 150, 100, true, "");
                //};

                /// Officer Tapped
                var tapOfficer = new TapGestureRecognizer();
                tapOfficer.Tapped += OnOfficerTapped;
                Lbl_Officer.GestureRecognizers.Add(tapOfficer);

                //// Save Tapped
                var tapSave = new TapGestureRecognizer();
                tapSave.Tapped += async (o, args) => await OnSaveClicked(o, args);
                Lbl_VisitSave.GestureRecognizers.Add(tapSave);
                Boxvw_Save.GestureRecognizers.Add(tapSave);

                //Lbl_AddActions tapped.
                var tapAddAction = new TapGestureRecognizer();
                tapAddAction.Tapped += AddActionClick;
                Lbl_AddActions.GestureRecognizers.Add(tapAddAction);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
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
            try
            {
                SRiVisitMeta visit = AppData.PropertyModel.SelectedVisit.Visit;
                if (!AppData.ConfigModel.Actions(visit.Organisation, visit.Visit.VisitType).Any())
                {
                    LockScreen.ToDisplayAlert(this, "Cannot add Action", "No action types set up in Uniform for this visit type. Please contact your Uniform administrator for assistance.", "OK");
                }
                else
                {
                    Task.Delay(250);
                    PopupContent.ShowPopupRelative(new AddActionView(this, PopupContent), BX_AddActions, Width * 0.40, GetHeight(GroupedListViewModel.Count), true, "");
                    if (Device.OS != TargetPlatform.iOS)
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
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        ///

        private double GetHeight(int count)
        {
            var minHeight = count * GroupedListView.GroupedListViewCellHeight + GroupedListView.GroupedListViewPadding;
            double maxHeight = SplitView.Instace().Height * 0.8;
            return maxHeight < minHeight ? maxHeight : minHeight;
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name		OnOfficerTapped
        /// 
        /// <summary>	Executed when the Edit function is tapped.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        private async void OnOfficerTapped(object sender, EventArgs e)
        {
            try
            {
                if (_groupedOffice != null && _groupedOffice.listData.Count != 0)
                {
                    int height = _groupedOffice.listData.Count * 90;

                    if (height > 400)
                    {
                        height = 400;
                    }
                    PopupContent.ShowPopupRelative(_groupedOffice, BX_Officer, 400, height, true, "");
                    PopupContent._triangleImage.Source = "";
                }
                else if (string.IsNullOrEmpty(AppData.PropertyModel.SelectedVisit.GroupMod))
                {
                    return;
                }
                else
                {
                    await LockScreen.ToDisplayAlert(this, "No Officer", "No officer types set up in Uniform for this visit type. Please contact your Uniform administrator for assistance.", "OK");
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
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
            try
            {
                Pkr_TimeTakenHours.SelectedIndexChanged += EventsChanged;
                Pkr_TimeTakenMinutes.SelectedIndexChanged += EventsChanged;
                Pkr_Miles.SelectedIndexChanged += EventsChanged;
                Pkr_DecimalMiles.SelectedIndexChanged += EventsChanged;
                Ed_Notes.TextChanged += NotesTextChanged;
                Dtp_CompletedDate.DateSelected += CompletedDateChanged;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        private void CompletedDateChanged(object sender, DateChangedEventArgs e)
        {
            try
            {
                AppData.PropertyModel.SelectedVisit.Edited = true;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        private void EventsChanged(object sender, EventArgs e)
        {
            try
            {
                AppData.PropertyModel.SelectedVisit.Edited = true;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        private void NotesTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                AppData.PropertyModel.SelectedVisit.Edited = true;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
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
        private async void AddActionsItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                if (_isExecute)
                {
                    _isExecute = false;
                    var selectedItem = ((VisitsActionData)e.Item);
                    var selectedIndex = selectedItem.ActionIndex;
                    AppData.PropertyModel.SelectedAction =
                                           new SelectedAction(AppData.PropertyModel.SelectedVisit.Visit.Visit.Actions[selectedIndex],
                                                              AppData.PropertyModel.SelectedVisit.MappingForAction(selectedIndex), false);
                    // await Navigation.PushModalAsync(new VisitActionDetailsPage(), true);
                    //App.Current.MainPage = new VisitActionDetailsPage();
                    PageNavigation.PushMainPage(new VisitActionDetailsPage());
                    _isExecute = true;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }


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

        /// ------------------------------------------------------------------------------------------------
        /// Name        OnLoad
        /// ------------------------------------------------------------------------------------------------
        /// <summary>
        /// Load data when the page is loaded.
        /// </summary>
        /// <param name="visitMeta">SRiVisit</param>
        /// ------------------------------------------------------------------------------------------------
        private void OnLoad(SRiVisitMeta visitMeta)
        {
            try
            {
                var visit = visitMeta.Visit;
                Lbl_TitleName.Text = visitMeta.VisitTypeDescription;              

                if (visit.DateScheduled != null)
                {
                    if (AppData.PropertyModel.SelectedVisit.Visit.Visit.Status == SyncStatus.New)
                    {
                        var completeDate = visit.DateScheduled.ToString("ddd dd MMM yyyy", "", "");
                        // var completedDateTime = Details.DateScheduled.ToString("h:mm tt", "", "");
                        Dtp_ScheduledDate.Date = Convert.ToDateTime(completeDate.Replace("\"", ""));
                        Tmp_ScheduledDate.Time = visit.DateScheduled.Value.TimeOfDay;
                        Dtp_ScheduledDate.IsVisible = true;
                        Tmp_ScheduledDate.IsVisible = true;
                    }
                    else
                    {
                        Lbl_ScheduledDate.Text = visit.DateScheduled != null ? visit.DateScheduled.LongishDateTimeFormat() : "No Value Set";
                    }

                }
                else
                {
                    Dtp_ScheduledDate.IsVisible = true;
                    Tmp_ScheduledDate.IsVisible = true;
                    Tmp_ScheduledDate.Time = DateTime.Now.TimeOfDay;
                    Dtp_ScheduledDate.Date = DateTime.Now.Date;
                }


                if (visit.DateVisit != null)
                {
                    Pkr_Status.IsVisible = true;
                    Pkr_Status.SelectedIndex = Pkr_Status.Items.IndexOf("Completed");
                    Dtp_CompletedDate.Date = Convert.ToDateTime(visit.DateVisit.ToString("ddd dd MMM yyyy", "", "").Replace("\"", ""));
                    Tmp_CompletedDate.Time = visit.DateVisit.Value.TimeOfDay;
                    Tmp_CompletedDate.IsVisible = true;
                    Dtp_CompletedDate.IsVisible = true;
                }
                else
                {
                    Pkr_Status.IsVisible = true;
                    Pkr_Status.SelectedIndex = Pkr_Status.Items.IndexOf("Outstanding");
                    Dtp_CompletedDate.IsVisible = false;
                    Tmp_CompletedDate.IsVisible = false;

                }

                if (visit.Hours != null)
                {
                    Pkr_TimeTakenHours.SelectedIndex = Pkr_TimeTakenHours.Items.IndexOf(visit.Hours.ToConvertHt().Item1);
                    Pkr_TimeTakenMinutes.SelectedIndex = Pkr_TimeTakenMinutes.Items.IndexOf(visit.Hours.ToConvertHt().Item2);
                }
                if (visit.Miles != null)
                {
                    Pkr_Miles.SelectedIndex = Pkr_Miles.Items.IndexOf(visit.Miles.ToConvertMt().Item1);
                    Pkr_DecimalMiles.SelectedIndex = Pkr_DecimalMiles.Items.IndexOf(visit.Miles.ToConvertMt().Item2 ?? Pkr_DecimalMiles.Items[0]);
                }
                if (!string.IsNullOrEmpty(visit.Notes))
                {
                    Ed_Notes.Text = visit.Notes;
                }

                // Added to get the officer Name
                int index;

                var orderedOfficers = AppData.ConfigModel.OfficerList(AppData.PropertyModel.SelectedVisit.Visit.Organisation,
                                     AppData.PropertyModel.SelectedVisit.GroupMod).OrderBy(o => o.OfficerCode + o.Name).ToList();

                _groupedOffice = new GroupedListView(orderedOfficers.Select(a => new KeyValuePair<string, string>(a.OfficerCode, a.Name)).ToList(), Lbl_Officer);
                //              
                var selectedvisit = AppData.PropertyModel.SelectedVisit.Visit.Visit;
                index = 0;
                if (!string.IsNullOrEmpty(selectedvisit.Officer) && orderedOfficers.Count > 0)
                {
                    for (int i = 0; i < orderedOfficers.Count; i++)
                        if (orderedOfficers[i].OfficerCode.Equals(selectedvisit.Officer))
                        {
                            index = i;
                            break;
                        }
                    Lbl_Officer.Text = $"{orderedOfficers[index].OfficerCode} - {orderedOfficers[index].Name}";
                }
                else if (string.IsNullOrEmpty(AppData.PropertyModel.SelectedVisit.GroupMod))
                {
                    var officerCode = AppData.MainModel.CurrentUser.OfficerCode(AppData.PropertyModel.SelectedVisit.Visit.Organisation);
                    var officers = AppData.ConfigModel.OfficerList(AppData.PropertyModel.SelectedVisit.Visit.Organisation).Where(x => x.OfficerCode == officerCode).OrderBy(o => o.OfficerCode + o.Name).ToList();

                    for (int i = 0; i < officers.Count; i++)
                        if (officers[i].OfficerCode.Equals(selectedvisit.Officer))
                        {
                            index = i;
                            break;
                        }
                    Lbl_Officer.Text = $"{officers[index].OfficerCode} - {officers[index].Name}";
                    Lbl_Officer.TextColor = Color.Black;
                }
                else
                {
                    Lbl_Officer.Text = "Value";
                }
                //
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
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
            try
            {
                var visit = details.Actions;
                _lstVisitsAction = new ObservableCollection<VisitsActionData>();
                for (int i = 0; i < visit.Count; i++)
                {
                    _visitsActionData = new VisitsActionData()
                    {
                        ActionTypeDescription = visit[i].ActionTypeDescription,
                        CompletedDate = visit[i].ActualDate.ToString("dd MMM yyyy", "Completed ", "No actual data set"),
                        ActionIndex = i,
                    };
                    _lstVisitsAction.Add(_visitsActionData);
                }
                Lstvw_Main.ItemsSource = _lstVisitsAction;
                Lstvw_Main.HeightRequest = (_lstVisitsAction.Count * 60) + 50;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
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
            try
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
                foreach (var minutes in AppContext.AppContext.CpiMinutes)
                {
                    Pkr_TimeTakenMinutes.Items.Add(minutes);
                }
                foreach (var hours in AppContext.AppContext.CpiHours)
                {
                    Pkr_TimeTakenHours.Items.Add(hours);
                }
                Pkr_Miles.SelectedIndex = Pkr_Miles.Items.IndexOf("0");
                Pkr_DecimalMiles.SelectedIndex = Pkr_DecimalMiles.Items.IndexOf(".00");
                Pkr_TimeTakenHours.SelectedIndex = Pkr_TimeTakenHours.Items.IndexOf("0h");
                Pkr_TimeTakenMinutes.SelectedIndex = Pkr_TimeTakenMinutes.Items.IndexOf("0m");
                Pkr_Status.Items.Add("Outstanding");
                Pkr_Status.Items.Add("Completed");
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
