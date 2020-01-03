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
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Controllers
{

    [Route(ControllerConstants.DefaultGroupsRoute)]
    [ApiController]
    //[Authorize]
    public class GroupsController : ControllerBase
    {
        private readonly ScimContext context;
        private readonly ILogger<UsersController> logger;
        private GroupProvider provider;
        private string[] alwaysRetuned = ControllerConstants.AlwaysRetunedAttributes;

        public GroupsController(ScimContext context, ILogger<UsersController> logger)
        {
            this.context = context;
            this.logger = logger;
            this.provider = new GroupProvider(this.context, this.logger);
        }

        [HttpGet]
        public async Task<ActionResult<ListResponse<Resource>>> Get()
        {

            string query = this.Request.QueryString.ToUriComponent();
            StringValues requested = this.Request.Query[QueryKeys.Attributes];
            StringValues exculted = this.Request.Query[QueryKeys.ExcludedAttributes];

            ListResponse<Resource> list = await this.provider.Query(query, requested, exculted).ConfigureAwait(false);

            this.Response.ContentType = ControllerConstants.DefaultContentType;
            return list;

        }

        [HttpGet(ControllerConstants.AttributeValueIdentifier)]
        public async Task<ActionResult<Core2Group>> Get(string id)
        {
            StringValues requested = this.Request.Query[QueryKeys.Attributes];
            StringValues exculted = this.Request.Query[QueryKeys.ExcludedAttributes];

            Core2Group Group = (Core2Group)await this.provider.GetById(id).ConfigureAwait(false);


            if (Group == null)
            {
                ErrorResponse notFoundError = new ErrorResponse(string.Format(CultureInfo.InvariantCulture, ErrorDetail.NotFound, id), ErrorDetail.Status404);
                return this.NotFound(notFoundError);
            }

            Group = ColumnsUtility.FilterAttributes(requested, exculted, Group, this.alwaysRetuned);

            this.Response.ContentType = ControllerConstants.DefaultContentType;
            return Group;
        }

        [HttpPost]
        public async Task<ActionResult<Core2Group>> Post(Core2Group item)
        {
            if (item.DisplayName == null)
            {
                return this.BadRequest();
            }

            bool Exists = this.context.Groups.Any(x => x.DisplayName == item.DisplayName);
            if (Exists == true)
            {
                ErrorResponse conflictError = new ErrorResponse(ErrorDetail.DisplaynameConflict, ErrorDetail.Status409);
                return this.NotFound(conflictError);
            }

            await this.provider.Add(item).ConfigureAwait(false);

            this.Response.ContentType = ControllerConstants.DefaultContentType;
            return this.CreatedAtAction(nameof(Get), new { id = item.DisplayName }, item);
        }

        [HttpPut(ControllerConstants.AttributeValueIdentifier)]
        public async Task<ActionResult<Core2Group>> Put(string id, Core2Group item)
        {
            if (id != item.Identifier)
            {
                ErrorResponse BadRequestError = new ErrorResponse(ErrorDetail.Mutability, ErrorDetail.Status400);
                return this.NotFound(BadRequestError);
            }

            Core2Group group = this.context.CompleteGroups().FirstOrDefault(g => g.Identifier.Equals(id, StringComparison.CurrentCulture));
            await this.provider.Replace(item, group).ConfigureAwait(false);

            this.Response.ContentType = ControllerConstants.DefaultContentType;
            return this.Ok(group);
        }

        [HttpDelete(ControllerConstants.AttributeValueIdentifier)]
        public async Task<IActionResult> Delete(string id)
        {
            Core2Group Group = await this.context.Groups.FindAsync(id).ConfigureAwait(false);

            if (Group == null)
            {
                ErrorResponse notFoundError = new ErrorResponse(string.Format(CultureInfo.InvariantCulture, ErrorDetail.NotFound, id), ErrorDetail.Status404);
                return this.NotFound(notFoundError);
            }

            await this.provider.Delete(Group).ConfigureAwait(false);

            this.Response.ContentType = ControllerConstants.DefaultContentType;
            return this.NoContent();
        }

        [HttpPatch(ControllerConstants.AttributeValueIdentifier)]
        public IActionResult Patch(string id, JObject body)
        {

            this.provider.Update(id, body);

            return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent);
        }
    }
}
