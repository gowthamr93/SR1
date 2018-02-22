using System;
using System.Threading.Tasks;
using Android.Text;
using ServiceRequest.DependencyInterfaces;
using ServiceRequest.Droid.DependencyClasses;
using ServiceRequest.AppContext;

[assembly: Xamarin.Forms.Dependency(typeof(InputDialog))]

namespace ServiceRequest.Droid.DependencyClasses
{
    public class InputDialog : IInputDialog
    {

        public static Func<Task> DisplayDialog;
        public static string InputText;

        public InputDialog()
        {
            try
            {
                InputText = "";
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        public async Task<string> ShowDialog(string inputText)
        {
            try
            {
                InputText = inputText;

                var invoke = DisplayDialog?.Invoke();
                if (invoke != null) await invoke;

                return InputText;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
    }
}