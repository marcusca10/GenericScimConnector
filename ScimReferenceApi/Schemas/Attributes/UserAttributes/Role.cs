using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{

	/// <summary>
	/// Class for tracking User Role by extedning TypedItem.
	/// </summary>
	[DataContract]
	public sealed class Role : TypedItem
	{
		/// <summary>
		/// Get or set Display.
		/// </summary>
		[DataMember(Name = AttributeNames.Display, IsRequired = false, EmitDefaultValue = false)]
		public string Display { get; set; }

		/// <summary>
		/// Get or set Value.
		/// </summary>
		[DataMember(Name = AttributeNames.Value, IsRequired = false, EmitDefaultValue = false)]
		public string Value { get; set; }
	}
}
