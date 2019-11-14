//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Controllers
{
    [Route("api/Schemas")]
    [ApiController]
    public class SchemasController : ControllerBase
    {
        public SchemasController()
        {
        }

        [HttpGet]
#pragma warning disable CA1822 // Contoller methods must not be static
        public ActionResult<IEnumerable<TypeScheme>> Get()
#pragma warning restore CA1822 // Mark members as static
        {

            string schmea = System.IO.File.ReadAllText("./JsonConstants/ReferenceCodeSchema.json");
            List<TypeScheme> items = JsonConvert.DeserializeObject<List<TypeScheme>>(schmea);
            return items;
        }

        [HttpGet("{id}")]
#pragma warning disable CA1822 // Contoller methods must not be static
        public ActionResult<TypeScheme> Get(string id)
#pragma warning restore CA1822 // Mark members as static
        {
            string schmea = System.IO.File.ReadAllText("./JsonConstants/ReferenceCodeSchema.json");
            List<TypeScheme> items = JsonConvert.DeserializeObject<List<TypeScheme>>(schmea);
            TypeScheme item = items.FirstOrDefault(sch => sch.Identifier.Equals(id, StringComparison.InvariantCultureIgnoreCase) || sch.Identifier.EndsWith(id,StringComparison.InvariantCultureIgnoreCase));
            if(item == null)
            {
                return NotFound();
            }
            return item;
        }
    }
}
