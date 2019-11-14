//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Controllers
{
    [Route("api/ResourceTypes")]
    [ApiController]
    public class ResourceTypesController : ControllerBase
    {
        public ResourceTypesController()
        {
        }

        [HttpGet]
#pragma warning disable CA1822 // Mark members as static do not mark static controler methods
        public ActionResult<IEnumerable<Core2ResourceType>> Get()
#pragma warning restore CA1822 // Mark members as static
        {
            string schmea = System.IO.File.ReadAllText("./JsonConstants/resourceTypes.json");
            List<Core2ResourceType> items = JsonConvert.DeserializeObject<List<Core2ResourceType>>(schmea);
            return items;
        }
    }
}
