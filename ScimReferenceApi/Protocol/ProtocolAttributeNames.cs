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
    public static class ProtocolAttributeNames
    {
        /// <summary>
        /// 
        /// </summary>
        public const string BulkOperationIdentifier = "bulkId";
        /// <summary>
        /// 
        /// </summary>
        public const string Count = "count";
        /// <summary>
        /// 
        /// </summary>
        public const string Data = "data";
        /// <summary>
        /// 
        /// </summary>
        public const string Detail = "detail";
        /// <summary>
        /// 
        /// </summary>
        public const string ErrorType = "scimType";
        /// <summary>
        /// 
        /// </summary>
        public const string ExcludedAttributes = "excludedAttributes";
        /// <summary>
        /// 
        /// </summary>
        public const string FailOnErrors = "failOnErrors";
        /// <summary>
        /// 
        /// </summary>
        public const string ItemsPerPage = "itemsPerPage";
        /// <summary>
        /// 
        /// </summary>
        public const string Location = "location";
        /// <summary>
        /// 
        /// </summary>
        public const string Method = "method";
        /// <summary>
        /// 
        /// </summary>
        public const string Operations = "Operations";
        /// <summary>
        /// 
        /// </summary>
        public const string Patch1Operation = "operation";
        /// <summary>
        /// 
        /// </summary>
        public const string Patch2Operation = "op";
        /// <summary>
        /// 
        /// </summary>
        public const string Path = AttributeNames.Path;
        /// <summary>
        /// 
        /// </summary>
        public const string Reference = "$ref";
        /// <summary>
        /// 
        /// </summary>
        public const string Resources = "Resources";
        /// <summary>
        /// 
        /// </summary>
        public const string Response = "response";
        /// <summary>
        /// 
        /// </summary>
        public const string Schemas = "schemas";
        /// <summary>
        /// 
        /// </summary>
        public const string SortBy = "sortBy";
        /// <summary>
        /// 
        /// </summary>
        public const string SortOrder = "sortOrder";
        /// <summary>
        /// 
        /// </summary>
        public const string StartIndex = "startIndex";
        /// <summary>
        /// 
        /// </summary>
        public const string Status = "status";
        /// <summary>
        /// 
        /// </summary>
        public const string TotalResults = "totalResults";
    }
}
