using System;
using ServiceRequest.Pages;
using Xamarin.Forms;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.AppContext;
using ServiceRequest.ViewModels;
using Idox.LGDP.Apps.Common.OnSite;
using System.IO;
using System.Threading.Tasks;

namespace ServiceRequest.Views.PopUp
{
    public partial class AddNewImageView:ContentView
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Properties
        /// ------------------------------------------------------------------------------------------------
        /// 
        private byte[] ImageBytes { get; set; }
        private bool IsSaveEnabled { get; set; }

        private bool _isExecute;
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Properties
        public static Action<OnSiteDocument> DocumentAdded;
        ///
        #endregion
        /// ------------------------------------------------------------------------------------------------
        
        /// ------------------------------------------------------------------------------------------------
        #region public Constructor
        /// ------------------------------------------------------------------------------------------------
        /// 
        public AddNewImageView()
        {
            try
            {
                InitializeComponent();
                IsSaveEnabled = false;
                ImageBytes = ImageData();
                // Tap cancel
                var tapCancel = new TapGestureRecognizer();
                tapCancel.Tapped += Lbl_CancelClicked;
                Lbl_Cancel.GestureRecognizers.Add(tapCancel);
                //Tap Save
                TapGestureRecognizer tapSave = new TapGestureRecognizer();
                tapSave.Tapped += OnSaveClicked;
                Btn_Save.GestureRecognizers.Add(tapSave);

                Txt_ImageName.TextChanged += ValidateTextBox;
                Txt_ImageDescription.TextChanged += ValidateTextBox;
                if (ImageBytes != null)
                    Img_Image.Source = ImageSource.FromStream(() => new MemoryStream(ImageBytes));
                Txt_ImageName.TextChanged += TextChanged;
                _isExecute = true;
                Btn_Save.TextColor = Styles.WindowBackgroundDark;
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

        /// ------------------------------------------------------------------------------------------------
        /// Name		ImageData
        /// 
        /// <summary>	To get the image in form of stream.
        /// </summary>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private byte[] ImageData()
        {
            try
            {
                using (MemoryStream oImageData = new MemoryStream())
                {
                    AppContext.AppContext.ImageSource.CopyTo(oImageData);
                    var bytesData = oImageData.ToArray();
                    return bytesData;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }
        /// ------------------------------------------------------------------------------------------------
        /// Name		Lbl_CancelClicked
        /// 
        /// <summary>	Dismisses the popup on clicking Cancel button.
        /// </summary>
        /// <param name="sender"> </param>
        ///    <param name="e"> event arguments</param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void Lbl_CancelClicked(object sender, EventArgs e)
        {
            try
            {               
                    SplitView.CenterPopupContent.DismisPopup();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
        /// ------------------------------------------------------------------------------------------------
        /// Name        OnSaveClicked
        /// 
        /// <summary>	Save image and Dismisses the popup on clicking Save button.
        /// </summary>
        /// <param name="sender"> </param>
        ///    <param name="e"> event arguments</param>
        /// ------------------------------------------------------------------------------------------------
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                if (_isExecute)
                {
                    _isExecute = false;
                    if (Txt_ImageDescription.Text != null && (Txt_ImageName.Text != null))
                    {
                        Btn_Save.TextColor = Styles.MainAccent;
                        Btn_Save.IsEnabled = true;
                        IsSaveEnabled = true;
                    }
                    if (IsSaveEnabled)
                    {
                        OnSiteDocument doc;
                        FileSystemArgs write;
                        doc = new OnSiteDocument()
                        {
                            Description = Txt_ImageDescription.Text,
                            Extension = ".png",
                            Id = Guid.NewGuid().ToString(),
                            MimeType = "application/png",
                            Name = Txt_ImageName.Text,
                            Status = SyncStatus.Changed,
                        };
                        write = await FileSystem.WriteAsync(ImageBytes, doc.FileName);
                        if (write.Error == null)
                        {
                            AppData.PropertyModel.AddDocument(doc);
                            //saving doc to file for preserving it when loggedout
                            PropertySummary.RefreshCount();
                            DocumentAdded?.Invoke(doc);
                            AppContext.AppContext.InspectionCell.RefreshList();
                        }
                        else
                            await SplitView.DisplayAlert("Saving Failed", write.Error.Message, "OK",null);
                       
                            SplitView.CenterPopupContent?.DismisPopup();                       
                    }
                   
                    await Task.Run(() =>
                    {
                        Task.Delay(2000).Wait();
                        _isExecute = true;
                    });
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        /// ------------------------------------------------------------------------------------------------
        /// Name         ValidateTextBox
        /// ------------------------------------------------------------------------------------------------
        /// <summary>    Enable/Disable Done Button on Text Changed in Entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void ValidateTextBox(object sender, EventArgs eventArgs)
       {
            try
            {
                if ((!string.IsNullOrWhiteSpace(Txt_ImageDescription.Text)) && (!string.IsNullOrWhiteSpace(Txt_ImageName.Text)))
                {
                    Btn_Save.TextColor = Styles.MainAccent;
                    Btn_Save.IsEnabled = IsSaveEnabled = true;
                }
                else
                {
                    Btn_Save.IsEnabled = IsSaveEnabled = false;
                    Btn_Save.TextColor = Styles.WindowBackgroundDark;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            var textlimit = 250;     //Enter text limit
            string text = Txt_ImageName.Text;      //Get Current Text
            if (text.Length <= textlimit) return;
            text = text.Remove(text.Length - 1);  // Remove Last character
            Txt_ImageName.Text = text;        //Set the Old value
        }

        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
