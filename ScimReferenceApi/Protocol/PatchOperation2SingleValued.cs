//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Patch
{
    [DataContract]
    public sealed class PatchOperation2SingleValued : PatchOperationBase
    {
        private const string Template = "{0}: [{1}]";

        [DataMember(Name = AttributeNames.Value, Order = 2)]
        private string valueValue;

        public PatchOperation2SingleValued()
        {
            this.OnInitialization();
        }

        public PatchOperation2SingleValued(OperationName operationName, string pathExpression, string value)
            : base(operationName, pathExpression)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.valueValue = value;
        }

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
