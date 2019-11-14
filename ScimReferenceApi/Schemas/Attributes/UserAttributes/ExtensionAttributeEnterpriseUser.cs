//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes.UserAttributes
{
    [DataContract]
    public sealed class ExtensionAttributeEnterpriseUser : ExtensionAttributeEnterpriseUserBase
    {
        [Key]
        [DataMember]
        [ScaffoldColumn(false)]
        public string Id { get; set; }
        [DataMember(Name = AttributeNames.Manager, IsRequired = false, EmitDefaultValue = false)]
        public Manager Manager
        {
            get;
            set;
        }
    }
}