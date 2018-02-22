using System;
using ServiceRequest;
using ServiceRequest.iOS;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(DisplayAlertPopup))]
namespace ServiceRequest.iOS
{
	public class DisplayAlertPopup: IDisplayAlertPopup
	{
		public void ShowAlert()
		{
			UIAlertView alert = new UIAlertView();
			alert.Title = "Already Saved";
			alert.Message = "This case is already saved to the device, if you save it " +
								   "again you will lose any changes not uploaded to Uniform.";
			alert.AddButton("OK");
			alert.Show();
		}
		public void ShowAlertMessage()
		{
			UIAlertView alert = new UIAlertView();
			alert.Message = "No results found";
			alert.AddButton("OK");
			alert.Show();
		}
		public void ShowAlertParagraph()
		{
			UIAlertView alert = new UIAlertView();
			alert.Title = "Error";
			alert.Message = "All items must have both Type and Paragraph set.";
			alert.AddButton("OK");
			alert.Show();
		}
		public void NoAudioAlert()
		{
			UIAlertView alert = new UIAlertView();
			alert.Title = "No Audio Recorded";
			alert.Message = "Please give the audio input.";
			alert.AddButton("OK");
			alert.Show();
		}
		public void AudioIsRecording()
		{
			UIAlertView alert = new UIAlertView();
			alert.Title = "Warning";
			alert.Message = "You can not save the Audio while recording.";
			alert.AddButton("OK");
			alert.Show();
		}

		public void InvalidCharacter()
		{
			UIAlertView alert = new UIAlertView();
			alert.Title = "Warning";
			alert.Message = "You have entered invalid character.";
			alert.AddButton("OK");
			alert.Show();
		}

		public void ExceedLength()
		{
			UIAlertView alert = new UIAlertView();
			alert.Title = "Warning";
			alert.Message = "Length should not exceed 50 digits";
			alert.AddButton("OK");
			alert.Show();
		}
		public void OnlyDigits()
		{
			UIAlertView alert = new UIAlertView();
			alert.Title = "Warning";
			alert.Message = "Phone numbers can not contain letters or special characters";
			alert.AddButton("OK");
			alert.Show();
		}
		public void Exceed12()
		{
			UIAlertView alert = new UIAlertView();
			alert.Title = "Warning";
			alert.Message = "Length should not exceed 12 digits";
			alert.AddButton("OK");
			alert.Show();
		}
		public void ValidMail()
		{
			UIAlertView alert = new UIAlertView();
			alert.Title = "Wrong E-Mail Format";
			alert.Message = "Please Enter a vaild Mail-id";
			alert.AddButton("OK");
			alert.Show();
		}
		public void Filldetails()
		{
			UIAlertView alert = new UIAlertView();
			alert.Title = "Warning";
			alert.Message = "Please fill the name";
			alert.AddButton("OK");
			alert.Show();
		}
		public void ConflictHelp()
		{
			UIAlertView alert = new UIAlertView();
			alert.Title = "Conflicts";
			alert.Message = "These items have conflicted with items in the cloud, you could be overwriting changes made by a Uniform user. \nSelect any items you wish to continue to upload and the cloud version will be overwritten. \n\nA record of any conflict will be available in the task log.";
			alert.AddButton("OK");
			alert.Show();
		}
		public void DeletionHelp()
		{
			UIAlertView alert = new UIAlertView();
			alert.Title = "Deletion";
			alert.Message = "Not all items were recognised by the cloud, these items are either deleted from the cloud or referenced under a new ID. \n\nYou can either mark these items for deletion now, or make a note of the changes you have made and delete them at a later date. ";
			alert.AddButton("OK");
			alert.Show();
		}

	}
}
