//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    public static class SchemaIdentifiers
    {
        private const string VersionSchemasCore2 = "core:2.0:";
        private const string ExtensionEnterprise2 = SchemaIdentifiers.Extension + "enterprise:2.0:";
        private const string Polling1 =
            SchemaIdentifiers.PrefixTypes2 +
            SchemaIdentifiers.Extension +
            "polling:1.0:";

        public const string Extension = "extension:";
        public const string None = "/";
        public const string PrefixTypes2 = "urn:ietf:params:scim:schemas:";
        public const string PrefixTypesActiveDirectory2 = "http://schemas.microsoft.com/2006/11/ResourceManagement/ADSCIM/2.0/";

        public const string Changed =
            SchemaIdentifiers.Polling1 +
            "Changed";

        public const string Core2EnterpriseUser =
            SchemaIdentifiers.PrefixTypes2 +
            SchemaIdentifiers.ExtensionEnterprise2 +
            ResourceTypes.User;

        public const string Core2EnterpriseUserDomainControllerServices =
            "urn:ietf:params:scim:schemas:extension:enterprise:2.0User";

        public const string Core2Group =
            SchemaIdentifiers.PrefixTypes2 +
            SchemaIdentifiers.VersionSchemasCore2 +
            ResourceTypes.Group;

        public const string Core2ResourceType =
            SchemaIdentifiers.PrefixTypes2 +
            SchemaIdentifiers.ExtensionEnterprise2 +
            ResourceTypes.ResourceType;

        public const string Core2ServiceConfiguration =
            SchemaIdentifiers.PrefixTypes2 +
            SchemaIdentifiers.VersionSchemasCore2 +
            ResourceTypes.ServiceProviderConfiguration;

        public const string Core2User =
            SchemaIdentifiers.PrefixTypes2 +
            SchemaIdentifiers.VersionSchemasCore2 +
            ResourceTypes.User;

        public const string ListResponse = "urn:ietf:params:scim:api:messages:2.0:ListResponse";
    }
}
