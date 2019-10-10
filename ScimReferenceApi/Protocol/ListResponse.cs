using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    /// <summary>
    /// List response, for a list of Resources.
    /// </summary>
    [DataContract]
    public class ListResponse<T> : Schematized where T : Resource
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ListResponse()
        {
            this.AddSchema(SchemaIdentifiers.ListResponse);
        }

        /// <summary>
        /// Get or set Identifier.
        /// </summary>
        [DataMember(Name = AttributeNames.Identifier)]
        public string Identifier { get; set; }

        /// <summary>
        /// Total Count of results.
        /// </summary>
        [DataMember(Name = ProtocolAttributeNames.TotalResults)]
        public int TotalResults { get; set; }

        /// <summary>
        /// Offset index (starts at 1)
        /// </summary>
        [DataMember(Name = ProtocolAttributeNames.StartIndex, IsRequired = false, EmitDefaultValue = false)]
        public int? StartIndex { get; set; }

        /// <summary>
        /// Number of items returned by page
        /// </summary>
        [DataMember(Name = ProtocolAttributeNames.ItemsPerPage, IsRequired = false, EmitDefaultValue = false)]
        public int? ItemsPerPage { get; set; }

        /// <summary>
        /// The list of Resources being returned.
        /// </summary>
        [DataMember(Name = ProtocolAttributeNames.Resources, IsRequired = true)]
        public IEnumerable<T> Resources { get; set; }
    }
}
