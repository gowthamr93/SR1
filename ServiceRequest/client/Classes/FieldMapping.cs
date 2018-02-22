
namespace Idox.LGDP.Apps.ServiceRequest.Client
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			FieldMapping
	/// 
	/// <summary>		Maps a field key and value with a field type.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class FieldMapping
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructor
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		FieldMapping
		/// 
		/// <summary>	Creates a new instance of the FiledMapping class.
		/// </summary>
		/// <param name="fieldName">		The field name.</param>
		/// <param name="fieldValue">		The field value.</param>
		/// <param name="mappingType">		The field mapping type.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public FieldMapping(string fieldName, string fieldValue, FieldType mappingType)
		{
			FieldName = fieldName;
			FieldValue = fieldValue;
			MappingType = mappingType;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		FieldName
		/// 
		/// <summary>	Gets and sets the FieldName for the mapping.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public string FieldName
		{
			get;
			private set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		FieldValue
		/// 
		/// <summary>	Gets and sets the FieldValue for the mapping.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public string FieldValue
		{
			get;
			 set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		MappingType
		/// 
		/// <summary>	Gets and sets the MappingType for the mapping.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public FieldType MappingType
		{
			get;
			private set;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}
