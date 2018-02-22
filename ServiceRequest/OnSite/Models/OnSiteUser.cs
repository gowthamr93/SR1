using System.Collections.Generic;

using Newtonsoft.Json;

namespace Idox.LGDP.Apps.Common.OnSite
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			OnSiteUser
	/// 
	/// <summary>		Users details as returned from the idox On-Site Authentication portal.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class OnSiteUser
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructor
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		OnSiteUser
		/// 
		/// <summary>	Creates a new instance of the OnSiteUser class.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public OnSiteUser()
		{
			Id = "";
			Username = "";
			Password = "";
			Profile = new OnSiteUserProfile();
			Roles = new List<OnSiteUserRole>();
			Organisations = new List<OnSiteUserOrganisation>();
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
		/// <summary>	Gets and sets the Id for the user.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("id")]
		public string Id { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Username
		/// 
		/// <summary>	Gets and sets the Username for the user.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("username")]
		public string Username { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Password
		/// 
		/// <summary>	Gets and sets the Password for the user.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("password")]
		public string Password { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Profile
		/// 
		/// <summary>	Gets and sets the Profile for the user.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("userProfile")]
		public OnSiteUserProfile Profile { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Roles
		/// 
		/// <summary>	Gets and sets the Roles collection for the user.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("roles")]
		public List<OnSiteUserRole> Roles { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Organisations
		/// 
		/// <summary>	Gets and sets the Organisations collection for the user.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("organisations")]
		public List<OnSiteUserOrganisation> Organisations { get; set; }
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Static Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		FromJson
		/// 
		/// <summary>	Creates a new instance of the OnSiteUser class from the Json string provided.
		/// </summary>
		/// <param name="json">		The json string.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public static OnSiteUser FromJson(string json)
		{
			try { return JsonConvert.DeserializeObject<OnSiteUser>(json); }
			catch { return null; }
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}

