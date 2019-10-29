using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    /// <summary>
    /// Metadata, of a Resource.
    /// </summary>
    [DataContract]
    public class Metadata : AttributeObject
    {

        /// <summary>
        /// Get or set Id Primary Key for persistant storage.
        /// </summary>
        [Key]
        [ScaffoldColumn(false)]
        public string Id { get; set; }

        /// <summary>
        /// Get or set ResourceType.
        /// </summary>
        [DataMember(Name = AttributeNames.ResourceType, Order = 0)]
        public string ResourceType { get; set; }

        /// <summary>
        /// Get or set Created. The datetime the resource was added.
        /// </summary>
        [DataMember(Name = AttributeNames.Created)]
        public DateTime Created { get; set; }

        /// <summary>
        /// Get or set LastModified. The last datetime the resource was updated.
        /// </summary>
        [DataMember(Name = AttributeNames.LastModified)]
        public DateTime LastModified { get; set; }

        /// <summary>
        /// Get or set location. The uri for the resource.
        /// </summary>
        public string location { get; set; }
    }
}
