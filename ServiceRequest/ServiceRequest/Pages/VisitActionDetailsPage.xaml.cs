using Idox.LGDP.Apps.Common.OnSite;
using Idox.LGDP.Apps.ServiceRequest.Client;
using PopUpSample;
using ServiceRequest.Views.PopUp;
using ServiceRequest.Views.ViewCells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceRequest.AppContext;
using ServiceRequest.Custom;
using ServiceRequest.DependencyInterfaces;
using ServiceRequest.ViewModels;
using ServiceRequest.Views;
using Xamarin.Forms;

namespace ServiceRequest.Pages
{
    public partial class VisitActionDetailsPage
    {
        /// ------------------------------------------------------------------------------------------------
        #region PublicVariables
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// 
        public string InputValue { get; set; }
        ///       
        public const string INSERT_MARKER = "^IN;";
        public static PopupLayouts CenterPopup { get; private set; }
        public static PopupLayouts RelativePopup { get; private set; }

        public static bool Isnew { get; set; }

        //public static CommentsPopup CustomTextPopup { get; set; }

        public static Action RefreshComment;

        public bool IsEditable
        {
            get { return _isEditable; }
            private set
            {
                if (_isEditable != value)
                {
                    _isEditable = value;
                    OnEditPropertyChanged();
                }
            }
        }
        public static TableSection TblSection { get; set; }

        public List<string> SingleTypeCode { get; private set; }

        private List<SRiActionParagraph> Paralist { get; }
        public static VisitActionDetailsPage CurrentInstance { get; private set; }
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------ 
        #region PrivateVariables
        /// ------------------------------------------------------------------------------------------------

        private GroupedListView _groupedOffice;

        private ScrollView PageScroll { get; set; }

        private bool _isEditable;
        private bool _isCellEditable;
        private bool _isExecute;
        private bool IsCellEditable
        {
            get { return _isCellEditable; }
            set
            {
                if (_isCellEditable != value)
                {
                    _isCellEditable = value;

                }
            }
        }

        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Constructors
        /// ------------------------------------------------------------------------------------------------
        /// 

