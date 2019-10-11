using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    /// <summary>
    /// Body for 
    /// </summary>
    public class SearchRequest
    {
        /// <summary>
        /// Requested Attributes
        /// </summary>
        [DataMember(Name = AttributeNames.Attributes)]
        public List<string> attributes { get; set; }
        /// <summary>
        /// Count of items
        /// </summary>
        [DataMember(Name = ProtocolAttributeNames.Count)]
        public int? count { get; set; }
        /// <summary>
        /// Excluded Attributes 
        /// </summary>
        [DataMember(Name = ProtocolAttributeNames.ExcludedAttributes)]
        public List<string> excludedAttributes { get; set; }
        /// <summary>
        /// Filters 
        /// </summary>
        [DataMember(Name = AttributeNames.Filter)]
        public string filter { get; set; }
        /// <summary>
        /// Sort order
        /// </summary>
        [DataMember(Name = ProtocolAttributeNames.SortOrder)]
        public string sortOrder { get; set; }
        /// <summary>
        /// Start index 
        /// </summary>
        [DataMember(Name = ProtocolAttributeNames.StartIndex)]
        public int? startIndex { get; set; }
    }
}
