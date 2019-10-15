using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    /// <summary>
    /// Class for OperationValue.
    /// </summary>
    [DataContract]
    public sealed class OperationValue
    {
        private const string Template = "{0} {1}";

        /// <summary>
        /// Get or set Reference.
        /// </summary>
        [DataMember(Name = ProtocolAttributeNames.Reference, Order = 0, IsRequired = false, EmitDefaultValue = false)]
        public string Reference
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set Value.
        /// </summary>
        [DataMember(Name = AttributeNames.Value, Order = 1)]
        public string Value
        {
            get;
            set;
        }

        /// <summary>
        /// ToString override.
        /// </summary>
        public override string ToString()
        {
            string result =
                string.Format(
                    CultureInfo.InvariantCulture,
                    OperationValue.Template,
                    this.Value,
                    this.Reference)
                .Trim();
            return result;
        }
    }
}
