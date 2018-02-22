using System.Collections.Generic;
using Xamarin.Forms;


namespace ServiceRequest.AppContext
{
    /// <summary>  It contains all the default colour's and style's used in the application.
    /// </summary>
    /// ------------------------------------------------------------------------------------------------
    /// 
    public static class Styles
    {
        /// ------------------------------------------------------------------------------------------------
        #region Private Static Variables
        /// ------------------------------------------------------------------------------------------------
        /// 
        private static Dictionary<string, Color> m_oColours;
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Static Variables
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static Color MainAccent { get; set; } = Color.FromRgba(12, 116, 193, 255);
        public static Color DarkText { get; set; } = Color.FromRgba(51, 51, 51, 255);
        public static Color MediumText { get; set; } = Color.FromRgba(102, 110, 115, 255);
        public static Color LightText { get; set; } = Color.FromRgba(156, 168, 175, 255);
        public static Color WindowBackground { get; set; } = Color.FromHex("#f2f4f8");  //Color.FromRgba(220, 223, 224, 255); old value
        public static Color WindowBackgroundDark { get; set; } = Color.FromRgba(180, 183, 184, 255);
        public static Color CellBackground { get; set; } = Color.FromRgba(246, 247, 247, 255);
        public static Color CellHighlight { get; set; } = Color.FromRgba(255, 255, 255, 255);
        public static Color CellHighlightAlt { get; set; } = Color.FromRgba(174, 183, 186, 255);
        public static Color ModalNavigationBar { get; set; } = Color.FromRgba(233, 235, 236, 255);
        public static Color GroupedTableBackground { get; set; } = Color.FromRgba(239, 239, 244, 255);
        public static Color GroupedTableSeparator { get; set; } = Color.FromRgba(200, 199, 204, 255);
        public static Color StatusBlue { get; set; } = Color.FromRgba(12, 116, 193, 255);
        public static Color StatusGreen { get; set; } = Color.FromRgba(0, 155, 0, 255);
        public static Color StatusAmber { get; set; } = Color.FromRgba(255, 126, 0, 25);
        public static Color StatusRed { get; set; } = Color.FromRgba(220, 0, 0, 255);
        public static Color MapPin { get; set; } = Color.FromRgb(209, 64, 0);
        public static Color UserMenu { get; set; } = Color.FromRgba(0, 67, 126, 255);
        public static Color Offline { get; set; } = Color.FromRgba(255, 50, 0, 255);
        public static Color Online { get; set; } = Color.FromRgba(45, 213, 0, 255);
        
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Static Constructor
        /// ------------------------------------------------------------------------------------------------
        /// 
        static Styles()
        {
            m_oColours = new Dictionary<string, Color>();
        }
        /// 
        #endregion
        /// ------------------------------------------------------------------------------------------------
        #region Public Static Functions, Properties and Methods
        /// ------------------------------------------------------------------------------------------------
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		AddColour
        /// 
        /// <summary>	Adds a colour to the style register under the specified key.
        /// </summary>
        /// <param name="key">			The key for the colour.</param>
        /// <param name="value">		The Color value.</param>
        /// 
        /// <remarks>	If  a key already exists, it's value is updated.
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static void AddColour(string key, Color value)
        {
            if (m_oColours.ContainsKey(key))
                m_oColours[key] = value;
            else
                m_oColours.Add(key, value);
        }
        /// 
        /// ------------------------------------------------------------------------------------------------
        /// Name		GetColour
        /// 
        /// <summary>	Gets a colour from the style register with the given key. If the key doesn't
        /// 			exist then Color.Black is returned.
        /// </summary>
        /// <param name="key">				The key of the colour.</param>
        /// 
        /// <returns>	Color.
        /// </returns>
        /// <remarks>
        /// </remarks>
        /// ------------------------------------------------------------------------------------------------
        /// 
        public static Color GetColour(string key)
        {
            if (m_oColours.ContainsKey(key))
                return m_oColours[key];
            else
                return Color.Black;
        }
        #endregion
        /// ------------------------------------------------------------------------------------------------
    }
}
