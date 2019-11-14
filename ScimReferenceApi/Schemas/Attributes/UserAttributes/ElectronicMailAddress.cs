//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
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
