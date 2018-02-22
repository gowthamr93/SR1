using System.Collections.Generic;
using System.Linq;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
	public class FilterTypeCollection : Dictionary<string, string>
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructor
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		FilterTypeCollection
		/// 
		/// <summary>	Creates a new instance of the FilterTypeCollection class.
		/// </summary>
		/// <param name="properties">		The properties that are going to be filtered by request type.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public FilterTypeCollection(List<SRiProperty> properties)
		{
			Dictionary<string, string> temp;
			//
			// Use the temporary dictionary as a way of collecting the values before adding them
			// to the local collection in Value order.
			temp = new Dictionary<string, string>();
			foreach (var p in properties)
				foreach (var rg in p.RequestGroups)
					if (!temp.ContainsKey(rg.GroupType))
						temp.Add(rg.GroupType, rg.Name);
			//
			foreach (var v in temp.OrderBy(t => t.Value))
				Add(v.Key, v.Value);
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		this[int]
		/// 
		/// <summary>	Gets the dictionary entry at the given index.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public KeyValuePair<string, string> this[int index]
		{
			get
			{
				KeyValuePair<string, string> result;
				int i;
				//
				i = 0;
				foreach (var filterType in this)
					if (i == index)
						result = filterType;
					else
						i++;
				//
				return result;
			}
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		TryGetAtIndex
		/// 
		/// <summary>	Tries to get the item at the index. Returns true if the item is found.
		/// </summary>
		/// <param name="index">	The index to look at.</param>
		/// <param name="item">		The item if found.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public bool TryGetAtIndex(int index, out KeyValuePair<string, string> item)
		{
			int i;
			//
			i = 0;
			foreach (var filterType in this)
				if (i == index)
				{
					item = filterType;
					return true;
				}
				else
					i++;
			//
			return false;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------

	}
}
