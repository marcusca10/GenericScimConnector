using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.GroupAttributes
{
	/// <summary>
	/// Members, of a Group.
	/// </summary>
	[DataContract]
	public class Member : TypedItem
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		internal Member()
		{
		}

		/// <summary>
		/// Get or set TypeName.
		/// </summary>
		[DataMember(Name = AttributeNames.Type, IsRequired = false)]
		public string TypeName { get; set; }

		/// <summary>
		/// Get or set Value.
		/// </summary>
		[DataMember(Name = AttributeNames.Value)]
		public string Value { get; set; }
	}
}
