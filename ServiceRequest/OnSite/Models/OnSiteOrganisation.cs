using Newtonsoft.Json;
using System.Collections.Generic;

namespace Idox.LGDP.Apps.Common.OnSite
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			OnSiteOrganisation
	/// 
	/// <summary>		Generic Organisation details as returned from the Idox On-Site Authentication portal.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class OnSiteOrganisation
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Id
		/// 
		/// <summary>	Gets and sets the Id for the organisation.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("id")]
		public string Id { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Name
		/// 
		/// <summary>	Gets and sets the Name for the organisation.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("name")]
		public string Name { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Properties
		/// 
		/// <summary>	Gets and sets the Properties collection for the organisation.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("organisationProperties")]
		public List<OnSiteUserOrganisationProperty> Properties { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		HasEnterprise
		/// 
		/// <summary>	Gets the HasEnterprise flag for the organisation by checking through each
		/// 			property field.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonIgnore]
		public bool HasEnterprise
		{
			get
			{
				bool enterprise = false;
				foreach (OnSiteUserOrganisationProperty prop in Properties)
					if (prop.Name.Equals("hasEnterprise") && prop.Value.Equals("true"))
				{
					enterprise = true;
					break;
				}
				//
				return enterprise;
			}
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}
