using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Idox.LGDP.Apps.Common.OnSite
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			OnSiteConfig
	/// 
	/// <summary>		The config data for a specific entity table passed by the API.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class OnSiteConfig : OnSiteJsonEntity<OnSiteConfig>
	{
		/// ------------------------------------------------------------------------------------------------
		#region Private Variables
		/// ------------------------------------------------------------------------------------------------
		/// 
		private static readonly string ALL_ENTITIES = "ALL";
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructor
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		OnSiteConfig
		/// 
		/// <summary>	Creates a new instance of the OnSiteConfig class.
		/// </summary>
		/// <param name="organisation">		The organisation.</param>
		/// <param name="module">			The module.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public OnSiteConfig(string organisation, string module)
		{
			Organisation = organisation;
			Module = module;
			Entity = ALL_ENTITIES;
			Hash = "";
			EntityHash = new Dictionary<string, string>();
			Data = new OnSiteConfigData();
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Json Properties
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("organisation")]
		public string Organisation { get; set; }
		/// 
		[JsonProperty("module")]
		public string Module { get; set; }
		/// 
		[JsonProperty("entity")]
		public string Entity { get; set; }
		/// 
		[JsonProperty("hash")]
		public string Hash { get; set; }
		/// 
		[JsonProperty("data")]
		public OnSiteConfigData Data
		{
			get;
			set;
		}
		//public string EscapedData
		//{
		//	get { return Data.ToJson(); }
		//	set
		//	{
		//		Exception error;
		//		//
		//		Data = OnSiteConfigData.FromJson(value, out error);
		//		if (error != null) throw error;
		//	}
		//}
		/// 
		[JsonProperty("entityhash")]
		public Dictionary<string, string> EntityHash { get; set; }
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		//[JsonIgnore]
		//public OnSiteConfigData Data
		//{
		//	get;
		//	private set;
		//}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		MergeConfig
		/// 
		/// <summary>	Merges the new config into this config.
		/// </summary>
		/// <param name="config">		The new config.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public void MergeNewConfig(OnSiteConfig config)
		{
			if (config.Hash != null && config.Entity != null)
			{
				if (config.Entity.Equals(ALL_ENTITIES))
					EntityHash = config.EntityHash;
				else
				{
					if (EntityHash == null)
						EntityHash = new Dictionary<string, string>();
					//
					if (EntityHash.ContainsKey(config.Entity))
					    EntityHash[config.Entity] = config.Hash;
				    else
						EntityHash.Add(config.Entity, config.Hash);
				}
				//
				if (Data.CodeLists == null)
					Data.CodeLists = new Dictionary<string, OnSiteConfigCodeList>();
				//
				foreach (var codeList in config.Data.CodeLists)
					if (Data.CodeLists.ContainsKey(codeList.Key))
						Data.CodeLists[codeList.Key] = codeList.Value;
					else
						Data.CodeLists.Add(codeList.Key, codeList.Value);
				//
				if (config.Data.OfficerList != null && config.Data.OfficerList.Count > 0)
					Data.OfficerList = config.Data.OfficerList;
				//
				if (config.Data.ParaLists != null && config.Data.ParaLists.Count > 0)
					Data.ParaLists = config.Data.ParaLists;
			}
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
	/// 
	/// ----------------------------------------------------------------------------------------------------
	/// Name			OnSiteConfigData
	/// 
	/// <summary>		The config data contained within the config outer wrapper.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class OnSiteConfigData : OnSiteJsonEntity<OnSiteConfigData>
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructor
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		OnSiteConfigData
		/// 
		/// <summary>	Creates a new instance of the OnSiteConfigData class.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public OnSiteConfigData()
		{
			CodeLists = new Dictionary<string, OnSiteConfigCodeList>();
			ParaLists = new Dictionary<string, OnSiteConfigParaList>();
			OfficerList = new OnSiteOfficerList();
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Json Properties
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("codelists")]
		public Dictionary<string, OnSiteConfigCodeList> CodeLists;

		[JsonProperty("codeparalists")]
		public Dictionary<string, OnSiteConfigParaList> ParaLists;

		[JsonProperty("officers")]
		public OnSiteOfficerList OfficerList;
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
	/// 
	/// ----------------------------------------------------------------------------------------------------
	/// Name			OnSiteConfigCache
	/// 
	/// <summary>		The config for multiple organisations in its cached form that is saved to the device.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class OnSiteConfigCache : OnSiteJsonEntity<OnSiteConfigCache>
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructors
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		OnSiteConfigCache
		/// 
		/// <summary>	Creates a new instance of the OnSiteConfigCache class.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public OnSiteConfigCache()
		{
			Configs = new List<OnSiteConfig>();
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Json Properties
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("configs")]
		public List<OnSiteConfig> Configs { get; set; }
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}
