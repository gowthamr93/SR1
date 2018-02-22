using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace ServiceRequest.AppContext
{
    public class RequestResponseEventArgs
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public Constructors
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		RequestResponseEventArgs
        /// 
        /// <summary>	Creates a new instance of the RequestResponseEventArgs class.
        /// </summary>
        /// <param name="responseString"> The valid response string.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public RequestResponseEventArgs(string responseString)
        {
            try
            {
                Text = responseString;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		RequestResponseEventArgs
        /// 
        /// <summary>	Creates a new instance of the RequestResponseEventArgs class.
        /// </summary>
        /// <param name="data">					The valid response data.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public RequestResponseEventArgs(byte[] data)
        {
            try
            {
                Data = data;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		RequestResponseEventArgs
        /// 
        /// <summary>	Creates a new instance of the RequestResponseEventArgs class.
        /// </summary>
        /// <param name="ex">					The error exception.</param>
        /// <param name="url">					The Url that caused the error.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public RequestResponseEventArgs(Exception ex, string url)
        {
            try
            {
                AuthenticationError = false;
                Error = ex;
                Url = url;
                Body = "";
                RequestType = "GET";
            }
            catch (Exception e)
            {
                LogTracking.LogTrace(e.ToString());
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		RequestResponseEventArgs
        /// 
        /// <summary>	Creates a new instance of the RequestResponseEventArgs class.
        /// </summary>
        /// <param name="ex">				The web error exception.</param>
        /// <param name="url">				The Url that caused the error.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public RequestResponseEventArgs(HttpRequestException ex, string url)
        {
            try
            {
                AuthenticationError = false;
                Error = ex;
                if (ex.Message != null)
                {
                    if (ServiceClient.StatCode == HttpStatusCode.Unauthorized)
                    {
                        AuthenticationError = true;
                    }
                    Text = ex.Message;
                }
                Url = url;
                Body = "";
                RequestType = "GET";
            }
            catch (Exception e)
            {
                LogTracking.LogTrace(e.ToString());
            }
        }

        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        public string Details
        {
            get
            {
                StringBuilder sb;
                //
                sb = new StringBuilder();
                sb.AppendLine("Network Error");
                sb.AppendLine();
                sb.AppendLine("Exception:");
                sb.AppendLine(Error.GetType().ToString());
                sb.AppendLine(Error.Message);
                sb.AppendLine();
                if (!string.IsNullOrEmpty(Url))
                {
                    sb.AppendLine("Request Details:");
                    sb.AppendLine(string.Format("Url: {0}", Url));
                    sb.AppendLine(string.Format("Type: {0}", RequestType));
                }
                if (!string.IsNullOrEmpty(Body))
                {
                    sb.AppendLine("Content:");
                    sb.AppendLine(Body);
                    sb.AppendLine();
                }
                if (!string.IsNullOrEmpty(Text))
                {
                    sb.AppendLine("Server Response:");
                    sb.AppendLine(Text);
                    sb.AppendLine();
                }
                sb.AppendLine("Stack Trace:");
                sb.AppendLine(Error.StackTrace);
                return sb.ToString();
            }
        }
        /// 
        public string Text { get; set; }
        /// 
        public byte[] Data { get; set; }
        /// 
        public string Body { get; set; }
        /// 
        public string Url { get; set; }
        /// 
        public Exception Error { get; set; }
        /// 
        public bool AuthenticationError { get; private set; }
        /// 
        public string RequestType { get; set; }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Private Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        private Encoding GetEncoding(string characterSet)
        {
            Encoding encoding;
            try
            {
                encoding = Encoding.GetEncoding(characterSet);
            }
            catch
            {
                encoding = Encoding.UTF8;
            }
            //
            return encoding;
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
