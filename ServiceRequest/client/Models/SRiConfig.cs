using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using Idox.LGDP.Apps.Common.OnSite;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			SRiConfig
	/// 
	/// <summary>		The top level object in the configuration json for SRi.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class SRiConfig : OnSiteJsonEntity<SRiConfig>
	{
		/// ------------------------------------------------------------------------------------------------
		#region Priivate Variables
		/// ------------------------------------------------------------------------------------------------
		/// 
		private Dictionary<string, string> m_oEntityHash;
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructors
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		SRiConfig
		/// 
		/// <summary>	Creates a new instance of the SRiConfig class.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public SRiConfig()
		{
			Organisation = "";
			Hash = NullHash;
			Data = new SRiConfigData();
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Organisation
		/// 
		/// <summary>	Gets and sets the Organisation for the config.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("organisation")]
		public string Organisation { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Entity
		/// 
		/// <summary>	Gets and sets the Entity for the config.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("entity")]
		public string Entity { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Hash
		/// 
		/// <summary>	Gets and sets the Hash for the config.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("hash")]
		public string Hash { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Data
		/// 
		/// <summary>	Gets and sets the Data for the config.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonIgnore]
		public SRiConfigData Data { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		EscapedData
		/// 
		/// <summary>	Gets and sets the EscapedData for the config.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("data")]
		public string EscapedData
		{
			get { return Data.ToJson(); }
			set
			{
				Exception error;
				//
				Data = SRiConfigData.FromJson(value, out error);
				if (error != null) throw error;
			}
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Static Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		NullHash
		/// 
		/// <summary>	Gets the NullHash for a config.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public static string NullHash
		{
			get { return "0"; }
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		MergeConfig
		/// 
		/// <summary>	Merges the new config into the existing config.
		/// </summary>
		/// <param name="config">		The new config.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public void MergeConfig(SRiConfig config)
		{
			if (!config.Hash.Equals(NullHash))
			{

			}
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
	/// ----------------------------------------------------------------------------------------------------
	#region Data & Cache
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class SRiConfigData : OnSiteJsonEntity<SRiConfigData>
	{
		/// ------------------------------------------------------------------------------------------------
		#region Json Properties
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("codelists")]
		public Dictionary<string, Dictionary<string, SRiConfigCode>> CodeLists { get; set; }
		/// 
		//[JsonProperty("codeparalists")]
		//public Dictionary<string, Dictionary<string, SRiConfigParagraph>> ParagraphLists { get; set; }
		/// 
		[JsonProperty("officers")]
		public Dictionary<string, Dictionary<string, SRiConfigOfficer>> Officers { get; set; }
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
	/// 
	public class SRiConfigCache : OnSiteJsonEntity<SRiConfigCache>
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Configs
		/// 
		/// <summary>	Gets and sets the Configs for the cache.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("configs")]
		public List<SRiConfig> Configs
		{
			get;
			set;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
	/// 
	#endregion
	/// ----------------------------------------------------------------------------------------------------
	#region Additional Models
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class SRiConfigCode
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Json Properties
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("description")]
		public string Description
		{
			get;
			set;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
	/// 
	//public class SRiConfigParagraph
	//{
	//	/// ------------------------------------------------------------------------------------------------
	//	#region Public Json Properties
	//	/// ------------------------------------------------------------------------------------------------
	//	/// 
	//	[JsonProperty("parcodevalue")]
	//	public string CodeValue { get; set; }
	//	/// 
	//	[JsonProperty("parusewp")]
	//	public string UseWP { get; set; }
	//	/// 
	//	[JsonProperty("parinserts")]
	//	public string Inserts { get; set; }
	//	/// 
	//	[JsonProperty("parparatext")]
	//	public string ParagraphText { get; set; }
	//	/// 
	//	#endregion
	//	/// ------------------------------------------------------------------------------------------------
	//	#region Public Functions, Properties and Methods
	//	/// ------------------------------------------------------------------------------------------------
	//	/// 
	//	[JsonIgnore]
	//	public string ParagraphPlainText
	//	{
	//		get { return ParagraphText.ToRTF2PlainText(); }
	//	}
	//	/// 
	//	#endregion
	//	/// ------------------------------------------------------------------------------------------------
	//}
	/// 
	public class SRiConfigOfficer
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Json Properties
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("offname")]
		public string Name { get; set; }
		/// 
		[JsonProperty("offtitle")]
		public string Title { get; set; }
		/// 
		[JsonProperty("offforename")]
		public string Forename { get; set; }
		/// 
		[JsonProperty("offsurname")]
		public string Surname { get; set; }
		/// 
		[JsonProperty("offjobtitle")]
		public string Jobtitle { get; set; }
		/// 
		[JsonProperty("offphone")]
		public string Phone { get; set; }
		/// 
		[JsonProperty("offemail")]
		public string Email { get; set; }
		/// 
		[JsonProperty("offidoxid")]
		public string IdoxID { get; set; }
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
	/// 
	#endregion
	/// ----------------------------------------------------------------------------------------------------
}

