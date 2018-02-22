using System;
namespace ServiceRequest
{
	public interface IDisplayAlertPopup
	{
		void ShowAlert();
		void ShowAlertMessage();
		void ShowAlertParagraph();
		void NoAudioAlert();
		void AudioIsRecording();
		void InvalidCharacter();
		void ExceedLength();
		void OnlyDigits();
		void Exceed12();
		void ValidMail();
		void Filldetails();
		void ConflictHelp();
		void DeletionHelp();
	}
}
