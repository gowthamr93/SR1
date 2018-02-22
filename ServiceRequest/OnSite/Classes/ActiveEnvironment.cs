using System;
using System.Collections.Generic;

namespace Idox.LGDP.Apps.Common.OnSite
{
	public class ActiveEnvironment
	{
		/// ------------------------------------------------------------------------------------------------
		#region Private Variables
		/// ------------------------------------------------------------------------------------------------
		/// 
		private string m_sNoAuthIP;
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructor
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		ActiveEnvironment
		/// 
		/// <summary>	Creates a new instance of the ActiveEnvironment class.
		/// </summary>
		/// <param name="environment">		The environment identifier.</param>
		/// <param name="noAuthIP">			The IP to use if the environment is NoAuth.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public ActiveEnvironment(OnSiteEnvironments environment, string noAuthIP = null)
		{
			if (environment == OnSiteEnvironments.NoAuth && noAuthIP == null)
				throw new ArgumentException("A valid host IP must be provided for the NoAuth environment.");
			//
			Environment = environment;
			m_sNoAuthIP = noAuthIP;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		APIHost
		/// 
		/// <summary>	Gets the API host for the environment.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public string APIHost
		{
			get
			{
				switch (Environment)
				{
					case OnSiteEnvironments.Dev:
					return "https://dev.identity.idoxgroup.com";
					case OnSiteEnvironments.QA:
					return "https://qa.identity.idoxgroup.com";
					case OnSiteEnvironments.Staging:
					return "https://staging.identity.idoxgroup.com";
					case OnSiteEnvironments.Production:
					return "https://identity.idoxgroup.com";
					case OnSiteEnvironments.NoAuth:
					return m_sNoAuthIP;
					default:
						throw new InvalidOperationException(
							string.Format("This environment [{0}] does not have a valid API host.",
							              Environment.ToString()));
				}
			}
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Environment
		/// 
		/// <summary>	Gets and sets the Environment.
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
		/// Name		Name
		/// 
		/// <summary>	Gets the environment Name.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public string Name
		{
			get
			{
				switch (Environment)
				{
					case OnSiteEnvironments.Dev: return "Dev";
					case OnSiteEnvironments.NoAuth: return "No Auth";
					case OnSiteEnvironments.Production: return "Live";
					case OnSiteEnvironments.QA: return "QA";
					case OnSiteEnvironments.Sales: return "Demo";
					case OnSiteEnvironments.Staging: return "Test";
					default: return "No Environment";
				}
			}
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}

	public class ActiveEnvironmentCollection : List<ActiveEnvironment>
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Contains
		/// 
		/// <summary>	Checks if the collection contains an ActiveEnvironment that matches
		/// 			the param name="environment".
		/// </summary>
		/// <param name="environment">		The environment to check for.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public bool Contains(OnSiteEnvironments environment)
		{
			foreach (var env in this)
				if (env.Environment == environment)
					return true;
			//
			return false;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		this[OnSiteEnvironment]
		/// 
		/// <summary>	Gets the ActiveEnvironment from the collection that matches the OnSiteEnvironment.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public ActiveEnvironment this[OnSiteEnvironments environment]
		{
			get
			{
				foreach (var env in this)
					if (env.Environment == environment)
						return env;
				//
				throw new KeyNotFoundException("The environment does not exist in the ActiveEnvironmentCollection.");
			}
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------

		/// ------------------------------------------------------------------------------------------------
	}
}
