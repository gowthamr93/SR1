using Idox.LGDP.Apps.Common.OnSite;
using System;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			SRiSearchResult
	/// 
	/// <summary>		Contains a SRiProperty as the search result content and allows for selection.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class SRiSearchResult
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructor
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		SRiSearchProperty
		/// 
		/// <summary>	Creates a new instance of the SRiSearchResult class.
		/// </summary>
		/// <param name="property">		The property that is the content of the result.</param>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public SRiSearchResult(SRiProperty property)
		{
			Property = property;
			Selected = false;
			RequestType = property.RequestGroups[0].Name;
			TargetResponse = property.RequestGroups[0].EarliestTargetDate;
            TargetResponseToString = TargetResponse.ToString("dd/MMM/yyyy @ HH:mm", "Target response ", "No target response");
        }
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Property
		/// 
		/// <summary>	Gets and sets the Property for the result.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public SRiProperty Property
		{
			get;
			private set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		RequestType
		/// 
		/// <summary>	Gets and sets the RequestType for the result.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public string RequestType
		{
			get;
			private set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Selected
		/// 
		/// <summary>	Gets and sets the Selected for the result.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public bool Selected
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		TargetResponse
		/// 
		/// <summary>	Gets and sets the TargetResponse for the result.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public DateTime? TargetResponse
		{
			get;
			private set;
		}

        public string TargetResponseToString
        {
            get;
            set; 
        }

       
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}
