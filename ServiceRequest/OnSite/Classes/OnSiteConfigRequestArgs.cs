using System;
using System.Collections.Generic;

namespace Idox.LGDP.Apps.Common.OnSite
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			OnSiteConfigRequestArgs
	/// 
	/// <summary>		Provides the list of organisation names for which configuration data is required. 
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class OnSiteConfigRequestArgs : EventArgs
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructors
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		OnSiteConfigRequestArgs
		/// 
		/// <summary>	Creates a new instance of the OnSiteConfigRequestArgs class.
		/// </summary>
		/// <param name="organisations">		The orgisation collection.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public OnSiteConfigRequestArgs(List<string> organisations, bool fromCache)
		{
			FromCache = fromCache;
			Organisations = organisations;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		FromCache
		/// 
		/// <summary>	Gets and sets the FromCache property for the args. If set to true then the
		/// 			application should try to load the configs from cache first and only then
		/// 			attempt to download them if any are missing.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public bool FromCache
		{
			get;
			private set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Organisations
		/// 
		/// <summary>	Gets and sets the Orgnisations for the request args.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public List<string> Organisations
		{
			get;
			private set;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}

