using System;
using System.Collections.Generic;

using Idox.LGDP.Apps.Common.OnSite;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
	public class SelectedRecord : SelectedBase
	{
		/// ------------------------------------------------------------------------------------------------
		#region Private Variables
		/// ------------------------------------------------------------------------------------------------
		/// 
		private List<SRiXRRecSummary> m_oRisks;
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Cosntructor
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		SelectedRecord
		/// 
		/// <summary>	Creates a new instance of the SelectedRecord class, using json serialisation to
		/// 			copy the record data from the original source.
		/// </summary>
		/// <param name="record">		The record selected.</param>
		/// <param name="iMap">			The index mapping of the record.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public SelectedRecord(SRiRecordMeta record, IndexMapping iMap, bool isNew)
			: base(iMap, false)
		{
			Exception error;
			//
			OnSiteSettings.ShouldSerialize = ShouldSerializeRule.Full;
			Record = SRiRecordMeta.FromJson(record.ToJson(), out error);
            Record.Record.Status = Record.Record.Status.SetNewOrChanged(isNew ? SyncStatus.New : SyncStatus.Changed);
			Record.Record.UpdateForSelection();
			if (error != null) throw error;
			//
			CreateMappings();
			CreateRisks();
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		AddNewInspection
		/// 
		/// <summary>	Tries to add a new inspection.
		/// </summary>
		/// <param name="inspectionType">		The inspection type.</param>
		/// <param name="failReason">			The fail reason.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public bool AddNewInspection(string inspectionType, out string failReason)
		{
			foreach (var inspection in Record.Record.Inspections)
				if (inspection.InspectionType.Equals(inspectionType))
				{
					failReason = "Cannot add an inspection of the same type as one that already exists.";
					return false;
				}
			//
			Edited = true;
			failReason = "";
			Record.Record.Inspections.Add(new SRiInspection()
			{
				KeyVal = OnSiteSettings.NewTempKey,
				InspectionType = inspectionType,
				ParentKeyVal = Record.Record.KeyVal,
				RiskRecords = new List<SRiXRRec>(),
				Visits = new List<SRiVisitMeta>()
			});
			return true;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		NewVisitMapping
		/// 
		/// <summary>	Gets the index mapping for a new visit.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public IndexMapping NewVisitMapping(int inspectionIndex)
		{
			return new IndexMapping(IndexMap.Property,
			                        IndexMap.RequestGroup.Value,
			                        IndexMap.Record.Value,
			                        inspectionIndex, -1);
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		InspectionTypeDescription
		/// 
		/// <summary>	Gets the type description for the inspection that has the given keyval.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public string InspectionTypeDescription(string keyVal)
		{
			foreach (var i in Record.Record.Inspections)
				if (i.KeyVal.Equals(keyVal))
					return i.InspectionTypeDescription;
			//
			return "No inspection type";
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		InspectionTypeDescription
		/// 
		/// <summary>	Gets the InspectionTypeDescription of the Inspection that contains the entity
		/// 			that is mapped.
		/// </summary>
		/// <param name="iMap">		The entity mapping. Can be Visit, Action or Treatment.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public string InspectionTypeDescription(IndexMapping iMap)
		{
			SRiInspection inspection;
			//
			if (iMap.RequestGroup.HasValue &&
			    iMap.Record.HasValue &&
			    iMap.Inspection.HasValue)
			{
				inspection = AppData.PropertyModel.EntityFromMapping(new IndexMapping(iMap.Property,
				                                                                      iMap.RequestGroup.Value,
				                                                                      iMap.Record.Value,
				                                                                      iMap.Inspection.Value)) as SRiInspection;
				if (inspection != null)
					return inspection.InspectionTypeDescription;
			}
			//
			return "No inspection type";
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Record
		/// 
		/// <summary>	Gets and sets the Record.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public SRiRecordMeta Record
		{
			get;
			private set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		CustomerMappings
		/// 
		/// <summary>	Gets and sets the CustomerMappings for the record. Allows the Customer/Contact 
		/// 			tree to be displayed in a flat list.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public List<FieldMapping> CustomerMappings
		{
			get;
			private set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		RiskAssessments
		/// 
		/// <summary>	Gets the collection of risk assessments for all inspections under this record.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public List<SRiXRRecSummary> RiskAssessments
		{
			get { return m_oRisks; }
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		InspectionMappings
		/// 
		/// <summary>	Gets and sets the InspectionMappings for the record. Allows the Inspection/Visit 
		/// 			tree to be displayed in a flat list.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public List<IndexMapping> VisitMappings
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
		/// <summary>	Creates the mappings list for the Inspections/Visits and Customer/Contacts.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public void CreateMappings()
		{
			VisitMappings = new List<IndexMapping>();
			for (int i = 0; i < Record.Record.Inspections.Count; i++)
			{
				VisitMappings.Add(new IndexMapping(IndexMap.Property, 
				                                   IndexMap.RequestGroup, 
				                                   IndexMap.Record, i));
				for (int v = 0; v < Record.Record.Inspections[i].Visits.Count; v++)
					VisitMappings.Add(new IndexMapping(IndexMap.Property, 
					                                   IndexMap.RequestGroup, 
					                                   IndexMap.Record, i, v));
			}
			//
			CustomerMappings = new List<FieldMapping>();
			foreach (var customer in Record.Record.Customers)
			{
				if (CustomerMappings.Count > 0) // Only add a header if this isn't the first customer.
					CustomerMappings.Add(new FieldMapping("", "", FieldType.CustomerHeader));
				CustomerMappings.Add(new FieldMapping("Customer type", customer.CustomerTypeDescription, FieldType.CustomerField));
				CustomerMappings.Add(new FieldMapping("Name", customer.Name, FieldType.CustomerField));
                CustomerMappings.Add(new FieldMapping("Address", customer.Address, FieldType.CustomerField));
			    foreach (var contact in customer.Contacts)
			    {
			        if (contact.ContactType == "PHONEH")
                        contact.ContactDescription = "Phone";
			        if (contact.ContactType == "EMAIL")
                        contact.ContactDescription = "Email";
			        if (contact.ContactType == "MOBILE")
                        contact.ContactDescription = "Mobile";
			        CustomerMappings.Add(new FieldMapping(contact.ContactDescription, contact.ContactAddress, FieldType.CustomerField));
			    }
			}
		}
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		CreateRisks
        /// 
        /// <summary>	Creates the collection of risk assessments from the the inspections in the record.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private void CreateRisks()
		{
			m_oRisks = new List<SRiXRRecSummary>();
			foreach (var i in Record.Record.Inspections)
				foreach (var r in i.RiskRecords)
					m_oRisks.Add(new SRiXRRecSummary(r, InspectionTypeDescription(i.KeyVal)));
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}