        public VisitActionDetailsPage()
        {
            try
            {
                InitializeComponent();
                Paralist = AppData.PropertyModel.SelectedAction.Action.Paragraphs;
                LoadPickerData();
                LoadData();
                DrawScreen();
                TapGestures();
                RefreshList();
                IsEditable = false;
                CurrentInstance = this;
                CheckLegislation();
                _isExecute = true;
                RefreshComment += RefreshCommentList;
                Pkr_Status.SelectedIndexChanged += Pkr_StatusIndexChanged;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        public async void OnCommentDeleted(SRiActionComment sRiActionComment)
        {
            try
            {
                if (await LockScreen.ToDisplayAlert(this, "Delete", "Do you want to delete this item?", "Yes", "No"))
                {
                    int index = AppData.PropertyModel.SelectedAction.Action.Comments.IndexOf(sRiActionComment);
                    AppData.PropertyModel.SelectedAction.Action.Comments.RemoveAt(index);
                    RefreshCommentList();
                    AppData.PropertyModel.SelectedAction.Action.Modified = true;
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
        #region public Functions



        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// ------------------------------------------------------------------------------------------------
        /// Name		OnDelete
        /// 
        /// <summary>	Delete's the row on the click event
        /// </summary>
        /// <param name="sRiActionParagraph"> </param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------

        public async void OnDelete(SRiActionParagraph sRiActionParagraph)
        {
            try
            {
                if (await LockScreen.ToDisplayAlert(this, "Delete", "Do you want to delete this item?", "Yes", "No"))
                {
                    Paralist.Remove(sRiActionParagraph);
                    OnEditPropertyChanged();
                    if (Paralist.Count == 0)
                    {
                        IsEditable = false;
                        IsCellEditable = false;
                        Tblvw_StandardParagraph.IsVisible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name		RefreshList
        /// 
        /// <summary>	Refresh the lists while the paragraph is selected and adds the next type and para.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        public void RefreshList()
        {
            try
            {
                int count = 0;
                TblSec_ParagraphList.Clear();
                if (Paralist != null)
                {
                    foreach (var sRiActionParagraph in Paralist)
                    {
                        sRiActionParagraph.IsEditable = IsEditable;
                        if (SingleTypeCode != null)
                        {
                            if (SingleTypeCode.Contains(sRiActionParagraph.ParagraphType))
                                sRiActionParagraph.IsCellEditable = !IsCellEditable;
                            else
                                sRiActionParagraph.IsCellEditable = IsCellEditable;
                        }
                        else
                            sRiActionParagraph.IsCellEditable = IsCellEditable;
                        AddTypedCell(sRiActionParagraph);
                        count++;
                    }
                }

                if ((Device.OS == TargetPlatform.Android) || (Device.OS == TargetPlatform.iOS))
                {
                    if (count > 0)
                    {
                        Tblvw_StandardParagraph.HeightRequest = count * 105;
                        Sl_Main.HeightRequest = count * 105 + 185;
                    }
                    else
                    {
                        Tblvw_StandardParagraph.HeightRequest = 0;
                        Sl_Main.HeightRequest = -1;
                    }
                }
                else
                {
                    if (count > 0)
                    {
                        Tblvw_StandardParagraph.HeightRequest = count * 105 + 150;
                        Sl_Main.HeightRequest = count * 105 + 185;
                    }
                    else
                    {
                        Tblvw_StandardParagraph.HeightRequest = 0;
                        Sl_Main.HeightRequest = -1;
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

        /// ------------------------------------------------------------------------------------------------
        #region private Functions

        /// ------------------------------------------------------------------------------------------------
        /// 
        private void RefreshCommentList()
        {
            //
            double viewcell_Height = 75;
            double st_Space = 82;
            //
            Lst_Comments.ItemsSource = null;
            Lst_Comments.ItemsSource = AppData.PropertyModel.SelectedAction.Action.Comments;
            if (AppData.PropertyModel.SelectedAction.Action.Comments != null)
            {
                Lst_Comments.HeightRequest = AppData.PropertyModel.SelectedAction.Action.Comments.Count * viewcell_Height;
                if (Lst_Comments.HeightRequest != 0)
                    Sl_Notes.HeightRequest = AppData.PropertyModel.SelectedAction.Action.Comments.Count * viewcell_Height +
                                             st_Space;
            }
            else
            {
                Lst_Comments.HeightRequest = 0;
            }
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name		LoadPickerData
        /// 
        /// <summary>	Loads the picker data on appearing.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        private void LoadPickerData()
        {
            Pkr_Status.Items.Add("Outstanding");
            Pkr_Status.Items.Add("Completed");

            foreach (var h in AppContext.AppContext.CpiHours)
            {
                Pkr_TimeTakenHours.Items.Add(h);
            }
            foreach (var m in AppContext.AppContext.CpiMinutes)
            {
                Pkr_TimeTakenMinutes.Items.Add(m);
            }
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name		Pkr_StatusIndexChanged
        /// 
        /// <summary>	Operations to be done when changing status picker.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
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

        private void OnEditPropertyChanged()
        {
            try
            {
                if (IsEditable)
                {
                    Lbl_Edit.Text = "Done";
                    SingleTypeCode = Paralist.GroupBy(x => x.ParagraphType).Where(group => group.Count() == 1)
                              .Select(x => x.Key).ToList();
                }
                else
                {
                    Lbl_Edit.Text = "Edit";
                    SingleTypeCode = null;
                }
                RefreshList();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name		CheckLegislation
        /// 
        /// <summary>	Checks whether the legislation should be added while appearing.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        private void CheckLegislation()
        {
            try
            {
                var visit = AppData.PropertyModel.SelectedVisit.Visit;
                if (AppData.ConfigModel.Legislations(visit.Organisation, visit.Visit.VisitType).Count != 0)
                {
                    var legislationStackLayout = new VisitActionDetailsViewCell();
                    Sl_View.Children.Add(legislationStackLayout);
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name		TapGestures
        /// 
        /// <summary>	It contains the Tapgesture recognisers for the whole Page.
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        private void TapGestures()
        {
            try
            {
                //Cancel
                var tapCancel = new TapGestureRecognizer();
                tapCancel.Tapped += async (o, args) => await CancelClick(o, args);
                Lbl_Cancel.GestureRecognizers.Add(tapCancel);
                Boxvw_Cancel.GestureRecognizers.Add(tapCancel);

                //Save
                var tapSave = new TapGestureRecognizer();
                tapSave.Tapped += async (o, args) => await SaveClick(o, args);
                Lbl_Save.GestureRecognizers.Add(tapSave);
                Boxvw_Save.GestureRecognizers.Add(tapSave);

                //AddParagraph
                var tapAddParagraph = new TapGestureRecognizer();
                tapAddParagraph.Tapped += AddPropertyClick;
                Lbl_AddParagraph.GestureRecognizers.Add(tapAddParagraph);

                //Edit
                var tapEdit = new TapGestureRecognizer();
                tapEdit.Tapped += OnEditTapped;
                Lbl_Edit.GestureRecognizers.Add(tapEdit);

                //Officer tapped
                var tapOfficer = new TapGestureRecognizer();
                tapOfficer.Tapped += OnOfficerTapped;
                Lbl_Officer.GestureRecognizers.Add(tapOfficer);

                //Add Comment tapped
                var tapAddComment = new TapGestureRecognizer();
                tapAddComment.Tapped += OnAddCommentTapped;
                Lbl_Comment.GestureRecognizers.Add(tapAddComment);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name		CancelClick
        /// 
        /// <summary>	Executed when the cancel button is clicked.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        private async Task CancelClick(object sender, EventArgs e)
        {
            try
            {
                Lbl_Cancel.IsVisible = Lbl_Cancel.IsEnabled = Boxvw_Cancel.IsVisible = Boxvw_Cancel.IsEnabled = false;
                PageNavigation.PopMainPage();
                //Application.Current.MainPage = SplitView.VisitActionsInstace;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name		SaveClick
        /// 
        /// <summary>	Executed when the save button is clicked.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        private async Task SaveClick(object sender, EventArgs e)
        {
            try
            {
                if (_isExecute)
                {
                    Lbl_Save.IsVisible = Lbl_Save.IsEnabled = Boxvw_Save.IsEnabled = false;
                    _isExecute = false;
                    var sriAction = AppData.PropertyModel.SelectedAction.Action;

                    if (Pkr_Status.IsVisible)
                    {
                        if (Pkr_Status.Items[Pkr_Status.SelectedIndex] == "Completed")
                            sriAction.ActualDateTime = Dtp_CompletedDate.Date + Tmp_CompletedDate.Time;
                        else
                            sriAction.ActualDate = null;
                    }
                    else
                    {
                        sriAction.ActualDateTime = Dtp_CompletedDate.Date + Tmp_CompletedDate.Time;
                    }


                    sriAction.Hours =
                    AppContext.AppContext.RevertHt(Pkr_TimeTakenHours.Items[Pkr_TimeTakenHours.SelectedIndex],
                        Pkr_TimeTakenMinutes.Items[Pkr_TimeTakenMinutes.SelectedIndex]);
                    if (Dtp_ScheduledDate.IsVisible || Tmp_ScheduledDate.IsVisible)
                        sriAction.DueDate = Dtp_ScheduledDate.Date + Tmp_ScheduledDate.Time;
                    var officeDetails = _groupedOffice?.SelectedValue?.Split('-');
                    if (officeDetails == null)
                    {
                        if (Lbl_Officer.Text != "Value")
                        {
                            var officerText = Lbl_Officer.Text?.Split('-');
                            if (officerText != null)
                            {
                                string officerCode = officerText[0].Remove(officerText[0].Length - 1);
                                sriAction.Officer = officerCode;
                            }
                        }
                    }
                    else
                    {
                        var officer = officeDetails[0].Remove(officeDetails[0].Length - 1);
                        sriAction.Officer = officer;
                    }
                    if (VisitActionDetailsViewCell.GroupedLegislation != null)
                    {
                        var legislationDetails = VisitActionDetailsViewCell.GroupedLegislation?.SelectedValue?.Split('-');
                        if (legislationDetails != null)
                        {
                            var legislation = legislationDetails[0].Remove(legislationDetails[0].Length - 1);
                            sriAction.LegislationType = legislation;
                        }
                    }
                    Exception error;
                    AppData.PropertyModel.SaveAction(out error);
                    //  AppData.PropertyModel.SaveVisit(out error);
                    if (error == null)
                    {
                        //await SplitView.Instace().Navigation.PopModalAsync();
                        PageNavigation.PopMainPage();
                        // Application.Current.MainPage = SplitView.VisitActionsInstace;
                        AppContext.AppContext.RefreshList.Invoke(sender, e);
                    }
                    else
                    {
                        throw error;
                    }
                    _isExecute = true;
                    Lbl_Save.IsVisible = Lbl_Save.IsEnabled = Boxvw_Save.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name		AddPropertyClick
        /// 
        /// <summary>	Opens the Paragraph Pop-Up on clicking Add Paragraph
        /// </summary>
        ///  /// ------------------------------------------------------------------------------------------------
        private void AddPropertyClick(object sender, EventArgs e)
        {
            try
            {
                CenterPopup.ShowPopupCenter(new ParagraphView(RefreshList), 0.5, "Adding Paragraph");
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }



        /// ------------------------------------------------------------------------------------------------
        /// Name		LoadData
        /// 
        /// <summary>	Loads the data to the view while appearing.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        private void LoadData()
        {
            try
            {
                var visitActionDetails = AppData.PropertyModel.SelectedAction.Action;
                var hoursTaken = AppData.PropertyModel.SelectedAction.Action.Hours.ToConvertHt();

                bool editable;
                editable = string.IsNullOrEmpty((AppData.PropertyModel.SelectedAction.Action.KeyVal));

                if (visitActionDetails.ActualDateTime != null)
                {
                    Pkr_Status.IsVisible = true;
                    Pkr_Status.SelectedIndex = Pkr_Status.Items.IndexOf("Completed");
                    Dtp_CompletedDate.Date = visitActionDetails.ActualDateTime.Value.Date;
                    Tmp_CompletedDate.Time = visitActionDetails.ActualDateTime.Value.TimeOfDay;
                    Tmp_CompletedDate.IsVisible = true;
                    Dtp_CompletedDate.IsVisible = true;
                }
                else
                {
                    Pkr_Status.IsVisible = true;
                    Pkr_Status.SelectedIndex = Pkr_Status.Items.IndexOf("Outstanding");
                    Tmp_CompletedDate.IsVisible = false;
                    Dtp_CompletedDate.IsVisible = false;
                    Tmp_CompletedDate.Time = DateTime.Now.TimeOfDay;
                    Dtp_CompletedDate.Date = DateTime.Now.Date;
                }


                if (visitActionDetails.DueDate != null)
                {
                    if (editable)
                    {
                        var scheduleDate = visitActionDetails.DueDate.ToString("ddd dd MMM yyyy", "", "");
                        Dtp_ScheduledDate.Date = Convert.ToDateTime(scheduleDate.Replace("\"", ""));
                        Tmp_ScheduledDate.Time = visitActionDetails.DueDate.Value.TimeOfDay;
                        Dtp_ScheduledDate.IsVisible = true;
                        Tmp_ScheduledDate.IsVisible = true;
                    }
                    else
                    {
                        Lbl_ScheduledDate.Text = visitActionDetails.DueDate != null ? visitActionDetails.DueDate.LongishDateTimeFormat() : "No Value Set";
                    }
                }
                else
                {
                    Dtp_ScheduledDate.IsVisible = true;
                    Tmp_ScheduledDate.IsVisible = true;
                    Tmp_ScheduledDate.Time = DateTime.Now.TimeOfDay;
                    Dtp_ScheduledDate.Date = DateTime.Now.Date;
                }


                Pkr_TimeTakenHours.SelectedIndex =
                    Pkr_TimeTakenHours.Items.IndexOf(hoursTaken.Item1 ?? Pkr_TimeTakenHours.Items[0]);

                Pkr_TimeTakenMinutes.SelectedIndex =
                    Pkr_TimeTakenMinutes.Items.IndexOf(hoursTaken.Item2 ?? Pkr_TimeTakenMinutes.Items[0]);
                
                //
                Lbl_Title.Text = visitActionDetails.ActionType;
                RefreshCommentList();

                var orderedOfficers = AppData.ConfigModel.OfficerList(AppData.PropertyModel.SelectedVisit.Visit.Organisation,
                                      AppData.PropertyModel.SelectedVisit.GroupMod).OrderBy(o => o.OfficerCode + o.Name).ToList();

                _groupedOffice = new GroupedListView(orderedOfficers.Select(a => new KeyValuePair<string, string>(a.OfficerCode, a.Name)).ToList(), Lbl_Officer);
                //
                int index;
                var action = AppData.PropertyModel.SelectedAction.Action;
                index = 0;
                if (!string.IsNullOrEmpty(action.Officer) && orderedOfficers.Count > 0)
                {
                    for (int i = 0; i < orderedOfficers.Count; i++)
                        if (orderedOfficers[i].OfficerCode.Equals(action.Officer))
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
                        if (officers[i].OfficerCode.Equals(action.Officer))
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

        /// ------------------------------------------------------------------------------------------------
        /// Name		AddTypedCell
        /// 
        /// <summary>	Adds the paragraph to the stacklayout after save function is executed in paragraphViewcell
        /// </summary>
        ///  <param name="sRiActionParagraph">		The paragraph being entered.</param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        private void AddTypedCell(SRiActionParagraph sRiActionParagraph)
        {
            try
            {
                Tblvw_StandardParagraph.IsVisible = true;
                if (Device.OS == TargetPlatform.Android)
                {
                    var address = sRiActionParagraph.Text?.Replace("\r",
                        Environment.NewLine);
                    sRiActionParagraph.Text = address;
                }

                if (sRiActionParagraph.PlainText.Contains("^IN"))
                {
                    //sRiActionParagraph.IsCellEditable = true;
                    //sRiActionParagraph.IsEditable = true;
                    TblSec_ParagraphList.Add(new ParagraphInputViewCell() { BindingContext = sRiActionParagraph });
                }
                else if (sRiActionParagraph.PlainText == ParagraphViewModel.CUSTOM_PLACEHOLDER ||
                        sRiActionParagraph.CellType == CellTypes.Custom)
                {
                    var customCell = AppData.PropertyModel.SelectedAction.Action.Paragraphs.FirstOrDefault(x => x == sRiActionParagraph);
                    if (customCell != null)
                    {
                        customCell.CellType = CellTypes.Custom;
                        if (customCell.Text == ParagraphViewModel.CUSTOM_PLACEHOLDER)
                            customCell.Text = ParagraphViewModel.TAP_CUSTOM_PLACEHOLDER;
                    }
                    TblSec_ParagraphList.Add(new ParagraphCustomViewCell() { BindingContext = sRiActionParagraph });
                }
                else
                {
                    TblSec_ParagraphList.Add(new ParagraphNormalViewCell() { BindingContext = sRiActionParagraph });
                }
                TblSection = TblSec_ParagraphList;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
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
                    RelativePopup.ShowPopupRelative(_groupedOffice, BX_Officer, 400, height, true, "");
                    RelativePopup._triangleImage.Source = "";
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

        private static void OnAddCommentTapped(object sender, EventArgs e)
        {
            try
            {
                Isnew = true;
                CenterPopup.ShowPopupCenter(new CommentsPopup(""), 0.5, "Edit Custom Text");
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name		DrawScreen
        /// 
        /// <summary>	PopLayout Onload function
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        private void DrawScreen()
        {
            try
            {
                CenterPopup = new PopupLayouts(Content, this, PageScroll);
                if (Device.OS != TargetPlatform.iOS)
                    Content = CenterPopup;

                RelativePopup = new PopupLayouts(Content, this, PageScroll);
                if (Device.OS != TargetPlatform.iOS)
                    Content = RelativePopup;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        /// ------------------------------------------------------------------------------------------------
        /// Name		OnEditTapped
        /// 
        /// <summary>	Executed when the Edit function is tapped.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------

        private void OnEditTapped(object sender, EventArgs e)
        {
            try
            {
                IsCellEditable = !IsCellEditable;
                IsEditable = !IsEditable;
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
