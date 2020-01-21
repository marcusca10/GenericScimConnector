//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Feature
    {
        public Feature(bool supported)
        {
            this.Supported = supported;
        }

        [DataMember(Name = AttributeNames.Supported)]
        public bool Supported
        {
            get;
            set;
        }
    }
}
