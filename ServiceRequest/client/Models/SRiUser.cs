using Newtonsoft.Json;
using System.Collections.Generic;

using Idox.LGDP.Apps.Common.OnSite;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			SRiUser
	/// 
	/// <summary>
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class SRiUser : OnSiteJsonEntity<SRiUser>
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructors
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		SRiUser
		/// 
		/// <summary>	Creates a new instance of the SRiUser class.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public SRiUser ()
		{
			HasEnterprise = false;
			IdoxId = "";
			IsAuthorised = false;
			IsValidated = false;
			LoginAction = LoginActions.None;
			Organisations = new List<OnSiteUserOrganisation>();
			Pin = "";
			Token = "";
            IsLoggedout = false;
        }
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		HasEnterprise
		/// 
		/// <summary>	Gets and sets the HasEnterprise flag for the user.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("enterprise")]
		public bool HasEnterprise
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Environment
		/// 
		/// <summary>	Gets and sets the Environment for the user.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("environment")]
		public OnSiteEnvironments Environment
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		IdoxId
		/// 
		/// <summary>	Gets and sets the Idox Id for the user.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("idoxId")]
		public string IdoxId
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		IsAuthorised
		/// 
		/// <summary>	Gets and sets the flag for whether the user is authorised.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonIgnore]
		public bool IsAuthorised
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		IsValidated
		/// 
		/// <summary>	Gets ands sets the IsValidated indicator for the user.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonIgnore]
		public bool IsValidated
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		LoginAction
		/// 
		/// <summary>	Gets and sets the LoginAction for the user.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonIgnore]
		public LoginActions LoginAction
		{
			get;
			set;
		}
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
		/// ------------------------------------------------------------------------------------------------
		/// Name		Pin
		/// 
		/// <summary>	Gets and sets the Pin for the user.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("pin")]
		public string Pin
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Token
		/// 
		/// <summary>	Gets and sets the authentication token for the user.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("Token")]
		public string Token
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Version
		/// 
		/// <summary>	Gets and sets the Version for the user.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("version")]
		public OnSiteVersion Version
		{
			get;
			set;
		}


        [JsonProperty("flag")]
        public bool IsLoggedout { get; set; }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}

