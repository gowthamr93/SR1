using Newtonsoft.Json;
using System.Collections.Generic;

using Idox.LGDP.Apps.Common.OnSite;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			SRiRecordInfo
	/// 
	/// <summary>		Contains extra information to be attached to the SRiRecord.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class SRiRecordInfo : OnSiteJsonEntity<SRiRecordInfo>
	{
		/// ------------------------------------------------------------------------------------------------
		#region Json Properties
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Title
		/// 
		/// <summary>	Gets and sets the Title for the record info.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("title")]
		public string Title
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Properties
		/// 
		/// <summary>	Gets and sets the Properties for the record info.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("properties")]
		public Dictionary<string, string> Properties
		{
			get;
			set;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}
