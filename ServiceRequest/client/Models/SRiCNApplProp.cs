using System;
using System.Collections.Generic;
using Newtonsoft.Json;

using Idox.LGDP.Apps.Common.OnSite;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
    /// ----------------------------------------------------------------------------------------------------
    /// Name			SRiCNApplProp
    /// 
    /// <summary>		The Property History entity for Service Request, represents the CNAPPLPROP object.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public class SRiCNApplProp : OnSiteJsonEntity<SRiCNApplProp>
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables
        /// ------------------------------------------------------------------------------------------------
        /// 
        private List<SRiCNApplLog> m_oLogs;
        private string m_sEntityKey;
        private string m_sKeyVal;
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Json Properties
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("modified")]
        public bool Modified { get; set; }
        /// 
        [JsonProperty("cnapplprop0entitykey")]
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
        [JsonProperty("cnapplprop0keyval")]
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
        [JsonProperty("cnapplprop0subsys")]
        public string SubSys { get; set; }
        /// 
        [JsonProperty("cnapplprop0prkeyval")]
        public string ParentKeyVal { get; set; }
        /// 
        [JsonProperty("cnapplprop0sysid")]
        public string SystemID { get; set; }
        /// 
        [JsonProperty("cnapplprop0historic")]
        public string Historic { get; set; }
        /// 
        [JsonProperty("cnappllog")]
        public List<SRiCNApplLog> Logs
        {
            get
            {
                if (m_oLogs == null)
                    m_oLogs = new List<SRiCNApplLog>();
                return m_oLogs;
            }
            set { m_oLogs = value; }
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
    /// Name			SRiCNApplPropMeta
    /// 
    /// <summary>		The wraper json object for the SRiCNApplProp.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public class SRiCNApplPropMeta
    {
        /// ------------------------------------------------------------------------------------------------
        #region Json Properties
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("id")]
        public string ID { get; set; }
        /// 
        [JsonProperty("organisation")]
        public string Organisation { get; set; }
        /// 
        [JsonProperty("received")]
        public string Received { get; set; }
        /// 
        [JsonProperty("version")]
        public string Version { get; set; }
        /// 
        [JsonIgnore]
        public SRiCNApplProp History { get; set; }
        /// 
        [JsonProperty("data")]
        public string HistoryData
        {
            get { return History.ToJson(); }
            set
            {
                Exception error;
                //
                History = SRiCNApplProp.FromJson(value, out error);
                if (error != null) throw error;
            }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    /// ----------------------------------------------------------------------------------------------------
    /// Name			SRiCNApplPropData
    /// 
    /// <summary>		The outer wraper json object for all property history data.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public class SRiCNApplPropData : OnSiteJsonEntity<SRiCNApplPropData>
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SRiCNApplPropData
        /// 
        /// <summary>	Creates a new instance of the SRiCNApplPropData class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public SRiCNApplPropData()
        {
            History = new List<SRiCNApplPropMeta>();
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Json Properties
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("module")]
        public string Module { get; set; }
        /// 
        [JsonProperty("appModule")]
        public string AppModule { get; set; }
        /// 
        [JsonProperty("entity")]
        public string Entity { get; set; }
        /// 
        [JsonProperty("cases")]
        public List<SRiCNApplPropMeta> History { get; set; }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		MergeOld
        /// 
        /// <summary>	Merges the old collection of history into the current collection. The only history 
        /// 			that are added are those that do not exist in the current collection. Match is
        /// 			done on EntityKey.
        /// </summary>
        /// <param name="data">		The old data.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void MergeOld(SRiCNApplPropData data, bool addIfNotThere)
        {
            bool exists;
            //
            foreach (var old in data.History)
            {
                exists = false;
                foreach (var i in History)
                    if (i.History.EntityKey.Equals(old.History.EntityKey))
                    {
                        exists = true;
                        //i.History.Status = old.History.Status;
                        break;
                    }
                //
                if (addIfNotThere && !exists)
                    History.Add(old);
            }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    #endregion
    /// ----------------------------------------------------------------------------------------------------
    #region Additional Entities
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public class SRiCNApplLog
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
        public bool Modified { get; set; }
        /// 
        [JsonProperty("cnappllog0keyval")]
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
        [JsonProperty("cnappllog0u_version")]
        public string UVersion { get; set; }
        /// 
        [JsonProperty("cnappllog0subsys")]
        public string SubSys { get; set; }
        /// 
        [JsonProperty("cnappllog0prkeyval")]
        public string ParentKeyVal { get; set; }
        /// 
        [JsonProperty("cnappllog0refval")]
        public string RefVal { get; set; }
        /// 
        [JsonProperty("cnappllog0descr")]
        public string Description { get; set; }
        /// 
        [JsonProperty("cnappllog0status")]
        public string Status { get; set; }
        /// 
        [JsonProperty("cnappllog0opendd")]
        public DateTime? OpenedDate { get; set; }
        /// 
        [JsonProperty("cnappllog0closedd")]
        public DateTime? ClosedDate { get; set; }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string SystemID { get; set; }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    #endregion
    /// ----------------------------------------------------------------------------------------------------
}
