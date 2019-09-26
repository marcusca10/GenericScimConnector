using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Controllers
{
	/// <summary>
	/// Api for Groups resource.
	/// </summary>
	[Route("api/Groups")]
	[ApiController]
	//[Authorize]
	public class GroupsController : ControllerBase
	{
		private readonly ScimContext _context;

		/// <summary>
		/// Constructor.
		/// </summary>
		public GroupsController(ScimContext context)
		{
			_context = context;
		}

		/// <summary>
		/// GET: api/Groups
		/// Return list of all Groups from persistent storage.
		/// </summary>
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Group>>> Get()
		{
			Response.ContentType = "application/scim+json";
			return Ok(await _context.Groups.ToListAsync().ConfigureAwait(false));
		}

		/// <summary>
		/// GET: api/Groups/5
		/// Return Group identified by id, if it exists.
		/// </summary>
		[HttpGet("{id}")]
		public async Task<ActionResult<Group>> Get(string id)
		{
			var Group = await _context.Groups.FindAsync(id).ConfigureAwait(false);

			if (Group == null)
			{
				return NotFound(new { detail = "Resource " + id + " not found", status = "404" });
			}

			Response.ContentType = "application/scim+json";
			return Ok(Group);
		}

		/// <summary>
		/// POST: api/Groups
		/// Creates a new Group if the item has non-null unique displayname.
		/// </summary>
		[HttpPost]
		public async Task<ActionResult<Group>> Post(Group item)
		{
			if (item.DisplayName == null)
			{
				return BadRequest();
			}

			var Exists = _context.Groups.Any(x => x.DisplayName == item.DisplayName);
			if (Exists == true)
			{
				return BadRequest(new { detail = "DisplayName already exists", status = "400" });
			}

			_context.Groups.Add(item);
			await _context.SaveChangesAsync().ConfigureAwait(false);
			Response.ContentType = "application/scim+json";
			return CreatedAtAction(nameof(Get), new { id = item.DisplayName }, item);
		}

		/// <summary>
		/// PUT: api/Groups/5
		/// Replace all values for given Group, if it exists.
		/// </summary>
		[HttpPut("{id}")]
		public async Task<ActionResult<Group>> Put(string id, Group item)
		{
			if (id != item.Identifier)
			{
				return BadRequest(new { detail = "Attribute 'id' is read only", status = "400" });
			}

			_context.Entry(item).State = EntityState.Modified;
			await _context.SaveChangesAsync().ConfigureAwait(false);
			Response.ContentType = "application/scim+json";
			return Ok(item);
		}

		/// <summary>
		/// DELETE: api/Groups/5
		/// Remove the given Group from persistent storage, if it exists.
		/// </summary>
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			var Group = await _context.Groups.FindAsync(id).ConfigureAwait(false);

			if (Group == null)
			{
				return NotFound(new { detail = "Resource " + id + " not found", status = "404" });
			}

			_context.Groups.Remove(Group);
			await _context.SaveChangesAsync().ConfigureAwait(false);
			Response.ContentType = "application/scim+json";
			return NoContent();
		}
        /// <summary>
        /// Method For PATCH Group.
        /// </summary>
        [HttpPatch("{id}")]
        public IActionResult Patch()
        {
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status405MethodNotAllowed);
        }
    }
}
