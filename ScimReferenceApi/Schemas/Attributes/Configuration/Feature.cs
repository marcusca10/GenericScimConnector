using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes
{
    /// <summary>
    /// Class for allowing supported get or set.
    /// </summary>
    [DataContract]
    public class Feature
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public Feature(bool supported)
        {
            this.Supported = supported;
        }

        /// <summary>
        /// Get or Set Supported.
        /// </summary>
        [DataMember(Name = AttributeNames.Supported)]
        public bool Supported
        {
            get;
            set;
        }
    }
}
