using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Android.Media;
using Android.Net;
using Java.Lang;
using ServiceRequest.AppContext;
using ServiceRequest.DependencyInterfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(ServiceRequest.Droid.DependencyClasses.AudioRecord))]
namespace ServiceRequest.Droid.DependencyClasses
{
    public class AudioRecord : IRecorder
    {
        /// ------------------------------------------------------------------------------------------------
        #region Privare Properties
        /// 
        private MediaRecorder _recorder;
        private MediaPlayer _player;
        private bool _paused = true;
        private const string Path = "/sdcard/voicerecorder.mp3";

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
                // IntializePlayer();
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
                _recorder = new MediaRecorder();
                _recorder.Reset();
                _recorder.SetAudioSource(AudioSource.Mic);
                return true;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return false;
            }
        }
        public void StopAudio()
        {
            
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
                var audioStream = File.ReadAllBytes(Path);
                return audioStream;
            }
            catch (FileNotFoundException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        ///
        /// Name                    AudioRecordStart
        /// 
        /// <summary>               Start the recorder to record the audio.
        /// </summary>
        /// 
        public void AudioRecordStart()
        {
            try
            {
                _recorder = new MediaRecorder();
                _recorder.Reset();
                _recorder.SetAudioSource(AudioSource.Mic);
                _recorder.SetOutputFormat(OutputFormat.Default);
                _recorder.SetAudioEncoder(AudioEncoder.Default);
                _recorder.SetOutputFile(Path);
                _recorder.Prepare();
                _recorder.Start();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        ///
        /// Name            AudioRecordStop
        /// 
        /// <summary>       Stop the recoreder if it is recording.
        /// </summary>
        /// 
        public void AudioRecordStop()
        {
            try
            {
                _recorder?.Stop();
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
        public void PlayRecordedAudio1()
        {
            try
            {
                if (_paused && _player != null)
                {
                    _paused = false;
                    PlayAudio();
                }
                else
                {
                    IntializePlayer();
                    PlayAudio();
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// 
        ///  Name           PlayAudio     
        ///  
        ///  <summary>     To Play the recorded audio
        ///  </summary>
        /// <returns></returns>
        private void PlayAudio()
        {
            var file = new Java.IO.File(Path);

            if (file.Exists())
            {
                {
                    Intent intent = new Intent(Intent.ActionView);
                    intent.SetDataAndType(Uri.Parse("file:///" + Path), Path.Split('.').LastOrDefault().GetContentType());
                    intent.SetFlags(ActivityFlags.ClearTop);
                    Forms.Context.StartActivity(intent);
                }
            }
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Private Functions
        ///
        /// Name                InitializePlayer
        /// 
        /// <summary>           Initialize the Player
        /// </summary>
        /// 
        private void IntializePlayer()
        {
            try
            {
                _player = new MediaPlayer();
                _player.SetDataSource(Path);
                _player.Prepare();
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
        public void ClearAudioFiles()
        {
            try
            {
                File.Delete(Path);
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