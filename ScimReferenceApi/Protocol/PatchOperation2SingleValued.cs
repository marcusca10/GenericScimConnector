using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Patch
{
    /// <summary>
    /// Patch operation extension for ops with only one value.
    /// </summary>
    [DataContract]
    public sealed class PatchOperation2SingleValued : PatchOperationBase
    {
        private const string Template = "{0}: [{1}]";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "The serialized value is consumed.")]
        [DataMember(Name = AttributeNames.Value, Order = 2)]
        private string valueValue;

        /// <summary>
        /// Constructor.
        /// </summary>
        public PatchOperation2SingleValued()
        {
            this.OnInitialization();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PatchOperation2SingleValued(OperationName operationName, string pathExpression, string value)
            : base(operationName, pathExpression)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.valueValue = value;
        }

        /// <summary>
        /// Get the value.
        /// </summary>
        public string Value
        {
            get
            {
                return this.valueValue;
            }
        }

        [OnDeserializing]
        private void OnDeserializing(StreamingContext context)
        {
            this.OnInitialization();
        }

        private void OnInitialization()
        {
            this.valueValue = string.Empty;
        }

        /// <summary>
        /// ToString overrride.
        /// </summary>
        public override string ToString()
        {
            string operation = base.ToString();
            string result =
                string.Format(
                    CultureInfo.InvariantCulture,
                    PatchOperation2SingleValued.Template,
                    operation,
                    this.valueValue);
            return result;
        }
    }
}
