using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Idox.LGDP.Apps.Common.OnSite;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.AppContext;
using ServiceRequest.DependencyInterfaces;
using ServiceRequest.Models;
using ServiceRequest.Pages;
using Xamarin.Forms;

namespace ServiceRequest.Views.PopUp
{
    public partial class RequestErrorPopup : ContentView
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables        
        /// ------------------------------------------------------------------------------------------------
        /// 
        private TapGestureRecognizer m_tapRefresh, m_tapSend, m_tapCancel;
        private RequestResponseEventArgs m_oResponseArgs;
        private string m_Response_args;
        private const string UserInfoPlaceholder = "Please click here and describe what you were doing prior to receiving the error eg:\nCreating a new inspection or adding a photo\nWhich buttons did you press and in what order?\nCan you consistently replicate this error with the steps above?\nWhen did you last sync and how many cases were outstanding?";
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Variables 
        /// ------------------------------------------------------------------------------------------------
        /// 
        public Action Done;
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        /// ------------------------------------------------------------------------------------------------
        /// 

        ///
        /// ------------------------------------------------------------------------------------------------   
        /// Name		RequestErrorPopup
        /// <summary>
        /// Constructor fires when the param type is RequestResponseEventArgs
        /// </summary>
        /// <param name="responseArgs"> Request Response Args.</param>
        /// ------------------------------------------------------------------------------------------------
        ///
        public RequestErrorPopup(RequestResponseEventArgs responseArgs)
        {
            InitializeComponent();
            m_oResponseArgs = responseArgs;
            TapGestures();
            SetContent();
            //
            EdUserInfo.Text = UserInfoPlaceholder;
            EdUserInfo.TextColor = Styles.WindowBackgroundDark;
            this.Done += () =>
            {
                SplitView.CenterPopupContent.DismisPopup();
                SendMail();
                SplitView.Instace().DisplayAlert("Mail Sent", "The mail has sent successfully", "Ok");
            };
        }

