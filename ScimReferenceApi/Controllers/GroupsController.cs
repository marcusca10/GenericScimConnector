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
	[Route(ControllerConstants.DefaultRouteGroups)]
	[ApiController]
	//[Authorize]
	public class GroupsController : ControllerBase
	{
    private readonly ScimContext _context;
    private readonly ILogger<UsersController> _logger;

		private GroupProvider provider;
		private string[] alwaysRetuned = ControllerConstants.AlwaysRetunedAttributes;

		public GroupsController(ScimContext context, ILogger<UsersController> logger)
		{
			this._context = context;
			this._logger = logger;
			this.provider = new GroupProvider(this._context, this._logger);
		}

		[HttpGet]
		public async Task<ActionResult<ListResponse<Resource>>> Get()
		{
			string query = this.Request.QueryString.ToUriComponent();
			StringValues requested = this.Request.Query[QueryKeys.Attributes];
			StringValues exculted = this.Request.Query[QueryKeys.ExcludedAttributes];

			try
			{
				ListResponse<Resource> list = await this.provider.Query(query, requested, exculted).ConfigureAwait(false);

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
		public async Task<ActionResult<Core2Group>> Get(string id)
		{
			StringValues requested = this.Request.Query[QueryKeys.Attributes];
			StringValues exculted = this.Request.Query[QueryKeys.ExcludedAttributes];

			try
			{
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
			catch (Exception e)
			{
				_logger.LogError(e.ToString());
				ErrorResponse databaseException = new ErrorResponse(ErrorDetail.DatabaseError, ErrorDetail.Status500);
				return this.StatusCode(500, databaseException);
				throw;
			}
		}

		[HttpPost]
		public async Task<ActionResult<Core2Group>> Post(Core2Group item)
		{
			if (String.IsNullOrWhiteSpace(item.DisplayName))
			{
				ErrorResponse badRequestError = new ErrorResponse(ErrorDetail.NoUsername, ErrorDetail.Status400);
				return this.BadRequest(badRequestError);
			}

			bool Exists = this._context.Groups.Any(x => x.DisplayName == item.DisplayName);
			if (Exists == true)
			{
				ErrorResponse conflictError = new ErrorResponse(ErrorDetail.DisplaynameConflict, ErrorDetail.Status409);
				return this.NotFound(conflictError);
			}

			try
			{
				await this.provider.Add(item).ConfigureAwait(false);

				this.Response.ContentType = ControllerConstants.DefaultContentType;
				return this.CreatedAtAction(nameof(Get), new { id = item.DisplayName }, item);
			}
			catch (Exception e)
			{
				_logger.LogError(e.ToString());
				ErrorResponse databaseException = new ErrorResponse(ErrorDetail.DatabaseError, ErrorDetail.Status500);
				return this.StatusCode(500, databaseException);
				throw;
			}
		}

		[HttpPut(ControllerConstants.AttributeValueIdentifier)]
		public async Task<ActionResult<Core2Group>> Put(string id, Core2Group item)
		{
			if (id != item.Identifier)
			{
				ErrorResponse BadRequestError = new ErrorResponse(ErrorDetail.Mutability, ErrorDetail.Status400);
				return this.NotFound(BadRequestError);
			}

			try
			{
				Core2Group group = this._context.CompleteGroups().FirstOrDefault(g => g.Identifier.Equals(id, StringComparison.CurrentCulture));
				await this.provider.Replace(item, group).ConfigureAwait(false);

				this.Response.ContentType = ControllerConstants.DefaultContentType;
				return this.Ok(group);
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
			try
			{
				Core2Group Group = await this._context.Groups.FindAsync(id).ConfigureAwait(false);

				if (Group == null)
				{
					ErrorResponse notFoundError = new ErrorResponse(string.Format(CultureInfo.InvariantCulture, ErrorDetail.NotFound, id), ErrorDetail.Status404);
					return this.NotFound(notFoundError);
				}

				await this.provider.Delete(Group).ConfigureAwait(false);

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
				this.provider.Update(id, body);

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
	}
}
