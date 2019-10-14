
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Controllers
{
    /// <summary>
    /// Service configuratio endpoint returns JSON for supported fetatures.
    /// </summary>
    [Route("api/ServiceProviderConfig")]
    [ApiController]
    public class ServiceProviderConfig : ControllerBase
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        public ServiceProviderConfig()
        {
        }

        /// <summary>
        /// GET: api/ServiceProviderConfig
        /// Returns the configuration (Currently all fetures are hardcoded to not supported).
        /// </summary>
        [HttpGet]
        public ActionResult<ServiceConfiguration> Get()
        {
            var config = new ServiceConfiguration(false, false, true, false, true, false);
            config.DocumentationResource = "http://example.com/help/scim.html";
            config.AddAuthenticationScheme(SCIMAuthenticationScheme.CreateOpenStandardForAuthorizationBearerTokenScheme());
            return Ok(config);
        }
    }
}
