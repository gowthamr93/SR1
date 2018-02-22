using Newtonsoft.Json;
using System;

namespace Idox.LGDP.Apps.Common.OnSite
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			OnSiteJsonEntity
	/// 
	/// <summary>		Generic Json entity definition used as the base for all json model 
	/// 				types in OnSite apps.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class OnSiteJsonEntity<T> : OnSiteEntity where T : OnSiteEntity
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Static Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		FromJson
		/// 
		/// <summary>	Deserialises json data to an instance of the object.
		/// </summary>
		/// <param name="json">		The json data.</param>
		/// <param name="error">	The json exception should one arrise.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public static T FromJson(string json, out Exception error)
		{
			error = null;
			try
			{
				return JsonConvert.DeserializeObject<T>(json);
			}
			catch (Exception ex)
			{
				error = ex;
				return null;
			}
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Clone
		/// 
		/// <summary>	Creates a clone of the instance.
		/// </summary>
		/// <param name="error">		Error output.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public T Clone(out Exception error)
		{
			return FromJson(ToJson(), out error);
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		ToJson
		/// 
		/// <summary>	Serialises the object to json, defaulting to no format and null values ignored.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public string ToJson()
		{
			return ToJson(Formatting.None, OnSiteSettings.SerializeNullValues ? 
			              				   NullValueHandling.Include : NullValueHandling.Ignore);
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		ToJson
		/// 
		/// <summary>	Serialises the object to json.
		/// </summary>
		/// <param name="format">			The format of the json.</param>
		/// <param name="valueHandling">	The value handling rule for null values.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public string ToJson(Formatting format, NullValueHandling valueHandling)
		{
			return JsonConvert.SerializeObject(this, format, new JsonSerializerSettings()
			{
				NullValueHandling = valueHandling
			});
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
	/// 
	/// ----------------------------------------------------------------------------------------------------
	/// Name			OnSiteEntity
	/// 
	/// <summary>		Base entity definition to allow for nullable type in OnSiteJsonEntity.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public abstract class OnSiteEntity { }
	/// 
	/// ----------------------------------------------------------------------------------------------------
	/// Name			IEditableEntity
	/// 
	/// <summary>		Interface to identify an entity as editable and allow for saving SelectedItems.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public interface IEditableEntity
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Modify
		/// 
		/// <summary>	Sets the entity as modified by changing the Modified flag to true.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		void Modify();
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Status
		/// 
		/// <summary>	Gets and sets the SyncStatus for the entity.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		SyncStatus Status { get; set; }
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------

	}
}

