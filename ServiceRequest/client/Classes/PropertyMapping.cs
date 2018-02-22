using System.Collections.Generic;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			PropertyMapping
	/// 
	/// <summary>		Creates a flat list of mappings for the main menu that relate to the child items
	/// 				of a SRiProperty.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class PropertyMapping
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructor
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		RecordMapping
		/// 
		/// <summary>	Creates a new instance of the RecordMapping class.
		/// </summary>
		/// <param name="property">		The record to map.</param>
		/// <param name="index">		The index of the record.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public PropertyMapping(SRiProperty property, int index)
		{
			Index = index;
			CreateMappings(property);
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		ActiveCollapsed
		/// 
		/// <summary>	Gets and sets the ActiveCollapsed mappings for the Record.
		/// </summary>
		/// <remarks>	These are the items that will be visible in the main menu when the property is not
		/// 			selected. If the collection	is empty then the entire Record element should not
		/// 			be included in the main menu.
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public List<IndexMapping> ActiveCollapsed
		{
			get;
			private set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		ActiveExpanded
		/// 
		/// <summary>	Gets and sets the ActiveExpanded mappings for the Record.
		/// </summary>
		/// <remarks>	These are the items that will be visible in the main menu when the property is
		/// 			selected. If the collection	is empty then the entire Record element should not
		/// 			be included in the main menu.
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public List<IndexMapping> ActiveExpanded
		{
			get;
			private set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Index
		/// 
		/// <summary>	The index of the mapped Record.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public int Index
		{
			get;
			private set;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Private Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		CreateMappings
		/// 
		/// <summary>	Creates the mapping collection from the record.
		/// </summary>
		/// <param name="property">		The record.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		private void CreateMappings(SRiProperty property)
		{
			List<IndexMapping> active;
			//
			ActiveCollapsed = new List<IndexMapping>();
			ActiveExpanded = new List<IndexMapping>();
			for (int rg = 0; rg < property.RequestGroups.Count; rg++)
			{
				// Each request group is actually a set or records grouped by request type. If the filter
				// by type property is set, make sure that the request group type matches.
				if (AppData.PropertyModel.FilterByType == null ||
				    property.RequestGroups[rg].GroupType.Equals(AppData.PropertyModel.FilterByType))
				{
					active = new List<IndexMapping>();
					for (int r = 0; r < property.RequestGroups[rg].Records.Count; r++)
					{
						switch (AppData.PropertyModel.Filter)
						{
							case FilterMode.All:
								active.Add(new IndexMapping(Index, rg, r));
							break;
							case FilterMode.Complete:
								if (CheckComplete(property.RequestGroups[rg].Records[r].Record))
									active.Add(new IndexMapping(Index, rg, r));
							break;
							case FilterMode.Incomplete:
								if (!CheckComplete(property.RequestGroups[rg].Records[r].Record))
									active.Add(new IndexMapping(Index, rg, r));
							break;
						}
					}
					//
					if (active.Count > 0)
					{
						ActiveCollapsed.Add(new IndexMapping(Index, rg));
						ActiveExpanded.Add(new IndexMapping(Index, rg));
						ActiveExpanded.AddRange(active);
					}
				}
			}
			//
			if (ActiveCollapsed.Count > 0)
				ActiveCollapsed.Insert(0, new IndexMapping(Index, null, null));
			if (ActiveExpanded.Count > 0)
				ActiveExpanded.Insert(0, new IndexMapping(Index, null, null));
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		CheckComplete
		/// 
		/// <summary>	Checks if all the visits in the record are complete. If they are True is returned.
		/// </summary>
		/// <param name="record">		The record.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		private bool CheckComplete(SRiRecord record)
		{
			bool incomplete;
			//
			incomplete = false;
			foreach (var i in record.Inspections)
			{
				foreach (var v in i.Visits)
					if (!v.Visit.DateVisit.HasValue)
					{
						incomplete = true;
						break;
					}
				if (incomplete) break;
			}
			return !incomplete;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}
