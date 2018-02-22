using Idox.LGDP.Apps.Common.OnSite;
using Idox.LGDP.Apps.ServiceRequest.Client;
using PopUpSample;
using ServiceRequest.Pages;
using ServiceRequest.Views.PopUp;
using ServiceRequest.Views.ViewCells;
using System;
using System.Collections.Generic;
using System.Linq;
using ServiceRequest.AppContext;
using Xamarin.Forms;

namespace ServiceRequest.Views
{
    public partial class VisitActionDetailsView
    {

        /// ------------------------------------------------------------------------------------------------
        #region publicVariables
        /// ------------------------------------------------------------------------------------------------
        /// 
        ///       
        public static PopupLayouts CenterPopup { get; private set; }
        public static PopupLayouts RelativePopup { get; private set; }

        public bool IsEditable
        {
            get { return _isEditable; }
            set
            {
                if (_isEditable != value)
                {
                    _isEditable = value;
                    OnEditPropertyChanged();
                }
            }
        }

        private List<SRiActionParagraph> Paralist { get; }
        public static VisitActionDetailsView CurrentInstance { get; private set; }
        #endregion

        #region privateVariables
        /// ------------------------------------------------------------------------------------------------

        private GroupedListView _groupedOffice;

        private ScrollView PageScroll { get; set; }

        private bool _isEditable;
       
      #endregion

        /// ------------------------------------------------------------------------------------------------
        #region Public Constructors
        /// ------------------------------------------------------------------------------------------------
        /// 

        public VisitActionDetailsView()
        {
            InitializeComponent();
            Paralist = AppData.PropertyModel.SelectedAction.Action.Paragraphs;
            LoadData();
            DrawScreen();
            TapGestures();
            RefreshList();
            IsEditable = false;
            CurrentInstance = this;
            CheckLegislation();
        }

        #endregion


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
            if (await DisplayAlert("Delete", "Do you want to delete this item?", "Yes", "No"))
            {
                Paralist.Remove(sRiActionParagraph);
                RefreshList();
                if (Paralist.Count == 0) IsEditable = false;
            }
        }
        #endregion

        /// ------------------------------------------------------------------------------------------------

        #region private Functions

        /// ------------------------------------------------------------------------------------------------
        /// 

        private void OnEditPropertyChanged()
        {
            Lbl_Edit.Text = IsEditable ? "Done" : "Edit";
            RefreshList();
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
            var visit = AppData.PropertyModel.SelectedVisit.Visit;
            if (AppData.ConfigModel.Legislations(visit.Organisation, visit.Visit.VisitType).Count != 0)
            {
                var legislationStackLayout = new VisitActionDetailsViewCell();
                Sl_View.Children.Add(legislationStackLayout);
            }
        }

