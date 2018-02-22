using Newtonsoft.Json;

namespace Idox.LGDP.Apps.Common.OnSite
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			OnSiteUserProfile
	/// 
	/// <summary>		Users profile contained within the user details as returned from 
	/// 				the idox On-Site Authentication portal.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class OnSiteUserProfile
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructors
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		OnSiteUserProfile
		/// 
		/// <summary>	Creates a new instance of the OnSiteUserProfile class.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public OnSiteUserProfile()
		{
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Id
		/// 
		/// <summary>	Gets and sets the ID for the user.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("id")]
		public string Id { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		EmailAddress
		/// 
		/// <summary>	Gets and sets the EmailAddress for the user.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("emailAddress")]
		public string EmailAddress { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		FirstName
		/// 
		/// <summary>	Gets and sets the FirstName for the user.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("firstName")]
		public string FirstName { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		LastName
		/// 
		/// <summary>	Gets and sets the LastName for the user.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("lastName")]
		public string LastName { get; set; }
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}

