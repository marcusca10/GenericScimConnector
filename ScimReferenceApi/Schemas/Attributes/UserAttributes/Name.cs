//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes;

    [DataContract]
    public sealed class Name : AttributeObject
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [DataMember(Name = AttributeNames.Formatted, Order = 0)]
        public string Formatted { get; set; }

        [DataMember(Name = AttributeNames.FamilyName, Order = 1)]
        public string FamilyName { get; set; }

        [DataMember(Name = AttributeNames.GivenName, Order = 1)]
        public string GivenName { get; set; }

        [DataMember(Name = AttributeNames.HonorificPrefix, Order = 1)]
        public string HonorificPrefix { get; set; }

        [DataMember(Name = AttributeNames.HonorificSuffix, Order = 1)]
        public string HonorificSuffix { get; set; }
    }
}
