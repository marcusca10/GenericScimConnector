using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Patch
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TOperation"></typeparam>
    [DataContract]
    public abstract class PatchRequest2Base<TOperation> : PatchRequestBase
         where TOperation : PatchOperationBase
    {
        [DataMember(Name = ProtocolAttributeNames.Operations, Order = 2)]
        private List<TOperation> operationsValue;
        private IReadOnlyCollection<TOperation> operationsWrapper;

        /// <summary>
        /// 
        /// </summary>
        protected PatchRequest2Base()
        {
            this.OnInitialization();
            this.OnInitialized();
            this.AddSchema(ProtocolSchemaIdentifiers.Version2PatchOperation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operations"></param>
        protected PatchRequest2Base(IReadOnlyCollection<TOperation> operations)
            : this()
        {
            this.operationsValue.AddRange(operations);
        }

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<TOperation> Operations
        {
            get
            {
                return this.operationsWrapper;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        public void AddOperation(TOperation operation)
        {
            if (null == operation)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            this.operationsValue.Add(operation);
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
            this.operationsValue = new List<TOperation>();
        }

        private void OnInitialized()
        {
            this.operationsWrapper = this.operationsValue.AsReadOnly();
        }
    }
}
