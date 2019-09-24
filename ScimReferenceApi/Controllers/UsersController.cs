using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Controllers
{

    /// <summary>
    /// Api for Users resource.
    /// </summary>
    [Route("api/Users")]
	[ApiController]
	[Authorize]
	public class UsersController : ControllerBase
	{
		private readonly ScimContext _context;
        private readonly ILogger<UsersController> _log;

        /// <summary>
        /// Constructor.
        /// </summary>
        public UsersController(ScimContext context, ILogger<UsersController> log)
		{
			_context = context;
            _log = log;
		}

		/// <summary>
		/// GET: api/Users
		/// Return list of all Users from persistent storage.
		/// </summary>
		[HttpGet]
		public async Task<ActionResult<ListResponse<User>>> Get()
		{
			var users = await _context.CompleteUsers().ToListAsync().ConfigureAwait(false);
			ListResponse<User> list = new ListResponse<User>()
			{
				TotalResults = users.Count,
				StartIndex = 1,//default value
				Resources = users
			};

			if (list.Resources.Any())
			{
				list.Identifier = Guid.NewGuid().ToString();
			}

			Response.ContentType = "application/scim+json";
			return Ok(list);
		}

		/// <summary>
		/// GET: api/Users/5
		/// Return User identified by given id. If not found, return Not Found status.
		/// </summary>
		[HttpGet("{id}")]
		public async Task<ActionResult<User>> Get(string id)
		{
            User User = await _context.CompleteUsers().FirstOrDefaultAsync(i => i.Identifier.Equals(id, StringComparison.Ordinal)).ConfigureAwait(false);
			if (User == null)
			{
				return NotFound(new { detail = "Resource " + id + " not found", status = "404" });
			}

			Response.ContentType = "application/scim+json";
			return Ok(User);
		}

		/// <summary>
		/// POST: api/Users
		/// Create a new user if given item has non-null unique username.
		/// </summary>
		[HttpPost]
		public async Task<ActionResult<User>> Post(User item)
		{
			if (item.UserName == null)
			{
				return BadRequest(new { detail = "No Username" });
			}

			var Exists = _context.Users.Any(x => x.UserName == item.UserName);
			if (Exists == true)
			{
				return BadRequest(new { detail = "Username already exists", status = "400" });
			}

			item.Metadata.Created = DateTime.Now;
			item.Metadata.LastModified = DateTime.Now;
			_context.Users.Add(item);
			await _context.SaveChangesAsync().ConfigureAwait(false);
            _log.LogInformation(item.UserName);
            Response.ContentType = "application/scim+json";
			return CreatedAtAction(nameof(Get), new { id = item.Identifier }, item);
		}

		/// <summary>
		/// PUT: api/Users/5
		/// Replace all values for the given User, if it exists.
		/// </summary>
		[HttpPut("{id}")]
		public async Task<ActionResult<User>> Put(string id, User item)
		{
			if (id != item.Identifier)
			{
				return BadRequest(new { detail = "Attribute 'id' is read only", status = "400" });
			}

			var User = _context.Users
				.Where(p => p.Identifier == item.Identifier).Include(u => u.Metadata)
					.Include(u => u.Name)
					.Include("ElectronicMailAddresses")
					.Include(u => u.PhoneNumbers)
					.Include(u => u.Roles)
					.Include(p => p.Addresses)
				.SingleOrDefault();

			if (User == null)
			{
				return NotFound(new { detail = "Resource " + id + " not found", status = "404" });
			}

			item.Metadata.LastModified = DateTime.Now;
			User.Metadata = item.Metadata;
			User.Name = item.Name;
			User.ElectronicMailAddresses = item.ElectronicMailAddresses;
			User.PhoneNumbers = item.PhoneNumbers;
			User.Roles = item.Roles;
			User.Addresses = item.Addresses;
			_context.Entry(User).CurrentValues.SetValues(item);
			await _context.SaveChangesAsync().ConfigureAwait(false);
            _log.LogInformation(item.UserName);
            Response.ContentType = "application/scim+json";
			return Ok(User);
		}

		/// <summary>
		/// DELETE: api/Users/5
		/// Remove the given User from persistent storage, if it exists.
		/// </summary>
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			var User = await _context.Users.FindAsync(id).ConfigureAwait(false);
			if (User == null)
			{
				return NotFound(new { detail = "Resource " + id + " not found", status = "404" });
			}

			_context.Users.Remove(User);
			await _context.SaveChangesAsync().ConfigureAwait(false);
            _log.LogInformation(item.UserName);
            Response.ContentType = "application/scim+json";
			return NoContent();
		}

        /// <summary>
        /// Method For PATCH User.
        /// </summary>
        [HttpPatch("{id}")]
        public IActionResult Patch()
        {
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status501NotImplemented);
        }
    }
}
