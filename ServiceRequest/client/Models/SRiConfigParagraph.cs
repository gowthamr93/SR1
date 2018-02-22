using Newtonsoft.Json;

using Idox.LGDP.Apps.Common.OnSite;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			CPiConfigParagraph
	/// 
	/// <summary>		Defines a pre-set paragraph contained with the configuration data.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class SRiConfigParagraph : OnSiteJsonEntity<SRiConfigParagraph>
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructors
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		CPiConfigParagraph
		/// 
		/// <summary>	Creates a new instance of the CPiConfigParagraph class.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public SRiConfigParagraph()
		{
			CodeValue = "";
			UseWP = "";
			Inserts = "";
			ParagraphText = "";
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		CodeValue
		/// 
		/// <summary>	Gets and sets the CodeValue for the paragraph.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("parcodevalue")]
		public string CodeValue { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		CodeValue
		/// 
		/// <summary>	Gets and sets the CodeValue for the paragraph.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("parusewp")]
		public string UseWP { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		CodeValue
		/// 
		/// <summary>	Gets and sets the CodeValue for the paragraph.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("parinserts")]
		public string Inserts { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		CodeValue
		/// 
		/// <summary>	Gets and sets the CodeValue for the paragraph.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("parparatext")]
		public string ParagraphText { get; set; }
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		PlainText
		/// 
		/// <summary>	Attempts to get the paragraph text in plain text.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonIgnore]
		public string ParagraphPlainText
		{
			get
			{
				return Common.OnSite.modExtensions.RTF2PlainText(ParagraphText);
			}
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}

