//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    [DataContract]
    public class Metadata : AttributeObject
    {
        [Key]
        [ScaffoldColumn(false)]
        public string Id { get; set; }

        [DataMember(Name = AttributeNames.ResourceType, Order = 0)]
        public string ResourceType { get; set; }

        [DataMember(Name = AttributeNames.Created)]
        public DateTime Created { get; set; }

        [DataMember(Name = AttributeNames.LastModified)]
        public DateTime LastModified { get; set; }

        public string location { get; set; }
    }
}
