using System;
using System.Collections.Generic;

using Idox.LGDP.Apps.Common.OnSite;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			RecordViewModel
	/// 
	/// <summary>		Handles all record request data operations.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class RecordViewModel : BaseViewModel
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Eventhandlers
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		ConfigurationsRequired
		/// 
		/// <summary>	The event is fired when premises data has been updated. A collection of each
		/// 			organisation currently in use is supplied in the event args. This allows the app
		/// 			to externally handle what configuration data is required.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public event EventHandler<OnSiteConfigRequestArgs> ConfigurationsRequired;
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
		private List<SRiRecord> m_oRecordList;
		private List<RecordMapping> m_oMappings;
		private int m_nSelectedRecord;
		private FilterMode m_eFilterMode;
		private SortMode m_eSortMode;
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
		public RecordViewModel()
		{
			m_oRecordList = new List<SRiRecord>();
			m_nSelectedRecord = -1;
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
		/// Name		AlphaIndex
		/// 
		/// <summary>	Gets the alpha index to show in the main menu and the map for the specified record.
		/// </summary>
		/// <param name="record">		The record.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public string AlphaIndex(SRiRecord record)
		{
			string index;
			//
			index = "?";
			for (int i = 0; i < Mappings.Count; i++)
				if (m_oRecordList[Mappings[i].Index].KeyVal.Equals(record.KeyVal))
			{
				index = i.ToAlphaIndex();
				break;
			}
			//
			return index;
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
				// TODO
				switch (iMap.Type)
				{
					case MappingType.Record:
						return m_oRecordList[iMap.Record.Value];
					case MappingType.Inspection:
						return m_oRecordList[iMap.Record.Value].Inspections[iMap.Inspection.Value];
					case MappingType.Visit:
						return m_oRecordList[iMap.Record.Value].Inspections[iMap.Inspection.Value].Visits[iMap.Visit.Value];
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
					UpdateProperty(PropertyType.RecordFilters);
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
			// TODO
			get { return false; }
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
		public List<RecordMapping> Mappings
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
		/// Name		RecordFromMapping
		/// 
		/// <summary>	Gets the record that is mapped by the record mapping.
		/// </summary>
		/// <param name="rMap">		The record mapping.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public SRiRecord RecordFromMapping(RecordMapping rMap)
		{
			if (rMap.Index >= 0 && rMap.Index < m_oRecordList.Count)
				return m_oRecordList[rMap.Index];
			else
				return null;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		RecordIsSelected
		/// 
		/// <summary>	Checks if the record mapped in the record mapping is the currently selected record.
		/// </summary>
		/// <param name="rMap">		The record mapping.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public bool RecordIsSelected(RecordMapping rMap)
		{
			return rMap.Index == m_nSelectedRecord;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		SelectedRecord
		/// 
		/// <summary>	Gets and sets the SelectedRecord.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public SRiRecord SelectedRecord
		{
			get
			{
				if (m_nSelectedRecord < 0 || m_nSelectedRecord >= m_oRecordList.Count)
					return null;
				else
					return m_oRecordList[m_nSelectedRecord];
			}
			set
			{
				if (value == null)
				{
					//SelectedVisit = null;
					m_nSelectedRecord = -1;
					UpdateProperty(PropertyType.SelectedRecord);
					UpdateProperty(PropertyType.SelectedVisit);
				}
				else if (SelectedRecord == null || !SelectedRecord.KeyVal.Equals(value.KeyVal))
				{
					//SelectedVisit = null;
					for (int i = 0; i < m_oRecordList.Count; i++)
						if (m_oRecordList[i].KeyVal.Equals(value.KeyVal))
						{
							m_nSelectedRecord = i;
							UpdateProperty(PropertyType.SelectedRecord);
							UpdateProperty(PropertyType.SelectedVisit);
							break;
						}
				}
			}
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
						m_oMappings.Sort(new RecordMappingComparer());
					//
					UpdateProperty(PropertyType.RecordFilters);
				}
			}
		}
		/// 
		public List<IndexUpdate> Update(List<SRiRecord> records, bool fromCache = false)
		{
			List<IndexUpdate> updates;
			List<OnSiteDocumentMetaParam> docMetaParams;
			bool recordExists;
			UpdateType updateType;
			//
			updates = new List<IndexUpdate>();
			docMetaParams = new List<OnSiteDocumentMetaParam>();
			foreach (SRiRecord r in records)
			{
				updateType = UpdateType.None;
				recordExists = false;
				for (int i = 0; i < m_oRecordList.Count; i++)
					if (m_oRecordList[i].KeyVal.Equals(r.KeyVal))
				{
					if (!fromCache)
						r.SetSavedStatus(m_oRecordList[i]);
					m_oRecordList[i] = r;
					updateType = UpdateType.Changed;
					recordExists = true;
					break;
				}
				//
				if (!recordExists)
				{
					updateType = UpdateType.New;
					m_oRecordList.Add(r);
				}
				//
				updates.Add(new IndexUpdate(-1, updateType));
				docMetaParams.Add(new OnSiteDocumentMetaParam()
				{
					Organisation = r.Organisation,
					Reference = r.RefVal
				});
			}
			//
			// If the data was not loaded from cache update the property.
			if (!fromCache)
				UpdateProperty(PropertyType.RecordData);
			//
			// Trigger the premises list as being updated. This will cause the new premises json
			// to be saved to cache. Also request configuration data for all organisations.
			RequestConfigs(fromCache);
			//
			// Update the premises mappings and gather the indicies for
			// the updated premises if they are currently mapped.
			UpdateMappings();
			for (int p = 0; p < records.Count; p++)
			{
				recordExists = false;
				for (int i = 0; i < Mappings.Count; i++)
				{
					if (m_oRecordList[Mappings[i].Index].KeyVal.Equals(records[p].KeyVal))
					{
						updates[p].PrimaryIndex = i;
						recordExists = true;
						break;
					}
				}
				//
				// If the updated Premises is not found in the filtered
				// list, mark the action as None as the Premises is not visible.
				if (!recordExists)
					updates[p].UpdateAction = UpdateType.None;
			}
			//
			UpdateProperty(PropertyType.RecordList);
			//
			// Either tell the app that the cache has loaded, or tell the app to get
			// the document meta data for the premises that were just downloaded.
			if (fromCache)
				UpdateProperty(PropertyType.CacheLoaded);
			else if (AppData.Environment != OnSiteEnvironments.Sales)
				RequestDocumentMeta(docMetaParams);
			//
			return updates;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Private Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		private void RequestConfigs(bool fromCache)
		{
			// TODO
		}
		/// 
		private void RequestDocumentMeta(List<OnSiteDocumentMetaParam> docParams)
		{
			// TODO
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
			RecordMapping mapping;
			//
			m_oMappings = new List<RecordMapping>();
			for (int i = 0; i < m_oRecordList.Count; i++)
			{
				mapping = new RecordMapping(m_oRecordList[i], i);
				if (mapping.Active.Count > 0)
					m_oMappings.Add(mapping);
			}
			//
			if (m_oMappings.Count > 0)
				m_oMappings.Sort(new RecordMappingComparer());
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}
