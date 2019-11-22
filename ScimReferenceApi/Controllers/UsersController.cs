//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Controllers
{

    [Route("api/Users")]
    [ApiController]
    //[Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ScimContext _context;
        private readonly ILogger<UsersController> _log;
        private UserProvider provider;
        private string[] allwaysRetuned = new string[] { AttributeNames.Identifier, AttributeNames.Schemas, AttributeNames.Active, AttributeNames.Metadata };//TODO Read from schema 

        public UsersController(ScimContext context, ILogger<UsersController> log)
        {
            this._context = context;
            this._log = log;
            this.provider = new UserProvider(_context, _log);
        }

        [HttpGet]
        public async Task<ActionResult<ListResponse<Resource>>> Get()
        {

            string query = this.Request.QueryString.ToUriComponent();
            IEnumerable<string> requested = this.Request.Query[QueryKeys.Attributes].SelectMany((att) => att.Split(','));
            IEnumerable<string> exculted = this.Request.Query[QueryKeys.ExcludedAttributes].SelectMany((att) => att.Split(','));

            ListResponse<Resource> list = await provider.Query(query, requested, exculted).ConfigureAwait(false);

            this.Response.ContentType = ControllerConstants.DefaultContentType;
            return list;
        }

        [HttpGet(ControllerConstants.UriID)]
        public async Task<ActionResult<User>> Get(string id)
        {

            User User = (User) await provider.GetById(id).ConfigureAwait(false);

            if (User == null)
            {
                ErrorResponse notFoundError = new ErrorResponse(string.Format(CultureInfo.InvariantCulture, ErrorDetail.NotFound, id), "404");
                return NotFound(notFoundError);
            }

            this.Response.ContentType = ControllerConstants.DefaultContentType;
            return Ok(User);
        }

		[HttpPost]
		[Route("/api/Users/.search")]
		public ActionResult<ListResponse<User>> Post([FromBody] SearchRequest searchRequest)
		{
            FilterUsers filterUsers = new FilterUsers(_context);
			IEnumerable<User> users = filterUsers.GetUsers(searchRequest.filter);
            string[] attributes = searchRequest.attributes?.ToArray() ?? Array.Empty<string>();
            string[] exculdedattribes = searchRequest.excludedAttributes?.ToArray() ?? Array.Empty<string>();
			users = users.Select(u =>
				ColumnsUtility.FilterAttributes(attributes, exculdedattribes, u, allwaysRetuned));
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
            this.Response.ContentType = ControllerConstants.DefaultContentType;
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

            await this.provider.Add(item).ConfigureAwait(false);

			this.Response.ContentType = ControllerConstants.DefaultContentType;
            return CreatedAtAction(nameof(Get), new { id = item.Identifier }, item);

		}

        [HttpPut(ControllerConstants.UriID)]
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

            await this.provider.Replace(item, User).ConfigureAwait(false);

            this.Response.ContentType = ControllerConstants.DefaultContentType;
            return Ok(User);
        }

        [HttpDelete(ControllerConstants.UriID)]
        public async Task<IActionResult> Delete(string id)
        {
            var User = await this._context.Users.FindAsync(id).ConfigureAwait(false);
            if (User == null)
            {
                ErrorResponse notFoundError = new ErrorResponse(string.Format(CultureInfo.InvariantCulture, ErrorDetail.NotFound, id), "404");
                return NotFound(notFoundError);
            }

            await this.provider.Delete(User).ConfigureAwait(false);
           

            this.Response.ContentType = ControllerConstants.DefaultContentType;
            return NoContent();
        }

        [HttpPatch(ControllerConstants.UriID)]
        public IActionResult Patch(string id, JObject body)
        {

            this.provider.Update(id, body);

            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent);
        }
    
		private static User BuildUser(JObject body)
		{
			if (body["schemas"] == null)
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
	}
}
