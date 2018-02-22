using System.Collections.Generic;

namespace Idox.LGDP.Apps.Common.OnSite
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			IndexUpdate
	/// 
	/// <summary>		Describes an update action on a table or list, giving the update action that was
	/// 				performed and the index for where in the collection it was performed.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class IndexUpdate
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructors
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		IndexUpdate
		/// 
		/// <summary>	Creates a new instance of the IndexUpdate class.
		/// </summary>
		/// <param name="index">				The primary index of the update.</param>
		/// <param name="action">				The type of update.</param>
		/// <param name="secondaryIndicies">	Any other indicies needed to identify the updated element.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public IndexUpdate(int index, UpdateType action, params int[] secondaryIndicies)
		{
			PrimaryIndex = index;
			UpdateAction = action;
			SecondaryIndicies = new List<int>();
			foreach (int i in secondaryIndicies)
				SecondaryIndicies.Add(i);
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		IsEmpty
		/// 
		/// <summary>	Returns true if the IndexUpdate is set to Empty.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public bool IsEmpty
		{
			get
			{
				if (PrimaryIndex < 0 && UpdateAction == UpdateType.None)
					return true;
				else
					return false;
			}
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		PrimaryIndex
		/// 
		/// <summary>	Gets and sets the PrimaryIndex of the update.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public int PrimaryIndex
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		SecondaryIndicies
		/// 
		/// <summary>	Gets and sets the SecondaryIndicies of the update.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public List<int> SecondaryIndicies
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		UpdateAction
		/// 
		/// <summary>	Gets and sets the UpdateAction for the update.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public UpdateType UpdateAction
		{
			get;
			set;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Static Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Empty
		/// 
		/// <summary>	Returns an empty update.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public static IndexUpdate Empty
		{
			get
			{
				return new IndexUpdate(-1, UpdateType.None);
			}
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}

