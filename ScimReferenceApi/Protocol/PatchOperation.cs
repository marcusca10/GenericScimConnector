using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public sealed class PatchOperation : PatchOperationBase
    {
        private const string Template = "{0}: [{1}]";

        [DataMember(Name = AttributeNames.Value, Order = 2)]
        private List<OperationValue> values;
        private IReadOnlyCollection<OperationValue> valuesWrapper;

        /// <summary>
        /// 
        /// </summary>
        public PatchOperation()
        {
            this.OnInitialization();
            this.OnInitialized();
        }

        /// <summary>
        /// 
        /// </summary>
        public PatchOperation(OperationName operationName, string pathExpression)
            : base(operationName, pathExpression)
        {
            this.OnInitialization();
            this.OnInitialized();
        }


        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<OperationValue> Value
        {
            get
            {
                return this.valuesWrapper;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddValue(OperationValue value)
        {
            if (null == value)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.values.Add(value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static PatchOperation Create(OperationName operationName, string pathExpression, string value)
        {
            if (string.IsNullOrWhiteSpace(pathExpression))
            {
                throw new ArgumentNullException(nameof(pathExpression));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            OperationValue operationValue = new OperationValue();
            operationValue.Value = value;

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
            this.values = new List<OperationValue>();
        }

        private void OnInitialized()
        {
            switch (this.values)
            {
                case List<OperationValue> valueList:
                    this.valuesWrapper = valueList.AsReadOnly();
                    break;
                default:
                    throw new NotSupportedException("ExceptionInvalidValue");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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