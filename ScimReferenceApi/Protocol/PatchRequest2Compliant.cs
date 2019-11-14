﻿//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Patch
{
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
    public sealed class PatchRequest2Legacy : PatchRequest2Base<PatchOperation>
    {
        public PatchRequest2Legacy()
        {
        }
        public PatchRequest2Legacy(IReadOnlyCollection<PatchOperation> operations)
            : base(operations)
        {
        }
    }

}
