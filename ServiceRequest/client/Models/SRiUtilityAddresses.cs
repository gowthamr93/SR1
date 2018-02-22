using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Idox.LGDP.Apps.Common.OnSite;
using Newtonsoft.Json;

namespace Idox.LGDP.Apps.ServiceRequest.Client.Models
{
    public class SRiUtilityAddresses : OnSiteJsonEntity<SRiUtilityAddresses>
    {
        #region Json Properties
        /// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Modified
		/// 
		/// <summary>	Gets and sets the Modified for the record.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("uri")]
        public Uri Uri
        {
            get;
            set;
        }
        /// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Modified
		/// 
		/// <summary>	Gets and sets the Modified for the record.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		///
        [JsonProperty("query")]
        public string InputQuery
        {
            get;
            set;
        }
        /// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Modified
		/// 
		/// <summary>	Gets and sets the Modified for the record.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		///
        [JsonProperty("totalresults")]
        public long Results
        {
            get;
            set;
        }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Modified
		/// 
		/// <summary>	Gets and sets the Modified for the record.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		///
        [JsonProperty("dataset")]
        public string DataSet
        {
            get;
            set;
        }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Modified
		/// 
		/// <summary>	Gets and sets the Modified for the record.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		///
        [JsonProperty("maxresults")]
        public long MaxResults
        {
            get;
            set;
        }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Modified
		/// 
		/// <summary>	Gets and sets the Modified for the record.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		///
        [JsonProperty("offset")]
        public double Offset
        {
            get;
            set;
        }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Modified
		/// 
		/// <summary>	Gets and sets the Modified for the record.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		///
        [JsonProperty("outCoordRef")]
        public string OutCoordinateReference
        {
            get;
            set;
        }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Modified
		/// 
		/// <summary>	Gets and sets the Modified for the record.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		///
        [JsonProperty("addresses")]
        public List<SRiUtilityAddress> Addresses
        {
            get;
            set;
        }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Modified
		/// 
		/// <summary>	Gets and sets the Modified for the record.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		///
        [JsonProperty("inCoordRef")]
        public string InCoordinateReference
        {
            get;
            set;
        }
        #endregion
    }
}
