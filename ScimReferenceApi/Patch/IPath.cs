using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPath
    {
        /// <summary>
        /// 
        /// </summary>
        string AttributePath { get; }
        /// <summary>
        /// 
        /// </summary>
        string SchemaIdentifier { get; }
        /// <summary>
        /// 
        /// </summary>
        IReadOnlyCollection<IFilter> SubAttributes { get; }
        /// <summary>
        /// 
        /// </summary>
        IPath ValuePath { get; }
    }
}