//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using System.Collections.Generic;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    internal interface IFilterExpression
    {
        IReadOnlyCollection<IFilter> ToFilters();
    }
}
