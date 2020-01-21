//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    using System.Runtime.Serialization;

    [DataContract]
    public sealed class ElectronicMailAddress : TypedValue
    {
        public ElectronicMailAddress()
        {
        }

        public const string Home = "home";
        public const string Other = "other";
        public const string Work = "work";
    }
}
