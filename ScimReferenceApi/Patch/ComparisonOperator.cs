using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    /// <summary>
    /// 
    /// </summary>
    public enum ComparisonOperator
    {
        /// <summary>
        /// 
        /// </summary>
        BitAnd,
        /// <summary>
        /// 
        /// </summary>
        EndsWith,
        /// <summary>
        /// 
        /// </summary>
        Equals,
        /// <summary>
        /// 
        /// </summary>
        EqualOrGreaterThan,
        /// <summary>
        /// 
        /// </summary>
        GreaterThan,
        /// <summary>
        /// 
        /// </summary>
        Includes,
        /// <summary>
        /// 
        /// </summary>
        IsMemberOf,
        /// <summary>
        /// 
        /// </summary>
        MatchesExpression,
        /// <summary>
        /// 
        /// </summary>
        NotBitAnd,
        /// <summary>
        /// 
        /// </summary>
        NotEquals,
        /// <summary>
        /// 
        /// </summary>
        NotMatchesExpression
    }
}
