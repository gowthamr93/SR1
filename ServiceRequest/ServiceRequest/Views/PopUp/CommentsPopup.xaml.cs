using System;
using System.Collections.Generic;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.AppContext;
using ServiceRequest.Pages;

namespace ServiceRequest.Views.PopUp
{
    public partial class CommentsPopup
    {
        private SRiActionComment _currentContext;
        public Action<string> CustomText;
        public CommentsPopup(string text)
        {
            try
            {
                InitializeComponent();
                Edt_CustomText.Text = text;
                Lbl_CommentsTitle.Text = (text == "") ? "Add Comments" : "Edit Comments";
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }


        private void OnSaveTapped(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Edt_CustomText.Text) && VisitActionDetailsPage.Isnew)
                {
                    if (AppData.PropertyModel.SelectedAction.Action.Comments == null)
                        AppData.PropertyModel.SelectedAction.Action.Comments = new List<SRiActionComment>();

                    AppData.PropertyModel.SelectedAction.Action.Comments.Add
                        (new SRiActionComment
                        {
                            Comments = Edt_CustomText.Text,
                            KeyVal = String.Empty,
                            ActionKeyVal = AppData.PropertyModel.SelectedAction.Action.KeyVal,
                            CreatedDate = DateTime.Now,
                            UpdatedBy = AppData.MainModel.CurrentUser.IdoxId,
                            UVersion = String.Empty,
                            InspectionKeyVal = AppData.PropertyModel.SelectedVisit.Visit.Visit.InspectionKeyVal,
                        });
                    VisitActionDetailsPage.RefreshComment?.Invoke();
                    AppData.PropertyModel.SelectedAction.Action.Modified = true;
                }
                else if (!string.IsNullOrWhiteSpace(Edt_CustomText.Text) && !VisitActionDetailsPage.Isnew)
                {
                    CustomText.Invoke(Edt_CustomText.Text);
                }
                VisitActionDetailsPage.CenterPopup.DismisPopup();

            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private void OnCancelTapped(object sender, EventArgs e)
        {
            try
            {
                VisitActionDetailsPage.CenterPopup.DismisPopup();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

    }
}
