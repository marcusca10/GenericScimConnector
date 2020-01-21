//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.GroupAttributes
{
    using System.Runtime.Serialization;

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
