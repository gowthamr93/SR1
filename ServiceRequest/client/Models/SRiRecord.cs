using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

using Idox.LGDP.Apps.Common.OnSite;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
    /// ----------------------------------------------------------------------------------------------------
    /// Name			SRiRecord
    /// 
    /// <summary>		The Complainant Record entity for Service Request, represents the SRREC object.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public class SRiRecord : OnSiteJsonEntity<SRiRecord>
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables
        /// ------------------------------------------------------------------------------------------------
        /// 
        private List<SRiPCLocation> m_oPestControls;
        private List<SRiCustomer> m_oCustomers;
        private List<SRiInspection> m_oInspections;
        private List<SRiPRNotePadMeta> m_oNotes;
        private List<SRiCNApplPropMeta> m_oHistory;
        /// 
        private Dictionary<string, string> m_oBasicDetails;
        private Dictionary<string, string> m_oPestDetails;
        private DateTime? m_oReceived;
        private DateTime? m_oTargetResponse;
        private string m_sKeyVal;
        private string m_sEntityKey;
        /// 
        private List<string> m_oHistoryKeys;
        private List<string> m_oNotesKeys;
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
        /// 
        [JsonProperty("srrec0entitykey")]
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
        /// <summary>	Gets and sets the KeyVal for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("srrec0keyval")]
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
        /// Name		RefVal
        /// 
        /// <summary>	Gets and sets the RefVal for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("srrec0refval")]
        public string RefVal
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		UVersion
        /// 
        /// <summary>	Gets and sets the UVersion for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("srrec0u_version")]
        public string UVersion
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ParentKeyVal
        /// 
        /// <summary>	Gets and sets the ParentKeyVal for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("srrec0prkeyval")]
        public string ParentKeyVal
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Name
        /// 
        /// <summary>	Gets and sets the Name for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("srrec0name")]
        public string Name
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		TradeName
        /// 
        /// <summary>	Gets and sets the TradeName for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("srrec0tradename")]
        public string TradeName
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Address
        /// 
        /// <summary>	Gets and sets the Address for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("srrec0address")]
        public string Address
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Contact
        /// 
        /// <summary>	Gets and sets the Contact for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("srrec0contact")]
        public string Contact
        {
            get { return GetBasicDetail("Contact"); }
            set { SetBasicDetail("Contact", value); }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		MapEast
        /// 
        /// <summary>	Gets and sets the MapEast for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("srrec0mapeast")]
        public string MapEast
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		MapNorth
        /// 
        /// <summary>	Gets and sets the MapNorth for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("srrec0mapnorth")]
        public string MapNorth
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Latitude
        /// 
        /// <summary>	Gets and sets the Latitude for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("srrec0latitude")]
        public double? Latitude
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Longitude
        /// 
        /// <summary>	Gets and sets the Longitude for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("srrec0longitude")]
        public double? Longitude
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		UPRN
        /// 
        /// <summary>	Gets and sets the UPRN for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("srrec0uprn")]
        public string UPRN
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Received
        /// 
        /// <summary>	Gets and sets the Received for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("srrec0recepd")]
        public DateTime? Received
        {
            get { return m_oReceived; }
            set
            {
                m_oReceived = value;
                SetBasicDetail("Received", value.ToDateString());
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		TargetResponseDate
        /// 
        /// <summary>	Gets and sets the TargetResponseDate for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("srrec0targrespd")]
        public DateTime? TargetResponseDate
        {
            get { return m_oTargetResponse; }
            set
            {
                m_oTargetResponse = value;
                SetBasicDetail("Target Response", value.ToDateString());
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Details
        /// 
        /// <summary>	Gets and sets the Details for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("srrec0details")]
        public string Details
        {
            get { return GetBasicDetail("Details"); }
            set { SetBasicDetail("Details", value); }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		RequestType
        /// 
        /// <summary>	Gets and sets the RequestType for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("srrec0srrectype")]
        public string RequestType
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		RequestKind
        /// 
        /// <summary>	Gets and sets the RequestKind for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("srrec0srreckind")]
        public string RequestKind
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		StatusKind
        /// 
        /// <summary>	Gets and sets the StatusKind for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("srrec0srstat")]
        public string StatusKind
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		EventDate
        /// 
        /// <summary>	Gets and sets the EventDate for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("srrec0eventdt")]
        public DateTime? EventDate
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		XBSubSys
        /// 
        /// <summary>	Gets and sets the XBSubSys for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("srrec0xbsubsys")]
        public string XBSubSys
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		XBKeyVal
        /// 
        /// <summary>	Gets and sets the XBKeyVal for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("srrec0xbkeyval")]
        public string XBKeyVal
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Extras
        /// 
        /// <summary>	Gets and sets the Extras for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("srxtra")]
        public List<SRiExtra> Extras
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		PestControls
        /// 
        /// <summary>	Gets and sets the PestControls for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("pclocn")]
        public List<SRiPCLocation> PestControls
        {
            get
            {
                if (m_oPestControls == null)
                    m_oPestControls = new List<SRiPCLocation>();
                return m_oPestControls;
            }
            set
            {
                m_oPestControls = value;
                foreach (var p in m_oPestControls)
                    PestDetails.Add(p.PestControlType, p.Location);
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Customers
        /// 
        /// <summary>	Gets and sets the Customers for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("srcustomer")]
        public List<SRiCustomer> Customers
        {
            get
            {
                if (m_oCustomers == null)
                    m_oCustomers = new List<SRiCustomer>();
                return m_oCustomers;
            }
            set { m_oCustomers = value; }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Inspections
        /// 
        /// <summary>	Gets and sets the Inspections for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("xirec")]
        public List<SRiInspection> Inspections
        {
            get
            {
                if (m_oInspections == null)
                    m_oInspections = new List<SRiInspection>();
                return m_oInspections;
            }
            set { m_oInspections = value; }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		PRNotePadEntityKeys
        /// 
        /// <summary>	Gets and sets the PRNotePadEntityKeys for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("prnotepad0entitykey")]
        public List<string> PRNotePadEntityKeys
        {
            get
            {
                if (m_oNotesKeys == null)
                    m_oNotesKeys = new List<string>();
                return m_oNotesKeys;
            }
            set { m_oNotesKeys = value; }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		CNApplPropEntityKeys
        /// 
        /// <summary>	Gets and sets the CNApplPropEntityKeys for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("cnapplprop0entitykey")]
        public List<string> CNApplPropEntityKeys
        {
            get
            {
                if (m_oHistoryKeys == null)
                    m_oHistoryKeys = new List<string>();
                return m_oHistoryKeys;
            }
            set { m_oHistoryKeys = value; }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Notes
        /// 
        /// <summary>	Gets and sets the Notes for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public List<SRiPRNotePadMeta> Notes
        {
            get
            {
                if (m_oNotes == null)
                    m_oNotes = new List<SRiPRNotePadMeta>();
                return m_oNotes;
            }
            set { m_oNotes = value; }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		History
        /// 
        /// <summary>	Gets and sets the History for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public List<SRiCNApplPropMeta> History
        {
            get
            {
                if (m_oHistory == null)
                    m_oHistory = new List<SRiCNApplPropMeta>();
                return m_oHistory;
            }
            set { m_oHistory = value; }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Status
        /// 
        /// <summary>	Gets and sets the Status for the record.
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
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		BasicDetails
        /// 
        /// <summary>	Gets the basic details for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public Dictionary<string, string> BasicDetails
        {
            get
            {
                if (m_oBasicDetails == null)
                    m_oBasicDetails = new Dictionary<string, string>();
                return m_oBasicDetails;
            }
            private set { m_oBasicDetails = value; }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		DistanceFromUser
        /// 
        /// <summary>	Gets and sets the DistanceFromUser for the Record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public float DistanceFromUser
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		HasValidCoords
        /// 
        /// <summary>	Checks that the Latitude and Longitude have values.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public bool HasValidCoords
        {
            get { return Latitude.HasValue && Longitude.HasValue; }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		PestDetails
        /// 
        /// <summary>	Gets the pest control details for the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public Dictionary<string, string> PestDetails
        {
            get
            {
                if (m_oPestDetails == null)
                    m_oPestDetails = new Dictionary<string, string>();
                return m_oPestDetails;
            }
            private set { m_oPestDetails = value; }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Modify
        /// 
        /// <summary>	Sets the modified flag for the record.
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
        public void SetSavedStatus(SRiRecord record)
        {
            // TODO
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		UpdateForSelection
        /// 
        /// <summary>	Updates the fields in the record ready for display.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void UpdateForSelection()
        {
            if (Inspections.Count > 0)
                SetBasicDetail("Inspection type", Inspections[0].InspectionTypeDescription);
        }
        /// 
        public void UpdateVersions(SRiRecord record)
        {
            UVersion = record.UVersion;
            foreach (SRiExtra extra in Extras)
                extra.UpdateVersions(record.Extras);
            foreach (SRiPCLocation pest in PestControls)
                pest.UpdateVersions(record.PestControls);
            foreach (SRiCustomer customer in Customers)
                customer.UpdateVersions(record.Customers);
            foreach (SRiInspection inspection in Inspections)
                inspection.UpdateVersions(record.Inspections);
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Private Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetBasicDetail
        /// 
        /// <summary>	Gets an entry from the basic details collection.
        /// </summary>
        /// <param name="key">		The key of the value.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private string GetBasicDetail(string key)
        {
            if (BasicDetails.ContainsKey(key))
                return BasicDetails[key];
            else
                return null;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SetBasicDetail
        /// 
        /// <summary>	Sets a value in the basic details collection.
        /// </summary>
        /// <param name="key">		The key of the value.</param>
        /// <param name="value">	The value to set.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void SetBasicDetail(string key, string value)
        {
            if (BasicDetails.ContainsKey(key))
                BasicDetails[key] = value;
            else
                BasicDetails.Add(key, value);
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
    /// Name			SRiRecordMeta
    /// 
    /// <summary>		The wraper json object for the SRiRecord.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public class SRiRecordMeta : OnSiteJsonEntity<SRiRecordMeta>
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
        /// <summary>	Gets and sets the ID for the record meta.
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
        /// <summary>	Gets and sets the Organisation for the record meta.
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
        /// <summary>	Gets and sets the Received for the record meta.
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
        /// Name		Version
        /// 
        /// <summary>	Gets and sets the Version for the record meta.
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
        /// Name		Record
        /// 
        /// <summary>	Gets and sets the Record for the record meta.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public SRiRecord Record
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		RecordData
        /// 
        /// <summary>	Gets and sets the RecordData for the record meta.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("data")]
        public string RecordData
        {
            get { return Record.ToJson(); }
            set
            {
                Exception error;
                //
                Record = SRiRecord.FromJson(value, out error);
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
        /// Name        MergeInspection
        /// 
        /// <summary>   Merges the inspection contents to an existing inspection, or adds if not existing.
        /// </summary>
        /// <param name="inspection">       The inspection.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void MergeInspection(SRiInspection inspection)
        {
            bool exists;
            //
            exists = false;
            foreach (var i in Record.Inspections)
                if (i.KeyVal.Equals(inspection.KeyVal))
                {
                    exists = true;
                    foreach (var v in inspection.Visits)
                        i.MergeVisit(v);
                    break;
                }
            //
            // This should never happen as the merge is more about making sure new Visits are passed from 
            // the oldRecord to the newRecord (response from record upload), but just in case...
            if (!exists)
                Record.Inspections.Add(inspection);
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		RequestKindDescription
        /// 
        /// <summary>	Gets the code description for the RequestKind code.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string RequestKindDescription
        {
            get
            {
                if (string.IsNullOrEmpty(Record.RequestKind))
                    return "No request kind";
                else
                    return AppData.ConfigModel.CodeDescription(Organisation, "SRKIND", Record.RequestKind);
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		RequestTypeDescription
        /// 
        /// <summary>	Gets the code description for the RequestType code.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string RequestTypeDescription
        {
            get
            {
                if (string.IsNullOrEmpty(Record.RequestType))
                    return "No request type";
                else
                    return AppData.ConfigModel.CodeDescription(Organisation, "SRRECTYPE", Record.RequestType);
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		StatusKindDescription
        /// 
        /// <summary>	Gets the code description for the StatusKind code.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string StatusKindDescription
        {
            get
            {
                if (string.IsNullOrEmpty(Record.StatusKind))
                    return "No status";
                else
                    return AppData.ConfigModel.CodeDescription(Organisation, "SRSTAT", Record.StatusKind);
            }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    /// ----------------------------------------------------------------------------------------------------
    /// Name			SRiRecordData
    /// 
    /// <summary>		The outer wraper json object for all record data.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public class SRiRecordData : OnSiteJsonEntity<SRiRecordData>
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables
        /// ------------------------------------------------------------------------------------------------
        /// 
        private List<SRiRecordMeta> m_oRecords;
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SRiRecordData
        /// 
        /// <summary>	Creates a new instance of the SRiRecordData class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public SRiRecordData()
        {
            Records = new List<SRiRecordMeta>();
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
        /// <summary>	Gets and sets the Module for the record data.
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
        /// <summary>	Gets and sets the AppModule for the record data.
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
        /// <summary>	Gets and sets the Entity for the record data.
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
        /// Name		Records
        /// 
        /// <summary>	Gets and sets the Records for the record data.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("cases")]
        public List<SRiRecordMeta> Records
        {
            get
            {
                if (m_oRecords == null)
                    m_oRecords = new List<SRiRecordMeta>();
                return m_oRecords;
            }
            set { m_oRecords = value; }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		LastSync
        /// 
        /// <summary>	Gets and sets the LastSync for the record data.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("lastSync")]
        public DateTime? LastSync
        {
            get;
            set;
        }
        public bool ShouldSerializeLastSync() { return OnSiteSettings.ShouldSerialize != ShouldSerializeRule.DataForAPI; }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		AddBogusGeoData
        /// 
        /// <summary>	Runs through the records and foreach unique Address a random lat/long 
        /// 			value is generated.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void AddBogusGeoData()
        {
            Dictionary<string, double[]> geos;
            double lat, lon;
            Random rand;
            //
            // The desired bounds of the latitude and longitude.
            // 51.633396 -3.826398
            // 51.639309 -3.816785
            //
            rand = new Random();
            geos = new Dictionary<string, double[]>();
            foreach (var r in Records)
            {
                if (r.Record.Address == null)
                    r.Record.Address = "No address given";
                //
                // Allow original geo data 
                if (r.Record.Latitude != null && r.Record.Longitude != null)
                {
                    if (geos.ContainsKey(r.Record.Address))
                    {
                        geos[r.Record.Address][0] = r.Record.Latitude.Value;
                        geos[r.Record.Address][1] = r.Record.Longitude.Value;
                    }
                    else
                        geos.Add(r.Record.Address, new double[2] { r.Record.Latitude.Value, r.Record.Longitude.Value });
                }
                else if (geos.ContainsKey(r.Record.Address))
                {
                    r.Record.Latitude = geos[r.Record.Address][0];
                    r.Record.Longitude = geos[r.Record.Address][1];
                }
                else
                {
                    lat = 51.633396 + (0.0055 * rand.NextDouble());
                    lon = -3.826398 + (0.01 * rand.NextDouble());
                    r.Record.Latitude = lat;
                    r.Record.Longitude = lon;
                    geos.Add(r.Record.Address, new double[2] { lat, lon });
                }
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		AddHistory
        /// 
        /// <summary>	Adds all the history from the history meta list to the correct record.
        /// </summary>
        /// <param name="history">		The history meta list.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void AddHistory(List<SRiCNApplPropMeta> history)
        {
            foreach (var h in history)
                foreach (var r in Records)
                    if (r.Record.CNApplPropEntityKeys.Contains(h.History.EntityKey))
                    {
                        r.Record.History.Add(h);
                        break;
                    }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		AddNotes
        /// 
        /// <summary>	Adds all the notes from the notes meta list to the correct record.
        /// </summary>
        /// <param name="notes">		The notes meta list.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void AddNotes(List<SRiPRNotePadMeta> notes)
        {
            foreach (var n in notes)
                foreach (var r in Records)
                    if (r.Record.PRNotePadEntityKeys.Contains(n.PropertyNotes.EntityKey))
                    {
                        r.Record.Notes.Add(n);
                        break;
                    }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		AddVisits
        /// 
        /// <summary>	Adds all the visits from the visit meta list to the correct inspection in the
        /// 			correct record.
        /// </summary>
        /// <param name="visits">		The visit meta list.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void AddVisits(List<SRiVisitMeta> visits)
        {
            int unmatched;
            //
            unmatched = visits.Count;
            foreach (var v in visits)
                foreach (var r in Records)
                    if (v.Visit.RecordEntityKey.Equals(r.Record.EntityKey))
                    {
                        foreach (var i in r.Record.Inspections)
                            if (v.Visit.InspectionKeyVal.Equals(i.KeyVal))
                            {
                                i.Visits.Add(v);
                                unmatched--;
                                break;
                            }
                        break;
                    }
            //
            if (unmatched > 0)
                throw new Exception("Invalid data references. Unable to match all XIVISIT entities to XIRECs.");
        }
        /// 
        public List<SRiProperty> CreateProperties(SRiCPInfoData cpinfoData,
                                                  SRiLICaseData licaseData,
                                                  SRiCNApplPropData historyData,
                                                  SRiPRNotePadData notesData)
        {
            List<SRiProperty> properties;
            bool recordAdded;
            // 
            properties = new List<SRiProperty>();
            foreach (var r in Records)
            {
                recordAdded = false;
                foreach (var p in properties)
                    if (p.Matches(r))
                    {
                        p.AddRecord(r);
                        recordAdded = true;
                        break;
                    }
                //
                if (!recordAdded)
                    properties.Add(new SRiProperty(r));
            }
            //
            foreach (var p in properties)
            {
                if (cpinfoData.CPInfos.Count > 0)
                    p.AddCPData(cpinfoData);
                if (licaseData.LICases.Count > 0)
                    p.AddLIData(licaseData);
            }
            //
            if (notesData.Notes.Count > 0)
                AddNotes(notesData.Notes);
            if (historyData.History.Count > 0)
                AddHistory(historyData.History);
            //
            return properties;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		EntityKeys
        /// 
        /// <summary>	Gets the collection of EntityKeys for the records.
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
                foreach (var r in Records)
                    keys.Add(r.Record.EntityKey);
                //
                return keys.ToArray();
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		MergeOld
        /// 
        /// <summary>	Merges the old collection of records into the current collection. The only records 
        /// 			that are added are those that do not exist in the current collection. Match is
        /// 			done on EntityKey.
        /// </summary>
        /// <param name="data">		The old data.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void MergeOld(SRiRecordData data, bool addIfNotThere)
        {
            bool exists;
            //
            foreach (var old in data.Records)
            {
                exists = false;
                foreach (var r in Records)
                    if (r.Record.EntityKey.Equals(old.Record.EntityKey))
                    {
                        exists = true;
                        r.Record.Status = old.Record.Status;
                        break;
                    }
                //
                if (addIfNotThere && !exists)
                    Records.Add(old);
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		CPEntityKeys
        /// 
        /// <summary>	Gets and sets the CPEntityKeys for all the records in the data.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string[] CPEntityKeys
        {
            get
            {
                List<string> entityKeys;
                //
                entityKeys = new List<string>();
                foreach (var r in Records)
                {
                    if (!string.IsNullOrEmpty(r.Record.XBSubSys) &&
                        r.Record.XBSubSys.ToUpper().Equals("CP") &&
                        !entityKeys.Contains(r.Record.EntityKey))
                        entityKeys.Add(r.Record.EntityKey);
                }
                return entityKeys.ToArray();
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		LIEntityKeys
        /// 
        /// <summary>	Gets and sets the LIEntityKeys for all the records in the data.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string[] LIEntityKeys
        {
            get
            {
                List<string> entityKeys;
                //
                entityKeys = new List<string>();
                foreach (var r in Records)
                {
                    if (!string.IsNullOrEmpty(r.Record.XBSubSys) &&
                        r.Record.XBSubSys.ToUpper().Equals("LI") &&
                        !entityKeys.Contains(r.Record.EntityKey))
                        entityKeys.Add(r.Record.EntityKey);
                }
                return entityKeys.ToArray();
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		NotePadEntityKeys
        /// 
        /// <summary>	Gets and sets the NotePadEntityKeys for all the records in the data.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string[] NotePadEntityKeys
        {
            get
            {
                List<string> entityKeys;
                //
                entityKeys = new List<string>();
                foreach (var r in Records)
                    foreach (string entityKey in r.Record.PRNotePadEntityKeys)
                        if (!entityKeys.Contains(entityKey))
                            entityKeys.Add(entityKey);
                //
                return entityKeys.ToArray();
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		HistoryEntityKeys
        /// 
        /// <summary>	Gets and sets the HistoryEntityKeys for all the records in the data.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string[] HistoryEntityKeys
        {
            get
            {
                List<string> entityKeys;
                //
                entityKeys = new List<string>();
                foreach (var r in Records)
                    foreach (string entityKey in r.Record.CNApplPropEntityKeys)
                        if (!entityKeys.Contains(entityKey))
                            entityKeys.Add(entityKey);
                //
                return entityKeys.ToArray();
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		VisitEntityKeys
        /// 
        /// <summary>	Gets and sets the VisitEntityKeys for all the records in the data.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string[] VisitEntityKeys
        {
            get
            {
                List<string> EntityKeys;
                //
                EntityKeys = new List<string>();
                foreach (var r in Records)
                    foreach (var i in r.Record.Inspections)
                        foreach (string entityKey in i.VisitEntityKeys)
                            if (!EntityKeys.Contains(entityKey))
                                EntityKeys.Add(entityKey);
                //
                return EntityKeys.ToArray();
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
    public class SRiExtra
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
        [JsonProperty("srxtra0keyval")]
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
        [JsonProperty("srrec0keyval")]
        public string RecordKeyVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("srxtra0u_version")]
        public string UVersion
        {
            get;
            set;
        }
        /// 
        [JsonProperty("srxtra0seqno")]
        public string SequenceNumber
        {
            get;
            set;
        }
        /// 
        [JsonProperty("srxtra0flabel")]
        public string Label
        {
            get;
            set;
        }
        /// 
        [JsonProperty("srxtra0fname")]
        public string Name
        {
            get;
            set;
        }
        /// 
        [JsonProperty("srxtra0ftype")]
        public string Type
        {
            get;
            set;
        }
        /// 
        [JsonProperty("srxtra0fvalue")]
        public string Value
        {
            get;
            set;
        }
        /// 
        [JsonProperty("srxtra0fvalue_text")]
        public string ValueText
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
        /// Name		TypeDescription
        /// 
        /// <summary>	Gest the code description for the Type code.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string TypeDescription
        {
            get
            {
                if (string.IsNullOrEmpty(Type))
                    return "No type";
                else if (AppData.PropertyModel.SelectedRecord != null)
                    return AppData.ConfigModel.CodeDescription(AppData.PropertyModel.SelectedRecord.Record.Organisation,
                                                               "FTYPE",
                                                               Type);
                else
                    return Type;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name        UpdateVersions
        /// 
        /// <summary>   Updates the UVersion to make the supplied matching object.
        /// </summary>
        /// <param name="extras">       The extras.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void UpdateVersions(List<SRiExtra> extras)
        {
            foreach (SRiExtra extra in extras)
                if (extra.KeyVal.Equals(KeyVal))
                {
                    UVersion = extra.UVersion;
                    break;
                }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    public class SRiPCLocation
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
        [JsonProperty("pclocn0keyval")]
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
        [JsonProperty("srrec0keyval")]
        public string SRRecKeyVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("pclocn0u_version")]
        public string UVersion
        {
            get;
            set;
        }
        /// 
        [JsonProperty("pclocn0pesttype")]
        public string PestControlType
        {
            get;
            set;
        }
        /// 
        [JsonProperty("pclocn0location")]
        public string Location
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
        /// Name		PestControlTypeDescription
        /// 
        /// <summary>	Gets the code description for the PestControlType code.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string PestControlTypeDescription
        {
            get
            {
                if (string.IsNullOrEmpty(PestControlType))
                    return "No pest type";
                else if (AppData.PropertyModel.SelectedRecord != null)
                    return AppData.ConfigModel.CodeDescription(AppData.PropertyModel.SelectedRecord.Record.Organisation,
                                                               "PESTTYPE",
                                                               PestControlType);
                else
                    return PestControlType;

            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name        UpdateVersions
        /// 
        /// <summary>   Updates the UVersion to the UVersion of the matching object from the collection.
        /// </summary>
        /// <param name="pests">        The pests.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void UpdateVersions(List<SRiPCLocation> pests)
        {
            foreach (SRiPCLocation pest in pests)
                if (pest.KeyVal.Equals(KeyVal))
                {
                    UVersion = pest.UVersion;
                    break;
                }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    public class SRiCustomer
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
        [JsonProperty("srcustomer0keyval")]
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
        [JsonProperty("srrec0keyval")]
        public string RecordKeyVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("srcustomer0u_version")]
        public string UVersion
        {
            get;
            set;
        }
        /// 
        [JsonProperty("srcustomer0name")]
        public string Name
        {
            get;
            set;
        }
        /// 
        [JsonProperty("srcustomer0srcusttype")]
        public string CustomerType
        {
            get;
            set;
        }
        /// 
        [JsonProperty("srcustomer0address")]
        public string Address
        {
            get;
            set;
        }
        /// 
        [JsonProperty("srcontact")]
        public List<SRiContact> Contacts
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
        /// Name		CustomerTypeDescription
        /// 
        /// <summary>	Gets the code description for the CustomerType code.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string CustomerTypeDescription
        {
            get
            {
                if (string.IsNullOrEmpty(CustomerType))
                    return "No customer type";
                else if (AppData.PropertyModel.SelectedRecord != null)
                    return AppData.ConfigModel.CodeDescription(AppData.PropertyModel.SelectedRecord.Record.Organisation,
                                                               "SRCUSTTYPE",
                                                               CustomerType);
                else if (AppData.PropertyModel.SelectedProperty != null)
                    return AppData.ConfigModel.CodeDescription(AppData.PropertyModel.SelectedProperty.Organisation,
                                                               "SRCUSTTYPE",
                                                               CustomerType);
                else
                    return CustomerType;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name        UpdateVersions
        /// 
        /// <summary>   Updates the UVersion to the UVersion of the matching object from the collection.
        /// </summary>
        /// <param name="customers">        The customers.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void UpdateVersions(List<SRiCustomer> customers)
        {
            foreach (SRiCustomer customer in customers)
                if (customer.KeyVal.Equals(KeyVal))
                {
                    UVersion = customer.UVersion;
                    foreach (SRiContact contact in Contacts)
                        contact.UpdateVersions(customer.Contacts);
                    break;
                }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    public class SRiContact
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
        [JsonProperty("srcontact0keyval")]
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
        [JsonProperty("srcustomer0keyval")]
        public string CustomerKeyVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("srcontact0u_version")]
        public string UVersion
        {
            get;
            set;
        }
        /// 
        [JsonProperty("srrec0keyval")]
        public string SRRecKeyVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("srcontact0srconttype")]
        public string ContactType
        {
            get;
            set;
        }
        /// 
        [JsonProperty("srcontact0contaddr")]
        public string ContactAddress
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
        /// Name		ContactTypeDescription
        /// 
        /// <summary>	Gets the code description for the ContactType code.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string ContactTypeDescription
        {
            get
            {
                if (string.IsNullOrEmpty(ContactType))
                    return "No contact type";
                else if (AppData.PropertyModel.SelectedRecord != null)
                    return AppData.ConfigModel.CodeDescription(AppData.PropertyModel.SelectedRecord.Record.Organisation,
                                                               "CNCONTTYPE",
                                                               ContactType);
                else
                    return ContactType;

            }
        }

        [JsonIgnore]
        public string ContactDescription
        {
            get;
            set;
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name        UpdateVersions
        /// 
        /// <summary>   Updates the UVersion to the UVersion of the matching object from the collection.
        /// </summary>
        /// <param name="contacts">        The contacts.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void UpdateVersions(List<SRiContact> contacts)
        {
            foreach (SRiContact contact in contacts)
                if (contact.KeyVal.Equals(KeyVal))
                {
                    UVersion = contact.UVersion;
                    break;
                }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    public class SRiInspection : OnSiteEntity
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables
        /// ------------------------------------------------------------------------------------------------
        /// 
        private List<SRiVisitMeta> m_oVisits;
        private List<SRiXRRec> m_oRiskRecords;
        private List<string> m_oVisitEntityKeys;
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
        [JsonProperty("xirec0keyval")]
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
        [JsonProperty("xirec0u_version")]
        public string UVersion
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xirec0mdkeyval")]
        public string MdKeyVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xirec0mdsubsys")]
        public string MdSubSys
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xirec0refval")]
        public string RefVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xirec0prkeyval")]
        public string ParentKeyVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xirec0xirectype")]
        public string InspectionType
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xirec0xistat")]
        public string Status
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xirec0groupmod")]
        public string GroupMod
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xrrec")]
        public List<SRiXRRec> RiskRecords
        {
            get
            {
                if (m_oRiskRecords == null)
                    m_oRiskRecords = new List<SRiXRRec>();
                return m_oRiskRecords;
            }
            set { m_oRiskRecords = value; }
        }
        /// 
        [JsonProperty("xivisit0entitykey")]
        public List<string> VisitEntityKeys
        {
            get
            {
                if (m_oVisitEntityKeys == null)
                    m_oVisitEntityKeys = new List<string>();
                return m_oVisitEntityKeys;
            }
            set { m_oVisitEntityKeys = value; }
        }
        /// 
        [JsonProperty("visits")]
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

        [JsonIgnore]
        public DateTime? EarliestScheduledDate
        {
            get
            {
                DateTime? dt = null;
                foreach (var v in Visits)
                    if (!dt.HasValue ||
                        v.Visit.DateScheduled.HasValue &&
                        v.Visit.DateScheduled.Value < dt.Value)
                        dt = v.Visit.DateScheduled;

                return dt;
            }
        }

        // Visits should only be serialized when doing a Full serialize. They shouldn't be included with data for
        // the API or with data that is being cached to the device.
        public bool ShouldSerializeVisits() { return OnSiteSettings.ShouldSerialize == ShouldSerializeRule.Full; }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		InspectionTypeDescription
        /// 
        /// <summary>	Gets the code description for the InspectionType code.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string InspectionTypeDescription
        {
            get
            {
                if (string.IsNullOrEmpty(InspectionType))
                    return "No inspection type";
                else if (AppData.PropertyModel.SelectedRecord != null)
                    return AppData.ConfigModel.CodeDescription(AppData.PropertyModel.SelectedRecord.Record.Organisation,
                                                               "XIRECTYPE",
                                                               InspectionType);
                else if (AppData.PropertyModel.SelectedProperty != null)
                    return AppData.ConfigModel.CodeDescription(AppData.PropertyModel.SelectedProperty.Organisation,
                                                               "XIRECTYPE",
                                                               InspectionType);
                else
                    return InspectionType;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name        MergeVisit
        /// 
        /// <summary>   Adds the visit to the visit collection.
        /// </summary>
        /// <param name="visit">        The visit.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void MergeVisit(SRiVisitMeta visit)
        {
            bool exists;
            //
            exists = false;
            for (int v = 0; v < Visits.Count; v++)
                if (Visits[v].Visit.EntityKey.Equals(visit.Visit.EntityKey))
                {
                    exists = true;
                    if (visit.Status > Visits[v].Status)
                        Visits[v] = visit;
                }
            //
            if (!exists)
            {
                Visits.Add(visit);
                //
                // Make sure the entity key is in the key collection.
                if (VisitEntityKeys.Where(k => k.Equals(visit.Visit.EntityKey))
                                   .Select(k => k).ToList().Count == 0)
                    VisitEntityKeys.Add(visit.Visit.EntityKey);

            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name        UpdateVersions
        /// 
        /// <summary>   Updates the UVersion to the UVersion of the matching object from the collection.
        /// </summary>
        /// <param name="inspections">        The inspections.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void UpdateVersions(List<SRiInspection> inspections)
        {
            foreach (SRiInspection inspection in inspections)
                if (inspection.KeyVal.Equals(KeyVal))
                {
                    UVersion = inspection.UVersion;
                    foreach (SRiXRRec risk in RiskRecords)
                        risk.UpdateVersions(inspection.RiskRecords);
                    break;
                }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    public class SRiXRRec
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
        [JsonProperty("xrrec0keyval")]
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
        [JsonProperty("xrrec0u_version")]
        public string UVersion
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xrrec0refval")]
        public string RefVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xrrec0mdkeyval")]
        public string MDKeyVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xrrec0mdsubsys")]
        public string MDSubSys
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xrrec0xikeyval")]
        public string XIKeyVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xrrec0xrrectype")]
        public string RecType
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xrresult")]
        public List<SRiXRResult> Results
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
        /// Name		RecTypeDescription
        /// 
        /// <summary>	Gets the code description for the RecType code.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string RecTypeDescription
        {
            get
            {
                if (string.IsNullOrEmpty(RecType))
                    return "No risk type";
                else if (AppData.PropertyModel.SelectedRecord != null)
                    return AppData.ConfigModel.CodeDescription(AppData.PropertyModel.SelectedRecord.Record.Organisation,
                                                               "XRRECTYPE",
                                                               RecType);
                else
                    return RecType;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name        UpdateVersions
        /// 
        /// <summary>   Updates the UVersion to the UVersion of the matching object from the collection.
        /// </summary>
        /// <param name="risks">        The risk records.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void UpdateVersions(List<SRiXRRec> risks)
        {
            foreach (SRiXRRec risk in risks)
                if (risk.KeyVal.Equals(KeyVal))
                {
                    UVersion = risk.UVersion;
                    foreach (SRiXRResult result in Results)
                        result.UpdateVersions(risk.Results);
                    break;
                }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    public class SRiXRResult
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
        [JsonProperty("xrresult0keyval")]
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
        [JsonProperty("xrresult0u_version")]
        public string UVersion
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xrrec0keyval")]
        public string XRRecKeyVal
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xrresult0actual_date")]
        public DateTime? ActualDate
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xrresult0inspection_type")]
        public string InspectionType
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xrresult0score")]
        public int? Score
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xrresult0xrcatname")]
        public string CategoryName
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xrresult0target_date")]
        public DateTime? TargetDate
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xrresult0next_officer")]
        public string NextOfficer
        {
            get;
            set;
        }
        /// 
        [JsonProperty("xrresult0completed")]
        public string Completed
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
        /// Name		InspectionTypeDescription
        /// 
        /// <summary>	gets the code description for the InspectionType code.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string InspectionTypeDescription
        {
            get
            {
                if (string.IsNullOrEmpty(InspectionType))
                    return "No inspection type";
                else if (AppData.PropertyModel.SelectedRecord != null)
                    return AppData.ConfigModel.CodeDescription(AppData.PropertyModel.SelectedRecord.Record.Organisation,
                                                               "XIRECTYPE",
                                                               InspectionType);
                else
                    return InspectionType;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name        UpdateVersions
        /// 
        /// <summary>   Updates the UVersion to the UVersion of the matching object from the collection.
        /// </summary>
        /// <param name="results">        The risk results.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void UpdateVersions(List<SRiXRResult> results)
        {
            foreach (SRiXRResult result in results)
                if (result.KeyVal.Equals(KeyVal))
                {
                    UVersion = result.UVersion;
                    break;
                }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    public class SRiXRRecSummary : List<KeyValuePair<string, string>>
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public Constructor
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SRiXRRecSummary
        /// 
        /// <summary>	Creates a new instance of the SRiXRRecSummary class.
        /// </summary>
        /// <param name="rec">					The XRRec to form the summary.</param>
        /// <param name="inspectionTypeDesc">	The inspection type description for the XRREC.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public SRiXRRecSummary(SRiXRRec rec, string inspectionTypeDesc)
        {
            Add("Reference", rec.RefVal);
            Add("Assessment type", rec.RecTypeDescription);
            Add("Inspection type", inspectionTypeDesc);
            foreach (var result in rec.Results)
            {
                if (result.ActualDate.HasValue)
                    Add("Score on " + result.ActualDate.Value.ToString("dd/MM/yyyy"),
                        result.Score.HasValue ? result.Score.Value.ToString() : "No score");
                else if (result.TargetDate.HasValue)
                    Add("Assessment scheduled", result.TargetDate.Value.ToString("dd/MM/yyyy"));
            }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Private Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Add
        /// 
        /// <summary>	Adds a new item to the collection.
        /// </summary>
        /// <param name="name">		The name.</param>
        /// <param name="value">	The value.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void Add(string name, string value)
        {
            Add(new KeyValuePair<string, string>(name, value));
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    #endregion
    /// ----------------------------------------------------------------------------------------------------
}
