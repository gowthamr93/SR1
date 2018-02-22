using System.Collections.Generic;
using System.Threading.Tasks;

namespace Idox.LGDP.Apps.Common.OnSite
{
    /// ----------------------------------------------------------------------------------------------------
    /// Name			IAPI
    /// 
    /// <summary>		Generic API interface definition for an OnSite app.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public interface IAPI
    {
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
        Task<bool> GetConfigs(List<string> organisations);
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetDocument
        /// 
        /// <summary>	Gets the document data for a given record.
        /// </summary>
        /// <param name="doc">		The doc that should be downloaded.</param>
        /// <param name="entity">	The entity name that the document is attached to.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        Task<bool> GetDocument(OnSiteDocument doc, string entity);
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
        Task<bool> Sync();
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Search
        /// 
        /// <summary>	Performs a search using the supplied search term. Results are stored in the relevant
        /// 			view model as part of this method.
        /// </summary>
        /// <param name="searchTerm">		The search term.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        Task<bool> Search(string searchTerm);

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetDocument
        /// <summary>	Gets the document data for a given record.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        Task UtilityByCoordinates(double latitude, double longitude);
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetDocument
        /// <summary>	Gets the document data for a given record.
        /// </summary>
        /// <param name="address"></param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        Task UtilityByAddresses(string address);
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetDocument
        /// <summary>	Gets the document data for a given record.
        /// </summary>
        /// <param name="postcode"></param> 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        Task UtilityByPostCode(string postcode);
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
