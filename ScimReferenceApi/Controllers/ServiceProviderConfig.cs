//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
    using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes.Configuration;

    [Route(ControllerConstants.DefaultRouteServiceConfiguration)]
    [ApiController]
    public class ServiceProviderConfig : ControllerBase
    {
        public ServiceProviderConfig()
        {
        }

        [HttpGet]
        public ActionResult<ServiceConfiguration> Get()
        {
            ServiceConfiguration config = new ServiceConfiguration(false, false, true, false, true, false)
            {
                DocumentationResource = "http://example.com/help/scim.html"
            };

            config.AddAuthenticationScheme(SCIMAuthenticationScheme.CreateOpenStandardForAuthorizationBearerTokenScheme());
            return this.Ok(config);
        }
    }
}
