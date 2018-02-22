using ServiceRequest.AppContext;
using System;
using System.IO;
using System.Text;

namespace ServiceRequest.Classes
{
    /// <summary>  It Contains all the Error Reports from the application.
    /// </summary>
    /// ------------------------------------------------------------------------------------------------
    /// 
    public class ErrorReport
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ErrorReport
        /// 
        /// <summary>	Creates a new instance of the ErrorReport class instantiated from an 
        /// 			unmanaged native exception.
        /// </summary>
        /// <param name="title">		The title for the report.</param>
        /// <param name="ex">			The exception details.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public ErrorReport(string title, IOException ex)
        {
            try
            {
                StringBuilder sb;
                //
                sb = new StringBuilder();
                sb.AppendLine(title);
                sb.AppendLine();
                sb.AppendLine("Exception:");
                sb.AppendLine(ex.GetType().ToString());
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("Stack Trace:");
                if (ex.StackTrace != null)
                    foreach (char s in ex.StackTrace)
                        sb.AppendLine(s.ToString());

                Serialised = sb.ToString();
            }
            catch (Exception e)
            {
                LogTracking.LogTrace(e.ToString());
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ErrorReport
        /// 
        /// <summary>	Creates a new instance of the ErrorReport class instantiated from a 
        /// 			managed exception.
        /// </summary>
        /// <param name="title">		The title for the report.</param>
        /// <param name="ex">			The exception details.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public ErrorReport(string title, Exception ex)
        {
            try
            {
                StringBuilder sb;
                //
                sb = new StringBuilder();
                sb.AppendLine(title);
                sb.AppendLine();
                sb.AppendLine("Exception");
                sb.AppendLine(ex.GetType().ToString());
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("Stack Trace:");
                sb.AppendLine(ex.StackTrace);
                //
                Serialised = sb.ToString();
            }
            catch (Exception e)
            {
                LogTracking.LogTrace(e.ToString());

            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ErrorReport
        /// 
        /// <summary>	Creates a new instance of the ErrorReport class instantiated from an
        /// 			unrecognised exception.
        /// </summary>
        /// <param name="title">		The title for the report.</param>
        /// <param name="contents">		The string contents available for identifying the error.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public ErrorReport(string title, string contents)
        {
            try
            {
                StringBuilder sb;
                //
                sb = new StringBuilder();
                sb.AppendLine(title);
                sb.AppendLine();
                sb.AppendLine("Exception:");
                sb.AppendLine(contents);
                //
                Serialised = sb.ToString();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------

        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void AddFurtherInfo(string text)
        {
            try
            {
                StringBuilder sb;
                //
                sb = new StringBuilder(Serialised);
                sb.AppendLine();
                sb.Append(text);
                Serialised = sb.ToString();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());

            }
        }
        /// 
        public string Serialised { get; private set; }

        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
