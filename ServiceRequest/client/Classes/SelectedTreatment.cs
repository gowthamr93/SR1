using System;

using Idox.LGDP.Apps.Common.OnSite;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
	public class SelectedTreatment : SelectedBase
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructor
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		SelectedTreatment
		/// 
		/// <summary>	Creates a new instance of the SelectedTreatment class.
		/// </summary>
		/// <param name="visit">		The treatment being selected.</param>
		/// <param name="iMap">			The index mapping of the treatment selection.</param>
		/// <param name="isNew">		Whethher the entity is a new creation.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public SelectedTreatment(SRiTreatment treatment, IndexMapping iMap, bool isNew)
			: base(iMap, isNew)
		{
			Exception error;
			//
			OnSiteSettings.ShouldSerialize = ShouldSerializeRule.Full;
			Treatment = SRiTreatment.FromJson(treatment.ToJson(), out error);
			Treatment.Status = Treatment.Status.SetNewOrChanged(isNew ? SyncStatus.New : SyncStatus.Changed);
            if (error != null) throw error;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Treatment
		/// 
		/// <summary>	Gets and sets the Treatment for the selection.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public SRiTreatment Treatment
		{
			get;
			private set;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}
