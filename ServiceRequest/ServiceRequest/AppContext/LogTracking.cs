using ServiceRequest.ViewModels;
using System;
using System.Diagnostics;
using System.Text;

namespace ServiceRequest.AppContext
{
    ///
    /// ------------------------------------------------------------------------------------------------
    /// <summary>  It is used to track the log for exceptions.
    /// </summary>
    /// ------------------------------------------------------------------------------------------------
    /// 
    public static class LogTracking
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static void LogTrace(String logText)
        {
            try
            {
                if (AppContext.LogDetails == null)
                    AppContext.LogDetails = new StringBuilder();

                AppContext.LogDetails.AppendLine();
                AppContext.LogDetails.AppendLine(logText);
                Debug.WriteLine(logText);
            }
            catch (Exception)
            {
                // LogTrace(ex.ToString());
            }
        }

        public static void EntryToLogFile()
        {
            try
            {
                if (AppContext.LogDetails != null)
                {
                    var result = FileSystem.AppendText(AppContext.LogDetails.ToString(), "log.txt");
                }
            }
            catch (Exception ex)
            {
                LogTrace(ex.ToString());
            }
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------

    }
}
