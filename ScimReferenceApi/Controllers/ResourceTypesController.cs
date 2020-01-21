//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Controllers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
    using Newtonsoft.Json;

    [Route(ControllerConstants.DefaultRouteResourceType)]
    [ApiController]
    public class ResourceTypesController : ControllerBase
    {
        public ResourceTypesController()
        {
        }

        [HttpGet]
        public ActionResult<IEnumerable<Core2ResourceType>> Get()
        {
            string schmea = System.IO.File.ReadAllText(ControllerConstants.ResourceTypeFilePath);
            List<Core2ResourceType> items = JsonConvert.DeserializeObject<List<Core2ResourceType>>(schmea);
            return items;
        }
    }
}
