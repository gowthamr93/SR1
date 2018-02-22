using System;
using System.Collections.Generic;
using System.Linq;

namespace Idox.LGDP.Apps.Common.OnSite
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			OnSiteAddress
	/// 
	/// <summary>		Handles parsing address text from Uniform and allowing it to be 
	/// 				displayed in different formats.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class OnSiteAddress
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Cosntructors
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		OnSiteAddress
		/// 
		/// <summary>	Creates a new instance of the OnSiteAddress class.
		/// </summary>
		/// <param name="address">		The raw address field as received from Unfiorm.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public OnSiteAddress(string address)
		{
			Parse(address);
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Lines
		/// 
		/// <summary>	Gets and sets the Lines for the address.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public string[] Lines
		{
			get;
			private set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		LongAddress
		/// 
		/// <summary>	Gets the LongAddress for the address. All lines are joined with a comma separator.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public string LongAddress
		{
			get
			{
				if (Lines.Length == 1)
					return Lines[0];
				else
					return string.Join(", ", Lines);
			}
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		RawAddress
		/// 
		/// <summary>	Gets and sets the RawAddress for the address. This is the field 
		/// 			as it was from Uniform.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public string RawAddress
		{
			get;
			private set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		ShortAddress
		/// 
		/// <summary>	Gets the ShortAddress for the address. Only the first and last line are joined.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public string ShortAddress
		{
			get
			{
				if (Lines.Length == 1)
					return Lines[0];
				else
					return string.Format("{0}, {1}", Lines[0], Lines[Lines.Length - 1]);
			}
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Private Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Parse
		/// 
		/// <summary>	Parses the address text to seperate out the lines.
		/// </summary>
		/// <param name="address">		The address to parse.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		private void Parse(string address)
		{
			List<string> lines;
			RawAddress = address;
			//
			if (!string.IsNullOrEmpty(address))
			{
				// Try to parse by looking for the new row char.
				lines = address.Split('\r').ToList();
				//
				// Try to parse by comma if the previous attempt didn't work.
				if (lines.Count == 1)
					lines = address.Split(',').ToList();
				//
				for (int i = 0; i < lines.Count; i++)
				{
					// Trim the line and remove if it's blank.
					lines[i] = lines[i].Trim();
					if (lines[i].Length == 0)
					{
						lines.RemoveAt(i);
						i--;
					}
				}
				Lines = lines.ToArray();
			}
		    else
				Lines = new string[1] { "" };
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------

	}
}
