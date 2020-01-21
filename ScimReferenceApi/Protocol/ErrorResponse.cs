//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    using System.Runtime.Serialization;
    using Microsoft.AzureAD.Provisioning.ScimReference.Api.Patch;
    using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;

    public class ErrorResponse : Schematized
    {

        public ErrorResponse(string detail, string status)
        {
            this.detail = detail;
            this.status = status;
            this.AddSchema(ProtocolSchemaIdentifiers.Version2Error);
        }

        [DataMember(Name = ProtocolAttributeNames.Detail)]
        public string detail
        {
            get;
            set;
        }

        [DataMember(Name = ProtocolAttributeNames.Status)]
        public string status
        {
            get;
            set;
        }
    }
}
