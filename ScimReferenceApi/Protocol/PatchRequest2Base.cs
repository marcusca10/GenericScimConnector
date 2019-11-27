//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Patch
{
    [DataContract]
    public abstract class PatchRequest2Base<TOperation> : PatchRequestBase
         where TOperation : PatchOperationBase
    {
        [DataMember(Name = ProtocolAttributeNames.Operations, Order = 2)]
        private List<TOperation> operationsValue;
        private IReadOnlyCollection<TOperation> operationsWrapper;

        protected PatchRequest2Base()
        {
            this.OnInitialization();
            this.OnInitialized();
            this.AddSchema(ProtocolSchemaIdentifiers.Version2PatchOperation);
        }

        protected PatchRequest2Base(IReadOnlyCollection<TOperation> operations)
            : this()
        {
            this.operationsValue.AddRange(operations);
        }

        public IReadOnlyCollection<TOperation> Operations
        {
            get
            {
                return this.operationsWrapper;
            }
        }

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
