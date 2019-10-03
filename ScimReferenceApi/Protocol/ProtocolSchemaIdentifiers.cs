using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Patch
{
    /// <summary>
    /// 
    /// </summary>
    public static class ProtocolSchemaIdentifiers
    {
        private const string Error = "Error";

        private const string OperationPatch = "PatchOp";

        private const string VersionMessages2 = "2.0:";

        private const string PrefixMessages = "urn:ietf:params:scim:api:messages:";
        /// <summary>
        /// 
        /// </summary>
        public const string PrefixMessages2 = ProtocolSchemaIdentifiers.PrefixMessages + ProtocolSchemaIdentifiers.VersionMessages2;

        private const string RequestBulk = "BulkRequest";

        private const string ResponseBulk = "BulkResponse";
        private const string ResponseList = "ListResponse";
        private const string SearchRequest = "SearchRequest";
        /// <summary>
        /// 
        /// </summary>
        public const string Version2BulkRequest =
            ProtocolSchemaIdentifiers.PrefixMessages2 + ProtocolSchemaIdentifiers.RequestBulk;
        /// <summary>
        /// 
        /// </summary>
        public const string Version2BulkResponse =
            ProtocolSchemaIdentifiers.PrefixMessages2 + ProtocolSchemaIdentifiers.ResponseBulk;
        /// <summary>
        /// 
        /// </summary>
        public const string Version2Error =
            ProtocolSchemaIdentifiers.PrefixMessages2 + ProtocolSchemaIdentifiers.Error;
        /// <summary>
        /// 
        /// </summary>
        public const string Version2ListResponse =
            ProtocolSchemaIdentifiers.PrefixMessages2 + ProtocolSchemaIdentifiers.ResponseList;
        /// <summary>
        /// 
        /// </summary>
        public const string Version2PatchOperation =
            ProtocolSchemaIdentifiers.PrefixMessages2 + ProtocolSchemaIdentifiers.OperationPatch;
        /// <summary>
        /// 
        /// </summary>
        public const string Version2SearchRequest =
            ProtocolSchemaIdentifiers.PrefixMessages2 + ProtocolSchemaIdentifiers.SearchRequest;
    }
}