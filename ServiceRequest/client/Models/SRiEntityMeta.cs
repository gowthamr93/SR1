using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Idox.LGDP.Apps.ServiceRequest.Client
{
	public class SRiEntityMeta
	{
		/// ------------------------------------------------------------------------------------------------
		#region Public Constructor
		/// ------------------------------------------------------------------------------------------------
		/// 
		/// ------------------------------------------------------------------------------------------------
		/// Name		SRiEntityMeta
		/// 
		/// <summary>	Creates a new instance of the SRiEntityMeta class with default values.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// ------------------------------------------------------------------------------------------------
		/// 
		public SRiEntityMeta(string entityName, string entityKEy)
		{
			KeyName = entityName;
			KeyValue = entityKEy;
			StringFields = new Dictionary<string, string>();
			IntegerFields = new Dictionary<string, int>();
			DoubleFields = new Dictionary<string, double>();
			BooleanFields = new Dictionary<string, bool>();
			DateTimeFields = new Dictionary<string, DateTime>();
		}
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
		#region Json Properties
		/// ------------------------------------------------------------------------------------------------
		/// 
		[JsonProperty("keyName")]
		public string KeyName { get; set; }
		/// 
		[JsonProperty("keyValue")]
		public string KeyValue { get; set; }
		/// 
		[JsonProperty("stringFields")]
		public Dictionary<string, string> StringFields { get; set; }
		/// 
		[JsonProperty("integerFields")]
		public Dictionary<string, int> IntegerFields { get; set; }
		/// 
		[JsonProperty("doubleFields")]
		public Dictionary<string, double> DoubleFields { get; set; }
		/// 
		[JsonProperty("datetimeFields")]
		public Dictionary<string, DateTime> DateTimeFields { get; set; }
		/// 
		[JsonProperty("booleanFields")]
		public Dictionary<string, bool> BooleanFields { get; set; }
		/// 
		#endregion
		/// ------------------------------------------------------------------------------------------------
	}
}
