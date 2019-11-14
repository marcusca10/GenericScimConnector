//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    public static class ErrorDetail
    {
        public const string Unparsable = "Request is unparsable, syntactically incorrect, or violates schema.";
        public const string NoUsername = "Username cannot be null or empty";
        public const string UsernameConflict = "Username already exists";
        public const string DisplaynameConflict = "DisplayName already exists";
        public const string Mutability = "Attribute 'id' is read only";
        public const string NotFound = "Resource {0} not found";

    }
}
