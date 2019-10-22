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

    /// <summary>
    /// Instance of pathRequestLegacy
    /// </summary>
    [DataContract]
    public sealed class PatchRequest2Legacy : PatchRequest2Base<PatchOperation>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PatchRequest2Legacy()
        {
        }
        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="operations"></param>
        public PatchRequest2Legacy(IReadOnlyCollection<PatchOperation> operations)
            : base(operations)
        {
        }
    }

}
