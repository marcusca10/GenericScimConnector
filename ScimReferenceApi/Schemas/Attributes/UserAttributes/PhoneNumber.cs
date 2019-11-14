//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
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
