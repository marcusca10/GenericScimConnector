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

        public const string Status400 = "400";
        public const string Status401 = "401";
        public const string Status404 = "404";
        public const string Status409 = "409";
    }
}
