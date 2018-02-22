using System.Collections.Generic;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			RecordMapping
	/// 
	/// <summary>		
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class RecordMapping
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
		/// <param name="record">		The record to map.</param>
		/// <param name="index">		The index of the record.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public RecordMapping(SRiRecord record, int index)
		{
			Index = index;
			CreateMappings(record);
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Active
		/// 
		/// <summary>	Gets and sets the Active mappings for the Record.
		/// </summary>
		/// <remarks>	These are the items that will be visible in the main menu. If the collection
		/// 			is empty then the entire Record element should not be included in the main menu.
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public List<IndexMapping> Active
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
		/// <param name="record">		The record.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		private void CreateMappings(SRiRecord record)
		{
			List<IndexMapping> active;
			SRiVisit visit;
			//
			Active = new List<IndexMapping>();
			for (int i = 0; i < record.Inspections.Count; i++)
			{
				// Create temporary collection for storing the mappings for this inspection.
				active = new List<IndexMapping>();
				//
				// Find mappings for Visits.
				for (int v = 0; v < record.Inspections[i].Visits.Count; v++)
				{
					visit = record.Inspections[i].Visits[v];
					switch (AppData.RecordModel.Filter)
					{
						case FilterMode.All:
							active.Add(new IndexMapping(Index, i, v, null));
						break;
						case FilterMode.Complete:
							//if (visit.Completed)
							//	active.Add(new IndexMapping(Index, i, v, null));
						break;
						case FilterMode.Incomplete:
							//if (!visit.Completed)
							//	active.Add(new IndexMapping(Index, i, v, null));
						break;
					}
					// Note that we don't add Actions here because they are not visible on the left hand menu as cells.
				}
				//
				// If there are mappings for this Inspection then add them to
				// the mapping collection, with an entry for the inspection as well.
				if (active.Count > 0)
				{
					Active.Add(new IndexMapping(Index, i, null, null));
					Active.AddRange(active);
				}
			}
			//
			// If the Active mapping collection has entries, insert an entry at the start for the Record.
			if (Active.Count > 0)
				Active.Insert(0, new IndexMapping(Index, null, null, null));
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}
