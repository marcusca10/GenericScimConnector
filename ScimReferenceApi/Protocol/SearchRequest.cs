//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    public class SearchRequest
    {
        [DataMember(Name = AttributeNames.Attributes)]
        public List<string> attributes { get; set; }

        [DataMember(Name = ProtocolAttributeNames.Count)]
        public int? count { get; set; }

        [DataMember(Name = ProtocolAttributeNames.ExcludedAttributes)]
        public List<string> excludedAttributes { get; set; }

        [DataMember(Name = AttributeNames.Filter)]
        public string filter { get; set; }

        [DataMember(Name = ProtocolAttributeNames.SortOrder)]
        public string sortOrder { get; set; }

        [DataMember(Name = ProtocolAttributeNames.StartIndex)]
        public int? startIndex { get; set; }
    }
}
