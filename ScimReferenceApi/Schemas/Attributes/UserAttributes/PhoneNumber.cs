//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    using System.Runtime.Serialization;

    [DataContract]
    public sealed class PhoneNumber : TypedValue
    {
        public const string Fax = "fax";
        public const string Home = "home";
        public const string Mobile = "mobile";
        public const string Other = "other";
        public const string Pager = "pager";
        public const string Work = "work";

        public PhoneNumber()
        {
        }
    }
}
