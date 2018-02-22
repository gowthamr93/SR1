using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceRequest.DependencyInterfaces;
using ServiceRequest.iOS.DependencyClasses;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(InputDialog))]
namespace ServiceRequest.iOS.DependencyClasses
{
    public class InputDialog : UIAlertView, IInputDialog
    {
        public static Func<UIViewController> DisplayDialog;
        public static string InputText;
        public InputDialog()
        {
            InputText = "";
        }
        public async Task<string> ShowDialog(string inputText)
        {
            InputText = string.Empty;
            UIAlertView alert = new UIAlertView();
            alert.Title = inputText;
            alert.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
            alert.GetTextField(0).Text = inputText;
            alert.AddButton("Cancel");
            alert.AddButton("OK");
            var tcs = new TaskCompletionSource<nint>();
            alert.Clicked += (s, e) =>
            {
                if (e.ButtonIndex == 0)
                {
                    InputText = string.Empty;
                }
                if (e.ButtonIndex == 1)
                {
                    InputText = alert.GetTextField(0).Text;
                }
                tcs.SetResult(e.ButtonIndex);
            };
            alert.Show();
            await tcs.Task;
            return InputText;
        }
       
    }
}