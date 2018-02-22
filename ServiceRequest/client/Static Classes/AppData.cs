using System;
using System.Collections.Generic;

using Idox.LGDP.Apps.Common.OnSite;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			AppData
	/// 
	/// <summary>		Central location for application data.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public static class AppData
	{
		/// ------------------------------------------------------------------------------------------------
		#region Static Constructor
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		AppData
		/// 
		/// <summary>	Instantiates the AppData class.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		static AppData()
		{
			ActiveEnvironments = new ActiveEnvironmentCollection();
			ActiveEnvironments.Add(new ActiveEnvironment(OnSiteEnvironments.Production));
			ActiveEnvironments.Add(new ActiveEnvironment(OnSiteEnvironments.Staging));
			ActiveEnvironments.Add(new ActiveEnvironment(OnSiteEnvironments.Sales));
			//ActiveEnvironments.Add(new ActiveEnvironment(OnSiteEnvironments.NoAuth, "http://10.14.62.31:8069"));
			ActiveEnvironments.Add(new ActiveEnvironment(OnSiteEnvironments.Dev));
			ActiveEnvironments.Add(new ActiveEnvironment(OnSiteEnvironments.QA));
            MainModel = new MainViewModel();
            Environment = OnSiteEnvironments.Production;
			//
			APIVersionVerified = false;
			PropertyModel = new PropertyViewModel();
			ConfigModel = new ConfigViewModel();
			SyncInProgress = false;
			Version = new OnSiteVersion();
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Static Properties
		/// ------------------------------------------------------------------------------------------------
		/// 
		public static string CPINFO { get { return "cpinfo.txt"; } }
		public static string LICASE { get { return "licase.txt"; } }
		public static string HISTORY { get { return "history.txt"; } }
		public static string NOTES { get { return "notes.txt"; } }
		public static string RECORDS { get { return "records.txt"; } }
		public static string VISITS { get { return "visits.txt"; } }
		/// 
		public static string CONFIG { get { return "config.txt"; } }
		public static string USER { get { return "user.txt"; } }
		public static string DOCS { get { return "docs.txt"; } }
        /// 
        public const string IdoxReportMailId = "idoxonsitereport@idoxgroup.com";
        ///
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Static Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ActiveEnvironments
        /// 
        /// <summary>	Gets and sets the ActiveEnvironments for the app.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static ActiveEnvironmentCollection ActiveEnvironments
		{
			get;
			private set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		API
		/// 
		/// <summary>	Gets and sets the API interface to be used for the application.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public static IAPI API
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		APIVersionVerified
		/// 
		/// <summary>	Gets and sets the APIVersionVerified to signify if the API version has been
		/// 			verified during this session.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public static bool APIVersionVerified
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		ConfigModel
		/// 
		/// <summary>	Gets and sets the ConfigModel for the application.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public static ConfigViewModel ConfigModel
		{
			get;
			private set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Content
		/// 
		/// <summary>	Gets and sets the ContentController for the app.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public static IContentController Content
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Environment
		/// 
		/// <summary>	Gets and sets the OnSiteEnvironment for the application.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public static OnSiteEnvironments Environment
		{
            get { return MainModel.Environment; }
            set { MainModel.Environment = value; }
        }
        /// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		LastActivity
		/// 
		/// <summary>	Gets and sets the LastActivity for the application.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public static DateTime? LastActivity
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		LastSync
		/// 
		/// <summary>	Gets and sets the LastSync for the application.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public static DateTime? LastSync
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		MainModel
		/// 
		/// <summary>	Gets the MainViewModel for the application.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public static MainViewModel MainModel
		{
			get;
			private set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		PropertyModel
		/// 
		/// <summary>	Gets the PropertyViewModel for the application.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public static PropertyViewModel PropertyModel
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		SyncInProgress
		/// 
		/// <summary>	Gets and sets the SyncInProgress flag for the application.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public static bool SyncInProgress
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Version
		/// 
		/// <summary>	Gets and sets the Version for the application.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public static OnSiteVersion Version
		{
			get;
			set;
		}

	    

	    /// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}

