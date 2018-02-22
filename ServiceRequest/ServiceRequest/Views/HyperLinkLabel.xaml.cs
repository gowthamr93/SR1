using System;
using ServiceRequest.DependencyInterfaces;
using ServiceRequest.ViewModels;
using Xamarin.Forms;
using ServiceRequest.AppContext;
using ServiceRequest.Pages;

namespace ServiceRequest.Views
{
    public partial class HyperLinkLabel
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public Variables, Properties
        /// ------------------------------------------------------------------------------------------------
        /// 

        public Action TextChanged { get; set; }
        public string GetText
        {
            get { return _getText; }
        }
        ///
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Private Variables and Properties
        /// ------------------------------------------------------------------------------------------------
        /// 
        private string InputValue { get; set; }
        #endregion
        /// ------------------------------------------------------------------------------------------------
        
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables 
        /// ------------------------------------------------------------------------------------------------
        /// 
        private string _getText;
        #endregion
        /// ------------------------------------------------------------------------------------------------
        
        /// ------------------------------------------------------------------------------------------------
        #region Public Constructors
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// <summary>
        /// Initializes a new instance of the <see cref="T:HyperLinkLabel"/> class.
        /// </summary>
        public HyperLinkLabel(string value)
        {
			try
			{
				InitializeComponent();
				Text = value;
				InputValue = string.Empty;
				_getText = value;
				TapGestureRecognizer tapInput = new TapGestureRecognizer();
				tapInput.Tapped += InputTapped;
				GestureRecognizers.Add(tapInput);
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
        /// ------------------------------------------------------------------------------------------------
        /// Name		InputTapped
        /// 
        /// <summary>
        /// To give input value
        /// </summary>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async void InputTapped(object sender, EventArgs e)
        {
			try
			{
				var objSender = sender as Label;

				if (objSender != null)
				{
					if (Text != ParagraphViewModel.DEFAULT_VALUE_TEXT)
						InputValue = Text;

					InputValue = await DependencyService.Get<IInputDialog>().ShowDialog(InputValue);
					if (!string.IsNullOrWhiteSpace(InputValue))
					{
						objSender.Text = _getText = InputValue;
						TextChanged.Invoke();
                        VisitActionDetailsPage.CurrentInstance.RefreshList();
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
