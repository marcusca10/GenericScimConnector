using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    /// <summary>
    /// Enum for filter ops.
    /// </summary>
    public enum ComparisonOperator
    {
        /// <summary>
        /// BitAnd.
        /// </summary>
        BitAnd,
        /// <summary>
        /// EndsWith.
        /// </summary>
        EndsWith,
        /// <summary>
        /// Equals.
        /// </summary>
        Equals,
        /// <summary>
        /// EqualOrGreaterThan.
        /// </summary>
        EqualOrGreaterThan,
        /// <summary>
        /// GreaterThan.
        /// </summary>
        GreaterThan,
        /// <summary>
        /// Includes.
        /// </summary>
        Includes,
        /// <summary>
        /// IsMemberOf.
        /// </summary>
        IsMemberOf,
        /// <summary>
        /// MatchesExpression.
        /// </summary>
        MatchesExpression,
        /// <summary>
        /// NotBitAnd.
        /// </summary>
        NotBitAnd,
        /// <summary>
        /// NotEquals.
        /// </summary>
        NotEquals,
        /// <summary>
        /// NotMatchesExpression.
        /// </summary>
        NotMatchesExpression,
        /// <summary>
        /// StartsWith.
        /// </summary>
        StartsWith,
        /// <summary>
        /// Exists.
        /// </summary>
        Exists,
        /// <summary>
        /// LessThan.
        /// </summary>
        LessThan,
        /// <summary>
        /// LessThan.
        /// </summary>
        EqualOrLessThan
    }
}
