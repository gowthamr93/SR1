using System.Collections.Generic;
using Newtonsoft.Json;

using Idox.LGDP.Apps.Common.OnSite;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			SRiCNOfficer
	/// 
	/// <summary>		Used for consuming CNOfficer data from the API.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class SRiCNOfficer
	{
		/// ------------------------------------------------------------------------------------------------
		#region Json Properties
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("modified")]
		public bool Modified
		{
			get;
			set;
		}
		/// 
		[JsonProperty("cnofficer0offcode")]
		public string Code
		{
			get;
			set;
		}
		/// 
		[JsonProperty("cnofficer0name")]
		public string Name
		{
			get;
			set;
		}
		/// 
		[JsonProperty("cnofficer0idoxid")]
		public string IdoxID
		{
			get;
			set;
		}
		/// 
		[JsonProperty("cnofficer0jobtitle")]
		public string JobTitle
		{
			get;
			set;
		}
		/// 
		[JsonProperty("cnofficer0historic")]
		public string Historic
		{
			get;
			set;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
	/// ----------------------------------------------------------------------------------------------------
	#region Meta & Data
	/// ----------------------------------------------------------------------------------------------------
	/// 
	/// ----------------------------------------------------------------------------------------------------
	/// Name		SRiCNOfficerMeta
	/// 
	/// <summary>	Contains the meta properties for a SRiCNOfficer entity.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class SRiCNOfficerMeta
	{
		/// ------------------------------------------------------------------------------------------------
		#region Json Properties
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		ID
		/// 
		/// <summary>	Gets and sets the ID for the meta.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("id")]
		public string ID
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Organisation
		/// 
		/// <summary>	Gets and sets the Organisation for the meta.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("organisation")]
		public string Organisation
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Received
		/// 
		/// <summary>	Gets and sets the Received for the meta.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("received")]
		public string Received
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Version
		/// 
		/// <summary>	Gets and sets the Version for the meta.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("version")]
		public string Version
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Officer
		/// 
		/// <summary>	Gets and sets the Officer for the meta.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("data")]
		public SRiCNOfficer Officer
		{
			get;
			set;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
	/// 
	/// ----------------------------------------------------------------------------------------------------
	/// Name		SRiCNOfficerData
	/// 
	/// <summary>	The data layer for the SRiCNOfficer entities.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class SRiCNOfficerData : OnSiteJsonEntity<SRiCNOfficerData>
	{
		/// ------------------------------------------------------------------------------------------------
		#region Json Properties
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Module
		/// 
		/// <summary>	Gets and sets the Module for the data.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("module")]
		public string Module
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		AppModule
		/// 
		/// <summary>	Gets and sets the AppModule for the data.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("appModule")]
		public string AppModule
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Entity
		/// 
		/// <summary>	Gets and sets the Entity for the data.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("entity")]
		public string Entity
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Officers
		/// 
		/// <summary>	Gets and sets the Officers for the data.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("cases")]
		public List<SRiCNOfficerMeta> Officers
		{
			get;
			set;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
	/// 
	#endregion
	/// ----------------------------------------------------------------------------------------------------
}
