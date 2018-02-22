namespace Idox.LGDP.Apps.Common.OnSite
{
	public static class OnSiteSettings
	{
		/// ------------------------------------------------------------------------------------------------
		#region Static Cosntructor
		/// ------------------------------------------------------------------------------------------------
		/// 
		static OnSiteSettings()
		{
			ShouldSerialize = ShouldSerializeRule.None;
			SerializeNullValues = false;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		public static bool SerializeNullValues
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		KeyIsTemp
		/// 
		/// <summary>	Checks if the Key is temporary.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public static bool KeyIsTemp(string key)
		{
			return string.IsNullOrEmpty(key) || key.Substring(0, 5).Equals("TEMP_");
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		NewTempKey
		/// 
		/// <summary>	Gets a new temporary Key.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public static string NewTempKey
		{
			get
			{
				return string.Format("TEMP_{0}", System.Guid.NewGuid().ToString());
			}
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		ShouldSerialize
		/// 
		/// <summary>	Gets and sets the ShouldSerialize rule for the current serialization task.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public static ShouldSerializeRule ShouldSerialize
		{
			get;
			set;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}
