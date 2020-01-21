//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    using System.Collections.Generic;

    internal interface IFilterExpression
    {
        IReadOnlyCollection<IFilter> ToFilters();
    }
}
