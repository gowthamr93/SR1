using Newtonsoft.Json;
using System.Collections.Generic;

namespace Idox.LGDP.Apps.Common.OnSite
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			OnSiteUserOrganisation
	/// 
	/// <summary>		Organisation details contained within the user details as returned from 
	/// 				the Idox On-Site Authentication portal.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class OnSiteUserOrganisation
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Organisation
		/// 
		/// <summary>	Gets and sets the Organisation for the organisation.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("organisation")]
		public OnSiteOrganisation Organisation
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		IsAdmin
		/// 
		/// <summary>	Gets and sets the IsAdmin for the organisation.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("isAdmin")]
		public bool IsAdmin
		{
			get;
			set;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}

