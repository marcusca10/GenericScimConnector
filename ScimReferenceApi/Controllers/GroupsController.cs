//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Patch;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
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

    [Route("api/Groups")]
    [ApiController]
    //[Authorize]
    public class GroupsController : ControllerBase
    {
        private readonly ScimContext _context;

        public GroupsController(ScimContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ListResponse<Group>>> Get()
        {
            IEnumerable<Group> groups;

            string query = Request.QueryString.ToUriComponent();
            if (!string.IsNullOrWhiteSpace(query))
            {
                groups = new FilterGroups(_context).FilterGen(query);
            }
            else
            {
                groups = await this._context.CompleteGroups().ToListAsync().ConfigureAwait(false);
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

            int? count = null;
            int total = groups.Count();

            groups = groups.OrderBy(d => d.DisplayName).Skip(start - 1);

            if (countString != null)
            {
                count = int.Parse(countString, CultureInfo.InvariantCulture);
                groups = groups.Take(count.Value);
            }

            StringValues requested = Request.Query[QueryKeys.Attributes];
            StringValues exculted = Request.Query[QueryKeys.ExcludedAttributes];
            StringValues allwaysRetuned = new string[] { AttributeNames.Identifier, AttributeNames.Schemas, AttributeNames.Active, AttributeNames.Metadata };
            groups = groups.Select(u =>
                ColumnsUtility.SelectColumns(requested, exculted, u, allwaysRetuned)).ToList();

            this.Response.ContentType = ControllerConfiguration.DefaultContentType;

            ListResponse<Group> list = new ListResponse<Group>()
            {
                TotalResults = total,
                StartIndex = groups.Any() ? start : (int?)null,
                Resources = groups,
            };
            if (count.HasValue)
            {
                list.ItemsPerPage = count.Value;
            }


            return list;

        }

        [HttpGet(ControllerConfiguration.UriID)]
        public async Task<ActionResult<Group>> Get(string id)
        {
            Group Group = await this._context.CompleteGroups().FirstOrDefaultAsync(i => i.Identifier.Equals(id, StringComparison.Ordinal)).ConfigureAwait(false);

            if (Group == null)
            {
                ErrorResponse notFoundError = new ErrorResponse(string.Format(CultureInfo.InvariantCulture, ErrorDetail.NotFound, id), "404");
                return NotFound(notFoundError);
            }
            StringValues requested = Request.Query[QueryKeys.Attributes];
            StringValues exculted = Request.Query[QueryKeys.ExcludedAttributes];
            string[] allwaysRetuned = new string[] { AttributeNames.Identifier, AttributeNames.Schemas, AttributeNames.Active, AttributeNames.Metadata };
            Group = ColumnsUtility.SelectColumns(requested, exculted, Group, allwaysRetuned);
            this.Response.ContentType = ControllerConfiguration.DefaultContentType;
            return Group;
        }

        [HttpPost]
        [Route("/api/Groups/.search")]
        public ActionResult<ListResponse<Group>> Post([FromBody] SearchRequest searchRequest)
        {
            FilterGroups filterGroups = new FilterGroups(_context);
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
            this.Response.ContentType = ControllerConfiguration.DefaultContentType;
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

            item.meta.Created = DateTime.Now;
            item.meta.LastModified = DateTime.Now;
            this._context.Groups.Add(item);
            await this._context.SaveChangesAsync().ConfigureAwait(false);
            this.Response.ContentType = ControllerConfiguration.DefaultContentType;
            return CreatedAtAction(nameof(Get), new { id = item.DisplayName }, item);
        }

        [HttpPut(ControllerConfiguration.UriID)]
        public async Task<ActionResult<Group>> Put(string id, Group item)
        {
            if (id != item.Identifier)
            {
                ErrorResponse BadRequestError = new ErrorResponse(ErrorDetail.Mutability, "400");
                return NotFound(BadRequestError);
            }
            Group group = this._context.CompleteGroups().FirstOrDefault(g => g.Identifier.Equals(id, StringComparison.CurrentCulture));
            group.DisplayName = item.DisplayName;
            group.Members = item.Members;
            group.meta.LastModified = DateTime.Now;
            group.ExternalIdentifier = item.ExternalIdentifier;
            await this._context.SaveChangesAsync().ConfigureAwait(false);
            this.Response.ContentType = ControllerConfiguration.DefaultContentType;
            return Ok(group);
        }

        [HttpDelete(ControllerConfiguration.UriID)]
        public async Task<IActionResult> Delete(string id)
        {
            Group Group = await this._context.Groups.FindAsync(id).ConfigureAwait(false);

            if (Group == null)
            {
                ErrorResponse notFoundError = new ErrorResponse(string.Format(CultureInfo.InvariantCulture, ErrorDetail.NotFound, id), "404");
                return NotFound(notFoundError);
            }

            this._context.Groups.Remove(Group);
            await this._context.SaveChangesAsync().ConfigureAwait(false);
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

            Group groupToModify = this._context.CompleteGroups().FirstOrDefault((group) => group.Identifier.Equals(id, StringComparison.Ordinal));

            if (groupToModify != null)
            {
                if (patchRequest != null)
                {
                    foreach (var op in patchRequest.Operations)
                    {

                        PatchOperation patchOp = PatchOperation.Create(getOperationName(op.OperationName), op.Path.ToString(), op.Value);
                        groupToModify.Apply(patchOp);
                        groupToModify.meta.LastModified = DateTime.Now;

                    }
                }
                if (patchLegacy != null)
                {
                    foreach (var op in patchLegacy.Operations)
                    {
                        groupToModify.Apply(op);
                        groupToModify.meta.LastModified = DateTime.Now;
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
