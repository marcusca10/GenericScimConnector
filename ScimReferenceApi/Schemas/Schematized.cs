//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    [DataContract]
    public abstract class Schematized
    {
        [DataMember(Name = AttributeNames.Schemas, Order = 0)]
        private List<string> schemas;
        private IReadOnlyCollection<string> schemasWrapper;
        private object schemasLock;

        protected Schematized()
        {
            this.OnInitialization();
            this.OnInitialized();
        }

        public virtual IReadOnlyCollection<string> Schemas
        {
            get
            {
                return this.schemasWrapper;
            }
        }

        public void AddSchema(string schemaIdentifier)
        {
            if (string.IsNullOrWhiteSpace(schemaIdentifier))
            {
                throw new ArgumentNullException(nameof(schemaIdentifier));
            }

            Func<bool> containsFunction =
                new Func<bool>(
                    () =>
                        this
                        .schemas
                        .Any(
                            (string item) =>
                                string.Equals(
                                    item,
                                    schemaIdentifier,
                                    StringComparison.OrdinalIgnoreCase)));


            if (!containsFunction())
            {
                lock (this.schemasLock)
                {
                    if (!containsFunction())
                    {
                        this.schemas.Add(schemaIdentifier);
                    }
                }
            }
        }

        public bool Is(string scheme)
        {
            if (string.IsNullOrWhiteSpace(scheme))
            {
                throw new ArgumentNullException(nameof(scheme));
            }

            bool result =
                this
                .schemas
                .Any(
                    (string item) =>
                        string.Equals(item, scheme, StringComparison.OrdinalIgnoreCase));
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
            this.schemasLock = new object();
            this.schemas = new List<string>();
        }

        private void OnInitialized()
        {
            this.schemasWrapper = this.schemas.AsReadOnly();
        }
    }
}
