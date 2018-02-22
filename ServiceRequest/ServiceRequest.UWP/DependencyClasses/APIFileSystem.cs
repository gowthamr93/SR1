using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Idox.LGDP.Apps.Common.OnSite;
using Idox.LGDP.Apps.ServiceRequest.Client;
using ServiceRequest.AppContext;
using ServiceRequest.UWP.DependencyClasses;
using ServiceRequest.ViewModels;

[assembly: Xamarin.Forms.Dependency(typeof(APIFileSystem))]
namespace ServiceRequest.UWP.DependencyClasses
{
  public class APIFileSystem : IAPI
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables
        /// ------------------------------------------------------------------------------------------------
        /// 
        private Exception m_oJsonError;
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetConfigs
        /// 
        /// <summary>	Gets the configuration data for the organisations supplied.
        /// </summary>
        /// <param name="organisations">		The organisations that require configuration data.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public async Task<bool> GetConfigs(List<string> organisations)
        {
            List<OnSiteConfig> configs;
            //
            configs = new List<OnSiteConfig>();
            foreach (string org in organisations)
                configs.AddRange(await GetConfig(org));
            //
            AppData.ConfigModel.Add(configs);
            return true;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetDocumentMeta
        /// 
        /// <summary>	Gets the document meta data for a given property.
        /// </summary>
        /// <param name="docMetaParams">		The document meta params.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public async Task<bool> GetDocumentMeta(List<OnSiteDocumentMetaParam> docMetaParams)
        {
            // TODO
            await Task.Delay(100);
            return true;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Sync
        /// 
        /// <summary>	The main Sync process where all changes are uploaded and if the user is enabled
        /// 			for Enterprise then a search for all their assigned work items is made. Changes to
        /// 			the relevant view models are done as part of this method.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public async Task<bool> Sync()
        {

           AppData.MainModel.CurrentUser = new SRiUser()
            {
                HasEnterprise = true,
                IdoxId = "demo@idoxgroup.com",
                IsValidated = true,
                LoginAction = LoginActions.New,
                Pin = "",
                Environment = OnSiteEnvironments.Sales,
            };

            bool result;
            //
            // TODO : Upload Documents
            // TODO : Upload Records
            //
           result = await GetConfigs(new List<string>() { "" });
            result = await EnterpriseCall();
            //
            return result;
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Search
        /// 
        /// <summary>	Carries out a search on SRRECs using the search term.
        /// </summary>
        /// <param name="searchTerm">		The search term.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public async Task<bool> Search(string searchTerm)
        {
            List<SRiProperty> properties;
            List<SRiSearchResult> results;
            SRiCPInfoData cpinfoData;
            SRiLICaseData licaseData;
            SRiCNApplPropData historyData;
            SRiPRNotePadData notesData;
            SRiRecordData recordData;
            SRiVisitData visitData;
            //
            if (AppData.Environment != OnSiteEnvironments.Sales)
            {
                recordData = await GetRecords(searchTerm);
                if (!HandleJsonError() || recordData == null) return false;
                //
                // If the initial search didn't return anything, show a message and quit.
                if (recordData.Records.Count == 0)
                {
                    // DisplayAlert.Build("", "No results found").Show();

                    // await SplitView.Instace().DisplayAlert("No results found");
                    return false;
                }
                //
                visitData = await GetVisits(recordData.VisitKeyVals);
                if (!HandleJsonError() || visitData == null) return false;
                recordData.AddVisits(visitData.Visits);
                //
                // Download all the extra data required
                cpinfoData = await GetCPInfos(recordData.CPKeyVals);
                if (!HandleJsonError() || cpinfoData == null) return false;
                //
                licaseData = await GetLICases(recordData.LIKeyVals);
                if (!HandleJsonError() || licaseData == null) return false;
                //
                historyData = await GetPropertyHistory(recordData.HistoryKeyVals);
                if (!HandleJsonError() || historyData == null) return false;
                //
                notesData = await GetPropertyNotes(recordData.NotePadKeyVals);
                if (!HandleJsonError() || notesData == null) return false;
                //
                properties = recordData.CreateProperties(cpinfoData, licaseData, historyData, notesData);
                if (properties == null) return false;
                //
                results = new List<SRiSearchResult>();
                foreach (var p in properties)
                    results.Add(new SRiSearchResult(p));
                //
                AppData.PropertyModel.SearchResults = results;
               
            }
            //
            return true;
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Private Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		EnterpriseCall
        /// 
        /// <summary>	Downloads work items associated to the user.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async Task<bool> EnterpriseCall()
        {
            SRiVisitData visitData;
            SRiRecordData recordData;
            SRiCPInfoData cpinfoData;
            SRiLICaseData licaseData;
            SRiCNApplPropData historyData;
            SRiPRNotePadData notesData;
            List<SRiProperty> properties;
            //
            if (AppData.MainModel.CurrentUser.HasEnterprise ||
                AppData.Environment == OnSiteEnvironments.Sales)
            {
                // Download the visits associated to the current user.
                visitData = await GetVisits(AppData.MainModel.CurrentUser.IdoxId.ToUrlEncoded());
                if (!HandleJsonError() || visitData == null) return false;
                //
                // Download the SRRECs associated to the visits.
                recordData = await GetRecords(visitData.RecordKeyVals);
                if (!HandleJsonError() || recordData == null) return false;
                //
                // Re-download the visits so we have all visits associated to the SRRECs.
                visitData = await GetVisits(recordData.VisitKeyVals);
                if (!HandleJsonError() || visitData == null) return false;
                //
                // Download all the extra data required
                cpinfoData = await GetCPInfos(recordData.CPKeyVals);
                if (!HandleJsonError() || cpinfoData == null) return false;
                //
                licaseData = await GetLICases(recordData.LIKeyVals);
                if (!HandleJsonError() || licaseData == null) return false;
                //
                historyData = await GetPropertyHistory(recordData.HistoryKeyVals);
               // if (!HandleJsonError() || historyData == null) return false;
                //
                notesData = await GetPropertyNotes(recordData.NotePadKeyVals);
                if (!HandleJsonError() || notesData == null) return false;
                //
                // Populate the record data with the visits and then use it to create the property collection.
                recordData.AddVisits(visitData.Visits);
                properties = recordData.CreateProperties(cpinfoData, licaseData, historyData, notesData);
                if (properties == null) return false;
                //
                AppData.PropertyModel.Update(properties);
            }
            //
            //
            return true;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetConfig
        /// 
        /// <summary>	Gets the config data for the organisation.
        /// </summary>
        /// <param name="organisation">		The organisation.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async Task<List<OnSiteConfig>> GetConfig(string organisation)
        {
            ServiceClient client;
            RequestResponseEventArgs response;
            List<OnSiteConfig> configs;
            Exception error;
            FileSystemArgs fileResponse;
            m_oJsonError = null;
            configs = new List<OnSiteConfig>();
            if (AppData.Environment == OnSiteEnvironments.Sales || true)
            {
                // fileResponse = await ServiceRequest.ViewModels.FileSystem.ReadText("sr_config_srrec.txt");

                configs.Add(OnSiteConfig.FromJson(Readfile("ServiceRequest.sr_config_srrec.txt"), out error));
                if (error != null) m_oJsonError = error;
                configs.Add(OnSiteConfig.FromJson(Readfile("ServiceRequest.sr_config_xivisit.txt"), out error));
                if (error != null) m_oJsonError = error;
                configs.Add(OnSiteConfig.FromJson(Readfile("ServiceRequest.sr_config_cpinfo.txt"), out error));
                if (error != null) m_oJsonError = error;
                configs.Add(OnSiteConfig.FromJson(Readfile("ServiceRequest.sr_config_licase.txt"), out error));
                if (error != null) m_oJsonError = error;
            }
            else
            {
                client = await GetServiceClient();
                response = await client.DownloadStringAsync(GetUrl(string.Format("configuration?hash={0}&organisation={1}",
                                                                                 AppData.ConfigModel.Hash(organisation),
                                                                                 organisation)));
                if (HandleError(response))
                    configs.Add(OnSiteConfig.FromJson(response.Text, out m_oJsonError));
            }
            //
            return configs;
        }

        /////ReadFIle
        private string Readfile(string filename)
        {
            
                var currentAssembly = typeof(APIFileSystem).GetTypeInfo().Assembly;
                string text;
                using (var reader = new StreamReader(currentAssembly.GetManifestResourceStream(filename)))
                {
                    text = reader.ReadToEnd();
                }
                return text;
            
        }




        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetUrl
        /// 
        /// <summary>	A single point of maintinance for the base URL
        /// </summary>
        /// <param name="endPath">		The URL suffix</param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private string GetUrl(string endPath)
        {
            if (AppData.Environment == OnSiteEnvironments.NoAuth)
                return AppData.ServiceHost + "/sri/" + endPath;
            else
                return AppData.ServiceHost + "/agw/sri/" + endPath;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetServiceClient
        /// 
        /// <summary>	Returns a new service client with the authorisation header set to the current 
        /// 			user's token. If the API version has not yet been verified a call to check the 
        /// 			version matches the version held in the app is made.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async Task<ServiceClient> GetServiceClient()
        {
            ServiceClient client;
            //RequestResponseEventArgs response;
            //CPiAPIVersion version;
            //Exception error;
            bool result;
            //bool done;
            //
            client = new ServiceClient();
            client.AddAuthHeader(AppData.MainModel.CurrentUser.Token);
            //
            // Make sure the API version is verified. This is not needed for the Sales environment.
            if (AppData.Environment == OnSiteEnvironments.Sales)
                result = true;
            else
                result = AppData.APIVersionVerified;
            //
            if (!result)
            {
                // TODO : Check that the API version matches what the app expects it to be.
                await Task.Delay(100);
                result = true;

                //done = false;
                //error = null;
                //response = await client.DownloadStringAsync(GetUrl("configuration/version"));
                //if (response.Error == null && response.Text.Length > 0)
                //{
                //	version = CPiAPIVersion.FromJson(response.Text, out error);
                //	if (version != null)
                //	{
                //		done = true;
                //		if (version.Version != AppData.APIVersion)
                //			AlertView.Build("App Requires Updating",
                //			                "The app is out of sync with the API and requires updating before use.",
                //			                new AlertView.Button("Exit App", () =>
                //		{
                //			throw new ExitException();
                //		})).Show();
                //		else
                //			result = true;
                //	}
                //}
                ////
                //if (response.Error != null && response.AuthenticationError)
                //	HubView.HandleError(response);
                //else if (!done)
                //	AlertView.Build("API Version Error",
                //	                string.Format("It was not possible to verify the version of the API for the [{0}] environment.\r\n{1}", 
                //	                              AppData.Environment.GetDescription(),
                //	                              response.Error != null ? response.Error.Message : error != null ? error.Message : "The response was null.")).Show();
            }
            //
            if (result)
                return client;
            else
                return null;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		HandleError
        /// 
        /// <summary>	Checks the RequestResponseEventArgs for any error following a request completing.
        /// 			The approriate message is displayed for any error found, the method returns TRUE
        /// 			if there was no error.
        /// </summary>
        /// <param name="response">		The request response object.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private bool HandleError(RequestResponseEventArgs response)
        {
            if (response.Error == null)
                return true;
            //
            //if (Reachability.InternetConnectionStatus() == ReachabilityNetworkStatus.NotReachable)
            //    AlertView.Build("Network Error",
            //                    "It was not possible to complete the action due to a loss of network access. Please connect the device to a network and try again.").Show();
            //else if (response.AuthenticationError)
            //    AlertView.Build("Authentication Required",
            //                    "Your session has expired and you are required to re-authenticate before completing any further actions.\r\n\nWould you like to start a new session now?",
            //                    "No", new AlertView.Button("Yes", () =>
            //                    {
            //                        AppData.MainModel.CurrentUser = new SRiUser()
            //                        {
            //                            Environment = AppData.MainModel.CurrentUser.Environment,
            //                            IdoxId = AppData.MainModel.CurrentUser.IdoxId
            //                        };
            //                    })).Show();
            //else
            //    AlertView.Build("Request Error",
            //                    string.IsNullOrEmpty(response.Text) ? response.Error.Message : response.Text).Show();
            ////
            return false;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		HandleJsonError
        /// 
        /// <summary>	Checks if the Json error is null, if it isn't an error message is displayed.
        /// </summary>
        /// <returns>	TRUE if the error is null.
        /// </returns>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private bool HandleJsonError()
        {
            if (m_oJsonError == null)
                return true;
            //
            //AlertView.Build("Json Error", m_oJsonError.Message).Show();
            return false;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ReadFile
        /// 
        /// <summary>	Reads a text file from the app resource folder.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private static string ReadFile(string fileName)
        {
            return null;
            // return NSString.FromData(NSData.FromFile(fileName), NSStringEncoding.UTF8).ToString();
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region API Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetVisits
        /// 
        /// <summary>	Downloads XIVISIT data from the API.
        /// </summary>
        /// <param name="idoxId">		The IdoxID of the user.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async Task<SRiVisitData> GetVisits(string idoxId)
        {
            SRiVisitData data;
            ServiceClient client;
            RequestResponseEventArgs response;
            //
            m_oJsonError = null;
            if (AppData.Environment == OnSiteEnvironments.Sales || true)
                data = SRiVisitData.FromJson(Readfile("ServiceRequest.sr_xivisit.txt"), out m_oJsonError);
            else
            {
                client = await GetServiceClient();
                response = await client.DownloadStringAsync(GetUrl("xivisit?idoxid=" + idoxId));
                if (HandleError(response))
                    data = SRiVisitData.FromJson(response.Text, out m_oJsonError);
                else
                    data = null;
            }
            //
            return data;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetVisits
        /// 
        /// <summary>	Downloads XIVISIT data from the API.
        /// </summary>
        /// <param name="keyVals">		The KeyVals of the records to retreive.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async Task<SRiVisitData> GetVisits(string[] keyVals)
        {
            SRiVisitData data;
            ServiceClient client;
            RequestResponseEventArgs response;
            //
            m_oJsonError = null;
            if (AppData.Environment == OnSiteEnvironments.Sales || true)
                data = SRiVisitData.FromJson(Readfile("ServiceRequest.sr_xivisit.txt"), out m_oJsonError);
            else
            {
                client = await GetServiceClient();
                response = await client.DownloadStringAsync(GetUrl("xivisit?fieldname=keyvalkeyvals=" + string.Join("/", keyVals)));
                if (HandleError(response))
                    data = SRiVisitData.FromJson(response.Text, out m_oJsonError);
                else
                    data = null;
            }
            //
            return data;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetRecords
        /// 
        /// <summary>	Downloads SRREC data from the API using the supplied search term.
        /// </summary>
        /// <param name="searchTerm">		The search term for the query.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async Task<SRiRecordData> GetRecords(string searchTerm)
        {
            ServiceClient client;
            RequestResponseEventArgs response;
            SRiRecordData data;
            //
            m_oJsonError = null;
            if (AppData.Environment == OnSiteEnvironments.Sales || true)
                data = SRiRecordData.FromJson(Readfile("ServiceRequest.sr_srrec.txt"), out m_oJsonError);
            else
            {
                client = await GetServiceClient();
                response = await client.DownloadStringAsync(GetUrl("srrec?ref=" + searchTerm.ToBase64Url()));
                if (HandleError(response))
                    data = SRiRecordData.FromJson(response.Text, out m_oJsonError);
                else
                    data = null;
            }
            //
            return data;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetRecords
        /// 
        /// <summary>	Downloads SRREC data from the API using the supplied set of KeyVals.
        /// </summary>
        /// <param name="keyVals">		The KeyVals of the records to retreive.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async Task<SRiRecordData> GetRecords(string[] keyVals)
        {
            ServiceClient client;
            RequestResponseEventArgs response;
            SRiRecordData data;
            //
            m_oJsonError = null;
            if (AppData.Environment == OnSiteEnvironments.Sales || true)
                data = SRiRecordData.FromJson(Readfile("ServiceRequest.sr_srrec.txt"), out m_oJsonError);
            else
            {
                client = await GetServiceClient();
                response = await client.DownloadStringAsync(GetUrl("srrec?keyvals=" + string.Join("/", keyVals)));
                if (HandleError(response))
                    data = SRiRecordData.FromJson(response.Text, out m_oJsonError);
                else
                    data = null;
            }
            //
            return data;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetCPInfos
        /// 
        /// <summary>	Downloads CPINFO data from the API using the supplied set of KeyVals.
        /// </summary>
        /// <param name="keyVals">		The KeyVals of the records to retreive.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async Task<SRiCPInfoData> GetCPInfos(string[] keyVals)
        {
            ServiceClient client;
            RequestResponseEventArgs response;
            SRiCPInfoData data;
            //
            m_oJsonError = null;
            if (AppData.Environment == OnSiteEnvironments.Sales || true)
                data = SRiCPInfoData.FromJson(Readfile("ServiceRequest.sr_cpinfo.txt"), out m_oJsonError);
            else
            {
                client = await GetServiceClient();
                response = await client.DownloadStringAsync(GetUrl("cpinfo?keyvals=" + string.Join("/", keyVals)));
                if (HandleError(response))
                    data = SRiCPInfoData.FromJson(response.Text, out m_oJsonError);
                else
                    data = null;
            }
            //
            return data;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetLICases
        /// 
        /// <summary>	Downloads LICASE data from the API using the supplied set of KeyVals.
        /// </summary>
        /// <param name="keyVals">		The KeyVals of the records to retreive.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async Task<SRiLICaseData> GetLICases(string[] keyVals)
        {
            ServiceClient client;
            RequestResponseEventArgs response;
            SRiLICaseData data;
            //
            m_oJsonError = null;
            if (AppData.Environment == OnSiteEnvironments.Sales || true)
                data = SRiLICaseData.FromJson(Readfile("ServiceRequest.sr_licase.txt"), out m_oJsonError);
            else
            {
                client = await GetServiceClient();
                response = await client.DownloadStringAsync(GetUrl("licase?keyvals=" + string.Join("/", keyVals)));
                if (HandleError(response))
                    data = SRiLICaseData.FromJson(response.Text, out m_oJsonError);
                else
                    data = null;
            }
            //
            return data;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetPropertyNotes
        /// 
        /// <summary>	Downloads PRNOTEPAD data from the API using the supplied set of KeyVals.
        /// </summary>
        /// <param name="keyVals">		The KeyVals of the records to retreive.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async Task<SRiPRNotePadData> GetPropertyNotes(string[] keyVals)
        {
            ServiceClient client;
            RequestResponseEventArgs response;
            SRiPRNotePadData data;
            //
            m_oJsonError = null;
            if (AppData.Environment == OnSiteEnvironments.Sales || true)
                data = SRiPRNotePadData.FromJson(Readfile("ServiceRequest.sr_prnotepad.txt"), out m_oJsonError);
            else
            {
                client = await GetServiceClient();
                response = await client.DownloadStringAsync(GetUrl("prnotepad?keyvals=" + string.Join("/", keyVals)));
                if (HandleError(response))
                    data = SRiPRNotePadData.FromJson(response.Text, out m_oJsonError);
                else
                    data = null;
            }
            //
            return data;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetPropertyHistory
        /// 
        /// <summary>	Downloads CNAPPLPROP data from the API using the supplied set of KeyVals.
        /// </summary>
        /// <param name="keyVals">		The KeyVals of the records to retreive.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async Task<SRiCNApplPropData> GetPropertyHistory(string[] keyVals)
        {
            ServiceClient client;
            RequestResponseEventArgs response;
            SRiCNApplPropData data;
            //
            m_oJsonError = null;
            if (AppData.Environment == OnSiteEnvironments.Sales || true)
                data = SRiCNApplPropData.FromJson(Readfile("ServiceRequest.sr_cnapplprop.txt"), out m_oJsonError);
            else
            {
                client = await GetServiceClient();
                response = await client.DownloadStringAsync(GetUrl("cnapplprop?keyvals=" + string.Join("/", keyVals)));
                if (HandleError(response))
                    data = SRiCNApplPropData.FromJson(response.Text, out m_oJsonError);
                else
                    data = null;
            }
            //
            return data;
        }
        /// 
        #endregion-------------------------------------------------------------------------------



    }
}
