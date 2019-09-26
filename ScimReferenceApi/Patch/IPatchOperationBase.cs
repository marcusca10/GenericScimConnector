using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPatchOperationBase
    {
        /// <summary>
        /// 
        /// </summary>
        OperationName Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        IPath Path { get; set; }
    }
}
