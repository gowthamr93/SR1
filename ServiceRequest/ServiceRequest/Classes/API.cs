using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Idox.LGDP.Apps.Common.OnSite;
using Idox.LGDP.Apps.ServiceRequest.Client;
using Idox.LGDP.Apps.ServiceRequest.Client.Models;
using Plugin.Geolocator.Abstractions;
using ServiceRequest.AppContext;
using ServiceRequest.Pages;
using ServiceRequest.ViewModels;
using ServiceRequest.Views;
using ServiceRequest.Views.PopUp;
using Xamarin.Forms;

namespace ServiceRequest.Classes
{
    /// <summary>  It contains all the API's Data that is used in the application.
    /// </summary>
    /// ------------------------------------------------------------------------------------------------
    /// 
    public class API : IAPI
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables
        /// ------------------------------------------------------------------------------------------------
        /// 
        private Exception m_oJsonError;
        public static List<SRiResolution> m_Resolution = new List<SRiResolution>();
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------

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
            try
            {
                List<OnSiteConfig> configs;
                //
                configs = new List<OnSiteConfig>();
                foreach (string org in organisations)
                    configs.AddRange(await GetConfig(org));
                //
                AppData.ConfigModel.Add(configs);
                return configs.Count > 0;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return false;
            }
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
            bool result;
            try
            {
                //
                result = await UploadDocuments(AppData.PropertyModel.UploadableDocuments);
                if (result)
                    result = await UploadRecords(AppData.PropertyModel.UploadableRecords);
                if (result)
                    result = await UploadVisits(AppData.PropertyModel.UploadableVisits);
                //
                if (m_Resolution?.Count > 0)
                    result = false;
                else
                {
                    if (result)
                        result = await GetConfigs(AppData.MainModel.CurrentUser.Organisations.Select(o => o.Organisation.Name).ToList());
                    if (result)
                        result = await EnterpriseCall();
                }
                //
                return result;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return false;
            }
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
            try
            {
                if (!await GetConfigs(AppData.MainModel.CurrentUser.Organisations.Select(o => o.Organisation.Name).ToList()))
                    return false;
                //
                results = new List<SRiSearchResult>();
                recordData = await GetRecords(searchTerm);
                // recordData.AddBogusGeoData();
                if (!HandleJsonError() || recordData == null) return false;
                //
                // If the initial search didn't return anything, show a message and quit.
                if (recordData.Records.Count > 0)
                {
                    if (recordData.VisitEntityKeys.Length > 0)
                    {
                        visitData = await GetVisits(recordData.VisitEntityKeys);
                        if (!HandleJsonError() || visitData == null) return false;
                        recordData.AddVisits(visitData.Visits);
                    }
                    //
                    // Download all the extra data required
                    cpinfoData = await GetCPInfos(recordData.CPEntityKeys);
                    if (!HandleJsonError() || cpinfoData == null) return false;
                    //
                    licaseData = await GetLICases(recordData.LIEntityKeys);
                    if (!HandleJsonError() || licaseData == null) return false;
                    //
                    historyData = await GetPropertyHistory(recordData.HistoryEntityKeys);
                    if (!HandleJsonError() || historyData == null) return false;
                    //
                    notesData = await GetPropertyNotes(recordData.NotePadEntityKeys);
                    if (!HandleJsonError() || notesData == null) return false;
                    //
                    properties = recordData.CreateProperties(cpinfoData, licaseData, historyData, notesData);
                    if (properties == null) return false;
                    //
                    foreach (var p in properties)
                        results.Add(new SRiSearchResult(p));
                }
                //
                if (results.Count > 0)
                    AppData.PropertyModel.SearchResults = results;
                else
                {
                    if (Device.OS == TargetPlatform.iOS)
                    {
                        DependencyService.Get<IDisplayAlertPopup>().ShowAlertMessage();
                    }
                    else
                    {
                        await SplitView.Instace().DisplayAlert("", "No results found", "OK");
                    }
                    return false;
                }

                //
                return true;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return false;
            }
        }

        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------

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
            SRiVisitData visitData, oldVisitData;
            SRiRecordData recordData, oldRecordData, allocatedRecords;
            SRiCPInfoData cpinfoData, oldCpinfoData;
            SRiLICaseData licaseData, oldLicaseData;
            SRiCNApplPropData historyData, oldHistoryData;
            SRiPRNotePadData notesData, oldNotesData;
            OnSiteDocumentData docData;
            OnSiteDocumentCache docs;
            List<SRiProperty> properties;
            try
            {
                //
                // Get the current data from memory.
                AppData.PropertyModel.CurrentData(out oldCpinfoData, out oldLicaseData,
                                                  out oldHistoryData, out oldNotesData,
                                                  out oldVisitData, out oldRecordData, out docs);
                //
                // Download the visits associated to the current user.
                // Include the new search results in the compilation of the "keys" array.
                visitData = await GetVisits();
                if (!HandleJsonError() || visitData == null) return false;
                //
                var keys = oldRecordData.Records.Select(x => x.Record.EntityKey).ToList();
                foreach (var k in visitData.RecordEntityKeys)
                {
                    if (!keys.Contains(k))
                        keys.Add(k);
                }
                //
                allocatedRecords = await GetRecords("idoxid", new string[1] { AppData.MainModel.CurrentUser.IdoxId });
                foreach (var r in allocatedRecords.Records)
                    if (!keys.Contains(r.EntityMeta.KeyValue))
                        keys.Add(r.EntityMeta.KeyValue);
                //
                if (keys.Count > 0)
                {
                    // Download the SRRECs associated to the visits.
                    recordData = await GetRecords("entitykey", keys.ToArray());
                    if (!HandleJsonError() || recordData == null) return false;
                    //Updated for new record creation
                    recordData.MergeOld(oldRecordData, false);
                    //
                    // Re-download the visits so we have all visits associated to the SRRECs.
                    if (recordData.VisitEntityKeys.Length > 0)
                    {
                        visitData = await GetVisits(recordData.VisitEntityKeys);
                        if (!HandleJsonError() || visitData == null) return false;
                        //Updated for new record creation
                        visitData.MergeOld(oldVisitData, false);
                    }
                    else
                        visitData = oldVisitData;
                    //
                    // Download document data for visit and records.

                    //
                    // Download all the extra data required
                    cpinfoData = await GetCPInfos(recordData.CPEntityKeys);
                    if (!HandleJsonError() || cpinfoData == null) return false;
                    //Updated for new record creation
                    cpinfoData.MergeOld(oldCpinfoData, false);
                    //
                    licaseData = await GetLICases(recordData.LIEntityKeys);
                    if (!HandleJsonError() || licaseData == null) return false;
                    //Updated for new record creation
                    licaseData.MergeOld(oldLicaseData, false);
                    //
                    historyData = await GetPropertyHistory(recordData.HistoryEntityKeys);
                    if (!HandleJsonError() || historyData == null) return false;
                    //Updated for new record creation
                    historyData.MergeOld(oldHistoryData, false);
                    //
                    notesData = await GetPropertyNotes(recordData.NotePadEntityKeys);
                    if (!HandleJsonError() || notesData == null) return false;
                    //Updated for new record creation
                    notesData.MergeOld(oldNotesData, false);
                    //
                    // Populate the record data with the visits and then use it to create the property collection.
                    recordData.AddVisits(visitData.Visits);
                    properties = recordData.CreateProperties(cpinfoData, licaseData, historyData, notesData);
                    if (properties == null) return false;
                    //
                    // Get Document data for records and visits.
                    if (AppData.Environment != OnSiteEnvironments.Sales)
                    {
                        docData = await GetDocuments("srrec", "entitykey", recordData.EntityKeys);
                        if (docData != null) properties.AddDocumentData(docData);
                        docData = await GetDocuments("xivisit", "entitykey", visitData.EntityKeys);
                        if (docData != null) properties.AddDocumentData(docData);
                        properties.UpdateDocStatus(docs);
                    }
                    else
                    {
                        docData = OnSiteDocumentData.FromJson(ReadFile("sr_docs.txt"), out m_oJsonError);
                        if (docData != null) properties.AddDocumentData(docData);
                    }
                }
                else
                    properties = new List<SRiProperty>();
                //
                AppData.PropertyModel.Update(properties);
                return true;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return false;
            }
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
        /// ------------------------------------------------------------------------------------------------
        /// 
     	private async Task<List<OnSiteConfig>> GetConfig(string organisation)
        {
            try
            {
                ServiceClient client;
                RequestResponseEventArgs response;
                List<OnSiteConfig> configs;
                Exception error;
                OnSiteConfig config;
                // 
                m_oJsonError = null;
                configs = new List<OnSiteConfig>();
                var entities = new string[4] { "srrec", "xivisit", "cpinfo", "licase" };
                if (AppData.Environment == OnSiteEnvironments.Sales)
                {
                    foreach (var entity in entities)
                    {
                        configs.Add(OnSiteConfig.FromJson(ReadFile(string.Format("sr_config_{0}.txt", entity)), out error));
                        if (error != null) m_oJsonError = error;
                    }
                }
                else
                {
                    client = await GetServiceClient();
                    foreach (var entity in entities)
                    {
                        response = await client.DownloadStringAsync(GetUrl(string.Format("{0}/configuration?hash={1}&organisation={2}",
                                                                                         entity,
                                                                                         AppData.ConfigModel.Hash(organisation, entity),
                                                                                         organisation)));
                        if (await HandleError(response))
                        {
                            config = OnSiteConfig.FromJson(response.Text, out m_oJsonError);
                            if (HandleJsonError())
                            {
                                config.Entity = entity;
                                configs.Add(config);
                            }
                            else break;
                        }
                        else break;
                    }
                }
                //
                return configs;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetDocument
        /// 
        /// <summary>	Downloads the document data for the given document.
        /// </summary>
        /// <param name="doc">		The document to download.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
       	public async Task<bool> GetDocument(OnSiteDocument doc, string entity)
        {
            try
            {
                ServiceClient client;
                RequestResponseEventArgs response;
                FileSystemArgs writeResponse;
                bool result;
                //
                result = true;
                client = await GetServiceClient();
                if (AppData.Environment != OnSiteEnvironments.Sales)
                    response = await client.DownloadDataAsync(GetUrl(string.Format("{0}/document/{1}", entity, doc.Id)));
                else
                    response = await client.DownloadDataAsync(string.Format("http://isbeta.idoxgroup.com/pssdservice/cache/sri/{0}", doc.FileName));
                //
                if (await HandleError(response))
                {
                    writeResponse = await FileSystem.WriteAsync(response.Data, doc.FileName);
                    result = writeResponse.Error == null;
                }
                else result = false;

                //
                AppData.PropertyModel.UpdateProperty(PropertyType.Documents);
                return result;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return false;
            }
        }
        /// 
        private async Task<OnSiteDocumentData> GetDocuments(string entity, string fieldname, string[] fieldValues)
        {
            try
            {
                ServiceClient client;
                RequestResponseEventArgs response;
                OnSiteDocumentData data;
                //
                data = null;
                if (AppData.Environment != OnSiteEnvironments.Sales)
                {
                    client = await GetServiceClient();
                    response = await client.DownloadStringAsync(GetUrl(string.Format("{0}/document?fieldname={1}&fieldvalue={2}",
                                                                                     entity, fieldname, string.Join(",", fieldValues))));
                    if (await HandleError(response))
                        data = OnSiteDocumentData.FromJson(response.Text, out m_oJsonError);
                }
                return data;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
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
            try
            {
                if (AppData.Environment == OnSiteEnvironments.NoAuth)
                    return string.Format("{0}/mobile/v1/srqaz/{1}",
                                         AppData.ActiveEnvironments[AppData.Environment].APIHost,
                                         endPath);
                else
                    return AppData.ActiveEnvironments[AppData.Environment].APIHost + "/agw/mobile/v1/sr/" + endPath;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
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
            try
            {
                ServiceClient client;
                //RequestResponseEventArgs response;
                //CPiAPIVersion version;
                //Exception error;
                //bool result;
                //bool done;
                //
                client = new ServiceClient();
                if (AppData.Environment == OnSiteEnvironments.Dev ||
                    AppData.Environment == OnSiteEnvironments.QA ||
                    AppData.Environment == OnSiteEnvironments.Staging ||
                    AppData.Environment == OnSiteEnvironments.Production)
                    client.AddAuthHeader(AppData.MainModel.CurrentUser.Token);
                //
                // There is currently no API version verification needed, just return the client instead.
                return client;
                //client.AddAuthHeader(AppData.MainModel.CurrentUser.Token);
                //
                // Make sure the API version is verified. This is not needed for the Sales environment.
                //if (AppData.Environment == OnSiteEnvironments.Sales)
                //    result = true;
                //else
                //    result = AppData.APIVersionVerified;
                ////
                //if (!result)
                //{
                //    // TODO : Check that the API version matches what the app expects it to be.
                //    await Task.Delay(100);
                //    result = true;

                //    //done = false;
                //    //error = null;
                //    //response = await client.DownloadStringAsync(GetUrl("configuration/version"));
                //    //if (response.Error == null && response.Text.Length > 0)
                //    //{
                //    //	version = CPiAPIVersion.FromJson(response.Text, out error);
                //    //	if (version != null)
                //    //	{
                //    //		done = true;
                //    //		if (version.Version != AppData.APIVersion)
                //    //			AlertView.Build("App Requires Updating",
                //    //			                "The app is out of sync with the API and requires updating before use.",
                //    //			                new AlertView.Button("Exit App", () =>
                //    //		{
                //    //			throw new ExitException();
                //    //		})).Show();
                //    //		else
                //    //			result = true;
                //    //	}
                //    //}
                //    ////
                //    //if (response.Error != null && response.AuthenticationError)
                //    //	HubView.HandleError(response);
                //    //else if (!done)
                //    //	AlertView.Build("API Version Error",
                //    //	                string.Format("It was not possible to verify the version of the API for the [{0}] environment.\r\n{1}", 
                //    //	                              AppData.Environment.GetDescription(),
                //    //	                              response.Error != null ? response.Error.Message : error != null ? error.Message : "The response was null.")).Show();
                //}
                ////
                //if (result)
                //    return client;
                //else
                //    return null;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
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
       	private async Task<bool> HandleError(RequestResponseEventArgs response)
        {
            try
            {
                if (response.Error == null)
                    return true;
                //
                if (Reachability.InternetConnectionStatus() == ReachabilityNetworkStatus.NotReachable)
                    await SplitView.Instace().DisplayAlert("Network Error",
                                     "It was not possible to complete the action due to a loss of network access. Please connect the device to a network and try again.", "OK");
                else if (response.AuthenticationError)
                {
                    if (await SplitView.DisplayAlert("Authentication Required",
                  "Your session has expired and you are required to re-authenticate before completing any further actions.\r\n\nWould you like to start a new session now?",
                  "Yes", "No"))
                    {
                        //On Logout or ReAuthentication when new request is in progress the new request operation must cancel
                        AppContext.AppContext.NewRecordInProgress = false;
                        App.Current.MainPage = new LoginPage();            //Modified for IDOX-315 on 08.05.2017
                        LoginPage.ReAuthenticate.Invoke();
                    }
                }
                else
                {
                    if (await SplitView.DisplayAlert("Request Error", string.IsNullOrEmpty(response.Text) ? response.Error.Message : response.Text, "Send Report", "OK"))
                         SplitView.CenterPopupContent.ShowPopupCenter(new RequestErrorPopup(response), 0.5, "Error Popup", true);
                }
                //
                AppContext.AppContext.LocationSelected = false;
                return false;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return false;
            }
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
            try
            {
                if (m_oJsonError == null)
                    return true;
                SplitView.Instace().DisplayAlert("Json Error", m_oJsonError.Message, "OK");
                return false;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return false;
            }
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
            try
            {
                var currentAssembly = typeof(API).GetTypeInfo().Assembly;
                string text;
                using (var reader = new StreamReader(currentAssembly.GetManifestResourceStream("ServiceRequest." + fileName)))
                {
                    text = reader.ReadToEnd();
                }
                return text;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------

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
        private async Task<SRiVisitData> GetVisits()
        {
            try
            {
                SRiVisitData data;
                ServiceClient client;
                RequestResponseEventArgs response;
                //
                m_oJsonError = null;
                if (AppData.Environment == OnSiteEnvironments.Sales)
                    data = SRiVisitData.FromJson(ReadFile("sr_xivisit.txt"), out m_oJsonError);
                else
                {
                    client = await GetServiceClient();
                    response = await client.DownloadStringAsync(GetUrl("xivisit/case/idoxid"));
                    if (await HandleError(response))
                        data = SRiVisitData.FromJson(response.Text, out m_oJsonError);
                    else
                        data = null;
                }
                //
                return data;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetVisits
        /// 
        /// <summary>	Downloads XIVISIT data from the API.
        /// </summary>
        /// <param name="entityKeys">		The EntityKeys of the records to retreive.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async Task<SRiVisitData> GetVisits(string[] entityKeys)
        {
            try
            {
                SRiVisitData data;
                ServiceClient client;
                RequestResponseEventArgs response;
                //
                m_oJsonError = null;
                data = null;
                if (AppData.Environment == OnSiteEnvironments.Sales)
                    data = SRiVisitData.FromJson(ReadFile("sr_xivisit.txt"), out m_oJsonError);
                else if (entityKeys.Length > 0)
                {
                    client = await GetServiceClient();
                    response = await client.DownloadStringAsync(GetUrl("xivisit/case?fieldname=entitykey&fieldvalue=" +
                                                                       string.Join(",", entityKeys)));
                    if (await HandleError(response))
                        data = SRiVisitData.FromJson(response.Text, out m_oJsonError);
                    else
                        data = null;
                }
                //
                return data;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
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
            try
            {
                ServiceClient client;
                RequestResponseEventArgs response;
                SRiRecordData data;
                //
                m_oJsonError = null;
                if (AppData.Environment == OnSiteEnvironments.Sales)
                    data = SRiRecordData.FromJson(ReadFile("sr_srrec.txt"), out m_oJsonError);
                else
                {
                    client = await GetServiceClient();
                    response = await client.DownloadStringAsync(GetUrl("srrec/case?fieldname=refval&fieldvalue=" + searchTerm));
                    if (await HandleError(response))
                        data = SRiRecordData.FromJson(response.Text, out m_oJsonError);
                    else
                        data = null;
                }
                //
                return data;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetRecords
        /// 
        /// <summary>	Downloads SRREC data from the API using the supplied set of KeyVals.
        /// </summary>
        /// <param name="fieldName">		The field name to use.</param>
        /// <param name="fieldValues">		The EntityKeys of the records to retreive.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async Task<SRiRecordData> GetRecords(string fieldName, string[] fieldValues)
        {
            try
            {
                ServiceClient client;
                RequestResponseEventArgs response;
                SRiRecordData data;
                //
                m_oJsonError = null;
                data = null;
                if (AppData.Environment == OnSiteEnvironments.Sales)
                    data = SRiRecordData.FromJson(ReadFile("sr_srrec.txt"), out m_oJsonError);
                else if (fieldValues.Length > 0)
                {
                    client = await GetServiceClient();
                    response = await client.DownloadStringAsync(GetUrl(string.Format("srrec/case?fieldname={0}&fieldvalue={1}",
                                                                                     fieldName, string.Join(",", fieldValues))));
                    if (await HandleError(response))
                    {
                        data = SRiRecordData.FromJson(response.Text, out m_oJsonError);

                    }
                    else
                        data = null;
                }
                //
                return data;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetCPInfos
        /// 
        /// <summary>	Downloads CPINFO data from the API using the supplied set of KeyVals.
        /// </summary>
        /// <param name="entityKeys">		The EntityKeys of the records to retreive.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
      	private async Task<SRiCPInfoData> GetCPInfos(string[] entityKeys)
        {
            try
            {
                ServiceClient client;
                RequestResponseEventArgs response;
                SRiCPInfoData data;
                //
                // Return empty data if no keys provided
                if (entityKeys.Length == 0)
                    return new SRiCPInfoData() { CPInfos = new List<SRiCPInfoMeta>() };
                //
                data = null;
                m_oJsonError = null;
                if (AppData.Environment == OnSiteEnvironments.Sales)
                    data = SRiCPInfoData.FromJson(ReadFile("sr_cpinfo.txt"), out m_oJsonError);
                else if (entityKeys.Length > 0)
                {
                    client = await GetServiceClient();
                    response = await client.DownloadStringAsync(GetUrl("cpinfo/case?fieldname=entity&fieldvalue=" +
                                                                       string.Join(",", entityKeys)));
                    if (await HandleError(response))
                        data = SRiCPInfoData.FromJson(response.Text, out m_oJsonError);
                    else
                        data = null;
                }
                //
                return data;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetLICases
        /// 
        /// <summary>	Downloads LICASE data from the API using the supplied set of KeyVals.
        /// </summary>
        /// <param name="entityKeys">		The EntityKeys of the records to retreive.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async Task<SRiLICaseData> GetLICases(string[] entityKeys)
        {
            try
            {
                ServiceClient client;
                RequestResponseEventArgs response;
                SRiLICaseData data;
                //
                // Return empty data if no keys provided
                if (entityKeys.Length == 0)
                    return new SRiLICaseData() { LICases = new List<SRiLICaseMeta>() };
                //
                data = null;
                m_oJsonError = null;
                if (AppData.Environment == OnSiteEnvironments.Sales)
                    data = SRiLICaseData.FromJson(ReadFile("sr_licase.txt"), out m_oJsonError);
                else if (entityKeys.Length > 0)
                {
                    client = await GetServiceClient();
                    response = await client.DownloadStringAsync(GetUrl("licase/case?fieldname=entitykey&fieldvalue=" +
                                                                       string.Join(",", entityKeys)));
                    if (await HandleError(response))
                        data = SRiLICaseData.FromJson(response.Text, out m_oJsonError);
                    else
                        data = null;
                }
                //
                return data;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetPropertyNotes
        /// 
        /// <summary>	Downloads PRNOTEPAD data from the API using the supplied set of KeyVals.
        /// </summary>
        /// <param name="entityKeys">		The EntityKey of the records to retreive.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
       	private async Task<SRiPRNotePadData> GetPropertyNotes(string[] entityKeys)
        {
            try
            {
                ServiceClient client;
                RequestResponseEventArgs response;
                SRiPRNotePadData data;
                //
                // Return empty data if no keys provided
                if (entityKeys.Length == 0)
                    return new SRiPRNotePadData() { Notes = new List<SRiPRNotePadMeta>() };
                //
                data = null;
                m_oJsonError = null;
                if (AppData.Environment == OnSiteEnvironments.Sales)
                    data = SRiPRNotePadData.FromJson(ReadFile("sr_prnotepad.txt"), out m_oJsonError);
                else if (entityKeys.Length > 0)
                {
                    client = await GetServiceClient();
                    response = await client.DownloadStringAsync(GetUrl("prnotepad/case?fieldname=entitykey&fieldvalue=" +
                                                                       string.Join(",", entityKeys)));
                    if (await HandleError(response))
                        data = SRiPRNotePadData.FromJson(response.Text, out m_oJsonError);
                    else
                        data = null;
                }
                //
                return data;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetPropertyHistory
        /// 
        /// <summary>	Downloads CNAPPLPROP data from the API using the supplied set of KeyVals.
        /// </summary>
        /// <param name="entityKeys">		The EntityKey of the records to retreive.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
      	private async Task<SRiCNApplPropData> GetPropertyHistory(string[] entityKeys)
        {
            try
            {
                ServiceClient client;
                RequestResponseEventArgs response;
                SRiCNApplPropData data;
                //
                // Return empty data if no keys provided
                if (entityKeys.Length == 0)
                    return new SRiCNApplPropData() { History = new List<SRiCNApplPropMeta>() };
                //
                data = null;
                m_oJsonError = null;
                if (AppData.Environment == OnSiteEnvironments.Sales)
                    data = SRiCNApplPropData.FromJson(ReadFile("sr_cnapplprop.txt"), out m_oJsonError);
                else if (entityKeys.Length > 0)
                {
                    client = await GetServiceClient();
                    response = await client.DownloadStringAsync(GetUrl("cnapplprop/case?fieldname=entitykey&fieldvalue=" +
                                                                       string.Join(",", entityKeys)));
                    if (await HandleError(response))
                        data = SRiCNApplPropData.FromJson(response.Text, out m_oJsonError);
                    else
                        data = null;
                }
                //
                return data;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return null;
            }
        }
        /// 
        private async Task<bool> UploadDocuments(List<OnSiteDocumentUpload> documents)
        {
            try
            {
                ServiceClient client;
                FileSystemArgs fileResponse;
                RequestResponseEventArgs response;
                OnSiteDocumentData newDocData;
                byte[] data;
                //
                if (AppData.Environment != OnSiteEnvironments.Sales)
                {
                    foreach (var docUpload in documents)
                    {
                        fileResponse = await FileSystem.ReadDataAsync(docUpload.Document.FileName);
                        if (fileResponse.Error != null)
                        {
                            await SplitView.Instace().DisplayAlert("File Read Error", fileResponse.Error.Message, "OK");
                            return false;
                        }
                        data = fileResponse.BinaryContents;
                        //
                        // Upload meta data.
                        client = await GetServiceClient();
                        OnSiteSettings.SerializeNullValues = false;
                        OnSiteSettings.ShouldSerialize = ShouldSerializeRule.DataForAPI;
                        response = await client.UploadStringAsync(GetUrl(string.Format("{0}/document", docUpload.Data.Entity)),
                                                                  false, docUpload.Data.ToJson(), ContentType.Json);
                        if (!await HandleError(response))
                            return false;
                        //
                        newDocData = OnSiteDocumentData.FromJson(response.Text, out m_oJsonError);
                        if (!HandleJsonError())
                            return false;
                        if (newDocData == null)
                        {
                            await SplitView.Instace().DisplayAlert("Deserialization Error", "The new document meta was not received from the API.", "OK");
                            return false;
                        }
                        //
                        // Rename the cached file.
                        fileResponse = await FileSystem.RenameAsync(newDocData.EntityDocumentLists[0].Documents[0].FileName, docUpload.Document.FileName);
                        if (fileResponse.Error != null)
                        {
                            await SplitView.Instace().DisplayAlert("File Rename Error", fileResponse.Error.Message, "OK");
                            return false;
                        }
                        //
                        // Update the collection in memory.
                        AppData.PropertyModel.Update(newDocData.EntityDocumentLists[0].Documents[0], docUpload);
                        //
                        // Upload the document data.
                        response = await client.UploadDataAsync(GetUrl(string.Format("{0}/document/{1}", docUpload.Data.Entity, newDocData.EntityDocumentLists[0].Documents[0].Id)),
                                                                true, data, ContentType.BinaryData);
                        if (!await HandleError(response))
                            return false;
                    }
                }
                //
                return true;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return false;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		UploadRecords
        /// 
        /// <summary>	Uploads the records to the API.
        /// </summary>
        /// <param name="records">		The records.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
      	private async Task<bool> UploadRecords(List<SRiRecordMeta> records)
        {
            try
            {
                ServiceClient client;
                RequestResponseEventArgs response;
                SRiRecordData data;
                SRiRecordData responseData;
                List<SRiResolution> resolutions;
                string url;
                bool isPut;
                //
                m_oJsonError = null;
                if (AppData.Environment != OnSiteEnvironments.Sales)
                {
                    client = await GetServiceClient();
                    resolutions = new List<SRiResolution>();
                    foreach (var record in records)
                    {
                        record.QueryState = "";
                        data = new SRiRecordData()
                        {
                            AppModule = "sr",
                            Module = "sr",
                            Entity = "srrec",
                            Records = new List<SRiRecordMeta>() { record }
                        };
                        //
                        if (record.Record.Status == SyncStatus.New)
                        {
                            url = GetUrl("srrec/case");
                            isPut = false;
                        }
                        else
                        {
                            url = GetUrl(string.Format("srrec/case/{0}", record.ID));
                            isPut = true;
                        }
                        //
                        responseData = null;
                        OnSiteSettings.SerializeNullValues = true;
                        OnSiteSettings.ShouldSerialize = ShouldSerializeRule.DataForAPI;
                        response = await client.UploadStringAsync(url, isPut, data.ToJson(), ContentType.Json);
                        if (await HandleError(response))
                        {
                            responseData = SRiRecordData.FromJson(response.Text, out m_oJsonError);
                            if (HandleJsonError() && responseData != null && responseData.Records.Count == 1)
                            {
                                if (responseData.Records[0].QueryState.ToUpper().Equals("S"))
                                {
                                    responseData.Records[0].Record.Status = SyncStatus.Saved;
                                    AppData.PropertyModel.Update(responseData.Records[0], record);
                                }
                                else
                                    m_Resolution.Add(new SRiResolution(record, responseData.Records[0]));
                            }

                            else return false;
                        }
                        else return false;
                    }
                }
                //
                return true;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return false;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		UploadVisits
        /// 
        /// <summary>	Uploads the visits to the API.
        /// </summary>
        /// <param name="visits">		The visits.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private async Task<bool> UploadVisits(List<SRiVisitMeta> visits)
        {
            try
            {
                ServiceClient client;
                RequestResponseEventArgs response;
                SRiVisitData data;
                SRiVisitData responseData;
                List<SRiResolution> resolutions;
                string url;
                bool isPut;
                //
                m_oJsonError = null;
                if (AppData.Environment != OnSiteEnvironments.Sales)
                {
                    client = await GetServiceClient();
                    resolutions = new List<SRiResolution>();
                    foreach (var visit in visits)
                    {
                        visit.QueryState = "";
                        data = new SRiVisitData()
                        {
                            AppModule = "sr",
                            Module = "xi",
                            Entity = "xivisit",
                            Visits = new List<SRiVisitMeta>() { visit }
                        };
                        //
                        if (visit.Status == SyncStatus.New)
                        {
                            url = GetUrl("xivisit/case");
                            isPut = false;
                        }
                        else
                        {
                            url = GetUrl(string.Format("xivisit/case/{0}", visit.ID));
                            isPut = true;
                        }
                        //
                        responseData = null;
                        OnSiteSettings.SerializeNullValues = true;
                        OnSiteSettings.ShouldSerialize = ShouldSerializeRule.DataForAPI;
                        response = await client.UploadStringAsync(url, isPut, data.ToJson(), ContentType.Json);
                        if (await HandleError(response))
                        {
                            responseData = SRiVisitData.FromJson(response.Text, out m_oJsonError);
                            if (HandleJsonError() && responseData != null && responseData.Visits.Count == 1)
                            {
                                if (responseData.Visits[0].QueryState.ToUpper().Equals("S"))
                                {
                                    responseData.Visits[0].Status = SyncStatus.Saved;
                                    AppData.PropertyModel.Update(responseData.Visits[0], visit);
                                }
                                else
                                    m_Resolution.Add(new SRiResolution(visit, responseData.Visits[0]));
                            }

                            else return false;
                        }
                        else return false;
                    }
                }
                //
                return true;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
                return false;
            }
        }

        /// 
        ///  ------------------------------------------------------------------------------------------------
        ///  Name		GetVisits
        ///  
        ///  <summary>	Downloads XIVISIT data from the API.
        ///  </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <remarks>
        ///  </remarks>
        ///  ------------------------------------------------------------------------------------------------
        ///  
        public async Task UtilityByCoordinates(double latitude, double longitude)
        {
            try
            {
                SRiUtilityAddresses data;
                ServiceClient client;
                RequestResponseEventArgs response;
                //
                m_oJsonError = null;
                if (AppData.Environment == OnSiteEnvironments.Sales)
                    data = SRiUtilityAddresses.FromJson(string.Empty, out m_oJsonError);
                else
                {
                    client = await GetServiceClient();
                    response = await client.DownloadStringAsync(string.Format("{0}/agw/mobile/v1/utility/address/nearby?point={1},{2}", AppData.ActiveEnvironments[AppData.Environment].APIHost, latitude, longitude));
                    if (await HandleError(response))
                        data = SRiUtilityAddresses.FromJson(response.Text, out m_oJsonError);
                    else
                        data = null;
                }
                //
                AddNewCaseView.UtilityAddressesByCoord = data;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// 
        ///  ------------------------------------------------------------------------------------------------
        ///  Name		GetVisits
        ///  
        ///  <summary>	Downloads XIVISIT data from the API.
        ///  </summary>
        /// <param name="address"></param>
        /// <remarks>
        ///  </remarks>
        ///  ------------------------------------------------------------------------------------------------
        ///  
        public async Task UtilityByAddresses(string address)
        {
            try
            {
                SRiUtilityAddresses data;
                ServiceClient client;
                RequestResponseEventArgs response;
                //
                m_oJsonError = null;
                if (AppData.Environment == OnSiteEnvironments.Sales)
                    data = SRiUtilityAddresses.FromJson(string.Empty, out m_oJsonError);
                else
                {
                    client = await GetServiceClient();
                    response = await client.DownloadStringAsync(string.Format("{0}/agw/mobile/v1/utility/address/lookup?address={1}", AppData.ActiveEnvironments[AppData.Environment].APIHost, address));
                    if (await HandleError(response))
                        data = SRiUtilityAddresses.FromJson(response.Text, out m_oJsonError);
                    else
                        data = null;
                }
                //
                AddNewCaseView.UtilityAddressesByAdd = data;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }

        /// 
        ///  ------------------------------------------------------------------------------------------------
        ///  Name		GetVisits
        ///  
        ///  <summary>	Downloads XIVISIT data from the API.
        ///  </summary>
        /// <param name="postcode"></param>
        /// <remarks>
        ///  </remarks>
        ///  ------------------------------------------------------------------------------------------------
        ///  
        public async Task UtilityByPostCode(string postcode)
        {
            try
            {
                SRiUtilityAddresses data;
                ServiceClient client;
                RequestResponseEventArgs response;
                //
                m_oJsonError = null;
                if (AppData.Environment == OnSiteEnvironments.Sales)
                    data = SRiUtilityAddresses.FromJson(string.Empty, out m_oJsonError);
                else
                {
                    client = await GetServiceClient();
                    response = await client.DownloadStringAsync(string.Format("{0}/agw/mobile/v1/utility/address/lookup?postcode={1}", AppData.ActiveEnvironments[AppData.Environment].APIHost, postcode));
                    if (await HandleError(response))
                        data = SRiUtilityAddresses.FromJson(response.Text, out m_oJsonError);
                    else
                        data = null;
                }
                //
                AddNewCaseView.UtilityAddressesByPost = data;
            }
            catch (Exception ex)
            {
                LogTracking.LogTrace(ex.ToString());
            }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
