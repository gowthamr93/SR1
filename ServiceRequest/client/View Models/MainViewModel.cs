
using Idox.LGDP.Apps.Common.OnSite;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
	public class MainViewModel : BaseViewModel
	{
		/// ------------------------------------------------------------------------------------------------
		#region Private Variables
		/// ------------------------------------------------------------------------------------------------
		/// 
		private SRiUser m_oCurrentUser;
        private OnSiteEnvironments m_Environment;
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		CurrentUser
        /// 
        /// <summary>	Gets and sets the CurrentUser for the application.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public SRiUser CurrentUser
		{
			get
			{
				if (m_oCurrentUser == null)
					m_oCurrentUser = new SRiUser();
				return m_oCurrentUser;
			}
			set
			{
				m_oCurrentUser = value;
				UpdateProperty(PropertyType.User);
			}
		}
        
        /// ------------------------------------------------------------------------------------------------
        /// Name		CurrentEnvironment
        /// 
        /// <summary>	Gets and sets the CurrentEnvironment for the application.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public OnSiteEnvironments Environment
        {
            get { return m_Environment; }
            set
            {
                m_Environment = value;
                UpdateProperty(PropertyType.Environment);
            }

        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}

