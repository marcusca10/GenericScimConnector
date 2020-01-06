//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    public static class ControllerConstants
    {
        public static readonly string[] AlwaysRetunedAttributes = 
        {
            AttributeNames.Identifier,
            AttributeNames.Schemas,
            AttributeNames.Active,
            AttributeNames.Metadata 
        };//TODO: Read from schema
        
        public const string AttributeValueIdentifier = "{id}";

        public const string DefaultContentType = "application/scim+json";
        public const string DefaultRouteApi = "Scim/";

        public const string DefaultRouteGroups = ControllerConstants.DefaultRouteApi + "groups";
        public const string DefaultRouteKey = ControllerConstants.DefaultRouteApi + "key";
        public const string DefaultRouteResourceType = ControllerConstants.DefaultRouteApi + "resourceTypes";
        public const string DefaultRouteSchemas = ControllerConstants.DefaultRouteApi + "schemas";
        public const string DefaultRouteServiceConfiguration = ControllerConstants.DefaultRouteApi + "serviceConfiguration";
        public const string DefaultRouteUsers = ControllerConstants.DefaultRouteApi + "users";
        public const string DefaultStartIndexString = "1";
        
        public const char DelimiterComma = ',';
                
        public const string ReferenceCodeSchemaFilePath = "./JsonConstants/ReferenceCodeSchema.json";
        public const string ResourceTypeFilePath = "./JsonConstants/resourceTypes.json";
    }
}
