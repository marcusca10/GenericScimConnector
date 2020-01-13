//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Services;
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

    [Route(ControllerConstants.DefaultRouteUsers)]
    [ApiController]
    //[Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ScimContext _context;
        private readonly ILogger<UsersController> _logger;
        private readonly IProviderService<Core2User> _provider;

        public UsersController(ScimContext context, ILogger<UsersController> logger)
        {
            this._context = context;
            this._logger = logger;
            this._provider = new UserProviderService(this._context, this._logger);
        }

        [HttpGet]
        public async Task<ActionResult<ListResponse<Resource>>> Get()
        {

            string query = this.Request.QueryString.ToUriComponent();
            IEnumerable<string> requested = this.Request.Query[QueryKeys.Attributes].SelectMany((att) => att.Split(ControllerConstants.DelimiterComma));
            IEnumerable<string> exculted = this.Request.Query[QueryKeys.ExcludedAttributes].SelectMany((att) => att.Split(ControllerConstants.DelimiterComma));

            try
            {
                ListResponse<Resource> list = await this._provider.Query(query, requested, exculted).ConfigureAwait(false);
            
                this.Response.ContentType = ControllerConstants.DefaultContentType;
                return list;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                ErrorResponse databaseException = new ErrorResponse(ErrorDetail.DatabaseError, ErrorDetail.Status500);
                return this.StatusCode(500, databaseException);
                throw;
            }
        }

        [HttpGet(ControllerConstants.AttributeValueIdentifier)]
        public async Task<ActionResult<Core2User>> Get(string id)
        {
            Core2User User;
            try
            {
                User = (Core2User)await this._provider.GetById(id).ConfigureAwait(false);
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                ErrorResponse databaseException = new ErrorResponse(ErrorDetail.DatabaseError, ErrorDetail.Status500);
                return this.StatusCode(500, databaseException);
                throw;
            }
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

            bool Exists = this._context.Users.Any(x => x.UserName == item.UserName);
            if (Exists == true)
            {
                ErrorResponse conflictError = new ErrorResponse(ErrorDetail.UsernameConflict, ErrorDetail.Status409);
                return this.Conflict(conflictError);
            }

            try
            {
                await this._provider.Add(item).ConfigureAwait(false);

                this.Response.ContentType = ControllerConstants.DefaultContentType;
                return this.CreatedAtAction(nameof(Get), new { id = item.Identifier }, item);
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                ErrorResponse databaseException = new ErrorResponse(ErrorDetail.DatabaseError, ErrorDetail.Status500);
                return this.StatusCode(500, databaseException);
                throw;
            }

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

            var User = this._context.CompleteUsers()
                .Where(p => p.Identifier == id)
                .SingleOrDefault();

            if (User == null)
            {
                ErrorResponse notFoundError = new ErrorResponse(string.Format(CultureInfo.InvariantCulture, ErrorDetail.NotFound, id), ErrorDetail.Status404);
                return this.NotFound(notFoundError);
            }


            try
            {
                await this._provider.Replace(item, User).ConfigureAwait(false);

                this.Response.ContentType = ControllerConstants.DefaultContentType;
                return this.Ok(User);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                ErrorResponse databaseException = new ErrorResponse(ErrorDetail.DatabaseError, ErrorDetail.Status500);
                return this.StatusCode(500, databaseException);
                throw;
            }
        }

        [HttpDelete(ControllerConstants.AttributeValueIdentifier)]
        public async Task<IActionResult> Delete(string id)
        {
            var User = await this._context.Users.FindAsync(id).ConfigureAwait(false);
            if (User == null)
            {
                ErrorResponse notFoundError = new ErrorResponse(string.Format(CultureInfo.InvariantCulture, ErrorDetail.NotFound, id), ErrorDetail.Status404);
                return this.NotFound(notFoundError);
            }

            try
            {
                await this._provider.Delete(User).ConfigureAwait(false);

                this.Response.ContentType = ControllerConstants.DefaultContentType;
                return this.NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                ErrorResponse databaseException = new ErrorResponse(ErrorDetail.DatabaseError, ErrorDetail.Status500);
                return this.StatusCode(500, databaseException);
                throw;
            }
        }

        [HttpPatch(ControllerConstants.AttributeValueIdentifier)]
        public IActionResult Patch(string id, JObject body)
        {
            try
            {
                this._provider.Update(id, body);

                return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                ErrorResponse databaseException = new ErrorResponse(ErrorDetail.DatabaseError, ErrorDetail.Status500);
                return this.StatusCode(500, databaseException);
                throw;
            }
        }

        private static Core2User BuildUser(JObject body)
        {
            if (body[AttributeNames.Schemas] == null)
            {
                throw new ArgumentException(AttributeNames.Schemas);
            }
            JEnumerable<JToken> schemas = body[AttributeNames.Schemas].Children();
            Core2User item;

            if (schemas.Contains(SchemaIdentifiers.Core2EnterpriseUser))
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
