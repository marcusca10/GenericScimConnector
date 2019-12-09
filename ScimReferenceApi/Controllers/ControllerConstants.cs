﻿//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    public static class ControllerConstants
    {
        public const string DefaultStartIndexString = "1";
        public const char DelimiterComma = ',';
        public const string DefaultContentType = "application/scim+json";
        public const string UriID = "{id}";
        public const string ReferenceCodeSchemaFilePath = "./JsonConstants/ReferenceCodeSchema.json";
        public const string ResourceTypeFilePath = "./JsonConstants/resourceTypes.json";
        public const string DefaultApiRoute = "api/";
        public static readonly string[] AllwaysRetunedAttributes = { AttributeNames.Identifier, AttributeNames.Schemas, AttributeNames.Active, AttributeNames.Metadata };//TODO Read from schema
        public const string DefaultGroupRoute = DefaultApiRoute + "Groups";
        public const string DefualtUserRoute = DefaultApiRoute + "Users";
    }
}