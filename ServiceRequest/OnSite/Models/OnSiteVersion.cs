using System;
using Newtonsoft.Json;

namespace Idox.LGDP.Apps.Common.OnSite
{
	public class OnSiteVersion : OnSiteJsonEntity<OnSiteVersion>
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructors
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		OnSiteVersion
		/// 
		/// <summary>	Creates a new instance of the OnSiteVersion class.
		/// </summary>
		/// <remarks>	All version numbers are set to zero.
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public OnSiteVersion()
			: this(0, 0, 0)
		{
		}

		public OnSiteVersion(string bundleVersion)
		{
			string[] v;
			//
			v = bundleVersion.Split('.');
			Major = v.Length > 0 ? Int32.Parse(v[0]) : 0;
			Minor = v.Length > 1 ? Int32.Parse(v[1]) : 0;
			Patch = v.Length > 2 ? Int32.Parse(v[2]) : 0;
		}

		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		OnSiteVersion
		/// 
		/// <summary>	Creates a new instance of the OnSiteVersion class.
		/// </summary>
		/// <param name="major">		The major version number.</param>
		/// <param name="minor">		The minor version number.</param>
		/// <param name="patch">		The patch version number.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public OnSiteVersion (int major, int minor, int patch)
		{
			Major = major;
			Minor = minor;
			Patch = patch;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Static Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		FromiOSBundleVersion
		/// 
		/// <summary>	Creates a new OnSiteVersion instance from the bundle version in an iOS app.
		/// </summary>
		/// <param name="version">		The bundle vesions string.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public static OnSiteVersion FromiOSBundleVersion(string version)
		{
			OnSiteVersion v;
			string[] vSplit;
			//
			v = new OnSiteVersion();
			vSplit = version.Split('.');
			v.Major = vSplit.Length > 0 ? Int32.Parse(vSplit[0]) : 0;
			v.Minor = vSplit.Length > 1 ? Int32.Parse(vSplit[1]) : 0;
			v.Patch = vSplit.Length > 2 ? Int32.Parse(vSplit[2]) : 0;
			//
			return v;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Json Properties
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("major")]
		public int Major { get; set; }
		/// 
		[JsonProperty("minor")]
		public int Minor { get; set; }
		/// 
		[JsonProperty("patch")]
		public int Patch { get; set; }
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Text
		/// 
		/// <summary>	Gets the textual display for the version number.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonIgnore]
		public string Text
		{
			get { return string.Format("v{0} ({0}.{1}.{2})", Major, Minor, Patch); }
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		IsCompatibleWith
		/// 
		/// <summary>	Checks if the OnSiteVersion is compatible with the supplied version.
		/// </summary>
		/// <param name="version">		The other version to check against.</param>
		/// 
		/// <remarks>	Only the Patch number can be different and versions still be compatible, any other
		/// 			differences will result in an incompatible match.
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public bool IsCompatibleWith(OnSiteVersion version)
		{
			if (Major != version.Major || Minor != version.Minor)
				return false;
			else
				return true;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}

