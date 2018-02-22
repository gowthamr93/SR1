using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using Idox.LGDP.Apps.Common.OnSite;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
    /// ----------------------------------------------------------------------------------------------------
    /// Name			SRiLICase
    /// 
    /// <summary>		Used for consuming LICase data from the API.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public class SRiLICase : OnSiteJsonEntity<SRiLICase>
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables
        /// ------------------------------------------------------------------------------------------------
        /// 
        private List<SRiLIParty> m_oParties;
        private string m_sEntityKey;
        private string m_sKeyVal;
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Json Properties
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("modified")]
        public bool Modified
        {
            get;
            set;
        }
        /// 
        [JsonProperty("licase0entitykey")]
        public string EntityKey
        {
            get
            {
                if (OnSiteSettings.ShouldSerialize == ShouldSerializeRule.DataForAPI &&
                    OnSiteSettings.KeyIsTemp(m_sEntityKey))
                    return "";
                else
                    return m_sEntityKey;
            }
            set { m_sEntityKey = value; }
        }
        /// 
        [JsonProperty("licase0keyval")]
        public string KeyVal
        {
            get
            {
                if (m_sKeyVal == null)
                    m_sKeyVal = "";
                return m_sKeyVal;
            }
            set { m_sKeyVal = value; }
        }
        /// 
        [JsonProperty("licase0u_version")]
        public string UVersion
        {
            get;
            set;
        }
        /// 
        [JsonProperty("licase0refval")]
        public string RefVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("licase0licntype")]
        public string CNType
        {
            get;
            set;
        }
        /// 
        [JsonProperty("licase0plateref")]
        public string PlateRef
        {
            get;
            set;
        }
        /// 
        [JsonProperty("licase0commend")]
        public DateTime? CommendDate
        {
            get;
            set;
        }
        /// 
        [JsonProperty("licase0expiryd")]
        public DateTime? ExpiryDate
        {
            get;
            set;
        }
        /// 
        [JsonProperty("liparty")]
        public List<SRiLIParty> LIParties
        {
            get
            {
                if (m_oParties == null)
                    m_oParties = new List<SRiLIParty>();
                return m_oParties;
            }
            set { m_oParties = value; }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// ----------------------------------------------------------------------------------------------------
    #region Meta & Data
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public class SRiLICaseMeta
    {
        /// ------------------------------------------------------------------------------------------------
        #region Json Properties
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ID
        /// 
        /// <summary>	Gets and sets the ID for the meta.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("id")]
        public string ID
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Organisation
        /// 
        /// <summary>	Gets and sets the Organisation for the meta.
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
        /// Name		Received
        /// 
        /// <summary>	Gets and sets the Received for the meta.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("received")]
        public string Received
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Version
        /// 
        /// <summary>	Gets and sets the Version for the meta.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("version")]
        public string Version
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		LICase
        /// 
        /// <summary>	Gets and sets the LICase for the meta.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public SRiLICase LICase
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		LICaseData
        /// 
        /// <summary>	Gets and sets the LICaseData for the meta.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("data")]
        public string LICaseData
        {
            get { return LICase.ToJson(); }
            set
            {
                Exception error;
                //
                LICase = SRiLICase.FromJson(value, out error);
                if (error != null) throw error;
            }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		CNTypeDescription
        /// 
        /// <summary>	gets the code description for the CNType code.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string CNTypeDescription
        {
            get
            {
                if (string.IsNullOrEmpty(LICase.CNType))
                    return "No type";
                else
                    return AppData.ConfigModel.CodeDescription(Organisation, "LICNTYPE", LICase.CNType);
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		PartyTypeDescription
        /// 
        /// <summary>	Gets the party type description for the Party.
        /// </summary>
        /// <param name="party">		The party.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public string PartyTypeDescription(SRiLIParty party)
        {
            if (string.IsNullOrEmpty(party.PartyType))
                return "Unknown party";
            else
                return AppData.ConfigModel.CodeDescription(Organisation, "LIPTYTYPE", party.PartyType);
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    public class SRiLICaseData : OnSiteJsonEntity<SRiLICaseData>
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SRiLICaseData
        /// 
        /// <summary>	Creates a new instance of the SRiLICaseData class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public SRiLICaseData()
        {
            LICases = new List<SRiLICaseMeta>();
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Json Properties
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Module
        /// 
        /// <summary>	Gets and sets the Module for the data.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("module")]
        public string Module
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		AppModule
        /// 
        /// <summary>	Gets and sets the AppModule for the data.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("appModule")]
        public string AppModule
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Entity
        /// 
        /// <summary>	Gets and sets the Entity for the data.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("entity")]
        public string Entity
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		LICases
        /// 
        /// <summary>	Gets and sets the LICases for the data.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("cases")]
        public List<SRiLICaseMeta> LICases
        {
            get;
            set;
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		MergeOld
        /// 
        /// <summary>	Merges the old collection of licase into the current collection. The only licase 
        /// 			that are added are those that do not exist in the current collection. Match is
        /// 			done on EntityKey.
        /// </summary>
        /// <param name="data">		The old data.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void MergeOld(SRiLICaseData data, bool addIfNotThere)
        {
            bool exists;
            //
            foreach (var old in data.LICases)
            {
                exists = false;
                foreach (var i in LICases)
                    if (i.LICase.EntityKey.Equals(old.LICase.EntityKey))
                    {
                        exists = true;
                        //i.LICase.Status = old.LICase.Status;
                        break;
                    }
                //
                if (addIfNotThere && !exists)
                    LICases.Add(old);
            }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    #endregion
    /// ----------------------------------------------------------------------------------------------------
    #region Additional Models
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public class SRiLIParty
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private variables
        /// ------------------------------------------------------------------------------------------------
        /// 
        private string m_sKeyVal;
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Json Properties
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// 
        [JsonProperty("modified")]
        public bool Modified
        {
            get;
            set;
        }
        /// 
        [JsonProperty("liparty0keyval")]
        public string KeyVal
        {
            get
            {
                if (m_sKeyVal == null)
                    m_sKeyVal = "";
                return m_sKeyVal;
            }
            set { m_sKeyVal = value; }
        }
        /// 
        [JsonProperty("liparty0u_version")]
        public string UVersion
        {
            get;
            set;
        }
        /// 
        [JsonProperty("licase0keyval")]
        public string LICaseKeyVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("liparty0fullname")]
        public string FullName
        {
            get;
            set;
        }
        /// 
        [JsonProperty("liparty0liptytype")]
        public string PartyType
        {
            get;
            set;
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    #endregion
    /// ----------------------------------------------------------------------------------------------------
}
