using System.Collections.Generic;

using Idox.LGDP.Apps.Common.OnSite;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
    /// ----------------------------------------------------------------------------------------------------
    /// Name			ConfigViewModel
    /// 
    /// <summary>		Handles all configuration data operations.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    /// 
    public class ConfigViewModel : BaseViewModel
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Variables
        /// ------------------------------------------------------------------------------------------------
        /// 
        private Dictionary<string, OnSiteConfig> m_oConfigs;

        public bool IsNotEmpty
        {
            get
            {
                return m_oConfigs.Count > 0;
            }
        }

        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Constructors
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ConfigViewModel
        /// 
        /// <summary>	Creates a new instance of the ConfigurationManager class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public ConfigViewModel()
        {
            m_oConfigs = new Dictionary<string, OnSiteConfig>();
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Actions
        /// 
        /// <summary>	Gets the collection of Actions available for a specific Visit type under a 
        /// 			scpecific organisation code.
        /// </summary>
        /// <param name="organisation">		The organisation code.</param>
        /// <param name="visitType">		The Visit type.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public OnSiteConfigCodeList Actions(string organisation, string visitType)
        {
            return GetCodeList(organisation, string.Format("ACT_{0}", visitType));
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Add
        /// 
        /// <summary>	Adds a range of configuration data to the configuration collection. If the data was 
        /// 			loaded from cache then it shouldn't be saved back.
        /// </summary>
        /// <param name="configs">		The configuration data range to add.</param>
        /// <param name="fromCache">	Whether the data was loaded from cache.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void Add(List<OnSiteConfig> configs, bool fromCache = false)
        {
            foreach (var config in configs)
            {
                if (!m_oConfigs.ContainsKey(config.Organisation))
                    m_oConfigs.Add(config.Organisation, new OnSiteConfig(config.Organisation, config.Module));
                //
                m_oConfigs[config.Organisation].MergeNewConfig(config);
            }
            //
            if (!fromCache)
                UpdateProperty(PropertyType.Config);
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Cache
        /// 
        /// <summary>	Gets the json string to cache for the current configurations.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public string Cache
        {
            get
            {
                OnSiteConfigCache cache;
                //
                cache = new OnSiteConfigCache();
                foreach (string key in m_oConfigs.Keys)
                    cache.Configs.Add(m_oConfigs[key]);
                //
                return cache.ToJson();
            }
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Clear
        /// 
        /// <summary>	Clears all the data in memory.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public void Clear()
        {
            m_oConfigs.Clear();
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		CodeDescription
        /// 
        /// <summary>	Gets the description for a code list value.
        /// </summary>
        /// <param name="organisation">		The organisation for the configuration.</param>
        /// <param name="codeListKey">		The key of the code list.</param>
        /// <param name="code">				The code list value.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public string CodeDescription(string organisation, string codeListKey, string code)
        {
            OnSiteConfigCode cnCode;
            //
            cnCode = GetCodeList(organisation, codeListKey)[code];
            if (cnCode != null)
                return cnCode.Description;
            else
                return code;
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Hash
        /// 
        /// <summary>	Gets the Hash to use for a given organisation configuration.
        /// </summary>
        /// <param name="organisation">		The organisation.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public string Hash(string organisation, string entity)
        {
            if (m_oConfigs.ContainsKey(organisation) &&
                m_oConfigs[organisation].EntityHash.ContainsKey(entity))
                return m_oConfigs[organisation].EntityHash[entity];
            else
                return "0";
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Inspections
        /// 
        /// <summary>	Gets the XIREC codelist for all possible types of Inspections.
        /// </summary>
        /// <param name="organisation">		The organisation.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public OnSiteConfigCodeList Inspections(string organisation)
        {
            return GetCodeList(organisation, "XIRECTYPE");
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Legislations
        /// 
        /// <summary>	Gets the collection of Legislations available for a specific Visit type under a
        /// 			specific organisation code.
        /// </summary>
        /// <param name="organisation">			The organisation code.</param>
        /// <param name="visitType">			The Visit type.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public OnSiteConfigCodeList Legislations(string organisation, string visitType)
        {
            return GetCodeList(organisation, string.Format("LEG_{0}", visitType));
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetOfficerList
        /// 
        /// <summary>	Gets a officer list assigned to a specific organisation with a specific key.
        /// </summary>
        /// <param name="organisation">			The organisation.</param>
        /// <param name="groupMod">				The GroupMod code from the XIREC.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public List<OnSiteOfficer> OfficerList(string organisation, string groupMod)
        {
            if (m_oConfigs.ContainsKey(organisation))
                return m_oConfigs[organisation].Data.OfficerList.InGroupMod(groupMod);
            else
                return new List<OnSiteOfficer>();
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		OfficerList
        /// 
        /// <summary>	Gets a officer list assigned to a specific organisation with a specific key.
        /// </summary>
        /// <param name="organisation">			The organisation.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public List<OnSiteOfficer> OfficerList(string organisation)
        {
            if (m_oConfigs.ContainsKey(organisation))
                return m_oConfigs[organisation].Data.OfficerList;
            else
                return new List<OnSiteOfficer>();
        }

        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		ParagraphList
        /// <summary>	Gets the collection of standard paragraphs for the specified organisation.
        /// </summary>
        /// <param name="organisation">			The organisation.</param>
        /// <param name="team"> The team</param>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        public OnSiteConfigParaList ParagraphList(string organisation, string team)
        {
            string paraTeam;
            //
            if (string.IsNullOrEmpty(team))
                team = "SR";
            //
            paraTeam = string.Format("XI_{0}P", team);
            if (m_oConfigs.ContainsKey(organisation) &&
                m_oConfigs[organisation].Data.ParaLists.ContainsKey(paraTeam))
                return m_oConfigs[organisation].Data.ParaLists[paraTeam];
            else
                return new OnSiteConfigParaList();
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		Treatments
        /// 
        /// <summary>	Gets the collection of Treatments available for a specific Visit type under a 
        /// 			scpecific organisation code.
        /// </summary>
        /// <param name="organisation">		The organisation code.</param>
        /// <param name="visitType">		The Visit type.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public OnSiteConfigCodeList Treatments(string organisation)
        {
            return GetCodeList(organisation, "TREATT");
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		VisitTypes
        /// 
        /// <summary>	Gets the collection of available visit types for the organisation.
        /// </summary>
        /// <param name="organisation">		The organisation.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public OnSiteConfigCodeList Visits(string organisation)
        {
            return GetCodeList(organisation, "XIVTYPE");
        }
        /// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		CodeDescription
		/// 
		/// <summary>	Gets the description for a code list value.
		/// </summary>
		/// <param name="organisation">		The organisation for the configuration.</param>
		/// <param name="codeListKey">		The key of the code list.</param>
		/// <param name="code">				The code list value.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public OnSiteConfigCodeList AllRequestTypes(string organisation)
        {
            OnSiteConfigCodeList cnCode;
            //
            cnCode = GetFullCodeList(organisation, "SRRECTYPE");
            return cnCode;
        }
        /// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		ContactTypes
		/// 
		/// <summary>	Gets the description for a code list value.
		/// </summary>
		/// <param name="organisation">		The organisation for the configuration.</param>
		/// <param name="codeListKey">		The key of the code list.</param>
		/// <param name="code">				The code list value.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public OnSiteConfigCodeList ContactTypes(string organisation)
        {
            OnSiteConfigCodeList cnCode;
            //
            cnCode = GetFullCodeList(organisation, "SRCUSTTYPE");
            return cnCode;
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Private Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetCodeList
        /// 
        /// <summary>	Gets a code list assigned to a specific organisation with a specific key.
        /// </summary>
        /// <param name="org">			The organisation.</param>
        /// <param name="codeListKey">	The code list key.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private OnSiteConfigCodeList GetCodeList(string org, string codeListKey)
        {
            if (org != null && codeListKey != null &&
                m_oConfigs.ContainsKey(org) &&
                m_oConfigs[org].Data.CodeLists.ContainsKey(codeListKey))
                return m_oConfigs[org].Data.CodeLists[codeListKey];
            //
            return new OnSiteConfigCodeList();
        }
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetCodeList
        /// 
        /// <summary>	Gets a code list assigned to a specific organisation with a specific key.
        /// </summary>
        /// <param name="org">			The organisation.</param>
        /// <param name="codeListKey">	The code list key.</param>
        /// 
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        private OnSiteConfigCodeList GetFullCodeList(string org, string codeListKey)
        {
            if (org != null && codeListKey != null && m_oConfigs.ContainsKey(org) && m_oConfigs[org].Data.CodeLists.ContainsKey(codeListKey))
                return m_oConfigs[org].Data.CodeLists[codeListKey];
            //
            return new OnSiteConfigCodeList();
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}

