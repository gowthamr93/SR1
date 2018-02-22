using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using Idox.LGDP.Apps.Common.OnSite;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
    /// ----------------------------------------------------------------------------------------------------
    /// Name			SRiCPInfo
    /// 
    /// <summary>		Used for consuming CPInfo data from the API.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public class SRiCPInfo : OnSiteJsonEntity<SRiCPInfo>
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private variables
        /// ------------------------------------------------------------------------------------------------
        /// 
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
        [JsonProperty("cpinfo0entitykey")]
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
        [JsonProperty("cpinfo0keyval")]
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
        [JsonProperty("cpinfo0refval")]
        public string RefVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("cpinfo0u_version")]
        public string UVersion
        {
            get;
            set;
        }
        /// 
        [JsonProperty("cpinfo0tradeas")]
        public string TradeAs
        {
            get;
            set;
        }
        /// 
        [JsonProperty("cpinfo0prkeyval")]
        public string ParentKeyVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("cpinfo0address")]
        public string Address
        {
            get;
            set;
        }
        /// 
        [JsonProperty("cpinfo0mapeast")]
        public string MapEast
        {
            get;
            set;
        }
        /// 
        [JsonProperty("cpinfo0mapnorth")]
        public string MapNorth
        {
            get;
            set;
        }
        /// 
        [JsonProperty("cpinfo0latitude")]
        public double? Latitude
        {
            get;
            set;
        }
        /// 
        [JsonProperty("cpinfo0longitude")]
        public double? Longitude
        {
            get;
            set;
        }
        /// 
        [JsonProperty("cpinfo0occupier")]
        public string Occupier
        {
            get;
            set;
        }
        /// 
        [JsonProperty("cpinfo0contact")]
        public string Contact
        {
            get;
            set;
        }
        /// 
        [JsonProperty("cpinfo0cpuse")]
        public string PremisesUse
        {
            get;
            set;
        }
        /// 
        [JsonProperty("cpfhrsrec")]
        public List<SRiCPFHRS> FHRSRecords
        {
            get;
            set;
        }
        /// 
        [JsonProperty("scpuses")]
        public List<SRiCPUses> PremisesUses
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xsrec")]
        public List<SRiXSRec> XSRecords
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xnrec")]
        public List<SRiXNRec> XNRecords
        {
            get;
            set;
        }
        /// 
        [JsonProperty("srrec0keyval")]
        public List<string> SRRecKeyVals
        {
            get;
            set;
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// ----------------------------------------------------------------------------------------------------
    #region Meta & Data
    /// ----------------------------------------------------------------------------------------------------
    /// 
    /// ----------------------------------------------------------------------------------------------------
    /// Name			SRiCPInfoMeta
    /// 
    /// <summary>		Used for consuming CPInfo data from the API.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public class SRiCPInfoMeta
    {
        /// ------------------------------------------------------------------------------------------------
        #region Json Properties
        /// ------------------------------------------------------------------------------------------------
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
        /// Name		CPInfo
        /// 
        /// <summary>	Gets and sets the CPInfo for the meta.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public SRiCPInfo CPInfo
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		CPInfoData
        /// 
        /// <summary>	Gets and sets the CPInfoData for the meta.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("data")]
        public string CPInfoData
        {
            get { return CPInfo.ToJson(); }
            set
            {
                Exception error;
                //
                CPInfo = SRiCPInfo.FromJson(value, out error);
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
        /// Name		PremisesUseDescription
        /// 
        /// <summary>	Gets the code description for the PremisesUse code.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string PremisesUseDescription
        {
            get
            {
                if (string.IsNullOrEmpty(CPInfo.PremisesUse))
                    return "No premises use";
                else
                    return AppData.ConfigModel.CodeDescription(Organisation, "CPUSE", CPInfo.PremisesUse);
            }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    /// ----------------------------------------------------------------------------------------------------
    /// Name			SRiCPInfoData
    /// 
    /// <summary>		Used for consuming CPInfo data from the API.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public class SRiCPInfoData : OnSiteJsonEntity<SRiCPInfoData>
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SRiCPInfoData
        /// 
        /// <summary>	Creates a new instance of the SRiCPInfoData class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public SRiCPInfoData()
        {
            CPInfos = new List<SRiCPInfoMeta>();
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Json Properties
        /// ------------------------------------------------------------------------------------------------
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
        /// Name		CPInfos
        /// 
        /// <summary>	Gets and sets the CPInfos for the data.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("cases")]
        public List<SRiCPInfoMeta> CPInfos
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
        /// <summary>	Merges the old collection of cpinfo into the current collection. The only cpinfo 
        /// 			that are added are those that do not exist in the current collection. Match is
        /// 			done on EntityKey.
        /// </summary>
        /// <param name="data">		The old data.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void MergeOld(SRiCPInfoData data, bool addIfNotThere)
        {
            bool exists;
            //
            foreach (var old in data.CPInfos)
            {
                exists = false;
                foreach (var i in CPInfos)
                    if (i.CPInfo.EntityKey.Equals(old.CPInfo.EntityKey))
                    {
                        exists = true;
                        //i.CPInfo.Status = old.Status;
                        break;
                    }
                //
                if (addIfNotThere && !exists)
                    CPInfos.Add(old);
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
    public class SRiCPFHRS
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
        [JsonProperty("modified")]
        public bool Modified
        {
            get;
            set;
        }
        /// 
        [JsonProperty("cpfhrsrec0keyval")]
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
        [JsonProperty("cpfhrsrec0u_version")]
        public string UVersion
        {
            get;
            set;
        }
        /// 
        [JsonProperty("cpinfo0keyval")]
        public string CPInfoKeyVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("cpfhrsrec0cpfhrating")]
        public string Rating
        {
            get;
            set;
        }
        /// 
        [JsonProperty("cpfhrsrec0tot_score")]
        public double? TotalScore
        {
            get;
            set;
        }
        /// 
        [JsonProperty("cpfhrsrec0assessd")]
        public DateTime? Assessed
        {
            get;
            set;
        }
        /// 
        [JsonProperty("cpfhrsrec0rescore")]
        public string ResultScore
        {
            get;
            set;
        }
        /// 
        [JsonProperty("cpfhrsrec0fhrs_score")]
        public double? FHRSScore
        {
            get;
            set;
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    public class SRiCPUses
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
        [JsonProperty("modified")]
        public bool Modified
        {
            get;
            set;
        }
        /// 
        [JsonProperty("scpuses0keyval")]
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
        [JsonProperty("scpuses0u_version")]
        public string UVersion
        {
            get;
            set;
        }
        /// 
        [JsonProperty("cpinfo0keyval")]
        public string CPInfoKeyVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("scpuses0cpuse")]
        public string PremisesUse
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
        /// Name		PremisesUseDescription
        /// 
        /// <summary>	Gets the code description for the PremisesUse code.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string PremisesUseDescription
        {
            get
            {
                if (string.IsNullOrEmpty(PremisesUse))
                    return "No premises use";
                else if (AppData.PropertyModel.SelectedProperty != null)
                    return AppData.ConfigModel.CodeDescription(AppData.PropertyModel.SelectedProperty.Organisation,
                                                               "CPUSE",
                                                               PremisesUse);
                else
                    return PremisesUse;
            }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    public class SRiXSRec
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
        [JsonProperty("modified")]
        public bool Modified
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xsrec0keyval")]
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
        [JsonProperty("xsrec0u_version")]
        public string UVersion
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xsrec0refval")]
        public string refVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xsrec0sampled")]
        public DateTime? Sampled
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xsrec0xbkeyval")]
        public string XBKeyVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xsrec0xbsubsys")]
        public string XBSubSys
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xsrec0xssurv")]
        public string XSSurv
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xsrec0xikeyval")]
        public string XIKeyVal
        {
            get;
            set;
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    public class SRiXNRec
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
        [JsonProperty("modified")]
        public bool Modified
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xnrec0keyval")]
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
        [JsonProperty("xnrec0refval")]
        public string RefVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xnrec0u_version")]
        public string UVersion
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xnrec0mdkeyal")]
        public string MDKeyVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xnrec0mdsubsys")]
        public string MDSubSys
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xnrec0xnrectype")]
        public string XNRecType
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xnrec0servd")]
        public DateTime? Served
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xnrec0reqd")]
        public DateTime? Required
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
        /// Name		XNRecTypeDescription
        /// 
        /// <summary>	Gets the code description for the XNRecType code.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string XNRecTypeDescription
        {
            get
            {
                if (string.IsNullOrEmpty(XNRecType))
                    return "No type";
                else if (AppData.PropertyModel.SelectedProperty != null)
                    return AppData.ConfigModel.CodeDescription(AppData.PropertyModel.SelectedProperty.Organisation,
                                                               "XNRECTYPE", // TODO - check this is correct codelist name.
                                                               XNRecType);
                else
                    return XNRecType;
            }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    #endregion
    /// ----------------------------------------------------------------------------------------------------
}
