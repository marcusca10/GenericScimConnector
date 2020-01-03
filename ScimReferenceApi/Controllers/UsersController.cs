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

    [Route(ControllerConstants.DefualtUserRoute)]
    [ApiController]
    //[Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ScimContext context;
        private readonly ILogger<UsersController> logger;
        private UserProvider provider;

        public UsersController(ScimContext context, ILogger<UsersController> logger)
        {
            this.context = context;
            this.logger = logger;
            this.provider = new UserProvider(this.context, this.logger);
        }

        [HttpGet]
        public async Task<ActionResult<ListResponse<Resource>>> Get()
        {

            string query = this.Request.QueryString.ToUriComponent();
            IEnumerable<string> requested = this.Request.Query[QueryKeys.Attributes].SelectMany((att) => att.Split(ControllerConstants.DelimiterComma));
            IEnumerable<string> exculted = this.Request.Query[QueryKeys.ExcludedAttributes].SelectMany((att) => att.Split(ControllerConstants.DelimiterComma));

            ListResponse<Resource> list = await this.provider.Query(query, requested, exculted).ConfigureAwait(false);

            this.Response.ContentType = ControllerConstants.DefaultContentType;
            return list;
        }

        [HttpGet(ControllerConstants.AttributeValueIdentifier)]
        public async Task<ActionResult<Core2User>> Get(string id)
        {

            Core2User User = (Core2User)await this.provider.GetById(id).ConfigureAwait(false);

            if (User == null)
            {
                ErrorResponse notFoundError = new ErrorResponse(string.Format(CultureInfo.InvariantCulture, ErrorDetail.NotFound, id), ErrorDetail.Status404);
                return this.NotFound(notFoundError);
            }

            this.Response.ContentType = ControllerConstants.DefaultContentType;
            return this.Ok(User);
        }

        [HttpPost]
        public async Task<ActionResult<Core2User>> Post(JObject body)
        {

            Core2User item = null;
            try
            {
                item = BuildUser(body);
            }
            catch (ArgumentException)
            {
                ErrorResponse invalidJSON = new ErrorResponse(ErrorDetail.Unparsable, ErrorDetail.Status400);
                return this.BadRequest(invalidJSON);
            }
            if (String.IsNullOrWhiteSpace(item.UserName))
            {
                ErrorResponse badRequestError = new ErrorResponse(ErrorDetail.NoUsername, ErrorDetail.Status400);
                return this.BadRequest(badRequestError);
            }

            bool Exists = this.context.Users.Any(x => x.UserName == item.UserName);
            if (Exists == true)
            {
                ErrorResponse conflictError = new ErrorResponse(ErrorDetail.UsernameConflict, ErrorDetail.Status409);
                return this.Conflict(conflictError);
            }

            await this.provider.Add(item).ConfigureAwait(false);

            this.Response.ContentType = ControllerConstants.DefaultContentType;
            return this.CreatedAtAction(nameof(Get), new { id = item.Identifier }, item);

        }

        [HttpPut(ControllerConstants.AttributeValueIdentifier)]
        public async Task<ActionResult<Core2User>> Put(string id, Core2User item)
        {

            if (id != item.Identifier)
            {
                ErrorResponse badRequestError = new ErrorResponse(ErrorDetail.Mutability, ErrorDetail.Status400);
                return this.BadRequest(badRequestError);
            }
            if (item.UserName == null)
            {
                ErrorResponse badRequestError = new ErrorResponse(ErrorDetail.NoUsername, ErrorDetail.Status400);
                return this.BadRequest(badRequestError);
            }

            var User = this.context.CompleteUsers()
                .Where(p => p.Identifier == id)
                .SingleOrDefault();

            if (User == null)
            {
                ErrorResponse notFoundError = new ErrorResponse(string.Format(CultureInfo.InvariantCulture, ErrorDetail.NotFound, id), ErrorDetail.Status404);
                return this.NotFound(notFoundError);
            }

            await this.provider.Replace(item, User).ConfigureAwait(false);

            this.Response.ContentType = ControllerConstants.DefaultContentType;
            return this.Ok(User);
        }

        [HttpDelete(ControllerConstants.AttributeValueIdentifier)]
        public async Task<IActionResult> Delete(string id)
        {
            var User = await this.context.Users.FindAsync(id).ConfigureAwait(false);
            if (User == null)
            {
                ErrorResponse notFoundError = new ErrorResponse(string.Format(CultureInfo.InvariantCulture, ErrorDetail.NotFound, id), ErrorDetail.Status404);
                return this.NotFound(notFoundError);
            }

            await this.provider.Delete(User).ConfigureAwait(false);


            this.Response.ContentType = ControllerConstants.DefaultContentType;
            return this.NoContent();
        }

        [HttpPatch(ControllerConstants.AttributeValueIdentifier)]
        public IActionResult Patch(string id, JObject body)
        {

            this.provider.Update(id, body);

            return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent);
        }

        private static Core2User BuildUser(JObject body)
        {
            if (body[AttributeNames.Schemas] == null)
            {
                throw new ArgumentException(AttributeNames.Schemas);
            }
            JEnumerable<JToken> shemas = body[AttributeNames.Schemas].Children();
            Core2User item;
            if (shemas.Contains(SchemaIdentifiers.Core2EnterpriseUser))
            {
                item = body.ToObject<Core2EnterpriseUser>();
            }
            else
            {
                item = body.ToObject<Core2User>();
            }

            return item;
        }
    }
}
