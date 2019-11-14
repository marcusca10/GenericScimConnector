//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using System;
using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes.Configuration
{
    [DataContract]
    public sealed class SCIMAuthenticationScheme
    {
        private const string AuthenticationTypeResourceValueOpenStandardForAuthenticationBearerToken =
            "oauthbearertoken";

        private const string DescriptionOpenStandardForAuthenticationBearerToken =
            "Authentication Scheme using the OAuth Bearer Token Standard";

        private const string DocumentationResourceValueOpenStandardForAuthenticationBearerToken =
            "http://example.com/help/oauth.html";

        private const string NameOpenStandardForAuthenticationBearerToken = "OAuth Bearer Token";

        private const string SpecificationResourceValueOpenStandardForAuthenticationBearerToken =
            "http://tools.ietf.org/html/draft-ietf-oauth-v2-bearer-01";

        private static readonly Lazy<Uri> DocumentationResourceOpenStandardForAuthenticationBearerToken =
            new Lazy<Uri>(
                () =>
                    new Uri(SCIMAuthenticationScheme.DocumentationResourceValueOpenStandardForAuthenticationBearerToken));

        private static readonly Lazy<Uri> SpecificationResourceOpenStandardForAuthenticationBearerToken =
            new Lazy<Uri>(
                () =>
                    new Uri(SCIMAuthenticationScheme.SpecificationResourceValueOpenStandardForAuthenticationBearerToken));

        [DataMember(Name = AttributeNames.Type)]
        public string AuthenticationType
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Description)]
        public string Description
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Documentation)]
        public Uri DocumentationResource
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Name)]
        public string Name
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Primary)]
        public bool Primary
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Specification)]
        public Uri SpecificationResource
        {
            get;
            set;
        }

        public static SCIMAuthenticationScheme CreateOpenStandardForAuthorizationBearerTokenScheme()
        {
            SCIMAuthenticationScheme result =
                new SCIMAuthenticationScheme()
                {
                    AuthenticationType =
                            SCIMAuthenticationScheme.AuthenticationTypeResourceValueOpenStandardForAuthenticationBearerToken,
                    Name =
                            SCIMAuthenticationScheme.NameOpenStandardForAuthenticationBearerToken,
                    Description =
                            SCIMAuthenticationScheme.DescriptionOpenStandardForAuthenticationBearerToken,
                    DocumentationResource =
                            SCIMAuthenticationScheme.DocumentationResourceOpenStandardForAuthenticationBearerToken.Value,
                    SpecificationResource =
                            SCIMAuthenticationScheme.SpecificationResourceOpenStandardForAuthenticationBearerToken.Value
                };
            return result;
        }
    }
}
