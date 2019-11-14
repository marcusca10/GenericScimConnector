﻿//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.SchemeEnpoint
{
    public enum Mutability
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "immutable", Justification = "The casing is as specified by a protocol")]
        immutable,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "read", Justification = "The casing is as specified by a protocol")]
        readOnly,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "read", Justification = "The casing is as specified by a protocol")]
        readWrite,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "write", Justification = "The casing is as specified by a protocol")]
        writeOnly
    }
}
