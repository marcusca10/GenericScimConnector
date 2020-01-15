//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Services;
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
		private readonly IProviderService<Core2Group> _provider;
		private readonly ILogger<GroupsController> _logger;
		private string[] alwaysRetuned = ControllerConstants.AlwaysRetunedAttributes;

		public GroupsController(IProviderService<Core2Group> provider, ILogger<GroupsController> logger)
		{
			this._provider = provider;
			this._logger = logger;
		}

		[HttpGet]
		public async Task<ActionResult<ListResponse<Resource>>> Get()
		{
			string query = this.Request.QueryString.ToUriComponent();
			StringValues requested = this.Request.Query[QueryKeys.Attributes];
			StringValues excluded = this.Request.Query[QueryKeys.ExcludedAttributes];

			try
			{
				ListResponse<Resource> list = await this._provider.Query(query, requested, excluded).ConfigureAwait(false);

				this.Response.ContentType = ControllerConstants.DefaultContentType;
				return list;
			}
			catch (Exception e)
			{
				this._logger.LogError(e.ToString());
				ErrorResponse databaseException = new ErrorResponse(ErrorDetail.DatabaseError, ErrorDetail.Status500);
				return this.StatusCode(500, databaseException);
				throw;
			}
		}

		[HttpGet(ControllerConstants.AttributeValueIdentifier)]
		public async Task<ActionResult<Core2Group>> Get(string id)
		{
			StringValues requested = this.Request.Query[QueryKeys.Attributes];
			StringValues excluded = this.Request.Query[QueryKeys.ExcludedAttributes];

			try
			{
				Core2Group group = (Core2Group)await this._provider.GetById(id).ConfigureAwait(false);

				if (group == null)
				{
					ErrorResponse notFoundError = new ErrorResponse(string.Format(CultureInfo.InvariantCulture, ErrorDetail.NotFound, id), ErrorDetail.Status404);
					return this.NotFound(notFoundError);
				}

				group = ColumnsUtility.FilterAttributes(requested, excluded, group, this.alwaysRetuned);

				this.Response.ContentType = ControllerConstants.DefaultContentType;
				return group;
			}
			catch (Exception e)
			{
				this._logger.LogError(e.ToString());
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

			var group = await this._provider.GetByName(item.DisplayName).ConfigureAwait(false);
			if (group != null)
			{
				ErrorResponse conflictError = new ErrorResponse(ErrorDetail.DisplaynameConflict, ErrorDetail.Status409);
				return this.NotFound(conflictError);
			}

			try
			{
				await this._provider.Add(item).ConfigureAwait(false);

				this.Response.ContentType = ControllerConstants.DefaultContentType;
				return this.CreatedAtAction(nameof(Get), new { id = item.DisplayName }, item);
			}
			catch (Exception e)
			{
				this._logger.LogError(e.ToString());
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
				ErrorResponse badRequestError = new ErrorResponse(ErrorDetail.Mutability, ErrorDetail.Status400);
				return this.NotFound(badRequestError);
			}

			try
			{
				Core2Group group = (Core2Group)await this._provider.GetById(id).ConfigureAwait(false);
				await this._provider.Replace(item, group).ConfigureAwait(false);

				this.Response.ContentType = ControllerConstants.DefaultContentType;
				return this.Ok(group);
			}
			catch (Exception e)
			{
				this._logger.LogError(e.ToString());
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
				var group = await this._provider.GetById(id).ConfigureAwait(false);
				if (group == null)
				{
					ErrorResponse notFoundError = new ErrorResponse(string.Format(CultureInfo.InvariantCulture, ErrorDetail.NotFound, id), ErrorDetail.Status404);
					return this.NotFound(notFoundError);
				}

				await this._provider.Delete(group).ConfigureAwait(false);

				this.Response.ContentType = ControllerConstants.DefaultContentType;
				return this.NoContent();
			}
			catch (Exception e)
			{
				this._logger.LogError(e.ToString());
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
				this._logger.LogError(e.ToString());
				ErrorResponse databaseException = new ErrorResponse(ErrorDetail.DatabaseError, ErrorDetail.Status500);
				return this.StatusCode(500, databaseException);
				throw;
			}
		}
	}
}
