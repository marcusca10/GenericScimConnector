//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    using System.Runtime.Serialization;

    [DataContract]
    public sealed class Address : TypedItem
    {
        public const string Home = "home";
        public const string Other = "other";
        public const string Untyped = "untyped";
        public const string Work = "work";

        public Address()
        {
        }

        [DataMember(Name = AttributeNames.Country)]
        public string Country { get; set; }

        [DataMember(Name = AttributeNames.Formatted)]
        public string Formatted { get; set; }

        [DataMember(Name = AttributeNames.Locality, IsRequired = false)]
        public string Locality { get; set; }

        [DataMember(Name = AttributeNames.PostalCode)]
        public string PostalCode { get; set; }

        [DataMember(Name = AttributeNames.Region)]
        public string Region { get; set; }

        [DataMember(Name = AttributeNames.StreetAddress)]
        public string StreetAddress { get; set; }
    }
}
