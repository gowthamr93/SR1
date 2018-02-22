using System.Collections.Generic;

using Newtonsoft.Json;

namespace Idox.LGDP.Apps.Common.OnSite
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			OnSiteUpdateDetail
	/// 
	/// <summary>		Details returned in the response of an update.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class OnSiteUpdateDetail
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Key
		/// 
		/// <summary>	Gets and sets the Key for the detail.
		/// </summary>
		/// <remarks>	For CP this is the ID of the item that was updated, ie: visid or rskid.
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("key")]
		public string Key { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Results
		/// 
		/// <summary>	Gets and sets the Results for the detail.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("results")]
		public List<OnSiteUpdateItem> Results { get; set; }
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}

