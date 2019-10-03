using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    /// <summary>
    /// 
    /// </summary>
    public enum OperationName
    {
        /// <summary>
        /// 
        /// </summary>
        Add,
        /// <summary>
        /// 
        /// </summary>
        Remove,
        /// <summary>
        /// 
        /// </summary>
        Replace,
        /// <summary>
        /// Cover scim gap.
        /// </summary>
        add,
        /// <summary>
        /// Cover scim gap.
        /// </summary>
        remove,
        /// <summary>
        /// Cover scim gap.
        /// </summary>
        replace,
    }
}