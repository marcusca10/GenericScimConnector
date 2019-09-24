using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
	/// <summary>
	/// Name, of a User.
	/// </summary>
	[DataContract]
	public sealed class Name
	{
		/// <summary>
		/// Get or set Id persistant storage primary key.
		/// </summary>
		[Key]
		[ScaffoldColumn(false)]
		public int Id { get; set; }

		/// <summary>
		/// Get or set Formatted the full Name.
		/// </summary>
		[DataMember(Name = AttributeNames.Formatted, Order = 0)]
		public string Formatted { get; set; }

		/// <summary>
		/// Get or set Familyname (Last name).
		/// </summary>
		[DataMember(Name = AttributeNames.FamilyName, Order = 1)]
		public string FamilyName { get; set; }

		/// <summary>
		/// Get or set GivenName (First name).
		/// </summary>
		[DataMember(Name = AttributeNames.GivenName, Order = 1)]
		public string GivenName { get; set; }

		/// <summary>
		/// Get or set HonorificPrefix. e.g. Ms. Mr. Dr.
		/// </summary>
		[DataMember(Name = AttributeNames.HonorificPrefix, Order = 1)]
		public string HonorificPrefix { get; set; }

		/// <summary>
		/// Get or set HonorificSuffix. e.g. II
		/// </summary>
		[DataMember(Name = AttributeNames.HonorificSuffix, Order = 1)]
		public string HonorificSuffix { get; set; }
	}
}
