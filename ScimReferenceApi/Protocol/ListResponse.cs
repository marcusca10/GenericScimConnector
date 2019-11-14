//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    [DataContract]
    public class ListResponse<T> : Schematized where T : Resource
    {
        public ListResponse()
        {
            this.AddSchema(SchemaIdentifiers.ListResponse);
        }

        [DataMember(Name = ProtocolAttributeNames.TotalResults)]
        public int TotalResults { get; set; }

        [DataMember(Name = ProtocolAttributeNames.StartIndex, IsRequired = false, EmitDefaultValue = false)]
        public int? StartIndex { get; set; }

        [DataMember(Name = ProtocolAttributeNames.ItemsPerPage, IsRequired = false, EmitDefaultValue = false)]
        public int? ItemsPerPage { get; set; }

        [DataMember(Name = ProtocolAttributeNames.Resources, IsRequired = true)]
        public IEnumerable<T> Resources { get; set; }
    }
}
