using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes.Configuration
{
    /// <summary>
    /// Class for defining authentication.
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
        /// Get or set AuthenticationType.
        /// </summary>
        [DataMember(Name = AttributeNames.Type)]
        public string AuthenticationType
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the desciption.
        /// </summary>
        [DataMember(Name = AttributeNames.Description)]
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the DocumentationResource.
        /// </summary>
        [DataMember(Name = AttributeNames.Documentation)]
        public Uri DocumentationResource
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the Name.
        /// </summary>
        [DataMember(Name = AttributeNames.Name)]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Get ot set pimay.
        /// </summary>
        [DataMember(Name = AttributeNames.Primary)]
        public bool Primary
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set SpecificationResource.
        /// </summary>
        [DataMember(Name = AttributeNames.Specification)]
        public Uri SpecificationResource
        {
            get;
            set;
        }

        /// <summary>
        /// Method fo making a SCIMAuthenticationScheme.
        /// </summary>
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
