using System;
using System.Collections.Generic;

namespace Idox.LGDP.Apps.Common.OnSite
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			IdoxOnSiteAuthCompleteArgs
	/// 
	/// <summary>		Contains all arguments for a complete authentication event.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class IdoxOnSiteAuthCompleteArgs : EventArgs
	{
		/// ------------------------------------------------------------------------------------------------
		#region Private Variables
		/// ------------------------------------------------------------------------------------------------
		/// 
		private Dictionary<string, string> m_oFragmentPairs;
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructors
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		IdoxOnSiteAuthCompleteArgs
		/// 
		/// <summary>	Creates a new isntance of the IdoxOnSiteAuthCompleteArgs class, supplied with
		/// 			the redirect fragment string signifying a successful authentication.
		/// </summary>
		/// <param name="redirectFragment">			The fragment string from the redirect fragment.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public IdoxOnSiteAuthCompleteArgs(string redirectFragment, OnSiteEnvironments environment)
		{
			Successful = true;
			Environment = environment;
			ProcessFragment(redirectFragment);
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		IdoxOnSiteAuthCompleteArgs
		/// 
		/// <summary>	Creates a new instance of the IdoxOnSiteAuthCompleteArgs class, setting the 
		/// 			successful flag to false and providing no fragment details.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public IdoxOnSiteAuthCompleteArgs()
		{
			Successful = false;
			Environment = OnSiteEnvironments.NotSpecified;
			m_oFragmentPairs = new Dictionary<string, string>();
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		AccessToken
		/// 
		/// <summary>	Gets the AccessToken for the authentication arguments.
		/// </summary>
		/// <remarks>	Can return null if the property was not available in the fragment string.
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public string AccessToken
		{
			get { return GetDetail("access_token"); }
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Email
		/// 
		/// <summary>	Gets the Email for the authentication arguments.
		/// </summary>
		/// <remarks>	Can return null if the property was not available in the fragment string.
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public string Email
		{
			get { return GetDetail("email"); }
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Environment
		/// 
		/// <summary>	Gets the Environment for the Authentication arguments.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public OnSiteEnvironments Environment
		{
			get;
			private set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		ExpiresIn
		/// 
		/// <summary>	Gets the ExpiresIn for the authentication arguments.
		/// </summary>
		/// <remarks>	Can return null if the property was not available in the fragment string.
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public string ExpiresIn
		{
			get { return GetDetail("expires_in"); }
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Jti
		/// 
		/// <summary>	Gets the Jti for the authentication arguments.
		/// </summary>
		/// <remarks>	Can return null if the property was not available in the fragment string.
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public string Jti
		{
			get { return GetDetail("jti"); }
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Scope
		/// 
		/// <summary>	Gets the Scope for the authentication arguments.
		/// </summary>
		/// <remarks>	Can return null if the property was not available in the fragment string.
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public string Scope
		{
			get { return GetDetail("scope"); }
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Successful
		/// 
		/// <summary>	Gets the Successful flag for the authentication arguments.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public bool Successful
		{
			get;
			private set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		TokenType
		/// 
		/// <summary>	Gets the TokenType for the authentication arguments.
		/// </summary>
		/// <remarks>	Can return null if the property was not available in the fragment string.
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public string TokenType
		{
			get { return GetDetail("token_type"); }
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Private Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		GetDetail
		/// 
		/// <summary>	Gets a specific detail from the fragment pairs collection.
		/// </summary>
		/// <param name="key">			The key of the detail.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		private string GetDetail(string key)
		{
			if (m_oFragmentPairs.ContainsKey(key))
				return m_oFragmentPairs[key];
			else
				return null;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		ProcessFragment
		/// 
		/// <summary>	Processes the fragment string into the dictionary collection.
		/// </summary>
		/// <param name="frag">			The fragment string.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		private void ProcessFragment(string frag)
		{
			string[] paramPair;
			//
			m_oFragmentPairs = new Dictionary<string, string>();
			foreach (string param in frag.Split('&'))
			{
				paramPair = param.Split('=');
				if (paramPair.Length == 2 && !m_oFragmentPairs.ContainsKey(paramPair[0]))
					m_oFragmentPairs.Add(paramPair[0], paramPair[1]);
			}
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}

