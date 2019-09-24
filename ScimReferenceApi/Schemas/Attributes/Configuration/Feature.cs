using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class Feature
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="supported"></param>
        public Feature(bool supported)
        {
            this.Supported = supported;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = AttributeNames.Supported)]
        public bool Supported
        {
            get;
            set;
        }
    }
}
