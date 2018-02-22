using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Text;
using Idox.LGDP.Apps.Common.OnSite;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.Pages;
using System.Threading.Tasks;
using Java.IO;
using ServiceRequest.Classes;
using ServiceRequest.ViewModels;
using ServiceRequest.AppContext;
using ServiceRequest.Droid.DependencyClasses;
using Android.Text;
using Android.Views.InputMethods;
using Android.Gms.Security;
using Javax.Net.Ssl;

namespace ServiceRequest.Droid
{
    [Activity(Label = "ServiceRequest", Icon = "@drawable/icon", MainLauncher = true,
         ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Landscape,
        WindowSoftInputMode = Android.Views.SoftInput.StateHidden | Android.Views.SoftInput.AdjustPan)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        public static Context context;
        private EditText input;
        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                base.OnCreate(bundle);
                InputDialog.DisplayDialog += OnDisplayDialog;
                AndroidEnvironment.UnhandledExceptionRaiser += HandleUnhandledException;

                global::Xamarin.Forms.Forms.Init(this, bundle);

                context = ApplicationContext; // or activity.getApplicationContext()
                PackageManager packageManager = context.PackageManager;
                string packageName = context.PackageName;
                AppData.Version = new OnSiteVersion(string.Format("{0}",packageManager.GetPackageInfo(packageName, 0).VersionName));

                LoadApplication(new App());

                //AppContext.AppContext.ShowInput = () =>
                //{
                //    InputMethodManager showinput = (InputMethodManager)GetSystemService(InputMethodService);
                //    showinput.ToggleSoftInput(ShowFlags.Forced, 0);
                //};


                if (Build.VERSION.SdkInt <= BuildVersionCodes.Kitkat)
                {
                    ProviderInstaller.InstallIfNeeded(ApplicationContext);
                }
                SSLContext sslContext = SSLContext.GetInstance("TLSv1.2");
                sslContext.Init(null, null, null);
                SSLEngine engine = sslContext.CreateSSLEngine();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        public override async void OnBackPressed()
        {
            if (await SplitView.DisplayAlert("Exit", "Do you want to exit Service Request?", "YES", "NO"))
            {
                    AppData.MainModel.CurrentUser.LoginAction = LoginActions.Existing;
                    var pid = global::Android.OS.Process.MyPid();
                    global::Android.OS.Process.KillProcess(pid);
                    base.OnBackPressed();
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		HandleUnhandledException
        /// 
        /// <summary>	Handles the unhandled exception event and caches an event report which is read
        /// 			when the app next launches.
        /// </summary>
        /// <param name="sender">		Event source.</param>
        /// <param name="e">			Exception arguments.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void HandleUnhandledException(object sender, RaiseThrowableEventArgs e)
        {
            IOException ioex;
            Exception ex;
            Task task;
            ErrorReport report;
            StringBuilder sb;
            try
            {
                ioex = e.Exception as IOException;
                ex = e.Exception as Exception;
                if (ioex != null)
                    report = new ErrorReport("Unhandled Native Exception", ioex);
                else if (ex != null)
                    report = new ErrorReport("Unhandled Managed Exception", ex);
                else
                    report = new ErrorReport("Unhandled Unrecognised Exception", e.ToString());
                // Make a record of the state of the data when the error occurred.
                sb = new StringBuilder();
                // sb.AppendLine("Premises Data:");
                //  sb.AppendLine(AppData.PremisesModel.Cache);
                //   sb.AppendLine();
                report.AddFurtherInfo(sb.ToString());
                task = new Task(async delegate { await writingfile(report); });
                task.Start();
                task.Wait(1000);
            }
            catch (Exception WriteError)
            {
                report = new ErrorReport("Error File Creation Exception", WriteError.ToString());
                task = new Task(async delegate { await writingfile(report); });
                task.Start();
                task.Wait(1000);
                LogTracking.LogTrace(WriteError.ToString());
            }
        }

        /// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		writingfile
		/// 
		/// <summary>	To write the error data in to file.
		/// </summary>
		/// <param name="report">		ErrorReport.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
        public static async Task writingfile(ErrorReport report)
        {
            try
            {
                // LogTracking.EntryToLogFile();
                var write = await ViewModels.FileSystem.Write(report.Serialised, "error.txt");
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        private Task OnDisplayDialog()
        {
            input = new EditText(this) { Text = InputDialog.InputText };

            // Specify the type of input expected; this, for example, sets the input as a password, and will mask the text
            input.SetRawInputType(InputTypes.ClassText);
            var tcs = new TaskCompletionSource<bool>();

            // Set up the buttons
            new AlertDialog.Builder(this)
           .SetTitle("Input")
           .SetMessage("Enter the value")
           .SetView(input)
           .SetNegativeButton("Cancel", delegate { return; })
           .SetPositiveButton("OK", delegate
           {
               InputDialog.InputText = input.Text;
               tcs.SetResult(true);
           })
           .Show();
            return tcs.Task;
        }
    }
}

