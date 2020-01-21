//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    using System.Collections.Generic;

    public interface IPath
    {
        string AttributePath { get; }

        string SchemaIdentifier { get; }

        IReadOnlyCollection<IFilter> SubAttributes { get; }

        IPath ValuePath { get; }
    }
}
