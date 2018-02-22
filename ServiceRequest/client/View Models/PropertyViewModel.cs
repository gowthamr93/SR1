using System;
using System.Collections.Generic;

using Idox.LGDP.Apps.Common.OnSite;
using System.Threading.Tasks;
using Idox.LGDP.Apps.ServiceRequest.Client.Classes;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
    /// ----------------------------------------------------------------------------------------------------
    /// Name			PropertyViewModel
    /// 
    /// <summary>		Holds all the SR data that is to be Synced and filled out on site.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public class PropertyViewModel : BaseViewModel
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public Eventhandlers
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		DocumentMetaRequired
        /// 
        /// <summary>	The event is fired when new premises data has been downloaded, or the document list
        /// 			of a premises has been refreshed. This tells the app to download document meta
        /// 			data for the premises detailed in the arguments.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public event EventHandler<OnSiteDocumentMetaArgs> DocumentMetaRequired;
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		IndexUpdated
        /// 
        /// <summary>	The event is fired when a mapped index is updated in the main menu. The args contain
        /// 			the index values for the table as well as the update type.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public event EventHandler<IndexUpdateEventArgs> IndexUpdated;
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables
        /// ------------------------------------------------------------------------------------------------
        /// 
        private List<SRiProperty> m_oPropertyList;
        private List<PropertyMapping> m_oMappings;
        private List<SRiSearchResult> m_oSearchResults;
        private int m_nSelectedProperty;
        private SelectedRecord m_oSelectedRecord;
        private FilterMode m_eFilterMode;
        private SortMode m_eSortMode;
        private string m_sFilterByType;
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Constructors
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		PropertyViewModel
        /// 
        /// <summary>	Creates a new instance of the PropertyViewModel class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public PropertyViewModel()
        {
            m_oPropertyList = new List<SRiProperty>();
            m_nSelectedProperty = -1;
            m_oSelectedRecord = null;
            m_eFilterMode = FilterMode.All;
            m_eSortMode = SortMode.Date;
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		AddDocument
        /// 
        /// <summary>	Adds a new document to the collection.
        /// </summary>
        /// <param name="doc">		The document to add.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void AddDocument(OnSiteDocument doc)
        {
            bool dataExists;
            OnSiteDocumentMeta docMeta;
            //
            if (SelectedProperty != null && SelectedRecord != null)
            {
                dataExists = false;
                doc.Status = SyncStatus.Changed;
                doc.SubEntity = "srrec";
                doc.SubEntityFieldName = "entitykey";
                doc.SubEntityFieldValue = SelectedRecord.Record.Record.EntityKey;
                docMeta = new OnSiteDocumentMeta()
                {
                    Documents = new List<OnSiteDocument>() { doc },
                    EntityKey = SelectedRecord.Record.Record.EntityKey,
                    Module = "sr",
                    Organisation = SelectedRecord.Record.Organisation,
                    Type = doc.Extension.GetDocumentName(),
                };
                foreach (var docData in SelectedProperty.Documents)
                    if (docData.Entity.Equals("srrec"))
                    {
                        docData.AddDocument(docMeta);
                        dataExists = true;
                        break;
                    }
                //
                // Add a new doc data to the collection for the "srrec" entity type.
                if (!dataExists)
                    SelectedProperty.Documents.Add(new OnSiteDocumentData()
                    {
                        AppModule = "sr",
                        Entity = "srrec",
                        EntityDocumentLists = new List<OnSiteDocumentMeta>() { docMeta }
                    });
                //
                UpdateProperty(PropertyType.Documents);
                UpdateProperty(PropertyType.PropertyData);
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetDocument
        /// 
        /// <summary>	Starts the download of document data by locating the the document in the 
        /// 			property collection.
        /// </summary>
        /// <param name="document">		The document to get.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public async Task<bool> GetDocument(OnSiteDocument document)
        {
            bool done, is_Success;
            //
            done = is_Success = false;
            foreach (var p in m_oPropertyList)
            {
                foreach (var data in p.Documents)
                {
                    foreach (var meta in data.EntityDocumentLists)
                    {
                        foreach (var doc in meta.Documents)
                            if (doc.Id.Equals(document.Id))
                            {
                                is_Success = await AppData.API.GetDocument(doc, data.Entity);
                                done = true;
                                break;
                            }
                        if (done) break;
                    }
                    if (done) break;
                }
                if (done) break;
            }
            return is_Success;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		AlphaIndex
        /// 
        /// <summary>	Gets the alpha index to show in the main menu and the map for the specified property.
        /// </summary>
        /// <param name="property">		The property.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public string AlphaIndex(SRiProperty property)
        {
            string index;
            int pIndex;
            //
            index = "?";
            for (int i = 0; i < Mappings.Count; i++)
            {
                pIndex = Mappings[i].Index;
                if (pIndex >= 0 && pIndex < m_oPropertyList.Count &&
                    m_oPropertyList[pIndex].Equals(property))
                {
                    index = i.ToAlphaIndex();
                    break;
                }
            }
            //
            return index;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		AlphaIndex
        /// 
        /// <summary>	Gets the alpha index to show in the main menu and the map for the specified property.
        /// </summary>
        /// <param name="pMap">		The property mapping.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public string AlphaIndex(PropertyMapping pMap)
        {
            string index;
            //
            index = "?";
            for (int i = 0; i < Mappings.Count; i++)
                if (Mappings[i].Index == pMap.Index)
                {
                    index = i.ToAlphaIndex();
                    break;
                }
            //
            return index;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Cache
        /// 
        /// <summary>	Gets all the string cache for each entity table.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public Dictionary<string, string> Cache
        {
            get
            {
                Dictionary<string, string> cache;
                SRiCPInfoData cpinfo;
                SRiLICaseData licase;
                SRiCNApplPropData history;
                SRiPRNotePadData notes;
                SRiRecordData records;
                SRiVisitData visits;
                OnSiteDocumentCache docs;
                //
                CurrentData(out cpinfo, out licase, out history, out notes, out visits, out records, out docs);
                cache = new Dictionary<string, string>();
                cache.Add(AppData.CPINFO, cpinfo.ToJson());
                cache.Add(AppData.LICASE, licase.ToJson());
                cache.Add(AppData.HISTORY, history.ToJson());
                cache.Add(AppData.NOTES, notes.ToJson());
                cache.Add(AppData.RECORDS, records.ToJson());
                cache.Add(AppData.VISITS, visits.ToJson());
                // Changed for IDSXSR-363. ShouldSerializeRule.None has been triggered for storing the Document ID to the text file. 
                OnSiteSettings.ShouldSerialize = ShouldSerializeRule.None;
                cache.Add(AppData.DOCS, docs.ToJson());
                return cache;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Clear
        /// 
        /// <summary>	Clears the contents of the view model.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void Clear()
        {
            SelectedAction = null;
            SelectedTreatment = null;
            SelectedVisit = null;
            SelectedRecord = null;
            SelectedProperty = null;
            m_oPropertyList = new List<SRiProperty>();
            m_oMappings = new List<PropertyMapping>();
            //
            UpdateProperty(PropertyType.PropertyFilters);
            UpdateProperty(PropertyType.PropertyList);
            UpdateProperty(PropertyType.PropertyData);
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		CurrentData
        /// 
        /// <summary>	Gets all the current data entities stored in memory.
        /// </summary>
        /// <param name="cpinfo">		The cpinfo data.</param>
        /// <param name="licase">		The licase data.</param>
        /// <param name="history">		The history data.</param>
        /// <param name="notes">		The notes data.</param>
        /// <param name="visits">		The visits data.</param>
        /// <param name="records">		The records data.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void CurrentData(out SRiCPInfoData cpinfo,
                                out SRiLICaseData licase,
                                out SRiCNApplPropData history,
                                out SRiPRNotePadData notes,
                                out SRiVisitData visits,
                                out SRiRecordData records,
                                out OnSiteDocumentCache docs)
        {
            cpinfo = new SRiCPInfoData();
            licase = new SRiLICaseData();
            history = new SRiCNApplPropData();
            notes = new SRiPRNotePadData();
            records = new SRiRecordData() { LastSync = AppData.LastSync };
            visits = new SRiVisitData();
            docs = new OnSiteDocumentCache();
            foreach (var p in m_oPropertyList)
            {
                cpinfo.CPInfos.AddRange(p.CPInfos);
                licase.LICases.AddRange(p.LICases);
                history.History.AddRange(p.PropertyHistory);
                notes.Notes.AddRange(p.PropertyNotes);
                docs.DocumentData.AddRange(p.Documents);
                foreach (var rg in p.RequestGroups)
                    foreach (var r in rg.Records)
                    {
                        records.Records.Add(r);
                        foreach (var i in r.Record.Inspections)
                            visits.Visits.AddRange(i.Visits);
                    }
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		EntityFromMapping
        /// 
        /// <summary>	Tries to retreive the OnSiteEntity identified by the IndexMapping. If the mapping
        /// 			is incorrect then null is returned.
        /// </summary>
        /// <param name="iMap">			The index mapping for the OnSiteEntity required.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public OnSiteEntity EntityFromMapping(IndexMapping iMap)
        {
            try
            {
                switch (iMap.Type)
                {
                    case MappingType.Property:
                        return m_oPropertyList[iMap.Property];
                    case MappingType.RequestGroup:
                        return m_oPropertyList[iMap.Property].RequestGroups[iMap.RequestGroup.Value];
                    case MappingType.Record:
                        return m_oPropertyList[iMap.Property].RequestGroups[iMap.RequestGroup.Value].Records[iMap.Record.Value];
                    case MappingType.Inspection:
                        return m_oPropertyList[iMap.Property].RequestGroups[iMap.RequestGroup.Value].Records[iMap.Record.Value].Record.Inspections[iMap.Inspection.Value];
                    case MappingType.Visit:
                        return m_oPropertyList[iMap.Property].RequestGroups[iMap.RequestGroup.Value].Records[iMap.Record.Value].Record.Inspections[iMap.Inspection.Value].Visits[iMap.Visit.Value];
                    case MappingType.Action:
                        return m_oPropertyList[iMap.Property].RequestGroups[iMap.RequestGroup.Value].Records[iMap.Record.Value].Record.Inspections[iMap.Inspection.Value].Visits[iMap.Visit.Value].Visit.Actions[iMap.Action.Value];
                    case MappingType.Treatment:
                        return m_oPropertyList[iMap.Property].RequestGroups[iMap.RequestGroup.Value].Records[iMap.Record.Value].Record.Inspections[iMap.Inspection.Value].Visits[iMap.Visit.Value].Visit.Treatments[iMap.Treatment.Value];
                    default:
                        return null;
                }
            }
            catch { return null; }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Filter
        /// 
        /// <summary>	Gets and sets the Filter for the premises collection.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public FilterMode Filter
        {
            get { return m_eFilterMode; }
            set
            {
                if (value != m_eFilterMode)
                {
                    m_eFilterMode = value;
                    UpdateMappings();
                    UpdateProperty(PropertyType.PropertyFilters);
                }
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		FilterAvailable
        /// 
        /// <summary>	Returns true if the filter can be applied.
        /// </summary>
        /// <remarks>	Filter options are only available if there are Visits to be filtered and a
        /// 			Property is not currently selected.
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public bool FilterAvailable
        {
            get
            {
                return m_oPropertyList != null && m_oPropertyList.Count > 0 && SelectedProperty == null;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		FilterByType
        /// 
        /// <summary>	Gets and sets the type filter for records.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public string FilterByType
        {
            get { return m_sFilterByType; }
            set
            {
                m_sFilterByType = value;
                UpdateMappings();
                UpdateProperty(PropertyType.PropertyFilters);
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		FilterTypes
        /// 
        /// <summary>	Gets and sets the FilterTypes for the properties.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public FilterTypeCollection FilterTypes
        {
            get;
            private set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Mappings
        /// 
        /// <summary>	Gets the Mappings for the currently filtered and sorted record collection.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public List<PropertyMapping> Mappings
        {
            get
            {
                if (m_oMappings == null)
                    UpdateMappings();
                return m_oMappings;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		PropertyFromMapping
        /// 
        /// <summary>	Gets the property that is mapped by the property mapping.
        /// </summary>
        /// <param name="pMap">		The property mapping.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public SRiProperty PropertyFromMapping(PropertyMapping pMap)
        {
            if (pMap.Index >= 0 && pMap.Index < m_oPropertyList.Count)
                return m_oPropertyList[pMap.Index];
            else
                return null;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		PropertyIsSelected
        /// 
        /// <summary>	Checks if the property mapped in the property mapping is the 
        /// 			currently selected property.
        /// </summary>
        /// <param name="pMap">		The record mapping.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public bool PropertyIsSelected(PropertyMapping pMap)
        {
            return pMap.Index == m_nSelectedProperty;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SaveAction
        /// 
        /// <summary>	Saves the selected action details to the action collection of the selected visit.
        /// </summary>
        /// <param name="error">		Any error that may occur.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void SaveAction(out Exception error)
        {
            IndexMapping iMap;
            //
            error = null;
            if (SelectedAction != null && SelectedVisit != null)
            {
                SelectedAction.Action.Modify();
                iMap = SelectedAction.IndexMap;
                if (iMap.Type == MappingType.Action)
                {
                    try
                    {
                        // Making a change to an Action is classed as a change to the parent Visit.
                        // The Visit should be marked as Edited, the Action saved to the Visit
                        // selection and the change required to be saved at the Visit level as well.
                        SelectedVisit.Edited = true;
                        if (SelectedAction.IsNew)
                            SelectedVisit.Visit.Visit.Actions.Add(SelectedAction.Action);
                        else
                            SelectedVisit.Visit.Visit.Actions[iMap.Action.Value] = SelectedAction.Action;
                        //
                        UpdateProperty(PropertyType.SelectedVisit);
                    }
                    catch (Exception ex) { error = ex; }
                }
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SaveTreatment
        /// 
        /// <summary>	Saves the selected treatment details to the treatment collection of the selected visit.
        /// </summary>
        /// <param name="error">		Any error that may occur.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void SaveTreatment(out Exception error)
        {
            IndexMapping iMap;
            //
            error = null;
            if (SelectedTreatment != null && SelectedVisit != null)
            {
                SelectedTreatment.Treatment.Modify();
                iMap = SelectedTreatment.IndexMap;
                if (iMap.Type == MappingType.Treatment)
                {
                    try
                    {
                        // Making a change to a Treatment is classed as a change to the parent Visit.
                        // The Visit should be marked as Edited, the Treatment saved to the Visit
                        // selection and the change required to be saved at the Visit level as well.
                        SelectedVisit.Edited = true;
                        if (SelectedTreatment.IsNew)
                            SelectedVisit.Visit.Visit.Treatments.Add(SelectedTreatment.Treatment);
                        else
                            SelectedVisit.Visit.Visit.Treatments[iMap.Treatment.Value] = SelectedTreatment.Treatment;
                    }
                    catch (Exception ex) { error = ex; }
                }
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SaveVisit
        /// 
        /// <summary>	Saves the selected visit details to the record held in memory. Once the details are
        /// 			set the PropertiesData property is updated which should trigger the data to be
        /// 			written to the device.
        /// </summary>
        /// <param name="error">		Any error that may occur.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void SaveVisit(out Exception error)
        {
            IndexUpdate update;
            IndexMapping iMap;
            SRiInspection inspection;
            //
            error = null;
            update = IndexUpdate.Empty;
            if (SelectedVisit != null)
            {
                iMap = SelectedVisit.IndexMap;
                if (iMap.Type == MappingType.Visit)
                {
                    // Update the record in data.
                    try
                    {
                        inspection = m_oPropertyList[iMap.Property]
                            .RequestGroups[iMap.RequestGroup.Value]
                            .Records[iMap.Record.Value].Record
                            .Inspections[iMap.Inspection.Value];
                        if (SelectedVisit.IsNew)
                        {
                            inspection.VisitEntityKeys.Add(SelectedVisit.Visit.Visit.EntityKey);
                            inspection.Visits.Add(SelectedVisit.Visit);
                            //
                            if (m_oPropertyList[iMap.Property].RequestGroups[iMap.RequestGroup.Value].Records[iMap.Record.Value].Record.Status != SyncStatus.New)
                                m_oPropertyList[iMap.Property].RequestGroups[iMap.RequestGroup.Value].Records[iMap.Record.Value].Record.Status = SyncStatus.Changed;
                        }
                        else
                        {
                            // Only mark the visit as modified if it is an existing visit and it's top level fields have changed.
                            if (inspection.Visits[iMap.Visit.Value].IsDifferentTo(SelectedVisit.Visit))
                                SelectedVisit.Visit.Modify();
                            inspection.Visits[iMap.Visit.Value] = SelectedVisit.Visit;
                        }
                        //
                        // Gather the index of the record in the main menu.
                        update = UpdateFromMapping(iMap, UpdateType.Changed);
                        //
                        // Update the record selection.
                        SelectedRecord = new SelectedRecord(m_oPropertyList[iMap.Property]
                                                            .RequestGroups[iMap.RequestGroup.Value]
                                                            .Records[iMap.Record.Value],
                                                            new IndexMapping(iMap.Property, iMap.RequestGroup, iMap.Record), false);
                    }
                    catch (Exception ex) { error = ex; }
                }
            }
            //
            // If there was no error when saving complete the process.
            if (error == null)
            {
                // Fire the property event that will cache the changes to file.
                UpdateProperty(PropertyType.PropertyData);
                if (!update.IsEmpty)
                    UpdateIndex(update);
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SaveRecord
        /// 
        /// <summary>	Saves the changes from the SelectedRecord.
        /// </summary>
        /// <param name="error">		Any error that might occur.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void SaveRecord(out Exception error)
        {
            IndexUpdate update;
            IndexMapping iMap;
            //
            error = null;
            update = IndexUpdate.Empty;
            if (SelectedRecord != null && SelectedRecord.Edited)
            {
                SelectedRecord.Record.Record.Modify();
                iMap = SelectedRecord.IndexMap;
                if (iMap.Type == MappingType.Record)
                {
                    // Update the record in data.
                    try
                    {
                        if (SelectedRecord.IsNew)
                        {
                            throw new NotImplementedException("This feature is not available yet.");
                        }
                        else
                            m_oPropertyList[iMap.Property]
                                .RequestGroups[iMap.RequestGroup.Value]
                                .Records[iMap.Record.Value] = SelectedRecord.Record;
                        //
                        // Gather the index of the record in the main menu.
                        update = UpdateFromMapping(iMap, UpdateType.Changed);
                        //
                        // Update the Property.
                        UpdateProperty(PropertyType.PropertyList);
                    }
                    catch (Exception ex) { error = ex; }
                }
            }
            //
            // If there was no error when saving complete the process.
            if (error == null)
            {
                // Fire the property event that will cache the changes to file.
                UpdateProperty(PropertyType.PropertyData);
                if (!update.IsEmpty)
                    UpdateIndex(update);
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SaveCustomer
        /// 
        /// <summary>	Saves the changes from the SelectedRecord to add ContactDetails.
        /// </summary>
        /// <param name="error">		Any error that might occur.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void SaveCustomer(out Exception error)
        {
            IndexUpdate update;
            IndexMapping iMap;
            //
            error = null;
            update = IndexUpdate.Empty;
            if (SelectedRecord != null)
            {
                SelectedRecord.Record.Record.Modify();
                iMap = SelectedRecord.IndexMap;
                if (iMap.Type == MappingType.Record)
                {
                    // Update the record in data.
                    try
                    {
                        m_oPropertyList[iMap.Property]
                            .RequestGroups[iMap.RequestGroup.Value]
                            .Records[iMap.Record.Value].Record.Customers = SelectedRecord.Record.Record.Customers;
                        // Gather the index of the record in the main menu.
                        update = UpdateFromMapping(iMap, UpdateType.Changed);
                        //
                    }
                    catch (Exception ex)
                    {
                        error = ex;
                    }
                }
            }
            //
            // If there was no error when saving complete the process.
            if (error == null)
            {
                // Fire the property event that will cache the changes to file.
                UpdateProperty(PropertyType.PropertyData);
                if (!update.IsEmpty)
                    UpdateIndex(update);
            }
        }




        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SaveSearchResults
        /// 
        /// <summary>	saves all selected search results to the property collection.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void SaveSearchResults()
        {
            bool exists;
            //
            foreach (var result in SearchResults)
            {
                if (result.Selected)
                {
                    exists = false;
                    foreach (var property in m_oPropertyList)
                        if (property.Matches(result.Property))
                        {
                            property.AddSearchResult(result);
                            exists = true;
                            break;
                        }
                    //
                    if (!exists)
                        m_oPropertyList.Add(result.Property);
                }
            }
            //
            // Update the mappings and calculate what updates are needed to be applied to the menu.
            UpdateMappings();
            UpdateProperty(PropertyType.PropertyFilters);
            UpdateProperty(PropertyType.PropertyData);
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SearchResults
        /// 
        /// <summary>	Gets and sets the SearchResults for the app.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public List<SRiSearchResult> SearchResults
        {
            get
            {
                if (m_oSearchResults == null)
                    m_oSearchResults = new List<SRiSearchResult>();
                return m_oSearchResults;
            }
            set
            {
                m_oSearchResults = value;
                UpdateProperty(PropertyType.SearchResults);
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SelectedProperty
        /// 
        /// <summary>	Gets and sets the SelectedProperty.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public SRiProperty SelectedProperty
        {
            get
            {
                if (m_nSelectedProperty < 0 || m_nSelectedProperty >= m_oPropertyList.Count)
                    return null;
                else
                    return m_oPropertyList[m_nSelectedProperty];
            }
            set
            {
                if (value == null)
                {
                    m_nSelectedProperty = -1;
                    m_oSelectedRecord = null;
                    UpdateProperty(PropertyType.SelectedProperty);
                    UpdateProperty(PropertyType.SelectedRecord);
                }
                else if (SelectedProperty == null || !SelectedProperty.Matches(value))
                {
                    for (int i = 0; i < m_oPropertyList.Count; i++)
                        if (m_oPropertyList[i].Matches(value))
                        {
                            m_nSelectedProperty = i;
                            m_oSelectedRecord = null;
                            UpdateProperty(PropertyType.SelectedProperty);
                            UpdateProperty(PropertyType.SelectedRecord);
                            break;
                        }
                }
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SelectedRecord
        /// 
        /// <summary>	Gets and sets the SelectedRecord for the application.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public SelectedRecord SelectedRecord
        {
            get { return m_oSelectedRecord; }
            set
            {
                m_oSelectedRecord = value;
                UpdateProperty(PropertyType.SelectedRecord);
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SelectedVisit
        /// 
        /// <summary>	Gets and sets the SelectedVisit for the application.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public SelectedVisit SelectedVisit
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SelectedAction
        /// 
        /// <summary>	Gets and sets the SelectedAction for the application.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public SelectedAction SelectedAction
        {
            get;
            set;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		SelectedTreatment
        /// 
        /// <summary>	Gets and sets the SelectedTreatment for the application.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public SelectedTreatment SelectedTreatment
        {
            get;
            set;
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Sort
        /// 
        /// <summary>	Gets and sets the Sort for the premises collection.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public SortMode Sort
        {
            get
            {
                return m_eSortMode;
            }
            set
            {
                if (value != m_eSortMode)
                {
                    m_eSortMode = value;
                    if (m_oMappings == null)
                        UpdateMappings();
                    else if (m_oMappings.Count > 0)
                        m_oMappings.Sort(new PropertyMappingComparer());
                    //
                    UpdateProperty(PropertyType.PropertyFilters);
                }
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Update
        /// 
        /// <summary>	Updates the document meta data in memory.
        /// </summary>
        /// <param name="newDoc">		The new document meta.</param>
        /// <param name="docUpload">	The document upload that was used before the update.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void Update(OnSiteDocument newDoc, OnSiteDocumentUpload docUpload)
        {
            foreach (var p in m_oPropertyList)
                foreach (var data in p.Documents)
                    if (data.Entity.Equals(docUpload.Data.Entity))
                        foreach (var meta in data.EntityDocumentLists)
                            if (meta.EntityKey.Equals(docUpload.Meta.EntityKey))
                                for (int i = 0; i < meta.Documents.Count; i++)
                                    if (meta.Documents[i].Id.Equals(docUpload.Document.Id))
                                    {
                                        newDoc.Status = SyncStatus.Saved;
                                        meta.Documents[i] = newDoc;
                                        break;
                                    }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Update
        /// 
        /// <summary>	Updates the property collection with data either from the API or loading from
        /// 			the device.
        /// </summary>
        /// <param name="properties">		The properties to add to the collection.</param>
        /// <param name="fromCache">		Whether the data was loaded from the device.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public List<IndexUpdate> Update(List<SRiProperty> properties, bool fromCache = false)
        {
            List<IndexUpdate> updates;
            bool propertyExists;
            UpdateType updateType;
            //
            updates = new List<IndexUpdate>();
            foreach (var p in properties)
            {
                updateType = UpdateType.None;
                propertyExists = false;
                for (int i = 0; i < m_oPropertyList.Count; i++)
                    if (m_oPropertyList[i].Matches(p))
                    {
                        if (!fromCache)
                            p.SetSavedStatus(m_oPropertyList[i]);
                        m_oPropertyList[i] = p;
                        updateType = UpdateType.Changed;
                        propertyExists = true;
                        break;
                    }
                //
                if (!propertyExists)
                {
                    updateType = UpdateType.New;
                    m_oPropertyList.Add(p);
                }
                updates.Add(new IndexUpdate(-1, updateType));
            }
            //
            // If the data was not loaded from cache update the property.
            if (!fromCache)
            {
                AppData.LastSync = DateTime.Now;
                UpdateProperty(PropertyType.PropertyData);
            }
            //
            // Update the premises mappings and gather the indicies for
            // the updated premises if they are currently mapped.
            UpdateMappings();
            for (int p = 0; p < properties.Count; p++)
            {
                propertyExists = false;
                for (int i = 0; i < Mappings.Count; i++)
                {
                    if (m_oPropertyList[Mappings[i].Index].Matches(properties[p]))
                    {
                        updates[p].PrimaryIndex = i;
                        propertyExists = true;
                        break;
                    }
                }
                //
                // If the updated Premises is not found in the filtered
                // list, mark the action as None as the Premises is not visible.
                if (!propertyExists)
                    updates[p].UpdateAction = UpdateType.None;
            }
            //
            UpdateProperty(PropertyType.PropertyList);
            //
            // Either tell the app that the cache has loaded, or tell the app to get
            // the document meta data for the premises that were just downloaded.
            if (fromCache)
                UpdateProperty(PropertyType.CacheLoaded);
            //
            return updates;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Update
        /// 
        /// <summary>	Updates the record held in memory.
        /// </summary>
        /// <param name="newRecord">		The new record data.</param>
        /// <param name="oldRecord">		The old record data.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void Update(SRiRecordMeta newRecord, SRiRecordMeta oldRecord)
        {
            bool done;
            //
            // Make sure the ShouldSerialize rule is reset to avoid temp keys from being ignored.
            done = false;
            OnSiteSettings.ShouldSerialize = ShouldSerializeRule.None;
            foreach (var p in m_oPropertyList)
            {
                foreach (var rg in p.RequestGroups)
                {
                    for (int i = 0; i < rg.Records.Count; i++)
                    {
                        if (oldRecord.Record.EntityKey.Equals(rg.Records[i].Record.EntityKey))
                        {
                            // Preserve the Visits from the old Record
                            foreach (SRiInspection inspection in oldRecord.Record.Inspections)
                                newRecord.MergeInspection(inspection);
                            //
                            // Check to see if the new record has changed so much 
                            // that a new property should be created.
                            if (!p.Matches(newRecord))
                            {
                                rg.Records.RemoveAt(i);
                                Update(new List<SRiProperty>() { new SRiProperty(newRecord) }, true);
                            }
                            else
                                rg.Records[i] = newRecord;
                            //
                            done = true;
                            break;
                        }
                    }
                    if (done) break;
                }
                if (done) break;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Update
        /// 
        /// <summary>	Updates the visit held in memory.
        /// </summary>
        /// <param name="newVisit">		The new visit data.</param>
        /// <param name="oldVisit">		The old visit data.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void Update(SRiVisitMeta newVisit, SRiVisitMeta oldVisit)
        {
            bool done;
            //
            // Make sure the ShouldSerialize rule is reset to avoid temp keys from being ignored.
            done = false;
            OnSiteSettings.ShouldSerialize = ShouldSerializeRule.None;
            foreach (var p in m_oPropertyList)
            {
                foreach (var rg in p.RequestGroups)
                {
                    foreach (var r in rg.Records)
                    {
                        if (oldVisit.Visit.RecordEntityKey.Equals(r.Record.EntityKey))
                        {
                            foreach (var i in r.Record.Inspections)
                            {
                                for (int v = 0; v < i.Visits.Count; v++)
                                    if (oldVisit.Visit.EntityKey.Equals(i.Visits[v].Visit.EntityKey))
                                    {
                                        i.Visits[v] = newVisit;
                                        done = true;
                                        break;
                                    }
                                if (done) break;
                            }
                        }
                        if (done) break;
                    }
                    if (done) break;
                }
                if (done) break;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		UpdateUserLocation
        /// 
        /// <summary>	Updates the user location used for determining the distance value for all
        /// 			the properties in the collection.
        /// </summary>
        /// <param name="latitude">		The user's latitude.</param>
        /// <param name="longitude">	The user's logitude.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void UpdateUserLocation(double latitude, double longitude)
        {
            foreach (var p in m_oPropertyList)
                p.UpdateDistance(latitude, longitude);
            //
            m_oMappings.Sort(new PropertyMappingComparer());
            UpdateProperty(PropertyType.PropertyFilters);
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		UploadableDocuments
        /// 
        /// <summary>	Gets a collection of documents that are changed and need to be uploaded.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public List<OnSiteDocumentUpload> UploadableDocuments
        {
            get
            {
                List<OnSiteDocumentUpload> docs;
                //
                // Split out each OnSiteDocument into it's own Meta/Data wrapper.
                docs = new List<OnSiteDocumentUpload>();
                foreach (var p in m_oPropertyList)
                    foreach (var docData in p.Documents)
                        foreach (var docMeta in docData.EntityDocumentLists)
                            foreach (var doc in docMeta.Documents)
                                if (doc.Status == SyncStatus.Changed)
                                    docs.Add(new OnSiteDocumentUpload(doc, docMeta, docData));
                //
                return docs;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		UploadableRecords
        /// 
        /// <summary>	
        /// </summary>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public List<SRiRecordMeta> UploadableRecords
        {
            get
            {
                List<SRiRecordMeta> records;
                //
                records = new List<SRiRecordMeta>();
                foreach (var p in m_oPropertyList)
                    foreach (var rg in p.RequestGroups)
                        foreach (var r in rg.Records)
                            if (r.Record.Status == SyncStatus.Changed || r.Record.Status == SyncStatus.New)
                                records.Add(r);
                //
                return records;
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		UploadableVisits
        /// 
        /// <summary>	Gets a collection of visits that are either changed or new and need to be uploaded.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public List<SRiVisitMeta> UploadableVisits
        {
            get
            {
                List<SRiVisitMeta> visits;
                //
                visits = new List<SRiVisitMeta>();
                foreach (var p in m_oPropertyList)
                    foreach (var rg in p.RequestGroups)
                        foreach (var r in rg.Records)
                            foreach (var i in r.Record.Inspections)
                                foreach (var v in i.Visits)
                                    if (v.Status == SyncStatus.New || v.Status == SyncStatus.Changed)
                                        visits.Add(v);
                //
                return visits;
            }
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetRefVal
        /// 
        /// <summary>	Gets the refernce value of the record from the particular visit.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public string GetRefVal(SRiVisitMeta sriVisit)
        {
            string value = null;
            foreach (var prop in m_oPropertyList)
            {
                foreach (var recs in prop.RequestGroups)
                {
                    foreach (var rec in recs.Records)
                    {
                        foreach (var recss in rec.Record.Inspections)
                        {
                            foreach (var visit in recss.Visits)
                            {
                                if (visit.Visit.KeyVal == sriVisit.Visit.KeyVal)
                                {
                                    value = rec.Record.RefVal;
                                    break;
                                }
                            }

                        }
                    }
                }
            }
            return value;
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetTradeName
        /// 
        /// <summary>	Gets the Trade name of the record from the particular visit.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public string GetTradeName(SRiVisitMeta sriVisit)
        {
            string value = null;
            foreach (var prop in m_oPropertyList)
            {
                foreach (var recs in prop.RequestGroups)
                {
                    foreach (var rec in recs.Records)
                    {
                        foreach (var recss in rec.Record.Inspections)
                        {
                            foreach (var visit in recss.Visits)
                            {
                                if (visit.Visit.KeyVal == sriVisit.Visit.KeyVal)
                                {
                                    value = rec.Record.TradeName;
                                    break;
                                }
                            }

                        }
                    }
                }
            }
            return value;
        }

        /// ------------------------------------------------------------------------------------------------
        /// Name        AddNewRecord
        /// 
        /// <summary>   Adds a new record to the data in memory, either by creating a new property or by
        ///             adding the new record to an existing property.
        /// </summary>
        /// <param name="newRecord">        The new record.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void AddNewRecord(SRiRecordMeta newRecord)
        {
            OnSiteAddress address;
            bool createProperty;
            //
            createProperty = true;
            address = new OnSiteAddress(newRecord.Record.Address);
            foreach (SRiProperty property in m_oPropertyList)
                if (property.Matches(newRecord))
                {
                    createProperty = false;
                    property.AddRecord(newRecord);
                    break;
                }
            //
            if (createProperty)
                m_oPropertyList.Add(new SRiProperty(newRecord));
            //
            UpdateMappings();
            UpdateProperty(PropertyType.PropertyData);
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Private Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		UpdateFromMapping
        /// 
        /// <summary>	Gets the IndexUpdate needed to represent the update type being applied to
        /// 			the specified index mapping.
        /// 
        /// 			If the index mapping is not found in the current mapping collection an
        /// 			empty index update is returned.
        /// </summary>
        /// <param name="iMap">			The index mapping.</param>
        /// <param name="updateType">	The update type.</param>
        /// 
        /// <remarks>	
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private IndexUpdate UpdateFromMapping(IndexMapping iMap, UpdateType updateType)
        {
            IndexUpdate update;
            //
            update = IndexUpdate.Empty;
            for (int i = 0; i < m_oMappings.Count; i++)
                if (m_oMappings[i].Index == iMap.Property)
                {
                    update = new IndexUpdate(i, updateType);
                    break;
                }
            //
            return update;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		UpdateIndex
        /// 
        /// <summary>	Fires the IndexUpdated method with the provided IndexUpdate args.
        /// </summary>
        /// <param name="updates">		The index update details for the event.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void UpdateIndex(params IndexUpdate[] updates)
        {
            if (IndexUpdated != null)
                IndexUpdated(this, new IndexUpdateEventArgs(updates));
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		UpdateMappings
        /// 
        /// <summary>	Updates the mappings used for populating the main table view.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void UpdateMappings()
        {
            PropertyMapping mapping;
            //
            FilterTypes = new FilterTypeCollection(m_oPropertyList);
            m_oMappings = new List<PropertyMapping>();
            for (int i = 0; i < m_oPropertyList.Count; i++)
            {
                mapping = new PropertyMapping(m_oPropertyList[i], i);
                if (mapping.ActiveExpanded.Count > 0)
                    m_oMappings.Add(mapping);
            }
            //
            if (m_oMappings.Count > 0)
                m_oMappings.Sort(new PropertyMappingComparer());
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		UpdateMappings
        /// 
        /// <summary>	Updates the mappings used for populating the main table view.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void UpdatePropertyList()
        {
            List<PropertyMapping> newMapping;
            PropertyMapping mapping;
            List<SRiProperty> sortedProperties;
            //
            sortedProperties = m_oPropertyList;
            sortedProperties.Sort(new PropertyComparer());
            newMapping = new List<PropertyMapping>();
            for (int i = 0; i < sortedProperties.Count; i++)
            {
                mapping = new PropertyMapping(sortedProperties[i], i);
                if (mapping.ActiveExpanded.Count > 0)
                    newMapping.Add(mapping);
            }
            m_oMappings = newMapping;
            m_oPropertyList = sortedProperties;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		PropertyFromMapping
        /// 
        /// <summary>	Gets the property that is mapped by the property mapping.
        /// </summary>
        /// <param name="pMap">		The property mapping.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void DeleteProperty(PropertyMapping pMap)
        {

            if (pMap.Index >= 0 && pMap.Index < m_oPropertyList.Count)
            {
                //
                Mappings.RemoveAt(pMap.Index);
                //
                m_oPropertyList.RemoveAt(pMap.Index);
                SelectedProperty = null;
            }
            else
            {
                throw new Exception("Deleting Property Failed");
            }
        }

        /// ------------------------------------------------------------------------------------------------

        /// Name        Delete
        /// <summary>   Removes a service request from the data structure. This could result in a request 
        ///             group and a property being removed as well.
        /// </summary>
        /// <param name="record">       The record meta to remove.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void Delete(SRiRecordMeta record)
        {
            bool found;
            //
            found = false;
            for (int p = 0; p < m_oPropertyList.Count; p++)
            {
                for (int rg = 0; rg < m_oPropertyList[p].RequestGroups.Count; rg++)
                {
                    if (found) break;
                    else
                    {
                        for (int r = 0; r < m_oPropertyList[p].RequestGroups[rg].Records.Count; r++)
                            if (m_oPropertyList[p].RequestGroups[rg].Records[r].ID.Equals(record.ID))
                            {
                                found = true;
                                m_oPropertyList[p].RequestGroups[rg].Records.RemoveAt(r);
                                //Refresh or update the Property fields
                                m_oPropertyList[p].Refresh();
                                //
                                if (m_oPropertyList[p].RequestGroups[rg].Records.Count == 0)
                                    m_oPropertyList[p].RequestGroups.RemoveAt(rg);
                                break;
                            }
                    }
                }
                //
                if (found)
                {
                    if (m_oPropertyList[p].RequestGroups.Count == 0)
                        m_oPropertyList.RemoveAt(p);
                    break;
                }
            }
            //
            UpdateMappings();
        }

        /// ------------------------------------------------------------------------------------------------

        /// Name        Delete
        /// 
        /// <summary>   Removes a Visit from the data structure.
        /// </summary>
        /// <param name="visit">        The visit meta to remove.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 

        public void Delete(SRiVisitMeta visit)
        {
            bool found;
            //
            found = false;
            foreach (SRiProperty property in m_oPropertyList) if (found) break;
                else foreach (SRiRequestGroup rg in property.RequestGroups) if (found) break;
                        else foreach (SRiRecordMeta r in rg.Records) if (found) break;
                                else foreach (SRiInspection i in r.Record.Inspections) if (found) break;
                                        else for (int v = 0; v < i.Visits.Count; v++)
                                                if (i.Visits[v].ID.Equals(visit.ID))
                                                {
                                                    found = true;
                                                    i.Visits.RemoveAt(v);
                                                    break;
                                                }
            //
            UpdateMappings();
        }

        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
