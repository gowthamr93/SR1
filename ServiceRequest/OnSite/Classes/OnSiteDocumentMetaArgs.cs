using System;
using System.Collections.Generic;

namespace Idox.LGDP.Apps.Common.OnSite
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name		OnSiteDocumentMetaParam
	/// 
	/// <summary>	Defines the parameters required for requesting document meta data from the API.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public struct OnSiteDocumentMetaParam
	{
		public string Organisation;
		public string Reference;
	}
	/// 
	/// ----------------------------------------------------------------------------------------------------
	/// Name			OnSiteDocumentMetaArgs
	/// 
	/// <summary>		Contains the reference and organisation codes to use in document meta requests.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class OnSiteDocumentMetaArgs : EventArgs
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructors
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		OnSiteDocumentMetaArgs
		/// 
		/// <summary>	Creates a new instance of the OnSiteDocumentMetaArgs class.
		/// </summary>
		/// <param name="requestParams">		The request params.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public OnSiteDocumentMetaArgs(List<OnSiteDocumentMetaParam> requestParams)
		{
			RequestParams = requestParams;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		RequestParams
		/// 
		/// <summary>	Gets and sets the RequestParams collection for the args.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public List<OnSiteDocumentMetaParam> RequestParams
		{
			get;
			private set;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}

