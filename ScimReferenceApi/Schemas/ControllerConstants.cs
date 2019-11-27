//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    public static class ControllerConstants
    {
        public const string DefaultStartIndexString = "1";
        public const string DefaultContentType = "application/scim+json";
        public const string UriID = "{id}";
        public static readonly string[] AllwaysRetunedAttributes = { AttributeNames.Identifier, AttributeNames.Schemas, AttributeNames.Active, AttributeNames.Metadata };//TODO Read from schema
    }
}
