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

    [Route(ControllerConstants.DefaultGroupsRoute)]
    [ApiController]
    [Authorize]
    public class GroupsController : ControllerBase
    {
        private readonly ScimContext _context;
        private readonly ILogger<UsersController> _log;
        private GroupProvider provider;
        private string[] allwaysRetuned = ControllerConstants.AllwaysRetunedAttributes;

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
            StringValues requested = this.Request.Query[QueryKeys.Attributes];
            StringValues exculted = this.Request.Query[QueryKeys.ExcludedAttributes];

            Group Group = (Group)await this.provider.GetById(id).ConfigureAwait(false);


            if (Group == null)
            {
                ErrorResponse notFoundError = new ErrorResponse(string.Format(CultureInfo.InvariantCulture, ErrorDetail.NotFound, id), ErrorDetail.Status404);
                return NotFound(notFoundError);
            }

            Group = ColumnsUtility.FilterAttributes(requested, exculted, Group, this.allwaysRetuned);

            this.Response.ContentType = ControllerConstants.DefaultContentType;
            return Group;
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
                ErrorResponse conflictError = new ErrorResponse(ErrorDetail.DisplaynameConflict, ErrorDetail.Status409);
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
                ErrorResponse BadRequestError = new ErrorResponse(ErrorDetail.Mutability, ErrorDetail.Status400);
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
                ErrorResponse notFoundError = new ErrorResponse(string.Format(CultureInfo.InvariantCulture, ErrorDetail.NotFound, id), ErrorDetail.Status404);
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
