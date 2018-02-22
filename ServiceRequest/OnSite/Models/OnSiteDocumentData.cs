using Newtonsoft.Json;
using System.Collections.Generic;

namespace Idox.LGDP.Apps.Common.OnSite
{
	public class OnSiteDocumentData : OnSiteJsonEntity<OnSiteDocumentData>
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructor
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		OnSiteDocumentData
		/// 
		/// <summary>	Creates a new instnace of the OnSiteDocumentData class.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public OnSiteDocumentData ()
		{
			Module = "";
			Reference = "";
			Documents = new List<OnSiteDocument>();
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Json Properties
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
		public string Module { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Reference
		/// 
		/// <summary>	Gets and sets the Reference for the data.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("ref")]
		public string Reference { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Documents
		/// 
		/// <summary>	Gets and sets the Documents for the data.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("docs")]
		public List<OnSiteDocument> Documents { get; set; }
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}

