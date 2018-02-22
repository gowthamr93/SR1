using System;
using System.Threading.Tasks;
using ServiceRequest.iOS;
using ServiceRequest.DependencyInterfaces;
using ServiceRequest.AppContext;
using MimeKit;
using MailKit.Net.Smtp;

[assembly: Xamarin.Forms.Dependency(typeof(SendMail))]
namespace ServiceRequest.iOS
{
	public class SendMail:ISendMail
	{
		public SendMail()
		{
		}

		public async Task<bool> Send(string message, string Subject)
		{
			try
			{

				MimeMessage emailMessage = new MimeMessage();
				emailMessage.From.Add(new MailboxAddress("Soujanya", "soujanya.G@dsrc.co.in"));
				emailMessage.To.Add(new MailboxAddress("Soujanya", "soujanya.G@dsrc.co.in"));
				emailMessage.To.Add(new MailboxAddress("Lavanya", "priyanka.p@dsrc.co.in"));
				emailMessage.To.Add(new MailboxAddress("Narmathai", "narmathai.r@dsrc.co.in"));
				emailMessage.Subject = Subject + "(iOS App)";
				emailMessage.Body = new TextPart("plain")
				{
					Text = message
				};
				if (Reachability.InternetConnectionStatus() != ReachabilityNetworkStatus.NotReachable)
				{
					using (var client = new SmtpClient())
					{
						client.Connect("220.227.139.123", 2525, false);

						// Note: since we don't have an OAuth2 token, disable
						// the XOAUTH2 authentication mechanism.
						client.AuthenticationMechanisms.Remove("XOAUTH2");

						// Note: only needed if the SMTP server requires authentication
						client.Authenticate("Soujanya.G@dsrc.co.in", "Dsrc0001");

						client.Send(emailMessage);
						client.Disconnect(true);
					}
					return true;
				}
				else
				{
					await Pages.SplitView.DisplayAlert("No Network Connection", "Unable to send Report, Please ensure the device has a network connection.", "OK",null);
					return false;
				}

			}
			catch (Exception ex)
			{
				LogTracking.LogTrace(ex.ToString());
				await Pages.SplitView.DisplayAlert("Exception ", ex.Message, "OK",null);
				return false;
			}

		}
	}
}
