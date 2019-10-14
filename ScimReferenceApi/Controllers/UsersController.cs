using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Patch;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Controllers
{

    /// <summary>
    /// Api for Users resource.
    /// </summary>
    [Route("api/Users")]
    [ApiController]
    //[Authorize]
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

            IEnumerable<User> users;

            string query = Request.QueryString.ToUriComponent();
            if (!string.IsNullOrWhiteSpace(query))
            {
                users = new FilterUsers(_context).FilterGen(query);
            }
            else
            {
                users = await _context.CompleteUsers().ToListAsync().ConfigureAwait(false);
            }

            NameValueCollection keyedValues = HttpUtility.ParseQueryString(query);
            IEnumerable<string> keys = keyedValues.AllKeys;
            string countString = keyedValues[QueryKeys.Count];
            string startIndex = keyedValues[QueryKeys.StartIndex];

            if (startIndex == null)
            {
                startIndex = "1";
            }

            int start = int.Parse(startIndex, CultureInfo.InvariantCulture);

            int total = users.Count();
            int? count = null;

            users = users.OrderBy(d => d.UserName).Skip(start - 1);
            if (countString != null)
            {
                count = int.Parse(countString, CultureInfo.CurrentCulture);
                users = users.Take(count.Value);
            }
            var requested = Request.Query[QueryKeys.Attributes];
            var exculded = Request.Query[QueryKeys.ExcludedAttributes];
            var allwaysRetuned = new string[] { AttributeNames.Identifier, "identifier", AttributeNames.Schemas, AttributeNames.Schema, AttributeNames.Active };//TODO Read from schema 
            users = users.Select(u =>
                (User)ColumnsUtility.SelectColumns(requested, exculded, u, allwaysRetuned)).ToList();
            //TODO: always includes meta but of default values, likely from user constructor?

            ListResponse<User> list = new ListResponse<User>()
            {
                TotalResults = total,
                StartIndex = users.Any() ? start : (int?)null,
                Resources = users,
            };
            if (count.HasValue)
            {
                list.ItemsPerPage = count.Value;
            }

            Response.ContentType = "application/scim+json";

            list.Identifier = Guid.NewGuid().ToString();
            return list;
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
                ErrorResponse notFoundError = new ErrorResponse("Resource " + id + " not found", "404");
                notFoundError.AddSchema(ProtocolSchemaIdentifiers.Version2Error);
                return NotFound(notFoundError);
            }

            Response.ContentType = "application/scim+json";
            return Ok(User);
        }

        /// <summary>
        /// Method for handeling User .search
        /// </summary>
        [HttpPost]
        [Route("/api/Users/.search")]
        public ActionResult<ListResponse<User>> Post([FromBody] SearchRequest searchRequest)
        {
            var filterUsers = new FilterUsers(_context);
            IEnumerable<User> users = filterUsers.GetUsers(searchRequest.filter);
            var allwaysRetuned = new string[] { AttributeNames.Identifier, "identifier", AttributeNames.Schemas, AttributeNames.Schema, AttributeNames.Active };//TODO Read from schema 
            var attributes = searchRequest.attributes?.ToArray() ?? Array.Empty<string>();
            var exculdedattribes = searchRequest.excludedAttributes?.ToArray() ?? Array.Empty<string>();
            users = users.Select(u =>
                (User)ColumnsUtility.SelectColumns(attributes, exculdedattribes, u, allwaysRetuned));
            int totalResults = users.Count();
            if (searchRequest.startIndex.HasValue)
            {
                if (searchRequest.startIndex > 1)
                {
                    users = users.Skip(searchRequest.startIndex.Value - 1);
                }
            }

            if (searchRequest.count.HasValue)
            {
                if (searchRequest.count >= 1)
                {
                    users = users.Take(searchRequest.count.Value);
                }
            }

            ListResponse<User> list = new ListResponse<User>()
            {
                Identifier = Guid.NewGuid().ToString(),
                TotalResults = totalResults,
                StartIndex = searchRequest.startIndex ?? null,
                Resources = users,
                ItemsPerPage = searchRequest.count ?? null,
            };
            return list;
        }

        /// <summary>
        /// POST: api/Users
        /// Create a new user if given item has non-null unique username.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<User>> Post(JObject body)
		{
			User item = BuildUser(body);
			if (item.UserName == null)
			{
				ErrorResponse badRequestError = new ErrorResponse("No Username", "400");
				badRequestError.AddSchema(ProtocolSchemaIdentifiers.Version2Error);
				return BadRequest(badRequestError);
			}

			var Exists = _context.Users.Any(x => x.UserName == item.UserName);
			if (Exists == true)
			{
				ErrorResponse conflictError = new ErrorResponse("Username already exists", "409");
				conflictError.AddSchema(ProtocolSchemaIdentifiers.Version2Error);
				return Conflict(conflictError);
			}

			item.Metadata.Created = DateTime.Now;
			item.Metadata.LastModified = DateTime.Now;
			_context.Users.Add(item);
			await _context.SaveChangesAsync().ConfigureAwait(false);
			_log.LogInformation(item.UserName);
			Response.ContentType = "application/scim+json";
			return CreatedAtAction(nameof(Get), new { id = item.Identifier }, item);
		}

		private static User BuildUser(JObject body)
		{
			var shemas = body["schemas"].Children();
			User item;
			if (shemas.Contains(SchemaIdentifiers.Core2EnterpriseUser))
			{
				item = body.ToObject<EnterpriseUser>();
			}
			else
			{
				item = body.ToObject<User>();
			}

			return item;
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
                ErrorResponse badRequestError = new ErrorResponse("Attribute 'id' is read only", "400");
                badRequestError.AddSchema(ProtocolSchemaIdentifiers.Version2Error);
                return BadRequest(badRequestError);
            }

            var User = _context.CompleteUsers()
                .Where(p => p.Identifier == id)
                .SingleOrDefault();

            if (User == null)
            {
                ErrorResponse notFoundError = new ErrorResponse("Resource " + id + " not found", "404");
                notFoundError.AddSchema(ProtocolSchemaIdentifiers.Version2Error);
                return NotFound(notFoundError);
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
                ErrorResponse notFoundError = new ErrorResponse("Resource " + id + " not found", "404");
                notFoundError.AddSchema(ProtocolSchemaIdentifiers.Version2Error);
                return NotFound(notFoundError);
            }

            _context.Users.Remove(User);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            _log.LogInformation(id);
            Response.ContentType = "application/scim+json";
            return NoContent();
        }

        /// <summary>
        /// Method For PATCH User.
        /// </summary>
        [HttpPatch("{id}")]
        public IActionResult Patch(string id, [FromBody]PatchRequest2Compliant patchRequest)
        {

            if (null == patchRequest)
            {
                string unsupportedPatchTypeName = patchRequest.GetType().FullName;
                throw new NotSupportedException(unsupportedPatchTypeName);
            }

            var usertoModify = _context.CompleteUsers().FirstOrDefault((user) => user.Identifier.Equals(id, StringComparison.Ordinal));
            if (usertoModify != null)
            {
                foreach (var op in patchRequest.Operations)
                {
                    if (op is PatchOperation2SingleValued singleValued)
                    {

                        var patchOp = PatchOperation.Create(getOperationName(singleValued.OperationName), singleValued.Path.ToString(), singleValued.Value);
                        usertoModify.Apply(patchOp);
                        usertoModify.Metadata.LastModified = DateTime.Now;
                    }
                    /* else if (op is PatchOperation patchOp)
                     {
                         usertoModify.Apply(patchOp);
                     }*/
                }
            }
            _context.SaveChanges();

            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent);
        }

        private static OperationName getOperationName(string operationName)
        {
            switch (operationName.ToLower(CultureInfo.CurrentCulture))
            {
                case "add":
                    return OperationName.Add;
                case "remove":
                    return OperationName.Remove;
                case "replace":
                    return OperationName.Replace;
                default:
                    throw new NotImplementedException("Invalid operatoin Name" + operationName);
            }
        }
    }
}
