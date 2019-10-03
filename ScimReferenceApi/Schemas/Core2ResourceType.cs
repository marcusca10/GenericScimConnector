//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    using System;
    using System.Runtime.Serialization;
	/// <summary>
	/// Class for stroing info about a resource.
	/// </summary>
    [DataContract]
    public sealed class Core2ResourceType: Schematized
    {
        private Uri endpoint;

        [DataMember(Name = AttributeNames.Endpoint)]
        private string endpointValue;
		/// <summary>
		/// 
		/// </summary>
        [DataMember(Name = AttributeNames.Identifier)]
        public string Identifier
        {
            get;
            set;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification="Serialized")]
        [DataMember(Name = AttributeNames.Name)]
        private string name;
		/// <summary>
		/// 
		/// </summary>
        public Core2ResourceType()
        {
            this.AddSchema(SchemaIdentifiers.Core2ResourceType);
            this.Metadata =
                new Metadata()
                    {
                        ResourceType = Types.ResourceType
                    };
        }
		/// <summary>
		/// 
		/// </summary>
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Setter can be called on de-serialization.")]
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
		/// <summary>
		/// 
		/// </summary>
        [DataMember(Name = AttributeNames.Metadata)]
        public Metadata Metadata
        {
            get;
            set;
        }
		/// <summary>
		/// 
		/// </summary>
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