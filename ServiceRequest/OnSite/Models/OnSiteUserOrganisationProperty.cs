using Newtonsoft.Json;

namespace Idox.LGDP.Apps.Common.OnSite
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			OnSiteUserOrganisationProperty
	/// 
	/// <summary>		Contains property information for Idox On-Site organisations.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class OnSiteUserOrganisationProperty
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructors
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		OnSiteUserOrganisationProperty
		/// 
		/// <summary>	Creates a new instance of the OnSiteUserOrganisationProperty class.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public OnSiteUserOrganisationProperty()
		{
			Id = "";
			Name = "";
			Value = "";
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
		/// <summary>	Gets and sets the Id for the organisation property.
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
		/// <summary>	Gets and sets the Name for the organisation property.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("property")]
		public string Name { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Value
		/// 
		/// <summary>	Gets and sets the Value for the organisation property.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("value")]
		public string Value { get; set; }
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}

