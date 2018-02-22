using System;
using System.IO;
using System.Threading.Tasks;
using AudioToolbox;
using AVFoundation;
using Foundation;
using ServiceRequest.AppContext;
using ServiceRequest.DependencyInterfaces;
using ServiceRequest.iOS;
using ServiceRequest.Views.PopUp;

[assembly: Xamarin.Forms.Dependency(typeof(AudioRecord))]
namespace ServiceRequest.iOS
{
	public class AudioRecord : IRecorder
	{

		/// ------------------------------------------------------------------------------------------------
		#region Private Properties
		/// 
	    private	AVAudioRecorder recorder; 
		private AVAudioPlayer _AudioPlayer;
		AVAudioSession audioSession;
		private NSError error; 
		private NSUrl url; 
		private NSDictionary settings;
		private string audioFilePath;
		private bool _isExecute;		

		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------

		/// ------------------------------------------------------------------------------------------------
		#region Public Constructor
		/// 

		public AudioRecord()
		{
			try
			{
				_isExecute = true;
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());
			}
		}

		///
		#endregion
		/// ------------------------------------------------------------------------------------------------


		/// ------------------------------------------------------------------------------------------------
		#region Public Functions

		///
		/// Name                    IsMicrophoneAvailable
		/// 
		/// <summary>               To check whether the audio device is Plugged-in. 
		/// </summary>
		/// 

		public bool IsMicrophoneAvailable()
		{
			try
			{
				audioSession = AVAudioSession.SharedInstance();
				var err = audioSession.SetCategory(AVAudioSessionCategory.PlayAndRecord);
				audioSession.SetActive(true);
				return true;
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());
				return false;
			}
		}

		///
		/// Name                    AudioRecordStart
		/// 
		/// <summary>               Start the recorder to record the audio.
		/// </summary>
		/// 
		public void AudioRecordStart() 		{ 			try 			{
			   string fileName = string.Format("Myfile{0}.wav", DateTime.Now.ToString("yyyyMMddHHmmss")); 			    audioFilePath = Path.Combine(Path.GetTempPath(), fileName); 				url = NSUrl.FromFilename(audioFilePath); //set up the NSObject Array of values that will be combined with the keys to make the NSDictionary NSObject[] values = new NSObject[] { NSNumber.FromFloat (44100.0f), //Sample Rate NSNumber.FromInt32 ((int)AudioToolbox.AudioFormatType.LinearPCM), //AVFormat NSNumber.FromInt32 (2), //Channels NSNumber.FromInt32 (16), //PCMBitDepth NSNumber.FromBoolean (false), //IsBigEndianKey NSNumber.FromBoolean (false) //IsFloatKey }; 			
				NSObject[] values = new NSObject[] 
				{   NSNumber.FromFloat (44100.0f), 
					NSNumber.FromInt32 ((int)AudioFormatType.LinearPCM), 
					NSNumber.FromInt32 (2), 				
				};

				NSObject[] keys = new NSObject[] 
				{   AVAudioSettings.AVSampleRateKey, 
					AVAudioSettings.AVFormatIDKey, 
					AVAudioSettings.AVNumberOfChannelsKey, 					
				}; 			    settings = NSDictionary.FromObjectsAndKeys(values, keys); 				recorder = AVAudioRecorder.Create(url, new AudioSettings(settings), out error); 				recorder.PrepareToRecord(); 				recorder.Record();
				AddNewAudioView.Playevent.Invoke(); 			} 			catch (Exception ex) 			{ 				LogTracking.LogTrace(ex.ToString());  			} 		} 
		///
        /// Name                PlayRecordedAudio1
        /// 
        /// <summary>           Checks the status of the player and plays it.
        /// </summary>
        /// 
		public void PlayRecordedAudio1()
		{
			try
			{
				if (_isExecute)
				{
						_isExecute = false;
					  
						_AudioPlayer = AVAudioPlayer.FromUrl(url);
						_AudioPlayer.NumberOfLoops = 0; 
						_AudioPlayer.Play();
						_AudioPlayer.FinishedPlaying += (sender, e) => 
					{
						_isExecute = true;
						AddNewAudioView.Playevent.Invoke();
					};

				}
			}
			
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());

			}
		}

		///
		/// Name            AudioRecordStop
		/// 
		/// <summary>       Stop the recoreder if it is in recording.
		/// </summary>
		/// 
		public void AudioRecordStop()
		{
			try
			{
				recorder?.Stop();
			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());

			}
		}

		public void StopAudio()
		{
			try
			{
				_isExecute = true;
				_AudioPlayer?.Stop();
			}
			catch(Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());
			}
		}

		///
		/// Name                    AudioByte
		/// 
		/// <summary>               Converts file to audio byte for saving it.
		/// </summary>
		/// 
		public async Task<byte[]> AudioByte()
		{
			try
			{				
				byte[] bytes = null;

				using (var sourceStream = File.Open(audioFilePath, FileMode.Open, FileAccess.Read))
				{
					var binaryReader = new BinaryReader(sourceStream);
					bytes = binaryReader.ReadBytes((int)sourceStream.Length);
				}

				return bytes;				
			}
			catch (FileNotFoundException e)
			{
				return null;
			}
			catch (Exception e)
			{
				return null;
			}
		}



		/// 
		///  Name           ClearAudioFiles     
		///  
		///  <summary>     Clears the audio file after saving.
		///  </summary>
		/// <returns></returns>
		public void ClearAudioFiles()
		{
			try
			{
				_AudioPlayer?.Stop();
				_AudioPlayer?.Dispose();
				recorder?.Dispose();
				File.Delete(audioFilePath);
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