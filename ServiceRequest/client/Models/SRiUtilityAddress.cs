using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Idox.LGDP.Apps.Common.OnSite;
using Newtonsoft.Json;

namespace Idox.LGDP.Apps.ServiceRequest.Client.Models
{
    public class SRiUtilityAddress: OnSiteJsonEntity<SRiUtilityAddress>
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
        [JsonProperty("address")]
        public string Address
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
        [JsonProperty("status")]
        public string Status
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
        [JsonProperty("uprn")]
        public long UPRN
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
        [JsonProperty("usrn")]
        public long USRN
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
        [JsonProperty("organisation")]
        public string Organisation
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
        [JsonProperty("minorAddress")]
        public string MinorAddress
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
        [JsonProperty("majorAddress")]
        public string MajorAddress
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
        [JsonProperty("street")]
        public string Street
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
        [JsonProperty("locality")]
        public string Locality
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
        [JsonProperty("town")]
        public string Town
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
        [JsonProperty("county")]
        public string County
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
        [JsonProperty("area")]
        public string Area
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
        [JsonProperty("postcode")]
        public string PostCode
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
        [JsonProperty("easting")]
        public double? East
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
        [JsonProperty("northing")]
        public double? North
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
        [JsonProperty("longitude")]
        public double? Longitude
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
        [JsonProperty("latitude")]
        public double? Latitude
        {
            get;
            set;
        }
        #endregion
    }
}
