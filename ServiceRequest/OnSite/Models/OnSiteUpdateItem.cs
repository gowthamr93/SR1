using Newtonsoft.Json;

namespace Idox.LGDP.Apps.Common.OnSite
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			OnSiteUpdateItem
	/// 
	/// <summary>		Update result item that refers to the update result of a particular 
	/// 				item that was updated.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class OnSiteUpdateItem
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Success
		/// 
		/// <summary>	Gets and sets the Success for the item.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("success")]
		public bool Success { get; set; }
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}

