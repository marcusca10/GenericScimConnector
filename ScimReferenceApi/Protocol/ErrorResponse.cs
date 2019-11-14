//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AzureAD.Provisioning.ScimReference.Api.Patch;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
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
