//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
using System.Globalization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Controllers
{
    [Route(ControllerConstants.DefaultRouteSchemas)]
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

        [HttpGet(ControllerConstants.AttributeValueIdentifier)]
        public ActionResult<TypeScheme> Get(string id)
        {
            string schmea = System.IO.File.ReadAllText(ControllerConstants.ReferenceCodeSchemaFilePath);
            List<TypeScheme> items = JsonConvert.DeserializeObject<List<TypeScheme>>(schmea);
            TypeScheme item = items.FirstOrDefault(sch => sch.Identifier.Equals(id, StringComparison.InvariantCultureIgnoreCase) || sch.Identifier.EndsWith(id, StringComparison.InvariantCultureIgnoreCase));
            if (item == null)
            {
                ErrorResponse notFoundError = new ErrorResponse(string.Format(CultureInfo.InvariantCulture, ErrorDetail.NotFound, id), ErrorDetail.Status404);
                return this.NotFound(notFoundError);
            }
            return item;
        }
    }
}
