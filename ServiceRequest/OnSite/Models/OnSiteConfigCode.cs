using Newtonsoft.Json;
using System.Collections.Generic;

namespace Idox.LGDP.Apps.Common.OnSite
{
	public class OnSiteConfigCode
	{
		/// ------------------------------------------------------------------------------------------------
		#region Json Properties
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("code")]
		public string Code { get; set; }
		/// 
		[JsonProperty("description")]
		public string Description { get; set; }
		/// 
		[JsonProperty("xtratext")]
		public string ExtraText { get; set; }
		/// 
		[JsonProperty("historic")]
		public string Historic { get; set; }
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}

	public class OnSiteConfigCodeList : List<OnSiteConfigCode>
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		public OnSiteConfigCode this[string code]
		{
			get
			{
				foreach (var c in this)
					if (c.Code.Equals(code))
						return c;
				//
				return null;
			}
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}
