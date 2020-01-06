//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    [DataContract]
    public class ServiceConfiguration : Schematized
    {

        [DataMember(Name = AttributeNames.AuthenticationSchemes)]
        private List<SCIMAuthenticationScheme> authenticationSchemes;
        private IReadOnlyCollection<SCIMAuthenticationScheme> authenticationSchemesWrapper;
        private object thisLock;

        public ServiceConfiguration(bool bulkRequestsSupport,
            bool supportsEntityTags,
            bool supportsFiltering,
            bool supportsPasswordChange,
            bool supportsPatching,
            bool supportsSorting)
        {
            this.AddSchema(SchemaIdentifiers.Core2ServiceConfiguration);
            this.OnInitialization();
            this.BulkRequests = new Feature(bulkRequestsSupport);
            this.EntityTags = new Feature(supportsEntityTags);
            this.Filtering = new Feature(supportsFiltering);
            this.PasswordChange = new Feature(supportsPasswordChange);
            this.Patching = new Feature(supportsPatching);
            this.Sorting = new Feature(supportsSorting);
            this.Metadata =
                new Metadata()
                {
                    ResourceType = ResourceTypes.ServiceProviderConfiguration,
                    Created = DateTime.Now,
                    LastModified = DateTime.Now
                };
        }

        [DataMember(Name = AttributeNames.Bulk)]
        public Feature BulkRequests
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Documentation)]
        public string DocumentationResource
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.EntityTag)]
        public Feature EntityTags
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Filter)]
        public Feature Filtering
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.ChangePassword)]
        public Feature PasswordChange
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Patch)]
        public Feature Patching
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Sort)]
        public Feature Sorting
        {
            get;
            set;
        }

        public void AddAuthenticationScheme(SCIMAuthenticationScheme authenticationScheme)
        {
            if (null == authenticationScheme)
            {
                throw new ArgumentNullException(nameof(authenticationScheme));
            }

            if (string.IsNullOrWhiteSpace(authenticationScheme.Name))
            {
                throw new ArgumentNullException(nameof(authenticationScheme));
            }

            Func<bool> containsFunction =
                new Func<bool>(
                        () =>
                            this
                            .authenticationSchemes
                            .Any(
                                (SCIMAuthenticationScheme item) =>
                                    string.Equals(item.Name, authenticationScheme.Name, StringComparison.OrdinalIgnoreCase)));


            if (!containsFunction())
            {
                lock (this.thisLock)
                {
                    if (!containsFunction())
                    {
                        this.authenticationSchemes.Add(authenticationScheme);
                    }
                }
            }
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
            this.thisLock = new object();
            this.authenticationSchemes = new List<SCIMAuthenticationScheme>();
        }

        private void OnInitialized()
        {
            this.authenticationSchemesWrapper = this.authenticationSchemes.AsReadOnly();
        }

        [DataMember(Name = AttributeNames.Metadata)]
        public Metadata Metadata
        {
            get;
            set;
        }
    }
}
