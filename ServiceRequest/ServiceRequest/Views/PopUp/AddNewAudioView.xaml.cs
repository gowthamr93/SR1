using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.DependencyInterfaces;
using System;
using System.Threading.Tasks;
using ServiceRequest.Pages;
using Xamarin.Forms;
using Idox.LGDP.Apps.Common.OnSite;
using ServiceRequest.ViewModels;
using ServiceRequest.AppContext;

namespace ServiceRequest.Views.PopUp
{
    public partial class AddNewAudioView
    {
        /// -------------------------------------------------------------------------------------------------
        #region Private Properties
        ///--------------------------------------------------------------------------------------------------
        ///
        public static bool _isRecording;
        public static bool _isStop;
        private bool IsSaveEnabled { get; set; }
		public static Action Playevent;
        public static Action StopRecord;
        private bool _isExecute;
        ///
        #endregion
        /// ------------------------------------------------------------------------------------------------
        
        /// -------------------------------------------------------------------------------------------------
        #region Public Constructor
        public AddNewAudioView()
        {
            try
            {
                InitializeComponent();
                // LoadPickerData();
                _isRecording = false;
				_isStop = false;
                Txt_AudioName.TextChanged += ValidateTextBox;
                Txt_AudioDescription.TextChanged += ValidateTextBox;
                IsSaveEnabled = false;
                //Tap record
                var tapRecord = new TapGestureRecognizer();
                tapRecord.Tapped += Record;
                Img_Start.GestureRecognizers.Add(tapRecord);
                Img_Stop.GestureRecognizers.Add(tapRecord);
                Img_Retake.GestureRecognizers.Add(tapRecord);
                //Tap Cancel
                var tapCancel = new TapGestureRecognizer();
                tapCancel.Tapped += (s, e) => Cancel();
                Lbl_Cancel.GestureRecognizers.Add(tapCancel);
                Bx_Cancel.GestureRecognizers.Add(tapCancel);
                //Tap Play
                var tapPlay = new TapGestureRecognizer();
                tapPlay.Tapped += PlayRecordedAudio;
                Img_Play.GestureRecognizers.Add(tapPlay);
                //Tap Save
                TapGestureRecognizer tapSave = new TapGestureRecognizer();
                tapSave.Tapped += OnSaveClicked;
                Btn_Save.GestureRecognizers.Add(tapSave);
                Bx_Save.GestureRecognizers.Add(tapSave);
                _isExecute = true;
                Btn_Save.TextColor = Styles.WindowBackgroundDark;
				Playevent += Refresh;
                StopRecord += StopRecording;
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
        /// Name        Record
        /// ------------------------------------------------------------------------------------------------
        ///<summary>    Start or stop the audio recorder.
        ///</summary>
        /// <param name="sender"> object</param>
        /// <param name="e"> EventArgs</param>
        private void Record(object sender, EventArgs e)
        {
            try
            {              
                    if (_isRecording)
                    {
                        //To stop audio record if it is recording.
                        _isRecording = false;
                        DependencyService.Get<IRecorder>().AudioRecordStop();
                        Img_Stop.IsVisible = false;
                        Img_Start.IsVisible = true;
                        Lbl_Play.IsVisible = true;
                        Img_Play.IsVisible = true;
                        Img_Retake.IsVisible = true;
                        Lbl_Title1.Text = "Click below button to start re-record.";
                    }
                    else
                    {
                        //To start audio record if it's not recording.
                        _isRecording = true;
                        Lbl_Title1.Text = "Recording...";
                        Img_Start.IsVisible = false;
                        Img_Stop.IsVisible = true;
                        Lbl_Play.IsVisible = false;
                        Img_Play.IsVisible = false;
                        Img_Retake.IsVisible = false;
                        DependencyService.Get<IRecorder>().AudioRecordStart();
                    }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
		public async void PlayRecordedAudio(object sender, EventArgs e)
        {
            try
            {
               

				if (Device.OS == TargetPlatform.iOS)
				{
					if (!_isStop)
					{
						_isStop = true;
						Img_Play.Source = "AudioStop.png";
						Lbl_Play.Text = "Stop Audio";
					    DependencyService.Get<IRecorder>().PlayRecordedAudio1();
					}
					else
					{
						_isStop = false;
						Img_Play.Source = "audio_play.png";
						Lbl_Play.Text = "Click here to Play";
						DependencyService.Get<IRecorder>().StopAudio();				
					}
				}
				else
				{
				  DependencyService.Get<IRecorder>().PlayRecordedAudio1();
				}


            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
      
        private void Refresh()
		{
			_isStop = false;
			Img_Play.Source = "audio_play.png";
			Lbl_Play.Text = "Click here to Play";;
		}

        private void StopRecording()
        {
            _isRecording = false;
            DependencyService.Get<IRecorder>().AudioRecordStop();
            Img_Stop.IsVisible = false;
            Img_Start.IsVisible = true;
            Lbl_Play.IsVisible = true;
            Img_Play.IsVisible = true;
            Img_Retake.IsVisible = true;
            Lbl_Title1.Text = "Recording Stopped! Click below button to start re-record.";
        }

        /// ------------------------------------------------------------------------------------------------
        ///  Name               Cancel
        /// ------------------------------------------------------------------------------------------------
        /// <summary>           Dismisses the popup on clicking Cancel button.
        /// </summary>
        private void Cancel()
        {
            try
            {
                DependencyService.Get<IRecorder>().ClearAudioFiles();
                SplitView.CenterPopupContent.DismisPopup();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
        /// ------------------------------------------------------------------------------------------------
        /// Name		OnSaveClicked
        /// 
        /// <summary>	Save audio and Dismisses the popup on clicking Save button.
        /// </summary>
        /// <param name="sender"> </param>
        ///    <param name="e"> event arguments</param>
        /// ------------------------------------------------------------------------------------------------
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                if (!_isRecording)
                {
                    if (_isExecute)
                    {
                        _isExecute = false;
                        if (Txt_AudioDescription.Text != null && (Txt_AudioName.Text != null))
                        {
                            Btn_Save.TextColor = Styles.MainAccent;
                            Btn_Save.IsEnabled = true;
                            IsSaveEnabled = true;
                        }
                        if (IsSaveEnabled)
                        {
                            var audio = await DependencyService.Get<IRecorder>().AudioByte();
							if (audio != null)
							{
								var doc = new OnSiteDocument()
								{
									Description = Txt_AudioDescription.Text,
									Extension = (Device.OS == TargetPlatform.iOS) ? ".wav" : ".mp3",
									Id = Guid.NewGuid().ToString(),
									MimeType = (Device.OS == TargetPlatform.iOS) ? "application/wav" : "application/mp3",
									Name = Txt_AudioName.Text,
									Status = SyncStatus.Changed,
								};
								var write = await FileSystem.WriteAsync(audio, doc.FileName);
								if (write.Error == null)
								{
									AppData.PropertyModel.AddDocument(doc);
									//saving doc to file for preserving it when loggedout
									PropertySummary.RefreshCount();
									AddNewImageView.DocumentAdded?.Invoke(doc);
									AppContext.AppContext.InspectionCell.RefreshList();
								}
								else
									await SplitView.DisplayAlert("Saving Failed", write.Error.Message, "OK", null);

								DependencyService.Get<IRecorder>().ClearAudioFiles();
								Cancel();
							}
							else
							{
								if (Device.OS == TargetPlatform.iOS)
								{
									
									DependencyService.Get<IDisplayAlertPopup>().NoAudioAlert();
								
								}
								else
								{
									await
										SplitView.DisplayAlert("No Audio Recorded", "Please give the audio input", "OK", null);
								}
							}
                        }

                        await Task.Run(() =>
                        {
                            Task.Delay(2000).Wait();
                            _isExecute = true;
                        });
                    }
                }
				else
				{
					if (Device.OS == TargetPlatform.iOS)
					{

						DependencyService.Get<IDisplayAlertPopup>().AudioIsRecording();

					}
					else
					{
						await SplitView.DisplayAlert("Warning", "You can not save the Audio while recording", "OK", null);
					}
				}
                
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                // ignored
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
                if ((!string.IsNullOrWhiteSpace(Txt_AudioName.Text)) && (!string.IsNullOrWhiteSpace(Txt_AudioDescription.Text)))
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
            string text = Txt_AudioName.Text;      //Get Current Text
            if (text.Length <= textlimit) return;
            text = text.Remove(text.Length - 1);  // Remove Last character
            Txt_AudioName.Text = text;        //Set the Old value
        }

        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
