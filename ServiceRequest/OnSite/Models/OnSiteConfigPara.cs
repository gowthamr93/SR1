using Newtonsoft.Json;
using System.Collections.Generic;

namespace Idox.LGDP.Apps.Common.OnSite
{
    public class OnSiteConfigPara
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private variables
        /// ------------------------------------------------------------------------------------------------
        /// 
        private string m_sKeyVal;
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Json Properties
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonProperty("keyval")]
        public string KeyVal
        {
            get
            {
                if (m_sKeyVal == null)
                    m_sKeyVal = "";
                return m_sKeyVal;
            }
            set { m_sKeyVal = value; }
        }
        /// 
        [JsonProperty("code")]
        public string Code { get; set; }
        /// 
        [JsonProperty("usewp")]
        public string UseWp { get; set; }
        /// 
        [JsonProperty("inserts")]
        public int? Inserts { get; set; }
        /// 
        [JsonProperty("paratext")]
        public string ParagraphText { get; set; }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        [JsonIgnore]
        public string ParagraphPlainText
        {
            get { return ParagraphText.ToRTF2PlainText(); }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }

    public class OnSiteConfigParaList : List<OnSiteConfigPara>
    {
        /// ------------------------------------------------------------------------------------------------
        #region Public Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        public OnSiteConfigPara this[string code]
        {
            get
            {
                foreach (var p in this)
                    if (p.Code.Equals(code))
                        return p;
                //
                return null;
            }
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
