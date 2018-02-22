using System;

using Idox.LGDP.Apps.Common.OnSite;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
	public class SelectedAction : SelectedBase
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructor
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		SelectedAction
		/// 
		/// <summary>	Creates a new instance of the SelectedAction class.
		/// </summary>
		/// <param name="action">		The action being selected.</param>
		/// <param name="iMap">			The index mapping of the action selection.</param>
		/// <param name="isNew">		Whethher the entity is a new creation.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public SelectedAction(SRiAction action, IndexMapping iMap, bool isNew)
			: base(iMap, isNew)
		{
			Exception error;
			//
			OnSiteSettings.ShouldSerialize = ShouldSerializeRule.Full;
			Action = SRiAction.FromJson(action.ToJson(), out error);
			Action.Status = Action.Status.SetNewOrChanged(isNew ? SyncStatus.New : SyncStatus.Changed);
            if (error != null) throw error;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		public SRiAction Action
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

		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}
