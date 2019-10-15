using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    /// <summary>
    /// Interface for Path of atrribute being modified.
    /// </summary>
    public interface IPath
    {
        /// <summary>
        /// Get the path to the attribue.
        /// </summary>
        string AttributePath { get; }

        /// <summary>
        /// Get the schema id.
        /// </summary>
        string SchemaIdentifier { get; }

        /// <summary>
        /// Get the list of sub attributes of the path.
        /// </summary>
        IReadOnlyCollection<IFilter> SubAttributes { get; }

        /// <summary>
        /// Path to the value.
        /// </summary>
        IPath ValuePath { get; }
    }
}
