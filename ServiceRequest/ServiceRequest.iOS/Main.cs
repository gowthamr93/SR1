using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Foundation;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.Classes;
using UIKit;

namespace ServiceRequest.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
			AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
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
		static void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			IOException nsex;
			Exception ex;
			ErrorReport report;
			StringBuilder sb;
			//
			nsex = e.ExceptionObject as IOException;
			ex = e.ExceptionObject as Exception;
			if (nsex != null)
				report = new ErrorReport("Unhandled Native Exception", nsex);
			else if (ex != null)
			{
				// Only create the crash report if the exception was not an ExitException.
				//if (ex is ExitException)
				//	return;
				//
				report = new ErrorReport("Unhandled Managed Exception", ex);
			}
			else
				report = new ErrorReport("Unhandled Unrecognised Exception", e.ToString());
			//
			// Make a record of the state of the data when the error occurred.
			sb = new StringBuilder();
			sb.AppendLine("Requests Data:");
			sb.AppendLine(AppData.PropertyModel.Cache.ToString());
			sb.AppendLine();
			report.AddFurtherInfo(sb.ToString());
			//
			//writingfile(report);
			//var write = await FileSystem.WriteAsync("", "error.txt");
			var fs = new FileSystem();
			var write = fs.Write(report.Serialised, "error.txt");
		}
    }
}
