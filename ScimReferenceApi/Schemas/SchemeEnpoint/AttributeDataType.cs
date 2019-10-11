using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    /// <summary>
    /// Enum of datatypes for use in identifying the attributes datatype.
    /// </summary>
    public enum AttributeDataType
    {
        /// <summary>
        /// String.
        /// </summary>
        String,
        /// <summary>
        /// Boolean.
        /// </summary>
        Boolean,
        /// <summary>
        /// Decimal.
        /// </summary>
        Decimal,
        /// <summary>
        /// Integer.
        /// </summary>
        Integer,
        /// <summary>
        /// DateTime.
        /// </summary>
        DateTime,
        /// <summary>
        /// Binary.
        /// </summary>
        Binary,
        /// <summary>
        /// Reference.
        /// </summary>
        Reference,
        /// <summary>
        /// Complex.
        /// </summary>
        Complex
    }
}
