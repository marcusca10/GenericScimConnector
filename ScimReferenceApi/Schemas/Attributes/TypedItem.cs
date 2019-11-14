//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    [DataContract]
    public abstract class TypedItem : AttributeObject
    {
        [Key]
        public string Id { get; set; }

        [DataMember(Name = AttributeNames.Type)]
        public string ItemType { get; set; }

        [DataMember(Name = AttributeNames.Primary, IsRequired = false)]
        public bool Primary { get; set; }
    }
}
