using Microsoft.AspNetCore.Mvc;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Controllers
{
	/// <summary>
	/// 
	/// </summary>
	[Route("api/ResourceType")]
	[ApiController]
	public class ResourceTypesController : ControllerBase
    {
		/// <summary>
		/// 
		/// </summary>
		public ResourceTypesController()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		[HttpGet]
#pragma warning disable CA1822 // Mark members as static do not mark static controler methods
		public ActionResult<IEnumerable<Core2ResourceType>> Get()
#pragma warning restore CA1822 // Mark members as static
		{
			var schmea = System.IO.File.ReadAllText("./JsonConstants/resourceTypes.json");
			var items = JsonConvert.DeserializeObject<List<Core2ResourceType>>(schmea);
			return items;
		}
	}
}
