using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    internal interface IFilterExpression
    {
        IReadOnlyCollection<IFilter> ToFilters();
    }
}
