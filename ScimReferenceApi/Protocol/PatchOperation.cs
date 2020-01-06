//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using Newtonsoft.Json.Linq;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    [DataContract]
    public sealed class PatchOperation : PatchOperationBase
    {
        private const string Template = "{0}: [{1}]";

        [DataMember(Name = AttributeNames.Value, Order = 2)]
        private List<JToken> values;
        private IReadOnlyCollection<JToken> valuesWrapper;

        public PatchOperation()
        {
            this.OnInitialization();
            this.OnInitialized();
        }

        public PatchOperation(OperationName operationName, string pathExpression)
            : base(operationName, pathExpression)
        {
            this.OnInitialization();
            this.OnInitialized();
        }

        public IReadOnlyCollection<JToken> Value
        {
            get
            {
                return this.valuesWrapper;
            }
            set
            {
                this.valuesWrapper = value;
            }
        }

        public void AddValue(JToken value)
        {
            if (null == value)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.values.Add(value);
        }

        public static PatchOperation Create(OperationName operationName, string pathExpression, string value)
        {
            if (string.IsNullOrWhiteSpace(pathExpression))
            {
                throw new ArgumentNullException(nameof(pathExpression));
            }

            if (string.IsNullOrWhiteSpace(value) && operationName != Protocol.OperationName.Remove)
            {
                throw new ArgumentNullException(nameof(value));
            }

            JObject operationValue = new JObject();
            operationValue.Add("value", value);

            PatchOperation result = new PatchOperation(operationName, pathExpression);
            result.AddValue(operationValue);

            return result;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            this.OnInitialized();
        }

        [OnDeserializing]
        private void OnDeserializing(StreamingContext context)
        {
            this.OnInitialization();
        }

        private void OnInitialization()
        {
            this.values = new List<JToken>();
        }

        private void OnInitialized()
        {
            switch (this.values)
            {
                case List<JToken> valueList:
                    this.valuesWrapper = valueList.AsReadOnly();
                    break;
                default:
                    throw new NotSupportedException("ExceptionInvalidValue");
            }
        }

        public override string ToString()
        {
            string allValues = string.Join(Environment.NewLine, this.Value);
            string operation = base.ToString();
            string result =
                string.Format(
                    CultureInfo.InvariantCulture,
                    PatchOperation.Template,
                    operation,
                    allValues);
            return result;
        }
    }
}
