using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    /// <summary>
    /// Class for holding an error.
    /// </summary>
    public class ErrorResponse : Schematized
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        public ErrorResponse(string detail, string status)
        {
            this.detail = detail;
            this.status = status;
        }

        /// <summary>
        /// Detail portion of an error response, describing in human readable format the error.
        /// </summary>
        [DataMember(Name = ProtocolAttributeNames.Detail)]
        public string detail
        {
            get;
            set;
        }

        /// <summary>
        /// The response code.
        /// </summary>
        [DataMember(Name = ProtocolAttributeNames.Status)]
        public string status
        {
            get;
            set;
        }
    }
}
