using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using Idox.LGDP.Apps.Common.OnSite;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			SRiRequestGroup
	/// 
	/// <summary>		Contains a selection of SRiRecord objects that all share the same type.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class SRiRequestGroup : OnSiteJsonEntity<SRiRequestGroup>
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructor
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		SRiRequestGroup
		/// 
		/// <summary>	Creates a new instnace of the SRiRequestGroup class using a SRiRecordMeta as
		/// 			the starting pram.
		/// </summary>
		/// <param name="record">		The record to create them all.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public SRiRequestGroup(SRiRecordMeta record)
		{
			Name = record.RequestTypeDescription;
			GroupType = record.Record.RequestType;
			Records = new List<SRiRecordMeta>() { record };
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Json Properties
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Name
		/// 
		/// <summary>	Gets and sets the Name for the group.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("name")]
		public string Name
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		GroupType
		/// 
		/// <summary>	Gets and sets the GroupType for the group.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("groupType")]
		public string GroupType
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Records
		/// 
		/// <summary>	Gets and sets the Records for the group.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("records")]
		public List<SRiRecordMeta> Records
		{
			get;
			set;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		EarliestTargetDate
		/// 
		/// <summary>	Gets the earliest target response date from all the records.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonIgnore]
		public DateTime? EarliestTargetDate
		{
			get
			{
				DateTime? dt = null;
				foreach (var r in Records)
					if (!dt.HasValue || r.Record.TargetResponseDate.HasValue && r.Record.TargetResponseDate.Value < dt.Value)
						dt = r.Record.TargetResponseDate;
				//
				return dt;
			}
		}

	    [JsonIgnore]
	    public DateTime? ScheduleDate
	    {
	        get
	        {
                DateTime? dt = null;
                foreach (var rec in Records)
                {
                    foreach (var res in rec.Record.Inspections)
                    {
                        if (!dt.HasValue ||
                            res.EarliestScheduledDate.HasValue && res.EarliestScheduledDate.Value < dt.Value)
                            dt = res.EarliestScheduledDate;
                    }
                }
                return dt;
            }
        }

        public List<SRiRecordMeta> TempRecords
        {
            get;
            set;
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
