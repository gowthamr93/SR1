using System;
using Newtonsoft.Json;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
	/// ----------------------------------------------------------------------------------------------------
	/// Name			IndexMapping
	/// 
	/// <summary>		Contains the index details of an item in the CPi model hierarchy.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// ----------------------------------------------------------------------------------------------------
	/// 
	public class IndexMapping
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructors
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		IndexMapping
		/// 
		/// <summary>	Creates a new instance of the IndexMapping class.
		/// </summary>
		/// <param name="property">		The index for the Property. Must have value.</param>
		/// <param name="requestGroup">	The index for the Request Group. If no value, mapping is Property.</param>
		/// <param name="record">		The index for the Record. If no value, mapping is Request Group.</param>
		/// <param name="inspection">	The index for the Inspection. If no value, mapping is Record.</param>
		/// <param name="visit">		The index for the Visit. If no value, mapping is Inspection.</param>
		/// <param name="action">		The index for the Action.</param>
		/// <param name="treatment">	The index for the Treatment.</param>
		/// 
		/// <remarks>	The index for Action and Treatment cannot be set in the same instance as they
		/// 			represent the same level of selection, just different entities. If they are both set
		/// 			and all other values are set then the mapping will be for an Action.
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public IndexMapping(int property, 
		                    int? requestGroup = null, 
		                    int? record = null, 
		                    int? inspection = null,
		                    int? visit = null,
		                    int? action = null,
		                    int? treatment = null)
		{
			Property = property;
			RequestGroup = requestGroup;
			Record = record;
			Inspection = inspection;
			Visit = visit;
			Action = action;
			Treatment = treatment;
			//
			SetMappingType();
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Public Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Property
		/// 
		/// <summary>	Gets and sets the Property for the index mapping.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("property")]
		public int Property
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		RequestGroup
		/// 
		/// <summary>	Gets and sets the RequestGroup for the index mapping.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("requestGroup")]
		public int? RequestGroup
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Record
		/// 
		/// <summary>	Gets and sets the Record for the index mapping.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("record")]
		public int? Record
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Inspection
		/// 
		/// <summary>	Gets and sets the Inspection for the index mapping.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("inspection")]
		public int? Inspection
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Visit
		/// 
		/// <summary>	Gets and sets the Visit for the index mapping.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("visit")]
		public int? Visit
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Action
		/// 
		/// <summary>	Gets and sets the Action for the index mapping.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("action")]
		public int? Action
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Treatment
		/// 
		/// <summary>	Gets and sets the Treatment for the index mapping.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("treatment")]
		public int? Treatment
		{
			get;
			set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Type
		/// 
		/// <summary>	Gets and sets the Type for the index mapping.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("mappingType")]
		public MappingType Type
		{
			get;
			private set;
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		Matches
		/// 
		/// <summary>	Checks if another IndexMapping instance matches the current one.
		/// </summary>
		/// <param name="map">		The other IndexMapping instance.</param>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public bool Matches(IndexMapping map)
		{
			return (map.Property == Property &&
			        map.RequestGroup == RequestGroup &&
			        map.Record == Record);
		}
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		ToString
		/// 
		/// <summary>	Returns a human readable representation of the values
		/// </summary>
		/// 
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public override string ToString()
		{
			return string.Format("Property: {0}, RequestGroup: {1}, Record: {2}", Property,
			                     RequestGroup.HasValue ? RequestGroup.Value.ToString() : "null",
			                     Record.HasValue ? Record.Value.ToString() : "null");
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Private Functions, Properties and Methods
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		SetMappingType
		/// 
		/// <summary>	Sets the mapping type based on the values present.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		private void SetMappingType()
		{
			Type = MappingType.Property;
			if (RequestGroup.HasValue)
				if (Record.HasValue)
					if (Inspection.HasValue)
						if (Visit.HasValue)
							if (Action.HasValue)
								Type = MappingType.Action;
							else if (Treatment.HasValue)
								Type = MappingType.Treatment;
							else
								Type = MappingType.Visit;
						else
							Type = MappingType.Inspection;
					else
						Type = MappingType.Record;
				else
					Type = MappingType.RequestGroup;
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}

