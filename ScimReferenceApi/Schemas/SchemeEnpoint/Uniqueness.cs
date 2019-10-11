using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.SchemeEnpoint
{
    /// <summary>
    /// Enum for setting uniqueness values.
    /// </summary>
    public enum Uniqueness
    {
        /// <summary>
        /// none
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "none", Justification = "The casing is as specified by a protocol")]
        none,

        /// <summary>
        /// server
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "server", Justification = "The casing is as specified by a protocol")]
        server,

        /// <summary>
        /// global
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "global", Justification = "The casing is as specified by a protocol")]
        global
    }
}
