//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes;

    [DataContract]
    public abstract class TypedItem : AttributeObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [DataMember(Name = AttributeNames.Type)]
        public string ItemType { get; set; }

        [DataMember(Name = AttributeNames.Primary, IsRequired = false)]
        public bool Primary { get; set; }
    }
}
