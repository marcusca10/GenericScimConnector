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

			try
			{
				ListResponse<Resource> list = await this.provider.Query(query, requested, exculted).ConfigureAwait(false);

				this.Response.ContentType = ControllerConstants.DefaultContentType;
				return list;

			}
			catch (Exception)
			{

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
			catch (Exception)
			{

				throw;
			}
		}

		[HttpPost]
		public async Task<ActionResult<Core2Group>> Post(Core2Group item)
		{
			if (item.DisplayName == null)
			{
				return this.BadRequest();
			}

			try
			{
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
			catch (Exception)
			{

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
				Core2Group group = this.context.CompleteGroups().FirstOrDefault(g => g.Identifier.Equals(id, StringComparison.CurrentCulture));
				await this.provider.Replace(item, group).ConfigureAwait(false);

				this.Response.ContentType = ControllerConstants.DefaultContentType;
				return this.Ok(group);

			}
			catch (Exception)
			{

				throw;
			}
		}

		[HttpDelete(ControllerConstants.AttributeValueIdentifier)]
		public async Task<IActionResult> Delete(string id)
		{
			try
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
			catch (Exception)
			{

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
			catch (Exception)
			{

				throw;
			}
		}
	}
}
