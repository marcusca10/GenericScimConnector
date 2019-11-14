//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using System.Collections.Generic;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    public interface IPath
    {
        string AttributePath { get; }

        string SchemaIdentifier { get; }

        IReadOnlyCollection<IFilter> SubAttributes { get; }

        IPath ValuePath { get; }
    }
}
