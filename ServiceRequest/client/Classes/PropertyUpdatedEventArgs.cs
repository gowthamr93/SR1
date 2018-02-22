using System;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			PropertyUpdatedEventArgs
	/// 
	/// <summary>		Contains the property details of the update event.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class PropertyUpdatedEventArgs : EventArgs
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructors
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		PropertyUpdatedEventArgs
		/// 
		/// <summary>	Creates a new instance of the PropertyUpdatedEventArgs class.
		/// </summary>
		/// <param name="propertyType">		The property type.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public PropertyUpdatedEventArgs(PropertyType propertyType)
		{
			PropertyType = propertyType;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		PropertyType
		/// 
		/// <summary>	Gets and sets the property type for the event args.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public PropertyType PropertyType
		{
			get;
			private set;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}

