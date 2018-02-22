using System;
using System.Collections.Generic;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
	public class RecordMappingComparer : IComparer<RecordMapping>
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Compare
		/// 
		/// <summary>	Compares PremisesMapping x and y using the sort mode provided in the constructor.
		/// </summary>
		/// <param name="x">		Object x.</param>
		/// <param name="y">		Object y.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public int Compare(RecordMapping x, RecordMapping y)
		{
			int result;
			SRiRecord recX, recY;
			//
			recX = AppData.RecordModel.EntityFromMapping(new IndexMapping(x.Index)) as SRiRecord;
			recY = AppData.RecordModel.EntityFromMapping(new IndexMapping(y.Index)) as SRiRecord;
			switch (AppData.RecordModel.Sort)
			{
				case SortMode.Alphabetical:
					result = CompareAlpha(recX, recY);
				break;
				case SortMode.AlphabeticalInverse:
					result = CompareAlphaInverse(recX, recY);
				break;	
				case SortMode.Date:
					result = CompareDate(recX, recY);
				break;
				case SortMode.Distance:
					result = CompareDistance(recX, recY);
				break;
				default :
					result = 0;
				break;
			}
			//
			return result;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Private Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		CompareAlpha
		/// 
		/// <summary>	Compares SRiRecord x and y based on an ascending alphabetical sort order.
		/// </summary>
		/// <param name="x">		Object x.</param>
		/// <param name="y">		Object y.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		int CompareAlpha(SRiRecord x, SRiRecord y)
		{
			return StringComparer.CurrentCulture.Compare(x.TradeName, y.TradeName);
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		CompareAlphaInverse
		/// 
		/// <summary>	Compares SRiRecord x and y based on a descending alphabetical sort order.
		/// </summary>
		/// <param name="x">		Object x.</param>
		/// <param name="y">		Object y.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		int CompareAlphaInverse(SRiRecord x, SRiRecord y)
		{
			int result;
			//
			switch (CompareAlpha(x, y))
			{
				case 1:
					result = -1;
				break;
				case -1:
					result = 1;
				break;
				default :
					result = 0;
				break;
			}
			//
			return result;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		CompareDate
		/// 
		/// <summary>	Compares SRiRecord x and y based on the premises earliest due date.
		/// </summary>
		/// <param name="x">		Object x.</param>
		/// <param name="y">		Object y.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		int CompareDate(SRiRecord x, SRiRecord y)
		{
			int result;
			//
			if (x.EarliestDueDate.HasValue && y.EarliestDueDate.HasValue)
				result = DateTime.Compare(x.EarliestDueDate.Value, y.EarliestDueDate.Value);
			else if (x.EarliestDueDate.HasValue)
				result = 1;
			else if (y.EarliestDueDate.HasValue)
				result = -1;
			else
				result = 0;
			//
			return result;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		CompareDistance
		/// 
		/// <summary>	Compares SRiRecord x and y based on the premises distance from the user.
		/// </summary>
		/// <param name="x">		Object x.</param>
		/// <param name="y">		Object y.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		int CompareDistance(SRiRecord x, SRiRecord y)
		{
			int result;
			//
			if (x.DistanceFromUser < y.DistanceFromUser)
				result = 1;
			else if (y.DistanceFromUser < x.DistanceFromUser)
				result = -1;
			else
				result = 0;
			//
			return result;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}
