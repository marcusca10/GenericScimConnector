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
        public ActionResult<IEnumerable<Core2ResourceType>> Get()
        {
            string schmea = System.IO.File.ReadAllText(ControllerConstants.ResourceTypeFilePath);
            List<Core2ResourceType> items = JsonConvert.DeserializeObject<List<Core2ResourceType>>(schmea);
            return items;
        }
    }
}
