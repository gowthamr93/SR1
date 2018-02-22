using System;

using Idox.LGDP.Apps.Common.OnSite;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
	public class SelectedVisit : SelectedBase
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructor
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		SelectedVisit
		/// 
		/// <summary>	Creates a new instance of the SelectedVisit class.
		/// </summary>
		/// <param name="visit">		The visit being selected.</param>
		/// <param name="iMap">			The index mapping of the visit selection.</param>
		/// <param name="isNew">		Whether the entity is a new creation.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public SelectedVisit(SRiVisitMeta visit, IndexMapping iMap, bool isNew)
			: base(iMap, isNew)
		{
			Exception error;
			//
			OnSiteSettings.ShouldSerialize = ShouldSerializeRule.Full;
			Visit = SRiVisitMeta.FromJson(visit.ToJson(), out error);
			Visit.Status = Visit.Status.SetNewOrChanged(isNew ? SyncStatus.New : SyncStatus.Changed);
            if (error != null) throw error;
			SetGroupMod();
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		GroupMod
		/// 
		/// <summary>	Gets and sets the GroupMod from the XIREC that visit selection is contained in.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public string GroupMod
		{
			get;
			private set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		MappingForAction
		/// 
		/// <summary>	Gets an IndexMapping for a specific action.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public IndexMapping MappingForAction(int actionIndex)
		{
			return new IndexMapping(IndexMap.Property,
			                        IndexMap.RequestGroup,
			                        IndexMap.Record,
			                        IndexMap.Inspection,
			                        IndexMap.Visit, actionIndex);
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		MappingForTreatment
		/// 
		/// <summary>	Gets an IndexMapping for a specific treatment.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public IndexMapping MappingForTreatment(int treatmentIndex)
		{
			return new IndexMapping(IndexMap.Property,
			                        IndexMap.RequestGroup,
			                        IndexMap.Record,
			                        IndexMap.Inspection,
			                        IndexMap.Visit, null, treatmentIndex);
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		NewActionMapping
		/// 
		/// <summary>	Gets an IndexMapping for a new action.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public IndexMapping NewActionMapping
		{
			get
			{
				return new IndexMapping(IndexMap.Property,
				                        IndexMap.RequestGroup,
				                        IndexMap.Record,
				                        IndexMap.Inspection,
				                        IndexMap.Visit, -1);
			}
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		NewTreatmentMapping
		/// 
		/// <summary>	Gets an IndexMapping for a new treatment.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public IndexMapping NewTreatmentMapping
		{
			get
			{
				return new IndexMapping(IndexMap.Property,
				                        IndexMap.RequestGroup,
				                        IndexMap.Record,
				                        IndexMap.Inspection,
				                        IndexMap.Visit, null, -1);
			}
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Visit
		/// 
		/// <summary>	Gets and sets the Visit entity.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public SRiVisitMeta Visit
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
		/// Name		SetGroupMod
		/// 
		/// <summary>	Gets the instance of the Inspection that contains this visit and extracts the
		/// 			GroupMod code from the Inspection for future use. If the Inspection cannot be
		/// 			found then a blank string is used as the GroupMod.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		private void SetGroupMod()
		{
			IndexMapping iMap;
			SRiInspection inspection;
			//
			iMap = new IndexMapping(IndexMap.Property,
			                        IndexMap.RequestGroup.Value,
			                        IndexMap.Record.Value,
			                        IndexMap.Inspection.Value);
			inspection = AppData.PropertyModel.EntityFromMapping(iMap) as SRiInspection;
			if (inspection != null)
				GroupMod = inspection.GroupMod;
			else
				GroupMod = "";
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}
