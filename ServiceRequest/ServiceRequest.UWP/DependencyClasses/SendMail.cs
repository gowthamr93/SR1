using LightBuzz.SMTP;
using ServiceRequest.AppContext;
using ServiceRequest.DependencyInterfaces;
using ServiceRequest.UWP.DependencyClasses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Email;
using Idox.LGDP.Apps.ServiceRequest.Client;

[assembly: Xamarin.Forms.Dependency(typeof(SendMail))]
namespace ServiceRequest.UWP.DependencyClasses
{
    public class SendMail : ISendMail
    {

        public async Task<bool> Send(string message, string Subject)
        {
            try
            {
                EmailMessage emailMessage = new EmailMessage();
                emailMessage.Sender = new EmailRecipient("Soujanya.G@dsrc.co.in", "Soujanya");
                emailMessage.To.Add(new EmailRecipient(AppData.IdoxReportMailId, "IDOX Onsite Report"));
                emailMessage.Subject = Subject + "(Windows app)";
                emailMessage.Body = message;
                if (Reachability.InternetConnectionStatus() != ReachabilityNetworkStatus.NotReachable)
                {
                    SmtpClient client = new SmtpClient("220.227.139.123", 2525, false, "Soujanya.G@dsrc.co.in", "Dsrc0001");
                    await client.SendMail(emailMessage);
                    return true;
                }
                else
                {
                    await Pages.SplitView.DisplayAlert("No Network Connection", "Unable to send Report, Please ensure the device has a network connection.", "OK", null);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                await Pages.SplitView.DisplayAlert("Exception ", ex.Message.ToString(), "OK", null);
                return false;
            }
        }
    }
}
