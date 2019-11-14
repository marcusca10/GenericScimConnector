﻿//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    [DataContract]
    public abstract class TypedValue : TypedItem
    {
        [DataMember(Name = AttributeNames.Value, Order = 0)]
        public string Value { get; set; }
    }
}
