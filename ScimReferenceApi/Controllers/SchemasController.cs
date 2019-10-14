using Microsoft.AspNetCore.Mvc;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Controllers
{
    /// <summary>
    /// Controller to return schemas json file.
    /// </summary>
    [Route("api/Schemas")]
    [ApiController]
    public class SchemasController : ControllerBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public SchemasController()
        {
        }

        /// <summary>
        /// HTTP GET for schema json.
        /// </summary>
        [HttpGet]
#pragma warning disable CA1822 // Contoller methods must not be static
        public ActionResult<IEnumerable<TypeScheme>> Get()
#pragma warning restore CA1822 // Mark members as static
        {

            var schmea = System.IO.File.ReadAllText("./JsonConstants/ReferenceCodeSchema.json");
            var items = JsonConvert.DeserializeObject<List<TypeScheme>>(schmea);
            return items;
        }
    }
}
