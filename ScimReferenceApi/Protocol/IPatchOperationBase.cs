using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    /// <summary>
    /// Interface for PatchOperationBase.
    /// </summary>
    public interface IPatchOperationBase
    {
        /// <summary>
        /// Get or set the operation name.
        /// </summary>
        OperationName Name { get; set; }
        /// <summary>
        /// Get or set the path of the attribute to be changed.
        /// </summary>
        IPath Path { get; set; }
    }
}
