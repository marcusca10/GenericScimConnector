//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using System;
using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    [DataContract]
    public sealed class Core2ResourceType : Schematized
    {
        private Uri endpoint;

        [DataMember(Name = AttributeNames.Endpoint)]
        private string endpointValue;

        [DataMember(Name = AttributeNames.Identifier)]
        public string Identifier
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Name)]
        private string name;

        public Core2ResourceType()
        {
            this.AddSchema(SchemaIdentifiers.Core2ResourceType);
            this.Metadata =
                new Metadata()
                {
                    ResourceType = ResourceTypes.ResourceType
                };
        }

        public Uri Endpoint
        {
            get
            {
                return this.endpoint;
            }

            set
            {
                this.endpoint = value;
                this.endpointValue = new UrlResourceIdentifier(value).RelativePath;
            }
        }

        private string EndpointValue
        {
            get
            {
                return this.endpointValue;
            }

            set
            {
                this.InitializeEndpoint(value);
                this.endpointValue = value;
            }
        }

        [DataMember(Name = AttributeNames.Metadata)]
        public Metadata Metadata
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Schema)]
        public string Schema
        {
            get;
            set;
        }

        private void InitializeEndpoint(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                this.endpoint = null;
                return;
            }

            this.endpoint = new Uri(value, UriKind.Relative);
        }

        private void InitializeEndpoint()
        {
            this.InitializeEndpoint(this.endpointValue);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            this.InitializeEndpoint();
        }

        [OnSerializing]
        private void OnSerializing(StreamingContext context)
        {
            this.name = this.Identifier;
        }
    }
}