using ServiceRequest.DependencyInterfaces;
using System;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
using ServiceRequest.UWP.DependencyClasses;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Media.Devices;
using ServiceRequest.AppContext;

[assembly: Xamarin.Forms.Dependency(typeof(AudioRecord))]
namespace ServiceRequest.UWP.DependencyClasses
{
    public class AudioRecord : IRecorder
    {
        /// -------------------------------------------------------------------------------------------------
        #region Private Properties and Variables
        /// -------------------------------------------------------------------------------------------------
        private MediaCapture _capture;
        private MediaCapture _playaudio;
        private InMemoryRandomAccessStream _buffer;
        private bool _record;
        private const string Path = "VoiceRecorder.mp3";
        private StorageFile _recordedFile;
        #endregion
        /// ------------------------------------------------------------------------------------------------
        
        /// -------------------------------------------------------------------------------------------------
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
                var inputDeviceId = MediaDevice.GetDefaultAudioCaptureId(AudioDeviceRole.Communications);
                if (!string.IsNullOrEmpty(inputDeviceId))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return false;
            }
        }
        public async void AudioRecordStart()
        {
            try
            {
                if (!_record)
                {
                    //already recored process  
                    StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                    await RecordProcess();
                    await
                        _capture.StartRecordToStreamAsync(MediaEncodingProfile.CreateMp3(AudioEncodingQuality.Auto), _buffer);
                    _recordedFile = await localFolder.CreateFileAsync(Path, CreationCollisionOption.ReplaceExisting);
                    await
                        _playaudio.StartRecordToStorageFileAsync(MediaEncodingProfile.CreateMp3(AudioEncodingQuality.Auto), _recordedFile);
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

       public void StopAudio()
        {
            //
        }
        ///
        /// Name                    AudioByte
        /// 
        /// <summary>               Converts file to audio bytes
        /// </summary>
        /// 
        public async Task<byte[]> AudioByte()
        {
            try
            {
                byte[] vedioStream = new byte[_buffer.Size];
                await _buffer.ReadAsync(vedioStream.AsBuffer(), (uint)_buffer.Size, InputStreamOptions.None);
                return vedioStream;
            }
            catch (Exception)
            {
                return null;
            }
        }

        ///
        /// Name            AudioRecordStop
        /// 
        /// <summary>       Stop the recoreder if it is recording.
        /// </summary>
        /// 
        public async void AudioRecordStop()
        {
            try
            {
                await _capture.StopRecordAsync();
                await _playaudio.StopRecordAsync();
                _record = false;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }

        ///
        /// Name                PlayRecordedAudio1
        /// 
        /// <summary>           Checks the status of the player and plays.
        /// </summary>
        /// 
        public async void PlayRecordedAudio1()
        {
            try
            {
                await PlayRecordedAudio();
                _capture.Dispose();
                _playaudio.Dispose();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------
        
        /// -------------------------------------------------------------------------------------------------
        #region Private Functions
        ///
        /// Name        RecordProcess
        /// 
        ///<summary>
        ///</summary>
        private async Task<bool> RecordProcess()
        {
            _buffer?.Dispose();
            _buffer = new InMemoryRandomAccessStream();
            _capture?.Dispose();
            _playaudio?.Dispose();
            try
            {
                MediaCaptureInitializationSettings settings = new MediaCaptureInitializationSettings
                {
                    StreamingCaptureMode = StreamingCaptureMode.Audio
                };
                _capture = new MediaCapture();
                await _capture.InitializeAsync(settings);
                _capture.RecordLimitationExceeded += sender =>
                {
                    _record = false;
                    throw new Exception("Record Limitation Exceeded ");
                };
                _capture.Failed += (sender, errorEventArgs) =>
                {
                    _record = false;
                    throw new Exception(string.Format("Code: {0}. {1}", errorEventArgs.Code, errorEventArgs.Message));
                };

                _playaudio = new MediaCapture();
                await _playaudio.InitializeAsync(settings);
                _playaudio.RecordLimitationExceeded += sender =>
                {
                    _record = false;
                    throw new Exception("Record Limitation Exceeded ");
                };
                _playaudio.Failed += (sender, errorEventArgs) =>
                {
                    _record = false;
                    throw new Exception(string.Format("Code: {0}. {1}", errorEventArgs.Code, errorEventArgs.Message));
                };
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                if (ex.InnerException != null && ex.InnerException.GetType() == typeof(UnauthorizedAccessException))
                {
                    throw ex.InnerException;
                }

            }
            return true;

        }

        /// 
        ///  Name           PlayRecordedAudio     
        ///  
        ///  <summary>     To Play the recorded audio
        ///  </summary>
        /// <returns></returns>
        private async Task PlayRecordedAudio()
        {
            try
            {
                await Windows.System.Launcher.LaunchFileAsync(_recordedFile, new Windows.System.LauncherOptions { DisplayApplicationPicker = false });
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// 
        ///  Name           ClearAudioFiles     
        ///  
        ///  <summary>     Clears the audio file after saving.
        ///  </summary>
        /// <returns></returns>
        public async void ClearAudioFiles()
        {
            try
            {
                await _buffer.FlushAsync();
                _buffer.Dispose();
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
