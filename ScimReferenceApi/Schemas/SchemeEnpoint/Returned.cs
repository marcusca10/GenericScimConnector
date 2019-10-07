using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.SchemeEnpoint
{
    /// <summary>
    /// Enum for defining when a schema value will be returned.
    /// </summary>
    public enum Returned
    {
        /// <summary>
        /// always.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "always", Justification = "The casing is as specified by a protocol")]
        always,
        /// <summary>
        /// @default.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "default", Justification = "The casing is as specified by a protocol")]
        @default,
        /// <summary>
        /// never eg password should never be passed in scim.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "never", Justification = "The casing is as specified by a protocol")]
        never,
        /// <summary>
        /// request.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "request", Justification = "The casing is as specified by a protocol")]
        request
    }
}