//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Controllers
{

    [Route("api/Groups")]
    [ApiController]
    [Authorize]
    public class GroupsController : ControllerBase
    {
        private readonly ScimContext _context;
        private readonly ILogger<UsersController> _log;
        private GroupProvider provider;

        public GroupsController(ScimContext context, ILogger<UsersController> log)
        {
            this._context = context;
            this._log = log;
            this.provider = new GroupProvider(_context, _log);
        }

        [HttpGet]
        public async Task<ActionResult<ListResponse<Resource>>> Get()
        {

            string query = this.Request.QueryString.ToUriComponent();
            StringValues requested = this.Request.Query[QueryKeys.Attributes];
            StringValues exculted = this.Request.Query[QueryKeys.ExcludedAttributes];

            ListResponse<Resource> list = await provider.Query(query, requested, exculted).ConfigureAwait(false);

            this.Response.ContentType = ControllerConstants.DefaultContentType;
            return list;

        }

        [HttpGet(ControllerConstants.UriID)]
        public async Task<ActionResult<Group>> Get(string id)
        {
            Group Group = (Group) await this.provider.GetById(id).ConfigureAwait(false);

            if (Group == null)
            {
                ErrorResponse notFoundError = new ErrorResponse(string.Format(CultureInfo.InvariantCulture, ErrorDetail.NotFound, id), "404");
                return NotFound(notFoundError);
            }
            StringValues requested = this.Request.Query[QueryKeys.Attributes];
            StringValues exculted = this.Request.Query[QueryKeys.ExcludedAttributes];
            string[] allwaysRetuned = new string[] { AttributeNames.Identifier, AttributeNames.Schemas, AttributeNames.Active, AttributeNames.Metadata };
            Group = ColumnsUtility.SelectColumns(requested, exculted, Group, allwaysRetuned);
            this.Response.ContentType = ControllerConstants.DefaultContentType;
            return Group;
        }

        [HttpPost]
        [Route("/api/Groups/.search")]
        public ActionResult<ListResponse<Group>> Post([FromBody] SearchRequest searchRequest)
        {
            FilterGroups filterGroups = new FilterGroups(this._context);
            IEnumerable<Group> groups = filterGroups.GetGroups(searchRequest.filter);
            string[] allwaysRetuned = new string[] { AttributeNames.Identifier, "identifier", AttributeNames.Schemas, AttributeNames.Active, AttributeNames.Metadata };//TODO Read from schema 
            string[] attributes = searchRequest.attributes?.ToArray() ?? Array.Empty<string>();
            string[] exculdedattribes = searchRequest.excludedAttributes?.ToArray() ?? Array.Empty<string>();
            groups = groups.Select(g =>
                ColumnsUtility.SelectColumns(attributes, exculdedattribes, g, allwaysRetuned)).ToList();
            if (searchRequest.startIndex.HasValue)
            {
                if (searchRequest.startIndex > 1)
                {
                    groups = groups.Skip(searchRequest.startIndex.Value - 1);
                }
            }

            if (searchRequest.count.HasValue)
            {
                if (searchRequest.count >= 1)
                {
                    groups = groups.Take(searchRequest.count.Value);
                }
            }

            ListResponse<Group> list = new ListResponse<Group>()
            {
                TotalResults = groups.Count(),
                StartIndex = groups.Any() ? 1 : (int?)null,
                Resources = groups
            };


            this.Response.ContentType = ControllerConstants.DefaultContentType;
            return list;
        }

        [HttpPost]
        public async Task<ActionResult<Group>> Post(Group item)
        {
            if (item.DisplayName == null)
            {
                return BadRequest();
            }

            bool Exists = this._context.Groups.Any(x => x.DisplayName == item.DisplayName);
            if (Exists == true)
            {
                ErrorResponse conflictError = new ErrorResponse(ErrorDetail.DisplaynameConflict, "409");
                return NotFound(conflictError);
            }

            await this.provider.Add(item).ConfigureAwait(false);

            this.Response.ContentType = ControllerConstants.DefaultContentType;
            return CreatedAtAction(nameof(Get), new { id = item.DisplayName }, item);
        }

        [HttpPut(ControllerConstants.UriID)]
        public async Task<ActionResult<Group>> Put(string id, Group item)
        {
            if (id != item.Identifier)
            {
                ErrorResponse BadRequestError = new ErrorResponse(ErrorDetail.Mutability, "400");
                return NotFound(BadRequestError);
            }
            
            Group group = this._context.CompleteGroups().FirstOrDefault(g => g.Identifier.Equals(id, StringComparison.CurrentCulture));
            await this.provider.Replace(item, group).ConfigureAwait(false);

            this.Response.ContentType = ControllerConstants.DefaultContentType;
            return Ok(group);
        }

        [HttpDelete(ControllerConstants.UriID)]
        public async Task<IActionResult> Delete(string id)
        {
            Group Group = await this._context.Groups.FindAsync(id).ConfigureAwait(false);

            if (Group == null)
            {
                ErrorResponse notFoundError = new ErrorResponse(string.Format(CultureInfo.InvariantCulture, ErrorDetail.NotFound, id), "404");
                return NotFound(notFoundError);
            }

            await this.provider.Delete(Group).ConfigureAwait(false);

            this.Response.ContentType = ControllerConstants.DefaultContentType;
            return NoContent();
        }

        [HttpPatch(ControllerConstants.UriID)]
        public IActionResult Patch(string id, JObject body)
        {

            this.provider.Update(id, body);

            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent);
        }
    }
}
