using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    /// <summary>
    /// Enum for patch operations.
    /// </summary>
    public enum OperationName
    {
        /// <summary>
        /// Add.
        /// </summary>
        Add,
        /// <summary>
        /// Remove.
        /// </summary>
        Remove,
        /// <summary>
        /// Replace.
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