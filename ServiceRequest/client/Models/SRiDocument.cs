using Newtonsoft.Json;

using Idox.LGDP.Apps.Common.OnSite;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			SRiDocument
	/// 
	/// <summary>		Contains information about a Document attached to an SR entity.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class SRiDocument : OnSiteJsonEntity<SRiDocument>
	{
		/// ------------------------------------------------------------------------------------------------
		#region Json Properties
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Id
		/// 
		/// <summary>	Gets and sets the Id for the Document.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("id")]
		public string Id { get; set; }
		public bool ShouldSerializeId() { return OnSiteSettings.ShouldSerialize != ShouldSerializeRule.DataForAPI; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Name
		/// 
		/// <summary>	Gets and sets the Name for the Document.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("name")]
		public string Name { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Extension
		/// 
		/// <summary>	Gets and sets the Extension for the Document.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("extension")]
		public string Extension { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Organisation
		/// 
		/// <summary>	Gets and sets the Organisation for the Document.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("organisation")]
		public string Organisation { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Size
		/// 
		/// <summary>	Gets and sets the Size for the Document.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("size")]
		public int? Size { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Hash
		/// 
		/// <summary>	Gets and sets the Hash for the Document.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("hash")]
		public string Hash { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Description
		/// 
		/// <summary>	Gets and sets the Description for the Document.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("description")]
		public string Description { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Category
		/// 
		/// <summary>	Gets and sets the Category for the Document.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("category")]
		public string Category { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		MimeType
		/// 
		/// <summary>	Gets and sets the MimeType for the Document.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("mimetype")]
		public string MimeType { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		PremisesAddress
		/// 
		/// <summary>	Gets and sets the PremisesAddress for the Document.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("address")]
		public string RecordAddress { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		PremisesTradingAs
		/// 
		/// <summary>	Gets and sets the PremisesTradingAs for the Document.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("tradingas")]
		public string RecordTradeName { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		RecordReference
		/// 
		/// <summary>	Gets and sets the RecordReference for the Document.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("reference")]
		public string RecordReference { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Status
		/// 
		/// <summary>	Gets and sets the sync status for the premises.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("status")]
		public SyncStatus Status { get; set; }
		public bool ShouldSerializeStatus() { return OnSiteSettings.ShouldSerialize != ShouldSerializeRule.DataForAPI; }
		/// 
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Data
		/// 
		/// <summary>	Gets and sets the Data for the document.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonIgnore]
		public byte[] Data { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		FileName
		/// 
		/// <summary>	Gets the file name to use when storing the document data in the local system.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonIgnore]
		public string FileName
		{
			get { return string.Format("{0}.{1}", Id, Extension); }
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}
