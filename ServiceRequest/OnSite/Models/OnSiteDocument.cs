using Newtonsoft.Json;
using System.Collections.Generic;

namespace Idox.LGDP.Apps.Common.OnSite
{
	public class OnSiteDocument
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Json Properties
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("id")]
		public string Id { get; set; }
		public bool ShouldSerializeId() { return OnSiteSettings.ShouldSerialize != ShouldSerializeRule.DataForAPI; }
		/// 
		[JsonProperty("subentity")]
		public string SubEntity { get; set; }
		/// 
		[JsonProperty("subentitykeyname")]
		public string SubEntityFieldName { get; set; }
		/// 
		[JsonProperty("subentitykeyvalue")]
		public string SubEntityFieldValue { get; set; }
		/// 
		[JsonProperty("name")]
		public string Name { get; set; }
		/// 
		[JsonProperty("extension")]
		public string Extension { get; set; }
		/// 
		[JsonProperty("size")]
		public int? Size { get; set; }
		/// 
		[JsonProperty("hash")]
		public string Hash { get; set; }
		/// 
		[JsonProperty("description")]
		public string Description { get; set; }
		/// 
		[JsonProperty("category")]
		public string Category { get; set; }
		/// 
		[JsonProperty("mimetype")]
		public string MimeType { get; set; }
        ///      
        [JsonProperty("status")]
		public SyncStatus Status { get; set; }
		public bool ShouldSerializeStatus() { return OnSiteSettings.ShouldSerialize != ShouldSerializeRule.DataForAPI; }
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		FileName
		/// 
		/// <summary>	Creates a FileName for the document from the Id and the file type in the Name property.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonIgnore]
		public string FileName
		{
			get { return string.Format("{0}{1}", Id, Extension); }
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}

	public class OnSiteDocumentUpload
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructor
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		OnSiteDocumentUpload
		/// 
		/// <summary>	Creates a new instance of the OnSiteDocumentUpload.
		/// </summary>
		/// <param name="doc">		The document.</param>
		/// <param name="data">		The document data.</param>
		/// <param name="meta">		The document meta.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public OnSiteDocumentUpload(OnSiteDocument doc, OnSiteDocumentMeta meta, OnSiteDocumentData data)
		{
			Document = doc;
			Meta = new OnSiteDocumentMeta()
			{
				EntityKey = meta.EntityKey,
				Module = meta.Module,
				Organisation = meta.Organisation,
				Documents = new List<OnSiteDocument>() { Document }
			};
			Data = new OnSiteDocumentData()
			{
				AppModule = data.AppModule,
				Entity = data.Entity,
				EntityDocumentLists = new List<OnSiteDocumentMeta>() { Meta }
			};
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		public OnSiteDocument Document
		{
			get;
			private set;
		}
		/// 
		public OnSiteDocumentData Data
		{
			get;
			private set;
		}
		/// 
		public OnSiteDocumentMeta Meta
		{
			get;
			private set;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}

	#region Meta & Data
	public class OnSiteDocumentMeta
	{
		/// ------------------------------------------------------------------------------------------------
		#region Private Variables
		/// ------------------------------------------------------------------------------------------------
		/// 
		private List<OnSiteDocument> m_oDocs;
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Json Properties
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("module")]
		public string Module { get; set; }
		/// 
		[JsonProperty("organisation")]
		public string Organisation { get; set; }
		/// 
		[JsonProperty("entitykey")]
		public string EntityKey { get; set; }
        /// 
        [JsonProperty("type")]
        public string Type { get; set; }

        /// 
        [JsonProperty("docs")]
		public List<OnSiteDocument> Documents 
		{ 
			get
			{
				if (m_oDocs == null)
					m_oDocs = new List<OnSiteDocument>();
				return m_oDocs;
			}
			set { m_oDocs = value; }
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}

	public class OnSiteDocumentCache : OnSiteJsonEntity<OnSiteDocumentCache>
	{
		/// ------------------------------------------------------------------------------------------------
		#region Private Variables
		/// ------------------------------------------------------------------------------------------------
		/// 
		private List<OnSiteDocumentData> m_oDocs;
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Json Properties
		/// ------------------------------------------------------------------------------------------------
		/// 
		public List<OnSiteDocumentData> DocumentData 
		{ 
			get
			{
				if (m_oDocs == null)
					m_oDocs = new List<OnSiteDocumentData>();
				return m_oDocs;
			}
			set { m_oDocs = value; }
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}

	public class OnSiteDocumentData : OnSiteJsonEntity<OnSiteDocumentData>
	{
		/// ------------------------------------------------------------------------------------------------
		#region Private Variables
		/// ------------------------------------------------------------------------------------------------
		/// 
		private List<OnSiteDocumentMeta> m_oEntityDocs;
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Json Properties
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("appmodule")]
		public string AppModule { get; set; }
		/// 
		[JsonProperty("entity")]
		public string Entity { get; set; }
		/// 
		[JsonProperty("entitydoclists")]
		public List<OnSiteDocumentMeta> EntityDocumentLists
		{
			get
			{
				if (m_oEntityDocs == null)
					m_oEntityDocs = new List<OnSiteDocumentMeta>();
				return m_oEntityDocs;
			}
			set { m_oEntityDocs = value; }
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		AddDocument
		/// 
		/// <summary>	Adds a new document meta to the collection. If meta f=already exists for the
		/// 			entity identified then the inner document is added to the metas inner collection.
		/// </summary>
		/// <param name="docMeta">		The document meta.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 

		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
	#endregion
}

