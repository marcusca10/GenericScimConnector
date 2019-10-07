using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    /// <summary>
    /// interface for defining a filter.
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// Get or set the next filter in a list of and filter expresions.
        /// </summary>
        IFilter AdditionalFilter { get; set; }
        /// <summary>
        /// Get the path of the value being operated on.
        /// </summary>
        string AttributePath { get; }
        /// <summary>
        /// Get the value that the attribute value will be compared against.
        /// </summary>
        string ComparisonValue { get; }
        /// <summary>
        /// Get the encoded value that the attribute value will be compared against.
        /// </summary>
        string ComparisonValueEncoded { get; }
        /// <summary>
        /// Get or set the DataType of the value being compared.
        /// </summary>
        AttributeDataType? DataType { get; set; }
        /// <summary>
        /// Get the operator that will be used to compare values.
        /// </summary>
        ComparisonOperator FilterOperator { get; }
        /// <summary>
        /// Serialize the filter.
        /// </summary>

        string Serialize();
    }
}