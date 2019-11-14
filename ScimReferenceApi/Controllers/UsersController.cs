//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Patch;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Controllers
{

    [Route("api/Users")]
    [ApiController]
    //[Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ScimContext _context;
        private readonly ILogger<UsersController> _log;

        public UsersController(ScimContext context, ILogger<UsersController> log)
        {
            this._context = context;
            this._log = log;
        }

        [HttpGet]
        public async Task<ActionResult<ListResponse<User>>> Get()
        {

            IEnumerable<User> users;

            string query = this.Request.QueryString.ToUriComponent();
            if (!string.IsNullOrWhiteSpace(query))
            {
                users = new FilterUsers(_context).FilterGen(query);
            }
            else
            {
                users = await this._context.CompleteUsers().ToListAsync().ConfigureAwait(false);
            }

            NameValueCollection keyedValues = HttpUtility.ParseQueryString(query);
            IEnumerable<string> keys = keyedValues.AllKeys;
            string countString = keyedValues[QueryKeys.Count];
            string startIndex = keyedValues[QueryKeys.StartIndex];

            if (startIndex == null)
            {
                startIndex = ControllerConfiguration.DefaultStartIndexString;
            }

            int start = int.Parse(startIndex, CultureInfo.InvariantCulture);

            if (start < 1)
            {
                start = 1;
            }

            int total = users.Count();
            int? count = null;

			users = users.OrderBy(d => d.UserName).Skip(start - 1);
			if (countString != null) {
				count = int.Parse(countString,CultureInfo.CurrentCulture);
				users = users.Take(count.Value);
			}
			IEnumerable<string> requested = Request.Query[QueryKeys.Attributes].SelectMany((att) => att.Split(','));
            IEnumerable<string> exculted = Request.Query[QueryKeys.ExcludedAttributes].SelectMany((att) => att.Split(','));
            string[] allwaysRetuned = new string[] { AttributeNames.Identifier, AttributeNames.Schemas, AttributeNames.Active, AttributeNames.Metadata };//TODO Read from schema 
			users = users.Select(u =>
				ColumnsUtility.SelectColumns(requested, exculted, u, allwaysRetuned)).ToList();


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

            this.Response.ContentType = ControllerConfiguration.DefaultContentType;

            return list;
        }

        [HttpGet(ControllerConfiguration.UriID)]
        public async Task<ActionResult<User>> Get(string id)
        {
            User User = await this._context.CompleteUsers().FirstOrDefaultAsync(i => i.Identifier.Equals(id, StringComparison.Ordinal)).ConfigureAwait(false);
            if (User == null)
            {
                ErrorResponse notFoundError = new ErrorResponse(string.Format(CultureInfo.InvariantCulture, ErrorDetail.NotFound, id), "404");
                return NotFound(notFoundError);
            }

            this.Response.ContentType = ControllerConfiguration.DefaultContentType;
            return Ok(User);
        }

		[HttpPost]
		[Route("/api/Users/.search")]
		public ActionResult<ListResponse<User>> Post([FromBody] SearchRequest searchRequest)
		{
            FilterUsers filterUsers = new FilterUsers(_context);
			IEnumerable<User> users = filterUsers.GetUsers(searchRequest.filter);
            string[] allwaysRetuned = new string[] { AttributeNames.Identifier, AttributeNames.Schemas, AttributeNames.Active,AttributeNames.Metadata };//TODO Read from schema 
            string[] attributes = searchRequest.attributes?.ToArray() ?? Array.Empty<string>();
            string[] exculdedattribes = searchRequest.excludedAttributes?.ToArray() ?? Array.Empty<string>();
			users = users.Select(u =>
				ColumnsUtility.SelectColumns(attributes, exculdedattribes, u, allwaysRetuned));
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
                TotalResults = totalResults,
                StartIndex = searchRequest.startIndex ?? null,
                Resources = users,
                ItemsPerPage = searchRequest.count ?? null,
            };
            this.Response.ContentType = ControllerConfiguration.DefaultContentType;
            return list;
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post(JObject body)
        {
            User item = null;
            try { 
                item = BuildUser(body);
            }
            catch(ArgumentException)
            {
                ErrorResponse invalidJSON = new ErrorResponse(ErrorDetail.Unparsable, "400");
                return BadRequest(invalidJSON);
            }
            if (String.IsNullOrWhiteSpace(item.UserName))
            {
                ErrorResponse badRequestError = new ErrorResponse(ErrorDetail.NoUsername, "400");
                return BadRequest(badRequestError);
            }

            bool Exists = this._context.Users.Any(x => x.UserName == item.UserName);
            if (Exists == true)
            {
                ErrorResponse conflictError = new ErrorResponse(ErrorDetail.UsernameConflict, "409");
                return Conflict(conflictError);
            }

			item.meta.Created = DateTime.Now;
			item.meta.LastModified = DateTime.Now;
			this._context.Users.Add(item);
			await this._context.SaveChangesAsync().ConfigureAwait(false);
			this._log.LogInformation(item.UserName);
			this.Response.ContentType = ControllerConfiguration.DefaultContentType;
			return CreatedAtAction(nameof(Get), new { id = item.Identifier }, item);
		}

        private static User BuildUser(JObject body)
        {
            if (body["schemas"]==null)
            {
                throw new ArgumentException("schemas");
            }
            JEnumerable<JToken> shemas = body["schemas"].Children();
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

        [HttpPut(ControllerConfiguration.UriID)]
        public async Task<ActionResult<User>> Put(string id, User item)
        {

            if (id != item.Identifier)
            {
                ErrorResponse badRequestError = new ErrorResponse(ErrorDetail.Mutability, "400");
                return BadRequest(badRequestError);
            }
            if (item.UserName == null)
            {
                ErrorResponse badRequestError = new ErrorResponse(ErrorDetail.NoUsername, "400");
                return BadRequest(badRequestError);
            }

            var User = this._context.CompleteUsers()
                .Where(p => p.Identifier == id)
                .SingleOrDefault();

            if (User == null)
            {
                ErrorResponse notFoundError = new ErrorResponse(string.Format(CultureInfo.InvariantCulture, ErrorDetail.NotFound, id), "404");
                return NotFound(notFoundError);
            }

            item.meta.LastModified = DateTime.Now;
            User.meta = item.meta;
            User.Name = item.Name;
            User.ElectronicMailAddresses = item.ElectronicMailAddresses;
            User.PhoneNumbers = item.PhoneNumbers;
            User.Roles = item.Roles;
            User.Addresses = item.Addresses;
            this._context.Entry(User).CurrentValues.SetValues(item);
            await this._context.SaveChangesAsync().ConfigureAwait(false);
            this._log.LogInformation(item.UserName);
            this.Response.ContentType = ControllerConfiguration.DefaultContentType;
            return Ok(User);
        }

        [HttpDelete(ControllerConfiguration.UriID)]
        public async Task<IActionResult> Delete(string id)
        {
            var User = await this._context.Users.FindAsync(id).ConfigureAwait(false);
            if (User == null)
            {
                ErrorResponse notFoundError = new ErrorResponse(string.Format(CultureInfo.InvariantCulture, ErrorDetail.NotFound, id), "404");
                return NotFound(notFoundError);
            }

            this._context.Users.Remove(User);
            await this._context.SaveChangesAsync().ConfigureAwait(false);
            this._log.LogInformation(id);
            this.Response.ContentType = ControllerConfiguration.DefaultContentType;
            return NoContent();
        }

        [HttpPatch(ControllerConfiguration.UriID)]
        public IActionResult Patch(string id, JObject body)
        {
            PatchRequest2Compliant patchRequest = null;
            PatchRequest2Legacy patchLegacy = null;
            try
            {
                patchRequest = body.ToObject<PatchRequest2Compliant>();
            }
            catch (Newtonsoft.Json.JsonException) { }
            if (patchRequest == null)
            {
                patchLegacy = body.ToObject<PatchRequest2Legacy>();
            }
            if (null == patchRequest && null == patchLegacy)
            {
                string unsupportedPatchTypeName = patchRequest.GetType().FullName;
                throw new NotSupportedException(unsupportedPatchTypeName);
            }

            var usertoModify = this._context.CompleteUsers().FirstOrDefault((user) => user.Identifier.Equals(id, StringComparison.Ordinal));
            if (patchRequest != null)
            {
                if (usertoModify != null)
                {
                    foreach (var op in patchRequest.Operations)
                    {
                        if (op is PatchOperation2SingleValued singleValued)
                        {

                            PatchOperation patchOp = PatchOperation.Create(getOperationName(singleValued.OperationName), singleValued.Path.ToString(), singleValued.Value);
                            usertoModify.Apply(patchOp);
                            usertoModify.meta.LastModified = DateTime.Now;
                        }
                    }
                }
            }
            else if(patchLegacy != null)
            {
                if (usertoModify != null)
                {
                    foreach (var op in patchLegacy.Operations)
                    {
                        usertoModify.Apply(op);
                        usertoModify.meta.LastModified = DateTime.Now;
                    }
                }
            }
            this._context.SaveChanges();

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
