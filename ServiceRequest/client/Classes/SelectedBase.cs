using System;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
	public class SelectedBase
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructor
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		SelectedBase
		/// 
		/// <summary>	Creates a new instance of the SelectedBase class.
		/// </summary>
		/// <param name="iMap">		The index mapping of the selected entity.</param>
		/// <param name="isNew">	Whether the entity is a new creation.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public SelectedBase(IndexMapping iMap, bool isNew)
		{
			IndexMap = iMap;
			IsNew = isNew;
			Edited = false;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Edited
		/// 
		/// <summary>	Gets and sets the Edited flag for the selection.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public bool Edited
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		IndexMap
		/// 
		/// <summary>	Gets and sets the IndexMap for the selection.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public IndexMapping IndexMap
		{
			get;
			private set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		IsNew
		/// 
		/// <summary>	Gets and sets the IsNew flag for the selection.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public bool IsNew
		{
			get;
			private set;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}
