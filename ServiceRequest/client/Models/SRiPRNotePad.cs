using System;
using System.Collections.Generic;
using Newtonsoft.Json;

using Idox.LGDP.Apps.Common.OnSite;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
    /// ----------------------------------------------------------------------------------------------------
    /// Name			SRiPRNotePad
    /// 
    /// <summary>		The Property Notes entity for Service Request, represents the PRNOTEPAD object.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public class SRiPRNotePad : OnSiteJsonEntity<SRiPRNotePad>
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables
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
        /// ------------------------------------------------------------------------------------------------
        /// Name		Modified
        /// 
        /// <summary>	Gets and sets the Modified for the property notes.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("modified")]
        public bool Modified
        {
            get;
            set;
        }
        /// 
        [JsonProperty("prnotepad0entitykey")]
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
        /// ------------------------------------------------------------------------------------------------
        /// Name		KeyVal
        /// 
        /// <summary>	Gets and sets the KeyVal for the property notes.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("prnotepad0keyval")]
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
        /// ------------------------------------------------------------------------------------------------
        /// Name		UVersion
        /// 
        /// <summary>	Gets and sets the UVersion for the property notes.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("prnotepad0u_version")]
        public string UVersion
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Summary
        /// 
        /// <summary>	Gets and sets the Summary for the property notes.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("prnotepad0summary")]
        public string Summary
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ParentKeyVal
        /// 
        /// <summary>	Gets and sets the ParentKeyVal for the property notes.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("prnotepad0prkeyval")]
        public string ParentKeyVal
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Text
        /// 
        /// <summary>	Gets and sets the Text for the property notes.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("prnotepad0notetext")]
        public string Text
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Confidential
        /// 
        /// <summary>	Gets and sets the Confidential for the property notes.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("prnotepad0confidential")]
        public string Confidential
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Priority
        /// 
        /// <summary>	Gets and sets the Priority for the property notes.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("prnotepad0priority")]
        public string Priority
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
    /// Name			SRiPRNotePadMeta
    /// 
    /// <summary>		The wraper json object for the SRiPRNotePad.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public class SRiPRNotePadMeta
    {
        /// ------------------------------------------------------------------------------------------------
        #region Json Properties
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ID
        /// 
        /// <summary>	Gets and sets the ID for the property notes meta.
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
        /// <summary>	Gets and sets the Organisation for the property notes meta.
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
        /// <summary>	Gets and sets the Received for the property notes meta.
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
        /// <summary>	Gets and sets the Version for the property notes meta.
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
        /// Name		PropertyNotes
        /// 
        /// <summary>	Gets and sets the PropertyNotes for the property notes meta.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public SRiPRNotePad PropertyNotes
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		PropertyNotesData
        /// 
        /// <summary>	Gets and sets the PropertyNotesData for the property notes meta.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("data")]
        public string PropertyNotesData
        {
            get { return PropertyNotes.ToJson(); }
            set
            {
                Exception error;
                //
                PropertyNotes = SRiPRNotePad.FromJson(value, out error);
                if (error != null) throw error;
            }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    /// ----------------------------------------------------------------------------------------------------
    /// Name			SRiPRNotePadData
    /// 
    /// <summary>		The outer wraper json object for all property notes data.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public class SRiPRNotePadData : OnSiteJsonEntity<SRiPRNotePadData>
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SRiPRNotePadData
        /// 
        /// <summary>	Creates a new instance of the SRiPRNotePadData class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public SRiPRNotePadData()
        {
            Notes = new List<SRiPRNotePadMeta>();
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
        /// <summary>	Gets and sets the Module for the property notes data.
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
        /// <summary>	Gets and sets the AppModule for the property notes data.
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
        /// <summary>	Gets and sets the Entity for the property notes data.
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
        /// Name		Notes
        /// 
        /// <summary>	Gets and sets the Notes for the property notes data.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("cases")]
        public List<SRiPRNotePadMeta> Notes
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
        /// <summary>	Merges the old collection of notes into the current collection. The only notes 
        /// 			that are added are those that do not exist in the current collection. Match is
        /// 			done on EntityKey.
        /// </summary>
        /// <param name="data">		The old data.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void MergeOld(SRiPRNotePadData data, bool addIfNotThere)
        {
            bool exists;
            //
            foreach (var old in data.Notes)
            {
                exists = false;
                foreach (var i in Notes)
                    if (i.PropertyNotes.EntityKey.Equals(old.PropertyNotes.EntityKey))
                    {
                        exists = true;
                        //i.PropertyNotes.Status = old.PropertyNotes.Status;
                        break;
                    }
                //
                if (addIfNotThere && !exists)
                    Notes.Add(old);
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
