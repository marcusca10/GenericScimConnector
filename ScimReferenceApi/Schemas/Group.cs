using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.GroupAttributes;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    /// <summary>
    /// Model for Core 2 Group.
    /// </summary>
    [DataContract]
    public class Group : Resource
    {
        /// <summary>
        /// Consructor.
        /// </summary>
        public Group()
        {
            this.AddSchema(SchemaIdentifiers.Core2Group);
            this.Metadata =
                new Metadata()
                {
                    ResourceType = Types.Group
                };
        }

        /// <summary>
        /// Get or set Metadata.
        /// </summary>
        [DataMember(Name = AttributeNames.Metadata)]
        public Metadata Metadata { get; set; }

        /// <summary>
        /// Get or set DisplayName.
        /// </summary>
        [DataMember(Name = AttributeNames.DisplayName)]
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// Get or set Members.
        /// </summary>
        [DataMember(Name = AttributeNames.Members, IsRequired = false, EmitDefaultValue = false)]
        public virtual IEnumerable<Member> Members { get; set; }
    }
}
