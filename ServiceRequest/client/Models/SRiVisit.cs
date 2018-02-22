using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Idox.LGDP.Apps.Common.OnSite;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
    /// ----------------------------------------------------------------------------------------------------
    /// Name			SRiVisit
    /// 
    /// <summary>		The Visit entity for Service Request, represents the XIVISIT object.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public class SRiVisit : OnSiteJsonEntity<SRiVisit>, IEditableEntity
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables
        /// ------------------------------------------------------------------------------------------------
        /// 
        private List<SRiAction> m_oActions;
        private List<SRiTreatment> m_oTreatments;
        private SRiVisitComment m_oComment;
        private string m_sKeyVal;
        private string m_sEntityKey;
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
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
        [JsonProperty("modified")]
        public bool Modified
        {
            get;
            set;
        }
        public bool ShouldSerializeModified() { return OnSiteSettings.ShouldSerialize != ShouldSerializeRule.ModifiedDiff; }
        /// 
        [JsonProperty("xivisit0entitykey")]
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
        /// <summary>	Gets and sets the KeyVal for the visit.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("xivisit0keyval")]
        public string KeyVal
        {
            get
            {
                if (OnSiteSettings.ShouldSerialize == ShouldSerializeRule.DataForAPI &&
                    OnSiteSettings.KeyIsTemp(m_sKeyVal))
                    return "";
                else
                    return m_sKeyVal;
            }
            set { m_sKeyVal = value; }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		UVersion
        /// 
        /// <summary>	Gets and sets the UVersion for the visit.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("xivisit0u_version")]
        public string UVersion
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		MDKeyVal
        /// 
        /// <summary>	Gets and sets the MDKeyVal for the visit.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("xivisit0mdkeyval")]
        public string MDKeyVal
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		MDSubSys
        /// 
        /// <summary>	Gets and sets the MDSubSys for the visit.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("xivisit0mdsubsys")]
        public string MDSubSys
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		RecordEntityKey
        /// 
        /// <summary>	Gets and sets the RecordEntityKey for the visit.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("srrec0entitykey")]
        public string RecordEntityKey
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		InspectionKeyVal
        /// 
        /// <summary>	Gets and sets the InspectionKeyVal for the visit.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("xirec0keyval")]
        public string InspectionKeyVal
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Officer
        /// 
        /// <summary>	Gets and sets the Officer for the visit.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("xivisit0voff")]
        public string Officer
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		IdoxID
        /// 
        /// <summary>	Gets and sets the IdoxID for the visit.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("xivisit0idoxid")]
        public string IdoxID
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		VisitType
        /// 
        /// <summary>	Gets and sets the VisitType for the visit.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("xivisit0xivtype")]
        public string VisitType
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		DateScheduled
        /// 
        /// <summary>	Gets and sets the DateScheduled for the visit.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("xivisit0datesched")]
        public DateTime? DateScheduled
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		DateVisit
        /// 
        /// <summary>	Gets and sets the DateVisit for the visit.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("xivisit0datevisit")]
        public DateTime? DateVisit
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Comments
        /// 
        /// <summary>	Gets and sets the Comments for the visit.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("xivisit0comments")]
        public string Comments
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Hours
        /// 
        /// <summary>	Gets and sets the Hours for the visit.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("xivisit0vhours")]
        public float? Hours
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Miles
        /// 
        /// <summary>	Gets and sets the Miles for the visit.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("xivisit0vmiles")]
        public float? Miles
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		VehicleType
        /// 
        /// <summary>	Gets and sets the VehicleType for the visit.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("xivisit0vehtype")]
        public string VehicleType
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		VisitComments
        /// 
        /// <summary>	Gets and sets the VisitComments for the visit.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("xivisit_comments")]
        public SRiVisitComment VisitComments
        {
            get
            {
                if (m_oComment == null)
                {
                    OnSiteSettings.ShouldSerialize = ShouldSerializeRule.DataForAPI;
                    m_oComment = new SRiVisitComment()
                    {
                        VisitKeyVal = KeyVal,
                        InspectionKeyVal = InspectionKeyVal,
                        Modified = true,
                        Comments = "",
                        UVersion = ""
                    };
                }
                return m_oComment;
            }
            set { m_oComment = value; }
        }
        public bool ShouldSerializeVisitComments() { return OnSiteSettings.ShouldSerialize != ShouldSerializeRule.ModifiedDiff; }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Treatments
        /// 
        /// <summary>	Gets and sets the Treatments for the visit.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("xipcwork")]
        public List<SRiTreatment> Treatments
        {
            get
            {
                if (m_oTreatments == null)
                    m_oTreatments = new List<SRiTreatment>();
                return m_oTreatments;
            }
            set { m_oTreatments = value; }
        }
        public bool ShouldSerializeTreatments() { return OnSiteSettings.ShouldSerialize != ShouldSerializeRule.ModifiedDiff; }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Actions
        /// 
        /// <summary>	Gets and sets the Actions for the visit.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("xiaction")]
        public List<SRiAction> Actions
        {
            get
            {
                if (m_oActions == null)
                    m_oActions = new List<SRiAction>();
                return m_oActions;
            }
            set { m_oActions = value; }
        }
        public bool ShouldSerializeActions() { return OnSiteSettings.ShouldSerialize != ShouldSerializeRule.ModifiedDiff; }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Status
        /// 
        /// <summary>	Gets and sets the Status for the visit.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("status")]
        public SyncStatus Status
        {
            get;
            set;
        }
        public bool ShouldSerializeStatus()
        {
            return OnSiteSettings.ShouldSerialize != ShouldSerializeRule.DataForAPI &&
                   OnSiteSettings.ShouldSerialize != ShouldSerializeRule.ModifiedDiff;
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Actions
        /// 
        /// <summary>	Gets and sets the Actions for the visit.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void Modify()
        {
            Modified = true;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Notes
        /// 
        /// <summary>	Gets and sets the Notes for the visit.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string Notes
        {
            get { return VisitComments.Comments; }
            set
            {
                VisitComments.Comments = value;
                VisitComments.Modified = true;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name        UpdateVersions
        /// 
        /// <summary>   Updates the UVersion to the UVersion supplied SRiVisit.
        /// </summary>
        /// <param name="visit">        The visit.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void UpdateVersions(SRiVisit visit)
        {
            UVersion = visit.UVersion;
            VisitComments.UVersion = visit.VisitComments.UVersion;
            foreach (SRiTreatment treatment in Treatments)
                treatment.UpdateVersions(visit.Treatments);
            foreach (SRiAction action in Actions)
                action.UpdateVersions(visit.Actions);
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    /// ----------------------------------------------------------------------------------------------------
    #region Meta & Data
    /// ----------------------------------------------------------------------------------------------------
    /// 
    /// ----------------------------------------------------------------------------------------------------
    /// Name			SRiVisitMeta
    /// 
    /// <summary>		The wraper json object for the SRiVisit.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public class SRiVisitMeta : OnSiteJsonEntity<SRiVisitMeta>, IEditableEntity
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables
        /// ------------------------------------------------------------------------------------------------
        /// 
        private string m_sReceived;
        private string m_sVersion;
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Json Properties
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ID
        /// 
        /// <summary>	Gets and sets the ID for the visit meta.
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
        /// <summary>	Gets and sets the Organisation for the visit meta.
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
        /// <summary>	Gets and sets the Received for the visit meta.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("received")]
        public string Received
        {
            get
            {
                if (OnSiteSettings.ShouldSerialize == ShouldSerializeRule.DataForAPI && m_sReceived.Equals("0"))
                    return "1";
                else
                    return m_sReceived;
            }
            set { m_sReceived = value; }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Status
        /// 
        /// <summary>	Gets and sets the Status of the entity.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("status")]
        public SyncStatus Status
        {
            get;
            set;
        }
        public bool ShouldSerializeStatus() { return OnSiteSettings.ShouldSerialize != ShouldSerializeRule.DataForAPI; }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Version
        /// 
        /// <summary>	Gets and sets the Version for the visit meta.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("version")]
        public string Version
        {
            get
            {
                if (OnSiteSettings.ShouldSerialize == ShouldSerializeRule.DataForAPI && m_sVersion == null)
                    return "";
                else
                    return m_sVersion;
            }
            set { m_sVersion = value; }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		QueryState
        /// 
        /// <summary>	Gets and sets the QueryState for the visit meta.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("querystate")]
        public string QueryState
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		EntityMeta
        /// 
        /// <summary>	Gets and sets the EntityMeta for the visit meta.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("metas")]
        public SRiEntityMeta EntityMeta
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Visit
        /// 
        /// <summary>	Gets and sets the Visit for the visit meta.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public SRiVisit Visit
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		VisitData
        /// 
        /// <summary>	Gets and sets the VisitData for the visit meta.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("data")]
        public string VisitData
        {
            get { return Visit.ToJson(); }
            set
            {
                Exception error;
                //
                Visit = SRiVisit.FromJson(value, out error);
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
        /// Name        IsDifferentTo
        /// 
        /// <summary>   Checks if the supplied visit is different to the current instance. The field 
        ///             compared are just the top level properties from the SRiVisit object.
        /// </summary>
        /// <param name="visitMeta">        The visit to compare with.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public bool IsDifferentTo(SRiVisitMeta visitMeta)
        {
            bool result;
            //
            OnSiteSettings.ShouldSerialize = ShouldSerializeRule.ModifiedDiff;
            result = !visitMeta.Visit.ToJson().Equals(Visit.ToJson());
            return result;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Modify
        /// 
        /// <summary>	Sets the entity as modified.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void Modify()
        {
            Visit.Modified = true;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		VehicleTypeDescription
        /// 
        /// <summary>	Gets the code description for the VehicleType code.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string VehicleTypeDescription
        {
            get
            {
                if (string.IsNullOrEmpty(Visit.VehicleType))
                    return "No vehicle type";
                else
                    return AppData.ConfigModel.CodeDescription(Organisation, "VEHTYPE", Visit.VehicleType);
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		VisitTypeDescription
        /// 
        /// <summary>	Gets the VisitTypeDescription for the visit.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string VisitTypeDescription
        {
            get
            {
                if (string.IsNullOrEmpty(Visit.VisitType))
                    return "No visit type";
                else
                    return AppData.ConfigModel.CodeDescription(Organisation, "XIVTYPE", Visit.VisitType);
            }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    /// ----------------------------------------------------------------------------------------------------
    /// Name			SRiVisitData
    /// 
    /// <summary>		The outer wraper json object for all visit data.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public class SRiVisitData : OnSiteJsonEntity<SRiVisitData>
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables
        /// ------------------------------------------------------------------------------------------------
        /// 
        private List<SRiVisitMeta> m_oVisits;
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SRiVisitData
        /// 
        /// <summary>	Creates a new instance of the SRiVisitData class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public SRiVisitData()
        {
            Visits = new List<SRiVisitMeta>();
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
        /// <summary>	Gets and sets the Module for the visit data.
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
        /// <summary>	Gets and sets the AppModule for the visit data.
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
        /// <summary>	Gets and sets the Entity for the visit data.
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
        /// Name		Visits
        /// 
        /// <summary>	Gets and sets the Visits for the visit data.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("cases")]
        public List<SRiVisitMeta> Visits
        {
            get
            {
                if (m_oVisits == null)
                    m_oVisits = new List<SRiVisitMeta>();
                return m_oVisits;
            }
            set { m_oVisits = value; }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		EntityKeys
        /// 
        /// <summary>	Gets the collection of EntityKeys for the visits.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string[] EntityKeys
        {
            get
            {
                List<string> keys;
                //
                keys = new List<string>();
                foreach (var v in Visits)
                    keys.Add(v.Visit.EntityKey);
                //
                return keys.ToArray();
            }
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		MergeOld
        /// <summary>	Merges the old collection of visits into the current collection. The only visits 
        /// 			that are added are those that do not exist in the current collection. Match is
        /// 			done on EntityKey.
        /// </summary>
        /// <param name="data">		The old data.</param>
        /// <param name="addIfNotThere"></param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        public void MergeOld(SRiVisitData data, bool addIfNotThere)
        {
            bool exists;
            //
            foreach (var old in data.Visits)
            {
                exists = false;
                foreach (var v in Visits)
                    if (old.Visit.EntityKey.Equals(v.Visit.EntityKey))
                    {
                        exists = true;
                        v.Status = old.Status;
                        break;
                    }
                //
                if (addIfNotThere && !exists)
                    Visits.Add(old);
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		RecordEntityKeys
        /// 
        /// <summary>	Returns all the associated SRREC entity keys referenced in each of the Visits.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string[] RecordEntityKeys
        {
            get
            {
                List<string> entityKeys;
                //
                entityKeys = new List<string>();
                foreach (var v in m_oVisits)
                    if (!entityKeys.Contains(v.Visit.RecordEntityKey))
                        entityKeys.Add(v.Visit.RecordEntityKey);
                return entityKeys.ToArray();
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
    public class SRiVisitComment
    {
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
        [JsonProperty("xirec0keyval")]
        public string InspectionKeyVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xivisit_comments0u_version")]
        public string UVersion
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xivisit0keyval")]
        public string VisitKeyVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xivisit_comments0comments")]
        public string Comments
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xivisit_comments0createdd")]
        public DateTime? CreatedDate
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xivisit_comments0updatedd")]
        public DateTime? UpdatedDate
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xivisit_comments0updatedby")]
        public string UpdatedBy
        {
            get;
            set;
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    public class SRiTreatment : OnSiteJsonEntity<SRiTreatment>, IEditableEntity
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
        [JsonProperty("version")]
        public string Version
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xipcwork0keyval")]
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
        [JsonProperty("xivisit0keyval")]
        public string VisitKeyVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xipcwork0u_version")]
        public string UVersion
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xirec0keyval")]
        public string InspectionKeyVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xipcwork0pccat")]
        public string Category
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xipcwork0treatt")]
        public string TreatmentType
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xipcwork0pesttype")]
        public string PestType
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xipcwork0location")]
        public string Location
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xipcwork0numbaits")]
        public int? NumberOfBaits
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xipcwork0numtakes")]
        public int? NumberOfTakes
        {
            get;
            set;
        }
        /// 
        [JsonProperty("status")]
        public SyncStatus Status
        {
            get;
            set;
        }
        public bool ShouldSerializeStatus() { return OnSiteSettings.ShouldSerialize != ShouldSerializeRule.DataForAPI; }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Modify
        /// 
        /// <summary>	Sets the entity as modified.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void Modify()
        {
            // TODO
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		PestTypeDescription
        /// 
        /// <summary>	Gets the code description for the PestType code.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string PestTypeDescription
        {
            get
            {
                if (string.IsNullOrEmpty(PestType))
                    return "No pest type";
                else if (AppData.PropertyModel.SelectedVisit != null)
                    return AppData.ConfigModel.CodeDescription(AppData.PropertyModel.SelectedVisit.Visit.Organisation,
                                                               "PESTTYPE",
                                                               PestType);
                else
                    return PestType;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		TreatmentTypeDescription
        /// 
        /// <summary>	Gets the code description for the TreatmentType code.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string TreatmentTypeDescription
        {
            get
            {
                if (string.IsNullOrEmpty(TreatmentType))
                    return "No treatment type";
                else if (AppData.PropertyModel.SelectedVisit != null)
                    return AppData.ConfigModel.CodeDescription(AppData.PropertyModel.SelectedVisit.Visit.Organisation,
                                                               "TREATT",
                                                               TreatmentType);
                else
                    return TreatmentType;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name        UpdateVersions
        /// 
        /// <summary>   Updates the UVersion to the UVersion of the matching object from the collection.
        /// </summary>
        /// <param name="treatments">        The pests.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void UpdateVersions(List<SRiTreatment> treatments)
        {
            foreach (SRiTreatment treatment in treatments)
                if (treatment.KeyVal.Equals(KeyVal))
                {
                    UVersion = treatment.UVersion;
                    break;
                }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    public class SRiAction : OnSiteJsonEntity<SRiAction>, IEditableEntity
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
        [JsonProperty("xiaction0keyval")]
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
        [JsonProperty("xivisit0keyval")]
        public string VisitKeyVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xiaction0u_version")]
        public string UVersion
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xiaction0mdkeyval")]
        public string MDKeyVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xiaction0mdsubsys")]
        public string MDSubSys
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xiaction0duedate")]
        public DateTime? DueDate
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xiaction0actdate")]
        public DateTime? ActualDate
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xiaction0acttime")]
        public string ActualTime
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xiaction0ahours")]
        public float? Hours
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xiaction0xiact")]
        public string ActionType
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xiaction0xileg")]
        public string LegislationType
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xiaction0actoff")]
        public string Officer
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xiactpara")]
        public List<SRiActionParagraph> Paragraphs
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xiactcomm")]
        public List<SRiActionComment> Comments
        {
            get;
            set;
        }
        /// 
        [JsonProperty("status")]
        public SyncStatus Status
        {
            get;
            set;
        }
        public bool ShouldSerializeStatus() { return OnSiteSettings.ShouldSerialize != ShouldSerializeRule.DataForAPI; }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ActualDateTime
        /// 
        /// <summary>	Combines the ActuualDate and ActualTime values into a single property 
        /// 			for use in the UI.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public DateTime? ActualDateTime
        {
            get
            {
                DateTime dt;
                if (ActualDate.HasValue)
                {
                    if (!string.IsNullOrEmpty(ActualTime) &&
                        DateTime.TryParseExact(string.Format("{0} {1}", ActualDate.Value.ToString("dd MM yyyy"), ActualTime),
                                               "dd MM yyyy HH:mm:ss",
                                               CultureInfo.InvariantCulture,
                                               DateTimeStyles.None, out dt))
                        return dt;
                    return ActualDate.Value;
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    ActualDate = value.Value.Date;
                    ActualTime = value.Value.ToString("HH:mm:ss");
                }
                else
                {
                    ActualDate = null;
                    ActualTime = null;
                }
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ActionTypeDescription
        /// 
        /// <summary>	Gets the code description for the ActionType code.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string ActionTypeDescription
        {
            get
            {
                if (string.IsNullOrEmpty(ActionType))
                    return "No action type";
                else if (AppData.PropertyModel.SelectedVisit != null)
                    return AppData.ConfigModel.CodeDescription(AppData.PropertyModel.SelectedVisit.Visit.Organisation,
                                                               string.Format("ACT_{0}", AppData.PropertyModel.SelectedVisit.Visit.Visit.VisitType),
                                                               ActionType);
                else
                    return ActionType;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		LegislationTypeDescription
        /// 
        /// <summary>	Gets the code description for the LegislationType code.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string LegislationTypeDescription
        {
            get
            {
                if (string.IsNullOrEmpty(LegislationType))
                    return "No legislation type";
                else if (AppData.PropertyModel.SelectedVisit != null)
                    return AppData.ConfigModel.CodeDescription(AppData.PropertyModel.SelectedVisit.Visit.Organisation,
                                                               string.Format("LEG_{0}", AppData.PropertyModel.SelectedVisit.Visit.Visit.VisitType),
                                                               LegislationType);
                else
                    return LegislationType;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Modify
        /// 
        /// <summary>	Sets the modified flag for the entity.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void Modify()
        {
            Modified = true;
        }
        /// 
        [JsonIgnore]
        public string OfficerDescription
        {
            get
            {
                if (string.IsNullOrEmpty(Officer))
                    return "No officer set";
                else if (AppData.PropertyModel.SelectedVisit != null)
                    return AppData.ConfigModel.CodeDescription(AppData.PropertyModel.SelectedVisit.Visit.Organisation, "", Officer);
                else
                    return Officer;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name        UpdateVersions
        /// 
        /// <summary>   Updates the UVersion to the UVersion of the matching object from the collection.
        /// </summary>
        /// <param name="actions">        The actions.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void UpdateVersions(List<SRiAction> actions)
        {
            foreach (SRiAction action in actions)
                if (action.KeyVal.Equals(KeyVal))
                {
                    UVersion = action.UVersion;
                    foreach (SRiActionParagraph para in Paragraphs)
                        para.UpdateVersions(action.Paragraphs);
                    foreach (SRiActionComment comment in Comments)
                        comment.UpdateVersions(action.Comments);
                    break;
                }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    public class SRiActionParagraph
    {
        /// 
        [JsonIgnore]
        private const char INSERT_DELIMITER = '\x1B';
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
        [JsonProperty("version")]
        public string Version
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xiactpara0keyval")]
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
        [JsonProperty("xiaction0keyval")]
        public string ActionKeyVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xiactpara0u_version")]
        public string UVersion
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xiactpara0xipara")]
        public string Paragraph
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xiactpara0paraseq")]
        public int? Sequence
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xiactpara0paratype")]
        public string ParagraphType
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xiactpara0descrip")]
        public string Description
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xiactpara0inserts")]
        public string Inserts
        {
            get
            {
                if (InsertsList == null)
                    return "";
                else
                    return string.Join(INSERT_DELIMITER.ToString(), InsertsList.ToArray());
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    InsertsList = new List<string>();
                else
                    InsertsList = value.Split(INSERT_DELIMITER).ToList();
            }
        }
        /// 
        [JsonIgnore]
        public List<string> InsertsList = new List<string>();
        /// 
        [JsonProperty("cncodepara0paratext")]
        public string Text
        {
            get;
            set;
        }

        [JsonIgnore]
        public string PlainText
        {
            get { return Text.ToRTF2PlainText(); }
        }

        /// 
        [JsonProperty("cncodepara0u_version")]
        public string ParagraphCodeUVersion
        {
            get;
            set;
        }
        /// 
        [JsonProperty("cncodepara0inserts")]
        public string ParagraphCodeInserts
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		CellType
        /// 
        /// <summary>	Attempts to get the paragraph Type.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public CellTypes CellType { get; set; }

        /// 
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		IsEditable
        /// 
        /// <summary>	To Check whether Cell is editable or not.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public bool IsEditable { get; set; }

        /// 
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		IsCellEditable
        /// 
        /// <summary>	To Check whether Cell is editable or not.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public bool IsCellEditable { get; set; }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ParagraphTypeDescription
        /// 
        /// <summary>	Gets the code description for the ParagraphType code.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string ParagraphTypeDescription
        {
            get
            {
                if (string.IsNullOrEmpty(ParagraphType))
                    return "No paragraph type";
                else if (AppData.PropertyModel.SelectedVisit != null)
                    return AppData.ConfigModel.CodeDescription(AppData.PropertyModel.SelectedVisit.Visit.Organisation,
                                                               "XIPARA",
                                                               ParagraphType);
                else
                    return ParagraphType;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name        UpdateVersions
        /// 
        /// <summary>   Updates the UVersion to the UVersion of the matching object from the collection.
        /// </summary>
        /// <param name="paras">        The paragraphs.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void UpdateVersions(List<SRiActionParagraph> paras)
        {
            foreach (SRiActionParagraph para in paras)
                if (para.KeyVal.Equals(KeyVal))
                {
                    UVersion = para.UVersion;
                    break;
                }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    public class SRiActionComment
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
        [JsonProperty("xiactcomm0keyval")]
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
        [JsonProperty("xiaction0keyval")]
        public string ActionKeyVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xivisit0u_version")]
        public string UVersion
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xirec0keyval")]
        public string InspectionKeyVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xiactcomm0createdd")]
        public DateTime? CreatedDate
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xiactcomm0res_comment")]
        public string ResComment
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xiactcomm0comments")]
        public string Comments
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xiactcomm0updatedby")]
        public string UpdatedBy
        {
            get;
            set;
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name        UpdateVersions
        /// 
        /// <summary>   Updates the UVersion to the UVersion of the matching object from the collection.
        /// </summary>
        /// <param name="comments">        The action comments.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void UpdateVersions(List<SRiActionComment> comments)
        {
            foreach (SRiActionComment comment in comments)
                if (comment.KeyVal.Equals(KeyVal))
                {
                    UVersion = comment.UVersion;
                    break;
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