        ///
        /// ------------------------------------------------------------------------------------------------   
        /// Name		RequestErrorPopup
        /// <summary>
        /// Constructor fires when the param type is String
        /// </summary>
        /// <param name="responseArgs">   String value.</param>
        /// ------------------------------------------------------------------------------------------------
        ///
        public RequestErrorPopup(String responseArgs)
        {
            InitializeComponent();
            m_Response_args = responseArgs;
            //imgRefresh.Source = "Recurring.png";
            m_tapSend = new TapGestureRecognizer();
            m_tapSend.Tapped += OnSendClicked;
            LblSend.GestureRecognizers.Add(m_tapSend);
            SetContent();
            this.Done += () => { /*LoginPage.PopupContent.DismisPopup();*/ };
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Private Functions
        /// ------------------------------------------------------------------------------------------------
        ///
        private async void SendMail()
        {
            StringBuilder body;
            //
            body = new StringBuilder();
            body.AppendLine(m_oResponseArgs.Details);
            body.AppendLine(GetOfficerInfo());
            body.AppendLine(GetDeviceInfo());
            if (!string.IsNullOrWhiteSpace(EdUserInfo.Text) && EdUserInfo.Text != UserInfoPlaceholder)
            {
                body.AppendLine("User Information:");
                body.AppendLine(EdUserInfo.Text);
            }
            await DependencyService.Get<ISendMail>().Send(body.ToString(), "SR Request Error Report");
        }

        ///
        /// ------------------------------------------------------------------------------------------------   
        /// Name		SetContent
        /// <summary>To set the text returned by content method to label text 
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        ///
        private void SetContent()
        {
            EdDebugInfo.Text = Content(m_oResponseArgs);
        }

        ///
		/// ------------------------------------------------------------------------------------------------   
		/// Name		Content
		/// <summary>
		/// To prepare text content for the label
		/// </summary>
		/// <param name="args"> Request Response Args.</param>
		/// <returns> String </returns>
		/// ------------------------------------------------------------------------------------------------
		///
		public static string Content(RequestResponseEventArgs args)
        {
            StringBuilder body;
            //
            body = new StringBuilder();
            body.AppendLine(args.Details);
            body.AppendLine(GetOfficerInfo());
            body.AppendLine(GetDeviceInfo());
            //
            return body.ToString();
        }

        ///
		/// ------------------------------------------------------------------------------------------------   
		/// Name		GetOfficerInfo
		/// <summary>
		/// To prepare text content for the label
		/// </summary>
		/// <returns> String </returns>
		/// ------------------------------------------------------------------------------------------------
		///
		public static string GetOfficerInfo()
        {
            StringBuilder body;
            DateTime? currentDateTime = DateTime.Now;
            //
            body = new StringBuilder();
            body.AppendLine("App Information:");
            body.AppendLine(string.Format("User Logged in: {0}", AppData.MainModel.CurrentUser.IdoxId));
            body.AppendLine(string.Format("Environment: {0}", AppData.MainModel.Environment));
            body.AppendLine(string.Format("Date/Time: {0}", currentDateTime.LongishDateTimeFormat()));
            body.AppendLine(string.Format("App Version: {0}", AppData.MainModel.CurrentUser.Version.Text));
            //
            return body.ToString();
        }

        ///
        /// ------------------------------------------------------------------------------------------------   
        /// Name		GetDeviceInfo
        /// <summary>
        /// To prepare text content for the label
        /// </summary>
        /// <returns> String </returns>
        /// ------------------------------------------------------------------------------------------------
        ///
        public static string GetDeviceInfo()
        {
            StringBuilder body;
            DeviceInfoModel info;
            //
            body = new StringBuilder();
            body.AppendLine("Device Information:");
            //
            info = DependencyService.Get<IDeviceInfo>().GetDeviceInfo();
            if (info != null)
            {
                body.AppendLine(string.Format("Name: {0}", info.Name));
                body.AppendLine(string.Format("Model Name: {0}", info.ModelName));
                body.AppendLine(string.Format("OS Version: {0}", info.OsVersion));
                body.AppendLine(string.Format("OS Name: {0}", info.OsVersionName));
                body.AppendLine(string.Format("Total Memory: {0} GB", info.TotalSpace));
                body.AppendLine(string.Format("Free Memory: {0} GB", info.FreeSpace));
            }
            //
            return body.ToString();
        }
        private void TapGestures()
        {
            //
            m_tapSend = new TapGestureRecognizer();
            m_tapSend.Tapped += OnSendClicked;
            LblSend.GestureRecognizers.Add(m_tapSend);
            //
            m_tapCancel = new TapGestureRecognizer();
            m_tapCancel.Tapped += OnCancelClicked;
            LblCancel.GestureRecognizers.Add(m_tapCancel);
            //
            EdDebugInfo.Focused += OnDebugInfoFocused;
            EdUserInfo.Focused += OnUserInfoFocused;
            EdUserInfo.Unfocused += OnUserInfoUnFocused;
            //
        }
        /// ------------------------------------------------------------------------------------------------   
        /// Name		OnSendClicked
        /// <summary>
        /// To close the Popup
        /// </summary>
        /// <param name="sender"> Default.</param>
        /// <param name="e">  Event Args.</param>
        /// ------------------------------------------------------------------------------------------------
        ///
        private void OnSendClicked(object sender, EventArgs e)
        {
            Done();
        }

        ///
        /// ------------------------------------------------------------------------------------------------   
        /// Name		OnCancelClicked
        /// <summary>
        /// To close the Popup
        /// </summary>
        /// <param name="sender"> Default.</param>
        /// <param name="e">  Event Args.</param>
        /// ------------------------------------------------------------------------------------------------
        ///
        private void OnCancelClicked(object sender, EventArgs e)
        {
            SplitView.CenterPopupContent.DismisPopup();
        }

        private void OnUserInfoFocused(object sender, FocusEventArgs e)
        {
            if (EdUserInfo.Text == UserInfoPlaceholder)
            {
                EdUserInfo.Text = string.Empty;
                EdUserInfo.TextColor = Color.Black;
            }
        }

        private void OnUserInfoUnFocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EdUserInfo.Text))
            {
                EdUserInfo.Text = UserInfoPlaceholder;
                EdUserInfo.TextColor = Styles.WindowBackgroundDark;
            }
        }

        private void OnDebugInfoFocused(object sender, FocusEventArgs e)
        {
            EdDebugInfo.Unfocus();
            LblDebugTitle.Focus();
        }

        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
