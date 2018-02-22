using System.Threading.Tasks;

namespace ServiceRequest.DependencyInterfaces
{
    public interface IRecorder
    {
        bool IsMicrophoneAvailable();
        void AudioRecordStart();
        void AudioRecordStop();
		void StopAudio();
        void PlayRecordedAudio1();
        Task<byte[]> AudioByte();
        void ClearAudioFiles();
    }
}
