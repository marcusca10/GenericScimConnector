//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.GroupAttributes
{
    [DataContract]
    public class Member : TypedItem
    {

        internal Member()
        {
        }

        [DataMember(Name = AttributeNames.DisplayName, IsRequired = false)]
        public string DisplayName { get; set; }

        [DataMember(Name = AttributeNames.Value)]
        public string Value { get; set; }
    }
}
