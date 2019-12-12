﻿//------------------------------------------------------------
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
    [Route(ControllerConstants.DefaultSchemasRoute)]
    [ApiController]
    public class SchemasController : ControllerBase
    {
        public SchemasController()
        {
        }

        [HttpGet]
        public ActionResult<IEnumerable<TypeScheme>> Get()
        {

            string schmea = System.IO.File.ReadAllText(ControllerConstants.ReferenceCodeSchemaFilePath);
            List<TypeScheme> items = JsonConvert.DeserializeObject<List<TypeScheme>>(schmea);
            return items;
        }

        [HttpGet(ControllerConstants.UriID)]
        public ActionResult<TypeScheme> Get(string id)
        {
            string schmea = System.IO.File.ReadAllText(ControllerConstants.ReferenceCodeSchemaFilePath);
            List<TypeScheme> items = JsonConvert.DeserializeObject<List<TypeScheme>>(schmea);
            TypeScheme item = items.FirstOrDefault(sch => sch.Identifier.Equals(id, StringComparison.InvariantCultureIgnoreCase) || sch.Identifier.EndsWith(id, StringComparison.InvariantCultureIgnoreCase));
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }
    }
}
