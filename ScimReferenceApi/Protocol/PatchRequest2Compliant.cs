//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Patch
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;

    [DataContract]
    public sealed class PatchRequest2Compliant : PatchRequest2Base<PatchOperation2SingleValued>
    {
        public PatchRequest2Compliant()
        {
        }

        public PatchRequest2Compliant(IReadOnlyCollection<PatchOperation2SingleValued> operations)
            : base(operations)
        {
        }
    }

    [DataContract]
    public sealed class PatchRequestSimple : PatchRequest2Base<PatchOperation>
    {
        public PatchRequestSimple()
        {
        }
        public PatchRequestSimple(IReadOnlyCollection<PatchOperation> operations)
            : base(operations)
        {
        }
    }

}
