using ModernHttpClient;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

using System.Text;
using System.Threading.Tasks;

namespace ServiceRequest.AppContext
{
    public class ServiceClient
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables
        /// ------------------------------------------------------------------------------------------------
        /// 

        private Dictionary<string, string> m_oHeaders;
        private Task task;

        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Variables
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static HttpStatusCode StatCode;
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        /// ------------------------------------------------------------------------------------------------
        ///

        /// ------------------------------------------------------------------------------------------------
        /// Name		ServieClient
        /// 
        /// <summary>	Creates a new instance of the ServiceClient class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public ServiceClient()
        {
            try
            {
                m_oHeaders = new Dictionary<string, string>();
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 

        /// ------------------------------------------------------------------------------------------------
        /// Name		AddHeader
        /// 
        /// <summary>	Adds a header value to the ServiceClient class to be used on future requests.
        /// </summary>
        /// <param name="name">			The name of the header.</param>
        /// <param name="value">		The value for the header.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void AddHeader(string name, string value)
        {
            try
            {
                if (m_oHeaders.ContainsKey(name))
                    m_oHeaders[name] = value;
                else
                    m_oHeaders.Add(name, value);
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		AddAuthHeader
		/// 
		/// <summary>	Adds an authorisation header
		/// </summary>
		/// <param name="token">		The authorisation token.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public void AddAuthHeader(string token)
        {
            try
            {
                AddHeader("Authorization", string.Format("Bearer {0}", token));
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		DownloadStringAsync
        /// 
        /// <summary>	Asynchronously downloadss string data from the url.
        /// </summary>
        /// <param name="url">		The url for the request.</param>
        /// <param name="firstTry">	Indicates that this is the first try at the request.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public async Task<RequestResponseEventArgs> DownloadStringAsync(string url, bool firstTry = true)
        {

            HttpClient client;
            RequestResponseEventArgs args;
            HttpResponseMessage result;
            string response;
            //
            try
            {
                client = new HttpClient(new NativeMessageHandler());
                StatCode = HttpStatusCode.InternalServerError;
                foreach (string name in m_oHeaders.Keys)
                    client.DefaultRequestHeaders.Add(name, m_oHeaders[name]);

                result = await client.GetAsync(url);
                if (result.IsSuccessStatusCode)
                {
                    response = await result.Content.ReadAsStringAsync();
                    args = new RequestResponseEventArgs(response);
                }
                else
                {
                    StatCode = result.StatusCode;

                    if (result.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new HttpRequestException();
                    }
                    else
                    {
                        if (result.Content != null)
                            response = await result.Content.ReadAsStringAsync();
                        else
                            response = result.ReasonPhrase;
                        //
                        throw new Exception(response);
                    }
                }

            }
            catch (HttpRequestException ex)
            {
                if (firstTry && ex.Message.Contains("(ReadDone2)"))
                {
                    await Task.Delay(500);
                    return await DownloadDataAsync(url, false);
                }

                args = new RequestResponseEventArgs(ex, url);
                LogTracking.LogTrace(ex.ToString());
            }
            catch (Exception ex)
            {
                args = new RequestResponseEventArgs(ex, url);
                LogTracking.LogTrace(ex.ToString());
            }
            //
            return args;
        }


        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		DownloadDataAsync
        /// 
        /// <summary>	Asynchronously downloads binary data from a url.
        /// </summary>
        /// <param name="url">		The Url for the request.</param>
        /// <param name="firstTry">	Indicates that this is the first try at the request.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public async Task<RequestResponseEventArgs> DownloadDataAsync(string url, bool firstTry = true)
        {
            HttpClient client;
            RequestResponseEventArgs args;
            byte[] response;
            string stringException;
            //
            try
            {
                client = new HttpClient(new NativeMessageHandler());
                StatCode = HttpStatusCode.InternalServerError;
                foreach (string name in m_oHeaders.Keys)
                    client.DefaultRequestHeaders.Add(name, m_oHeaders[name]);
                var result = await client.GetAsync(url);

                if (result.IsSuccessStatusCode)
                {
                    response = await result.Content.ReadAsByteArrayAsync();
                    args = new RequestResponseEventArgs(response);
                }
                else
                {
                    StatCode = result.StatusCode;
                    if (result.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new HttpRequestException();
                    }
                    else
                    {
                        if (result.Content != null)
                            stringException = await result.Content.ReadAsStringAsync();
                        else
                            stringException = result.ReasonPhrase;

                        throw new Exception(stringException);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                if (firstTry && ex.Message.Contains("(ReadDone2)"))
                {
                    await Task.Delay(500);
                    return await DownloadDataAsync(url, false);
                }

                args = new RequestResponseEventArgs(ex, url);
                LogTracking.LogTrace(ex.ToString());
            }
            catch (Exception ex)
            {
                args = new RequestResponseEventArgs(ex, url);
                LogTracking.LogTrace(ex.ToString());
            }
            //
            return args;
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		UploadDataAsync
        /// 
        /// <summary>	Asynchronously uploads data to the url.
        /// </summary>
        /// <param name="url">		The url for the request.</param>
        /// <param name="isPut">	Whether the method is PUT or POST.</param>
        /// <param name="data">		The data content for the request.</param>
        /// <param name="contentType"></param>
        /// <param name="firstTry">	Indicates that this is the first try at the request.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        public async Task<RequestResponseEventArgs> UploadDataAsync(string url,
                                                                       bool isPut,
                                                                       byte[] data,
                                                                       ContentType contentType,
                                                                       bool firstTry = true)
        {
            HttpClient client;
            RequestResponseEventArgs args;
            HttpResponseMessage result;
            byte[] response;
            string stringException;
            try
            {
                client = new HttpClient(new NativeMessageHandler());
                StatCode = HttpStatusCode.InternalServerError;

                foreach (string name in m_oHeaders.Keys)
                    client.DefaultRequestHeaders.Add(name, m_oHeaders[name]);
                ByteArrayContent byteContent = new ByteArrayContent(data);
                byteContent.Headers.Add("Content-Type", GetContentType(contentType));
                result = isPut ? await client.PutAsync(url, byteContent) : await client.PostAsync(url, byteContent);

                if (result.IsSuccessStatusCode)
                {
                    response = await result.Content.ReadAsByteArrayAsync();
                    args = new RequestResponseEventArgs(response);
                }
                else
                {
                    StatCode = result.StatusCode;
                    if (result.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new HttpRequestException();
                    }
                    else
                    {
                        if (result.Content != null)
                            stringException = await result.Content.ReadAsStringAsync();
                        else
                            stringException = result.ReasonPhrase;

                        throw new Exception(stringException);
                    }
                }

                args = new RequestResponseEventArgs(response);
            }
            catch (HttpRequestException ex)
            {

                if (firstTry && ex.Message.Contains("(ReadDone2)"))
                {
                    await Task.Delay(500);
                    return await UploadDataAsync(url, isPut, data, contentType, false);
                }
                args = new RequestResponseEventArgs(ex, url);
                args.Body = string.Format("Data content of length [{0}].", data.Length);
                args.RequestType = isPut ? "PUT" : "POST";
                LogTracking.LogTrace(ex.ToString());
            }
            catch (Exception ex)
            {
                args = new RequestResponseEventArgs(ex, url);
                LogTracking.LogTrace(ex.ToString());
            }

            return args;
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		UploadStringAsync
        /// 
        /// <summary>	Asynchronously uploads string content to the url.
        /// </summary>
        /// <param name="url">		The url for the request.</param>
        /// <param name="isPut">	Where the method is PUT or POST.</param>
        /// <param name="content">	The string content of the request.</param>
        /// <param name="contentType"></param>
        /// <param name="firstTry">	Indicates that this is the first try at the request.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        public async Task<RequestResponseEventArgs> UploadStringAsync(string url,
                                                                         bool isPut,
                                                                         string content,
                                                                         ContentType contentType,
                                                                         bool firstTry = true)
        {
            HttpClient client;
            RequestResponseEventArgs args;
            string response;
            //HttpResponseMessage result;
            //string response;
            //
            try
            {
                client = new HttpClient(new NativeMessageHandler());
                StatCode = HttpStatusCode.InternalServerError;
                foreach (string name in m_oHeaders.Keys)
                    client.DefaultRequestHeaders.Add(name, m_oHeaders[name]);
                //
                StringContent stringContent = new StringContent(content, Encoding.UTF8, GetContentType(contentType));
                HttpResponseMessage result = isPut ? await client.PutAsync(url, stringContent) : await client.PostAsync(url, stringContent);

                if (result.IsSuccessStatusCode)
                {
                    response = await result.Content.ReadAsStringAsync();
                    args = new RequestResponseEventArgs(response);
                }
                else
                {
                    StatCode = result.StatusCode;
                    if (result.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new HttpRequestException();
                    }
                    else
                    {
                        if (result.Content != null)
                            response = await result.Content.ReadAsStringAsync();
                        else
                            response = result.ReasonPhrase;
                        //
                        throw new Exception(response);
                    }
                }

            }
            catch (HttpRequestException ex)
            {
                if (firstTry && ex.Message.Contains("(ReadDone2)"))
                {
                    await Task.Delay(500);
                    return await UploadStringAsync(url, isPut, content, contentType, false);
                }
                args = new RequestResponseEventArgs(ex, url);
                LogTracking.LogTrace(ex.ToString());
            }
            catch (Exception ex)
            {
                args = new RequestResponseEventArgs(ex, url);
                LogTracking.LogTrace(ex.ToString());
            }
            //
            if (args.Error != null)
            {
                args.Body = content;
                args.RequestType = isPut ? "PUT" : "POST";
            }
            //
            return args;
        }

        /// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		GetContentType
		/// 
		/// <summary>	Gets the content type header value based on the ContentType enum.
		/// </summary>
		/// <param name="contentType">		The ContentType enum.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public string GetContentType(ContentType contentType)
        {
            try
            {
                switch (contentType)
                {
                    case ContentType.BinaryData:
                        return "application/octet-stream";
                    case ContentType.Json:
                        return "application/json";
                    case ContentType.Xml:
                        return "application/xml";
                    default:
                        return "text/plain";
                }
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }

        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
