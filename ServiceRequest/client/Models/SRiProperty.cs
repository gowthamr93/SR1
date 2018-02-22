using System;
using System.Collections.Generic;

using Idox.LGDP.Apps.Common.OnSite;
using Newtonsoft.Json;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
    /// ----------------------------------------------------------------------------------------------------
    /// Name			SRiProperty
    /// 
    /// <summary>		Top level item in the Service Request data, represents that parent entity of a SRREC.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public class SRiProperty : OnSiteJsonEntity<SRiProperty>
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public Constructors
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SRiProperty
        /// 
        /// <summary>	Creates a new instance of the SRIProperty class.
        /// </summary>
        /// <param name="record">		The first record in the record collection.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public SRiProperty(SRiRecordMeta record)
        {
            RequestGroups = new List<SRiRequestGroup>();
            Documents = new List<OnSiteDocumentData>();
            PropertyDetails = new List<SRiPropertyDetail>();
            CPInfos = new List<SRiCPInfoMeta>();
            LICases = new List<SRiLICaseMeta>();
            //
            Address = new OnSiteAddress(record.Record.Address);
            Latitude = record.Record.Latitude;
            Longitude = record.Record.Longitude;
            TradeName = record.Record.TradeName;
            UPRN = record.Record.UPRN;
            //Added by Gowtham for getting organisation name in the Property field. It has not been in the code given by louis.
            Organisation = record.Organisation;
            RequestGroups.Add(new SRiRequestGroup(record));
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		AddCPData
        /// 
        /// <summary>	Adds the CPINFO entities that are linked to any of the records.
        /// </summary>
        /// <param name="data">		The CPINFO data.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void AddCPData(SRiCPInfoData data)
        {
            bool added;
            //
            foreach (var p in data.CPInfos)
            {
                added = false;
                foreach (var rg in RequestGroups)
                {
                    foreach (var r in rg.Records)
                    {
                        if (!string.IsNullOrEmpty(r.Record.XBSubSys) &&
                            !string.IsNullOrEmpty(r.Record.XBKeyVal) &&
                            r.Record.XBSubSys.ToUpper().Equals("CP") &&
                            r.Record.XBKeyVal.Equals(p.CPInfo.KeyVal))
                        {
                            CPInfos.Add(p);
                            PropertyDetails.Add(new SRiPropertyDetail(p, r.Record));
                            added = true;
                            break;
                        }
                    }
                    if (added) break;
                }
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		AddLIData
        /// 
        /// <summary>	Adds the LICASE entities that are linked to any of the records.
        /// </summary>
        /// <param name="data">		The LICASE data.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void AddLIData(SRiLICaseData data)
        {
            bool added;
            //
            foreach (var p in data.LICases)
            {
                added = false;
                foreach (var rg in RequestGroups)
                {
                    foreach (var r in rg.Records)
                    {
                        if (!string.IsNullOrEmpty(r.Record.XBSubSys) &&
                            !string.IsNullOrEmpty(r.Record.XBKeyVal) &&
                            r.Record.XBSubSys.ToUpper().Equals("LI") &&
                            r.Record.XBKeyVal.Equals(p.LICase.KeyVal))
                        {
                            LICases.Add(p);
                            PropertyDetails.Add(new SRiPropertyDetail(p, r.Record));
                            added = true;
                            break;
                        }
                    }
                    if (added) break;
                }
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		AddRecord
        /// 
        /// <summary>	Adds a record to the request group collection. Putting it in either an existing
        /// 			group that has a matching request type or creating a new group.
        /// </summary>
        /// <param name="record">		The record to add.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void AddRecord(SRiRecordMeta record)
        {
            bool addedToGroup;
            //
            addedToGroup = false;
            foreach (var rg in RequestGroups)
                if (rg.GroupType.Equals(record.Record.RequestType))
                {
                    rg.Records.Add(record);
                    addedToGroup = true;
                    break;
                }
            //
            if (!addedToGroup)
                RequestGroups.Add(new SRiRequestGroup(record));
            //
            // If any of the record fields in the property are missing,
            // take the field from the new record.
            if (string.IsNullOrEmpty(UPRN))
                UPRN = record.Record.UPRN;
            if (string.IsNullOrEmpty(TradeName))
                TradeName = record.Record.TradeName;
            if (string.IsNullOrEmpty(Address.RawAddress))
                Address = new OnSiteAddress(record.Record.Address);
            if (!HasValidCoords)
            {
                Latitude = record.Record.Latitude;
                Longitude = record.Record.Longitude;
            }
            //Added by Gowtham for getting organisation name in the new property. It has not been in the code given by louis.
            Organisation = record.Organisation;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Address
        /// 
        /// <summary>	Gets and sets the Address for the property.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public OnSiteAddress Address
        {
            get;
            private set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		AddSearchResult
        /// 
        /// <summary>	Adds the contents of the search result to the property data.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void AddSearchResult(SRiSearchResult result)
        {
            bool requestGroupExists;
            bool recordExists;
            //
            // Figure out if a new request group is required, or if the record can go into an existing one.
            // If the same record is already in memory, do not add the new one.
            foreach (var rg in result.Property.RequestGroups)
                foreach (var r in rg.Records)
                {
                    requestGroupExists = false;
                    foreach (var localGroup in RequestGroups)
                        if (localGroup.GroupType.Equals(rg.GroupType))
                        {
                            requestGroupExists = true;
                            recordExists = false;
                            foreach (var localRecord in localGroup.Records)
                                if (localRecord.ID.Equals(r.ID))
                                {
                                    recordExists = true;
                                    break;
                                }
                            //
                            if (!recordExists)
                                localGroup.Records.Add(r);
                            break;
                        }
                    //
                    if (!requestGroupExists)
                        RequestGroups.Add(new SRiRequestGroup(r));
                }

        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		CPInfos
        /// 
        /// <summary>	Gets and sets the CPInfos for the property.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public List<SRiCPInfoMeta> CPInfos
        {
            get;
            private set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		DistanceFromUser
        /// 
        /// <summary>	Gets and sets the DistanceFromUser for the property.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public double DistanceFromUser
        {
            get;
            private set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		DocumentCount
        /// 
        /// <summary>	Gets  the DocumentCount for the property.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public int DocumentCount
        {
            get
            {
                int count;
                //
                count = 0;
                foreach (var docData in Documents)
                    foreach (var docMeta in docData.EntityDocumentLists)
                        foreach (var doc in docMeta.Documents)
                            count++;
                //
                return count;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Documents
        /// 
        /// <summary>	Gets and sets the Documents for the property.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public List<OnSiteDocumentData> Documents
        {
            get;
            private set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		EarliestDueDate
        /// 
        /// <summary>	Gets the EariestDue Date from all child entities.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public DateTime? EarliestDueDate
        {
            get
            {
                DateTime? dt;
                DateTime? rgDt;
                //
                dt = null;
                foreach (var rg in RequestGroups)
                {
                    rgDt = rg.EarliestTargetDate;
                    if (!dt.HasValue ||
                        rgDt.HasValue && rgDt.Value < dt.Value)
                        dt = rgDt;
                }
                return dt;
            }
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		EarliestScheduleDate
        /// 
        /// <summary>	Gets the EariestDue Schedule Date from all child entities.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public DateTime? EarliestScheduleDate
        {
            get
            {
                DateTime? dt;
                DateTime? rgDt;
                //
                dt = null;
                foreach (var rg in RequestGroups)
                {
                    rgDt = rg.ScheduleDate;
                    if (!dt.HasValue ||
                        rgDt.HasValue && rgDt.Value < dt.Value)
                        dt = rgDt;
                }
                return dt;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetPropertyHistory
        /// 
        /// <summary>	Gets a property history entity using the index.
        /// </summary>
        /// <param name="index">		The entity index.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public SRiCNApplLog GetPropertyHistory(int index)
        {
            SRiCNApplLog historyLog;
            bool done;
            int i;
            //
            i = 0;
            done = false;
            historyLog = null;
            foreach (var rg in RequestGroups)
            {
                foreach (var r in rg.Records)
                {
                    foreach (var h in r.Record.History)
                    {
                        foreach (var l in h.History.Logs)
                        {
                            if (i == index)
                            {
                                historyLog = l;
                                historyLog.SystemID = h.History.SystemID;
                                done = true;
                                break;
                            }
                            i++;
                        }
                        if (done) break;
                    }
                    if (done) break;
                }
                if (done) break;
            }
            //
            return historyLog;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetPropertyNotes
        /// 
        /// <summary>	Gets a property notes entity using the index.
        /// </summary>
        /// <param name="index">		The notes index.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public SRiPRNotePad GetPropertyNotes(int index)
        {
            SRiPRNotePad notes;
            bool done;
            int i;
            //
            i = 0;
            done = false;
            notes = null;
            foreach (var rg in RequestGroups)
            {
                foreach (var r in rg.Records)
                {
                    foreach (var n in r.Record.Notes)
                    {
                        if (i == index)
                        {
                            notes = n.PropertyNotes;
                            done = true;
                            break;
                        }
                        i++;
                    }
                    if (done) break;
                }
                if (done) break;
            }
            //
            return notes;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		HasValidCoords
        /// 
        /// <summary>	Checks if the latitude and Longitude have values.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public bool HasValidCoords
        {
            get { return Latitude.HasValue && Longitude.HasValue; }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Latitude
        /// 
        /// <summary>	Gets and sets the Latitude for the property.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public double? Latitude
        {
            get;
            private set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		LICases
        /// 
        /// <summary>	Gets and sets the LICases for the property.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
		public List<SRiLICaseMeta> LICases
        {
            get;
            private set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Longitude
        /// 
        /// <summary>	Gets and sets the Longitude for the property.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public double? Longitude
        {
            get;
            private set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Matches
        /// 
        /// <summary>	Compares the UPRN, Address and TradeName of the properties to find a match.
        /// </summary>
        /// <param name="property">		The property to match with.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public bool Matches(SRiProperty property)
        {
            bool match;
            //
            if (Match(TradeName, property.TradeName))
                match = Match(UPRN, property.UPRN) ||
                        Match(Address, property.Address);
            else match = false;
            //
            return match;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Matches
        /// 
        /// <summary>	Checks to see if the record matches the property on various fields.
        /// </summary>
        /// <param name="record">		The record.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public bool Matches(SRiRecordMeta record)
        {
            bool match;
            //
            if (Match(TradeName, record.Record.TradeName))
                match = (Match(UPRN, record.Record.UPRN) ||
                         Match(Address, new OnSiteAddress(record.Record.Address)));
            else match = false;
            //
            return match;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Organisation
        /// 
        /// <summary>	Gets and sets the Organisation for the property.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public string Organisation
        {
            get;
            private set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		PropertyDetails
        /// 
        /// <summary>	Gets and sets the PropertyDetails for the property.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public List<SRiPropertyDetail> PropertyDetails
        {
            get;
            private set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		PropertyHistory
        /// 
        /// <summary>	Gets the PropertyHistory for the property.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public List<SRiCNApplPropMeta> PropertyHistory
        {
            get
            {
                List<SRiCNApplPropMeta> history;
                //
                history = new List<SRiCNApplPropMeta>();
                foreach (var rg in RequestGroups)
                    foreach (var r in rg.Records)
                        history.AddRange(r.Record.History);
                //
                return history;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		PropertyHistoryCount
        /// 
        /// <summary>	Gets the number of history entities attached to the property.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public int PropertyHistoryCount
        {
            get
            {
                int count;
                //
                count = 0;
                foreach (var rg in RequestGroups)
                    foreach (var r in rg.Records)
                        foreach (var h in r.Record.History)
                            foreach (var l in h.History.Logs)
                                count++;
                //
                return count;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		PropertyNotes
        /// 
        /// <summary>	Gets the PropertyNotes for the property.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public List<SRiPRNotePadMeta> PropertyNotes
        {
            get
            {
                List<SRiPRNotePadMeta> notes;
                //
                notes = new List<SRiPRNotePadMeta>();
                foreach (var rg in RequestGroups)
                    foreach (var r in rg.Records)
                        notes.AddRange(r.Record.Notes);
                //
                return notes;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		PropertyNotesCount
        /// 
        /// <summary>	Gets the number of notes attached to the property.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public int PropertyNotesCount
        {
            get
            {
                int count;
                //
                count = 0;
                foreach (var rg in RequestGroups)
                    foreach (var r in rg.Records)
                        foreach (var n in r.Record.Notes)
                            count++;
                //
                return count;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		RefVal
        /// 
        /// <summary>	Gets and sets the RefVal for the property.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public string RefVal
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		RequestGroups
        /// 
        /// <summary>	Gets and sets the RequestGroups for the property.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public List<SRiRequestGroup> RequestGroups
        {
            get;
            private set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Status
        /// 
        /// <summary>	Gets and sets the Status for the property.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public SyncStatus Status
        {
            get
            {
                SyncStatus s;
                //
                // get the highest level status of all child entities.
                s = SyncStatus.None;
                foreach (var rg in RequestGroups)
                    foreach (var r in rg.Records)
                    {
                        s = s.Override(r.Record.Status);
                        foreach (var i in r.Record.Inspections)
                            foreach (var v in i.Visits)
                                s = s.Override(v.Status);
                    }
                //
                // Also check any document status.
                foreach (var docData in Documents)
                    foreach (var docMeta in docData.EntityDocumentLists)
                        foreach (var doc in docMeta.Documents)
                            s = s.Override(doc.Status);
                //
                return s;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		TradeName
        /// 
        /// <summary>	Gets and sets the TradeName for the property.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public string TradeName
        {
            get;
            private set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		UpdateDistance
        /// 
        /// <summary>	Updates the distance from the user location.
        /// </summary>
        /// <param name="userLat">		The user's latitude.</param>
        /// <param name="userLong">		The user's longitude.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void UpdateDistance(double userLat, double userLong)
        {
            double dst;
            //
            if (Latitude.HasValue && Longitude.HasValue)
            {
                dst = 0;
                var lat1 = Math.Abs(Latitude.Value);
                var lat2 = Math.Abs(userLat);
                var lon1 = Math.Abs(Longitude.Value);
                var lon2 = Math.Abs(userLong);
                //
                if (lat1 > lat2) dst += lat1 - lat2;
                else dst += lat2 - lat1;
                if (lon1 > lon2) dst += lon1 - lon2;
                else dst += lon2 - lon1;
                //
                DistanceFromUser = dst;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		UPRN
        /// 
        /// <summary>	Gets the UPRN for the property.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public string UPRN
        {
            get;
            private set;
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Private Functions and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name        Match
        /// 
        /// <summary>   Compares 2 OnSiteAddress properties.
        /// </summary>
        /// <param name="a">        OnSiteAddress a.</param>
        /// <param name="b">        OnSiteAddress b.</param>
        /// 
        /// <returns>   True if match.
        /// </returns>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private bool Match(OnSiteAddress a, OnSiteAddress b)
        {
            bool match;
            //
            if (a == null || b == null)
                match = false;
            else
                match = Match(a.LongAddress, b.LongAddress);
            //
            return match;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name        Match
        /// 
        /// <summary>   Compares 2 string properties.
        /// </summary>
        /// <param name="a">        String a.</param>
        /// <param name="b">        String b.</param>
        /// 
        /// <returns>   True if match.
        /// </returns>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private bool Match(string a, string b)
        {
            bool match;
            //
            if (string.IsNullOrEmpty(a))
                match = string.IsNullOrEmpty(b);
            else
                match = a.Equals(b);
            //
            return match;
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name        Refresh
        /// 
        /// <summary>   Refreshes the properties from the first record in the record groups collection.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void Refresh()
        {
            foreach (SRiRequestGroup rg in RequestGroups)
                foreach (SRiRecordMeta record in rg.Records)
                {
                    Address = new OnSiteAddress(record.Record.Address);
                    Latitude = record.Record.Latitude;
                    Longitude = record.Record.Longitude;
                    TradeName = record.Record.TradeName;
                    UPRN = record.Record.UPRN;
                    return;
                }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    /// ----------------------------------------------------------------------------------------------------
    /// Name			SRiPropertyData
    /// 
    /// <summary>		Wrapper class for the property collection held in the property view model.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public class SRiPropertyData
    {
        private List<SRiProperty> properties;
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		LastSync
        /// 
        /// <summary>	Gets and sets the LastSync for the data.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public DateTime? LastSync
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Properties
        /// 
        /// <summary>	Gets and sets the property collection.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public List<SRiProperty> Properties
        {
            get { return properties; }
            set { properties = value; }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
    /// 
    /// ----------------------------------------------------------------------------------------------------
    /// 
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public class SRiPropertyDetail
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public Constructors
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SRiPropertyDetail
        /// 
        /// <summary>	Creates a new instance of the SRiPropertyDetail class.
        /// </summary>
        /// <param name="cpinfo">		The CPInfo entity to pull details from.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public SRiPropertyDetail(SRiCPInfoMeta cpinfo, SRiRecord record)
        {
            Title = "COMMERCIAL PREMISES";
            Details = new SRiPropertyDetailsCollection();
            Details.Add("Reference", cpinfo.CPInfo.RefVal);
            Details.Add("SRREC reference", record.RefVal);
            Details.Add("Occupier", cpinfo.CPInfo.Occupier);
            Details.Add("Contact", cpinfo.CPInfo.Contact);
            Details.Add("Main use of site", cpinfo.PremisesUseDescription);
            foreach (var fhrs in cpinfo.CPInfo.FHRSRecords)
                if (!string.IsNullOrEmpty(fhrs.Rating) && fhrs.Assessed.HasValue)
                    Details.Add(string.Format("FHRS on {0}", fhrs.Assessed.ToDateString()), fhrs.Rating);
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SRiPropertyDetail
        /// 
        /// <summary>	Creates a new instance of the SRiPropertyDetail class.
        /// </summary>
        /// <param name="licase">		The LICase entity to pull details from.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public SRiPropertyDetail(SRiLICaseMeta licase, SRiRecord record)
        {
            Title = "LICENSING";
            Details = new SRiPropertyDetailsCollection();
            Details.Add("Reference", licase.LICase.RefVal);
            Details.Add("SRREC reference", record.RefVal);
            Details.Add("Type", licase.CNTypeDescription);
            Details.Add("License Plate", licase.LICase.PlateRef);
            Details.Add("Commend Date", licase.LICase.CommendDate);
            Details.Add("Expiry Date", licase.LICase.ExpiryDate);
            //
            foreach (var party in licase.LICase.LIParties)
                if (!string.IsNullOrEmpty(party.FullName))
                    Details.Add(licase.PartyTypeDescription(party), party.FullName);
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Properties
        /// ------------------------------------------------------------------------------------------------
        /// 
        public SRiPropertyDetailsCollection Details { get; set; }
        /// 
        public string Title { get; set; }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }

    public class SRiPropertyDetailsCollection : List<KeyValuePair<string, string>>
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Add
        /// 
        /// <summary>	Adds a new key value pair to the collection.
        /// </summary>
        /// <param name="key">		The key.</param>
        /// <param name="value">	The value.</param></param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void Add(string key, string value)
        {
            if (key != null && value != null)
                Add(new KeyValuePair<string, string>(key, value));
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Add
        /// 
        /// <summary>	Adds a new key value pair to the collection.
        /// </summary>
        /// <param name="key">		The key.</param>
        /// <param name="value">	The value.</param></param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void Add(string key, DateTime? value)
        {
            if (key != null && value.HasValue)
                Add(new KeyValuePair<string, string>(key, value.ToDateString()));
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
