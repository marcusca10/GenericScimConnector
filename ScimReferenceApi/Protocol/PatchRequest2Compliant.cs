using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Patch
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public sealed class PatchRequest2Compliant : PatchRequest2Base<PatchOperation2SingleValued>
    {
        /// <summary>
        /// 
        /// </summary>
        public PatchRequest2Compliant()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operations"></param>
        public PatchRequest2Compliant(IReadOnlyCollection<PatchOperation2SingleValued> operations)
            : base(operations)
        {
        }
    }
}