        private void TapGestures()
        {
            //Cancel
            var tapCancel = new TapGestureRecognizer();
            tapCancel.Tapped += CancelClick;
            Lbl_Cancel.GestureRecognizers.Add(tapCancel);

            //Save
            var tapSave = new TapGestureRecognizer();
            tapSave.Tapped += SaveClick;
            Lbl_Save.GestureRecognizers.Add(tapSave);

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
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name		CancelClick
        /// 
        /// <summary>	Executed when the cancel button is clicked.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        private void CancelClick(object sender, EventArgs e)
        {
            SplitView.Instace().Navigation.PopModalAsync();
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name		SaveClick
        /// 
        /// <summary>	Executed when the save button is clicked.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        private void SaveClick(object sender, EventArgs e)
        {   
            Exception error;
            AppData.PropertyModel.SelectedAction.Action.ActualDateTime = Dtp_CompletedDate.Date + Tmp_CompletedDate.Time;
            AppData.PropertyModel.SelectedAction.Action.Hours = AppContext.AppContext.RevertHt(Pkr_TimeTakenHours.Items[Pkr_TimeTakenHours.SelectedIndex], Pkr_TimeTakenMinutes.Items[Pkr_TimeTakenMinutes.SelectedIndex]);
            var officeDetails = _groupedOffice.SelectedValue?.Split('-');
            if (officeDetails != null)
            {
                var officer = officeDetails[0].Remove(officeDetails[0].Length- 1);
                AppData.PropertyModel.SelectedAction.Action.Officer = officer;
            }
            var legislationDetails = VisitActionDetailsViewCell.GroupedLegislation.SelectedValue?.Split('-');
            if (legislationDetails != null)
            {
                var legislation = legislationDetails[0].Remove(legislationDetails[0].Length - 1);
                AppData.PropertyModel.SelectedAction.Action.LegislationType = legislation;
            }
            AppData.PropertyModel.SaveAction(out error);
          //  AppData.PropertyModel.SaveVisit(out error);
            if (error == null)
            {
                SplitView.Instace().Navigation.PopModalAsync();
                AppContext.AppContext.RefreshList.Invoke(sender, e);
            }
            else
                throw error;
        }
        private void AddPropertyClick(object sender, EventArgs e)
        {
            CenterPopup.ShowPopupCenter(new ParagraphView(RefreshList), 0.5, "Adding Paragraph");
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name		RefreshList
        /// 
        /// <summary>	Refresh the lists while the paragraph is selected and adds the next type and para.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        private void RefreshList()
        {
            int count = 0;
            TblSec_ParagraphList.Clear();
            if (Paralist != null)
            {
                foreach (var sRiActionParagraph in Paralist)
                {
                    sRiActionParagraph.Modified = IsEditable;
                    AddTypedCell(sRiActionParagraph);
                    count++;
                }
            }

            Tblvw_StandardParagraph.HeightRequest = count * 100 + 150;
            Sl_Main.HeightRequest = count * 100 + 185;
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
            var visitActionDetails = AppData.PropertyModel.SelectedAction.Action;
            var hoursTaken = AppData.PropertyModel.SelectedAction.Action.Hours.ToConvertHt();

            foreach (var h in AppContext.AppContext.CpiHours)
            {
                Pkr_TimeTakenHours.Items.Add(h);
            }
            foreach (var m in AppContext.AppContext.CpiMinutes)
            {
                Pkr_TimeTakenMinutes.Items.Add(m);
            }
            if (visitActionDetails.ActualDateTime != null)
            {
                Dtp_CompletedDate.Date = visitActionDetails.ActualDateTime.Value.Date;
                Tmp_CompletedDate.Time = visitActionDetails.ActualDateTime.Value.TimeOfDay;
            }

            Lbl_ScheduledDate.Text = visitActionDetails.DueDate != null ? visitActionDetails.DueDate.LongishDateTimeFormat() : "No Value Set";

            Pkr_TimeTakenHours.SelectedIndex =
                Pkr_TimeTakenHours.Items.IndexOf(hoursTaken.Item1 ?? Pkr_TimeTakenHours.Items[0]);

            Pkr_TimeTakenMinutes.SelectedIndex =
                Pkr_TimeTakenMinutes.Items.IndexOf(hoursTaken.Item2 ?? Pkr_TimeTakenMinutes.Items[0]);

            _groupedOffice = new GroupedListView(
                    AppData.ConfigModel.OfficerList(AppData.PropertyModel.SelectedVisit.Visit.Organisation,
                    AppData.PropertyModel.SelectedVisit.GroupMod).OrderBy(o => o.OfficerCode + o.Name)
                    .Select(a => new KeyValuePair<string, string>(a.OfficerCode, a.Name)).ToList(), Lbl_Officer);

            int index;

            var action = AppData.PropertyModel.SelectedAction.Action;
            var orderedOfficers = AppData.ConfigModel.OfficerList(AppData.PropertyModel.SelectedVisit.Visit.Organisation,
                                  AppData.PropertyModel.SelectedVisit.GroupMod).OrderBy(o => o.OfficerCode + o.Name).ToList();

            index = 0;
            if (!string.IsNullOrEmpty(action.Officer))
            {
                for (int i = 0; i < orderedOfficers.Count; i++)
                    if (orderedOfficers[i].OfficerCode.Equals(action.Officer))
                    {
                        index = i;
                        break;
                    }
                Lbl_Officer.Text = $"{orderedOfficers[index].OfficerCode} - {orderedOfficers[index].Name}";
            }
            else
            {
                Lbl_Officer.Text = "None";
            }
            Lbl_Title.Text = visitActionDetails.ActionType;
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
            if (sRiActionParagraph.PlainText.Contains("^IN"))
            {
               // TblSec_ParagraphList.Add(new ParagraphInputViewCell() { BindingContext = sRiActionParagraph });
            }
            else
            {
                var customCell = AppData.PropertyModel.SelectedAction.Action.Paragraphs.FirstOrDefault(x => x == sRiActionParagraph);
                if (customCell != null)/* customCell.CellType = CellTypes.Custom;*/
                TblSec_ParagraphList.Add(new ParagraphCustomViewCell() { BindingContext = sRiActionParagraph });
            }
            //else if (sRiActionParagraph.PlainText.Contains("^IN"))
            //{
            //    TblSec_ParagraphList.Add(new ParagraphInputViewCell() { BindingContext = sRiActionParagraph });
            //}
            //else
            //{
            //    TblSec_ParagraphList.Add(new ParagraphNormalViewCell() { BindingContext = sRiActionParagraph });
            //}
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
            if (_groupedOffice != null)
            {
                RelativePopup.ShowPopupRelative(_groupedOffice, Lbl_Officer, 400, 600, true, "");
                RelativePopup._triangleImage.Source = "";
            }
            else
            {
               await DisplayAlert("No Officer", "No officer types set up in Uniform for this visit type. Please contact your Uniform administrator for assistance.", "Ok");
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
                CenterPopup = new PopupLayouts(Content, this, PageScroll);
                Content = CenterPopup;
                RelativePopup = new PopupLayouts(Content, this, PageScroll);
                Content = RelativePopup;
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
            IsEditable = !IsEditable;
        }
        #endregion
    }
}
