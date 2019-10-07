using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    /// <summary>
    /// Class for giving names to attribue strings.
    /// </summary>
    public static class ProtocolAttributeNames
    {
        /// <summary>
        /// BulkOperationIdentifier.
        /// </summary>
        public const string BulkOperationIdentifier = "bulkId";
        /// <summary>
        /// Count.
        /// </summary>
        public const string Count = "count";
        /// <summary>
        /// Data.
        /// </summary>
        public const string Data = "data";
        /// <summary>
        /// Detail.
        /// </summary>
        public const string Detail = "detail";
        /// <summary>
        /// ErrorType.
        /// </summary>
        public const string ErrorType = "scimType";
        /// <summary>
        /// ExcludedAttributes.
        /// </summary>
        public const string ExcludedAttributes = "excludedAttributes";
        /// <summary>
        /// FailOnErrors.
        /// </summary>
        public const string FailOnErrors = "failOnErrors";
        /// <summary>
        /// ItemsPerPage.
        /// </summary>
        public const string ItemsPerPage = "itemsPerPage";
        /// <summary>
        /// Location.
        /// </summary>
        public const string Location = "location";
        /// <summary>
        /// Method.
        /// </summary>
        public const string Method = "method";
        /// <summary>
        /// Operations.
        /// </summary>
        public const string Operations = "Operations";
        /// <summary>
        /// Patch1Operation.
        /// </summary>
        public const string Patch1Operation = "operation";
        /// <summary>
        /// Patch2Operation.
        /// </summary>
        public const string Patch2Operation = "op";
        /// <summary>
        /// Path.
        /// </summary>
        public const string Path = AttributeNames.Path;
        /// <summary>
        /// Reference.
        /// </summary>
        public const string Reference = "$ref";
        /// <summary>
        /// Resources.
        /// </summary>
        public const string Resources = "Resources";
        /// <summary>
        /// Response.
        /// </summary>
        public const string Response = "response";
        /// <summary>
        /// Schemas.
        /// </summary>
        public const string Schemas = "schemas";
        /// <summary>
        /// SortBy.
        /// </summary>
        public const string SortBy = "sortBy";
        /// <summary>
        /// SortOrder.
        /// </summary>
        public const string SortOrder = "sortOrder";
        /// <summary>
        /// StartIndex.
        /// </summary>
        public const string StartIndex = "startIndex";
        /// <summary>
        /// Status.
        /// </summary>
        public const string Status = "status";
        /// <summary>
        /// TotalResults.
        /// </summary>
        public const string TotalResults = "totalResults";
    }
}
