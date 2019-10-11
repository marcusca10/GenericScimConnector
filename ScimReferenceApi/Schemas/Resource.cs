using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    /// <summary>
    /// A Resource per RFC. An abstract class for objects to be synced across.
    /// </summary>
    [DataContract]
    public abstract class Resource : Schematized
    {
        /// <summary>
        /// Get or set external identifier.
        /// </summary>
        [DataMember(Name = AttributeNames.ExternalIdentifier, IsRequired = false, EmitDefaultValue = false)]
        public string ExternalIdentifier { get; set; }

        /// <summary>
        /// Get or set Identifier for persistent storage lookups.
        /// </summary>
        [DataMember(Name = AttributeNames.Identifier)]
        [Key]
        public string Identifier { get; set; }
    }
}
