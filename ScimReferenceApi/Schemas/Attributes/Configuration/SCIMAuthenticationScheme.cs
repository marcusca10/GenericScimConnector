﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes.Configuration
{
    /// <summary>
    /// 
    /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = AttributeNames.Type)]
        public string AuthenticationType
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = AttributeNames.Description)]
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = AttributeNames.Documentation)]
        public Uri DocumentationResource
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = AttributeNames.Name)]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = AttributeNames.Primary)]
        public bool Primary
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = AttributeNames.Specification)]
        public Uri SpecificationResource
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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