using System;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			BaseViewModel
	/// 
	/// <summary>		Defines a generic view model with property updated event.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class BaseViewModel
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Eventhandlers
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		PropertyUpdated
		/// 
		/// <summary>	The property updated event is fired whenever a certain model property has
		/// 			changed. Different areas of the application will listen to this event so they
		/// 			know when to update their UI components.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public EventHandler<PropertyUpdatedEventArgs> PropertyUpdated;
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		UpdateProperty
		/// 
		/// <summary>	Fires the PropertyUpdated event if it's subscribed, supplying the property type.
		/// </summary>
		/// <param name="propertyType">		The property type being updated.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public void UpdateProperty(PropertyType propertyType)
		{
			if (PropertyUpdated != null)
				PropertyUpdated(this, new PropertyUpdatedEventArgs(propertyType));
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}

