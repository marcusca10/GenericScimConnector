//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes;

    [DataContract]
    public class Metadata : AttributeObject
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
