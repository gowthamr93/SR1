using Newtonsoft.Json;
using System.Collections.Generic;

namespace Idox.LGDP.Apps.Common.OnSite
{
	public class OnSiteOfficer
	{
		/// ------------------------------------------------------------------------------------------------
		#region Json Properties
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("offcode")]
		public string OfficerCode { get; set; }
		/// 
		[JsonProperty("name")]
		public string Name { get; set; }
		/// 
		[JsonProperty("title")]
		public string Title { get; set; }
		/// 
		[JsonProperty("forename")]
		public string Forename { get; set; }
		/// 
		[JsonProperty("surname")]
		public string Surname { get; set; }
		/// 
		[JsonProperty("jobtitle")]
		public string JobTitle { get; set; }
		/// 
		[JsonProperty("phone")]
		public string PhoneNumber { get; set; }
		/// 
		[JsonProperty("email")]
		public string EmailAddress { get; set; }
		/// 
		[JsonProperty("idoxid")]
		public string IdoxId { get; set; }
		/// 
		[JsonProperty("teams")]
		public List<string> Teams { get; set; }
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}

	public class OnSiteOfficerList : List<OnSiteOfficer>
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		public OnSiteOfficer this[string offCode]
		{
			get
			{
				foreach (var o in this)
					if (o.OfficerCode.Equals(offCode))
						return o;
				//
				return null;
			}
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		InGroupMod
		/// 
		/// <summary>	Gets all the OnSiteOfficers entities that have the GroupMod code in their
		/// 			Teams collection.
		/// </summary>
		/// <param name="groupMod">		The GroupMod code.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public List<OnSiteOfficer> InGroupMod(string groupMod)
		{
			List<OnSiteOfficer> officers;
			//
			officers = new List<OnSiteOfficer>();
			foreach (var o in this)
				if (o.Teams != null && o.Teams.Contains(groupMod))
					officers.Add(o);
			//
			return officers;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}
