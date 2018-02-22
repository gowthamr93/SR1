using System;
using System.Threading.Tasks;
using Idox.LGDP.Apps.ServiceRequest.Client;
using Xamarin.Forms;
using ServiceRequest.AppContext;
using ServiceRequest.Pages;
using ServiceRequest.Views.PopUp;

namespace ServiceRequest.Views.ViewCells
{
    public partial class ActionCommentViewCell
    {
        private string InputValue { get; set; }

        public static ActionCommentViewCell CurrentInstance { get; private set; }

        private static CommentsPopup CommentsPopup { get; set; }

        private SRiActionComment _currentContext;

        public ActionCommentViewCell()
        {
            try
            {
                InitializeComponent();
                InputValue = string.Empty;
                CurrentInstance = this;

                TapGestureRecognizer imgDeleteTap = new TapGestureRecognizer();
                Img_Delete.GestureRecognizers.Add(imgDeleteTap);
                imgDeleteTap.Tapped += CommentDelete_Tapped;

                TapGestureRecognizer tapCustomText = new TapGestureRecognizer();
                LblCustomText.GestureRecognizers.Add(tapCustomText);
                tapCustomText.Tapped += LblCustomText_Tapped;
                BindingContextChanged += ActionCommentViewCell_BindingContextChanged;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private void LblCustomText_Tapped(object sender, EventArgs e)
        {
            try
            {
                VisitActionDetailsPage.Isnew = false;
                VisitActionDetailsPage.CenterPopup.ShowPopupCenter(CommentsPopup = new CommentsPopup(LblCustomText.Text), 0.5, "Edit Custom Text");
                CommentsPopup.CustomText = SaveCustomText;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private void CommentDelete_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (_currentContext != null)
                    VisitActionDetailsPage.CurrentInstance.OnCommentDeleted(_currentContext);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        private void ActionCommentViewCell_BindingContextChanged(object sender, EventArgs e)
        {
            try
            {
                var objsender = (SRiActionComment)BindingContext;
                if (objsender != null)
                    _currentContext = objsender;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        private void SaveCustomText(string editorText)
        {
            try
            {
                if (_currentContext.Comments == null || !_currentContext.Comments.Equals(editorText))
                    _currentContext.Modified = true;
                //
                _currentContext.Comments = editorText;
                VisitActionDetailsPage.RefreshComment?.Invoke();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }

        }

    }
}
