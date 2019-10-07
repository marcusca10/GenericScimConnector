using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Patch
{
    /// <summary>
    /// Instanciable extension of patchRequest2Base.
    /// </summary>
    [DataContract]
    public sealed class PatchRequest2Compliant : PatchRequest2Base<PatchOperation2SingleValued>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public PatchRequest2Compliant()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PatchRequest2Compliant(IReadOnlyCollection<PatchOperation2SingleValued> operations)
            : base(operations)
        {
        }
    }
}
