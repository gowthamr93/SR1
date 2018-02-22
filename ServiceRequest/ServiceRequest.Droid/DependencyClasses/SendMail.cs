using System;
using ServiceRequest.DependencyInterfaces;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using ServiceRequest.Droid.DependencyClasses;
using ServiceRequest.AppContext;

[assembly: Xamarin.Forms.Dependency(typeof(SendMail))]
namespace ServiceRequest.Droid.DependencyClasses
{
    class SendMail : ISendMail
    {
        public async Task<bool> Send(string message, string Subject)
        {
            try
            {

                MimeMessage emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Soujanya", "soujanya.G@dsrc.co.in"));
				emailMessage.To.Add(new MailboxAddress("Soujanya", "soujanya.G@dsrc.co.in"));
                emailMessage.To.Add(new MailboxAddress("Lavanya", "priyanka.p@dsrc.co.in"));
                emailMessage.To.Add(new MailboxAddress("Partha", "parthasarathi.u@dsrc.co.in"));
				emailMessage.To.Add(new MailboxAddress("Boobalan", "Boobalan.k@dsrc.co.in"));
                emailMessage.Subject = Subject + "(Android)";
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
                    await Pages.SplitView.Instace().DisplayAlert("No Network Connection", "Unable to send Report, Please ensure the device has a network connection.", "OK");
                    return false;
                }

            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                await Pages.SplitView.Instace().DisplayAlert("Exception ", ex.Message, "OK");
                return false;
            }

        }
    }
}