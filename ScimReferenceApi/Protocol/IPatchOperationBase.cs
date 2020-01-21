//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    public interface IPatchOperationBase
    {
        OperationName Name { get; set; }

        IPath Path { get; set; }
    }
}
