using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// 
        /// </summary>
        IFilter AdditionalFilter { get; set; }
        /// <summary>
        /// 
        /// </summary>
        string AttributePath { get; }
        /// <summary>
        /// 
        /// </summary>
        string ComparisonValue { get; }
        /// <summary>
        /// 
        /// </summary>
        string ComparisonValueEncoded { get; }
        /// <summary>
        /// 
        /// </summary>
        AttributeDataType? DataType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        ComparisonOperator FilterOperator { get; }
        /// <summary>
        /// 
        /// </summary>

        string Serialize();
    }
}