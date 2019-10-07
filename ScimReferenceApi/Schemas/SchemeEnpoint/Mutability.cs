using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.SchemeEnpoint
{
    /// <summary>
    /// enum for deffining mutability of schema attributes.
    /// </summary>
    public enum Mutability
    {
        /// <summary>
        /// Potential Gap
        /// immutable
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "immutable", Justification = "The casing is as specified by a protocol")]
        immutable,
        /// <summary>
        /// readOnly
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "read", Justification = "The casing is as specified by a protocol")]
        readOnly,
        /// <summary>
        /// readWrite
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "read", Justification = "The casing is as specified by a protocol")]
        readWrite,
        /// <summary>
        /// writeOnly
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "write", Justification = "The casing is as specified by a protocol")]
        writeOnly
    }
}
