//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes.Configuration;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Controllers
{
    [Route("api/ServiceProviderConfig")]
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
