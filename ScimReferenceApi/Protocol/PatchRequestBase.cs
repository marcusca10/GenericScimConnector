//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Patch
{
    using System.Runtime.Serialization;
    using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;

    [DataContract]
    public abstract class PatchRequestBase : Schematized
    {
    }
}
