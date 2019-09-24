using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
	/// <summary>
	/// Model of Core 2 User.
	/// </summary>
	[DataContract]
	public class User : Resource
	{
		/// <summary>
		/// Get or set user name.
		/// </summary>
		[DataMember(Name = AttributeNames.UserName)]
		public virtual string UserName { get; set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		public User()
		{
			this.AddSchema(SchemaIdentifiers.Core2User);
			this.Metadata =
				new Metadata()
				{
					ResourceType = Types.User,
				};

			Active = true;
		}

		/// <summary>
		/// Get or set Active.
		/// </summary>
		[DataMember(Name = AttributeNames.Active)]
		public virtual bool Active { get; set; }

		/// <summary>
		/// Get or set Addresses.
		/// </summary>
		[DataMember(Name = AttributeNames.Addresses, IsRequired = false, EmitDefaultValue = false)]
		public virtual IEnumerable<Address> Addresses { get; set; }

		/// <summary>
		/// Get or set DisplayName, should be human readable.
		/// </summary>
		[DataMember(Name = AttributeNames.DisplayName, IsRequired = false, EmitDefaultValue = false)]
		public virtual string DisplayName { get; set; }

		/// <summary>
		/// Get or set ElectronicMailAddresses, JSON name email.
		/// </summary>
		[DataMember(Name = AttributeNames.ElectronicMailAddresses, IsRequired = false, EmitDefaultValue = false)]
		public virtual IEnumerable<ElectronicMailAddress> ElectronicMailAddresses { get; set; }

		/// <summary>
		/// Get or set Metadata.
		/// </summary>
		[DataMember(Name = AttributeNames.Metadata)]
		public virtual Metadata Metadata { get; set; }

		/// <summary>
		/// Get or set Name.
		/// </summary>
		[DataMember(Name = AttributeNames.Name, IsRequired = false, EmitDefaultValue = false)]
		public virtual Name Name { get; set; }

		/// <summary>
		/// Get or set PhoneNumbers.
		/// </summary>
		[DataMember(Name = AttributeNames.PhoneNumbers, IsRequired = false, EmitDefaultValue = false)]
		public virtual IEnumerable<PhoneNumber> PhoneNumbers { get; set; }

		/// <summary>
		/// Get or set PreferredLanguage.
		/// </summary>
		[DataMember(Name = AttributeNames.PreferredLanguage, IsRequired = false, EmitDefaultValue = false)]
		public virtual string PreferredLanguage { get; set; }

		/// <summary>
		/// Get or set Roles.
		/// </summary>
		[DataMember(Name = AttributeNames.Roles, IsRequired = false, EmitDefaultValue = false)]
		public virtual IEnumerable<Role> Roles { get; set; }

		/// <summary>
		/// Get or set Title
		/// </summary>
		[DataMember(Name = AttributeNames.Title, IsRequired = false, EmitDefaultValue = false)]
		public virtual string Title { get; set; }
	}

	/// <summary>
	/// User extentions for queyable items
	/// </summary>
	public static class UserExtensions
	{
		/// <summary>
		/// Returns fully populated Users 
		/// </summary>
		public static IQueryable<User> CompleteUsers(this ScimContext context)
		{
			return context.Users.Include("Metadata")
					.Include("Name")
					.Include("ElectronicMailAddresses")
					.Include("PhoneNumbers")
					.Include("Roles")
					.Include("Addresses");
		}
	}
}
