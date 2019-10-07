using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    /// <summary>
    /// Class for giving names to QueryKey strings.
    /// </summary>
    public static class QueryKeys
    {
        /// <summary>
        /// Attributes.
        /// </summary>
        public const string Attributes = "attributes";
        /// <summary>
        /// Count.
        /// </summary>
        public const string Count = "count";
        /// <summary>
        /// Filter.
        /// </summary>
        public const string Filter = "filter";
        /// <summary>
        /// ExcludedAttributes.
        /// </summary>
        public const string ExcludedAttributes = "excludedAttributes";
        /// <summary>
        /// StartIndex.
        /// </summary>
        public const string StartIndex = "startIndex";
    }
}
