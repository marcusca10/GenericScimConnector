using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Patch;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Controllers
{
    /// <summary>
    /// Api for Groups resource.
    /// </summary>
    [Route("api/Groups")]
    [ApiController]
    //[Authorize]
    public class GroupsController : ControllerBase
    {
        private readonly ScimContext _context;

        /// <summary>
        /// Constructor.
        /// </summary>
        public GroupsController(ScimContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/Groups
        /// Return list of all Groups from persistent storage.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ListResponse<Group>>> Get()
        {
            IEnumerable<Group> groups; //= await _context.Groups.ToListAsync().ConfigureAwait(false);

            string query = Request.QueryString.ToUriComponent();
            if (!string.IsNullOrWhiteSpace(query))
            {
                groups = new FilterGroups(_context).FilterGen(query);
            }
            else
            {
                groups = await _context.Groups.ToListAsync().ConfigureAwait(false);
            }

            NameValueCollection keyedValues = HttpUtility.ParseQueryString(query);
            IEnumerable<string> keys = keyedValues.AllKeys;
            string countString = keyedValues[QueryKeys.Count];
            string startIndex = keyedValues[QueryKeys.StartIndex];

            if (startIndex == null)
            {
                startIndex = "1";
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

            var requested = Request.Query[QueryKeys.Attributes];
            var exculted = Request.Query[QueryKeys.ExcludedAttributes];
            var allwaysRetuned = new string[] { AttributeNames.Identifier, "identifier", AttributeNames.Schemas, AttributeNames.Schema, AttributeNames.Active };
            groups = groups.Select(u =>
                (Group)ColumnsUtility.SelectColumns(requested, exculted, u, allwaysRetuned)).ToList();

            Response.ContentType = "application/scim+json";

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

        /// <summary>
        /// GET: api/Groups/5
        /// Return Group identified by id, if it exists.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> Get(string id)
        {
            var Group = await _context.CompleteGroups().FirstOrDefaultAsync(i => i.Identifier.Equals(id, StringComparison.Ordinal)).ConfigureAwait(false);

            if (Group == null)
            {
                ErrorResponse notFoundError = new ErrorResponse("Resource " + id + " not found", "404");
                notFoundError.AddSchema(ProtocolSchemaIdentifiers.Version2Error);
                return NotFound(notFoundError);
            }
            var requested = Request.Query[QueryKeys.Attributes];
            var exculted = Request.Query[QueryKeys.ExcludedAttributes];
            var allwaysRetuned = new string[] { AttributeNames.Identifier, "identifier", AttributeNames.Schemas, AttributeNames.Schema, AttributeNames.Active };
            Group = (Group)ColumnsUtility.SelectColumns(requested, exculted, Group, allwaysRetuned);
            Response.ContentType = "application/scim+json";
            return Group;
        }

        /// <summary>
        /// Search endpoint send 
        /// </summary>
        [HttpPost]
        [Route("/api/Groups/.search")]
        public ActionResult<ListResponse<Group>> Post([FromBody] SearchRequest searchRequest)
        {
            var filterGroups = new FilterGroups(_context);
            IEnumerable<Group> groups = filterGroups.GetGroups(searchRequest.filter);
            var allwaysRetuned = new string[] { AttributeNames.Identifier, "identifier", AttributeNames.Schemas, AttributeNames.Schema, AttributeNames.Active };//TODO Read from schema 
            var attributes = searchRequest.attributes?.ToArray() ?? Array.Empty<string>();
            var exculdedattribes = searchRequest.excludedAttributes?.ToArray() ?? Array.Empty<string>();
            groups = groups.Select(g =>
                (Group)ColumnsUtility.SelectColumns(attributes, exculdedattribes, g, allwaysRetuned)).ToList();
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
            return list;
        }

        /// <summary>
        /// POST: api/Groups
        /// Creates a new Group if the item has non-null unique displayname.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Group>> Post(Group item)
        {
            if (item.DisplayName == null)
            {
                return BadRequest();
            }

            var Exists = _context.Groups.Any(x => x.DisplayName == item.DisplayName);
            if (Exists == true)
            {
                ErrorResponse conflictError = new ErrorResponse("DisplayName already exists", "409");
                conflictError.AddSchema(ProtocolSchemaIdentifiers.Version2Error);
                return NotFound(conflictError);
            }

            item.Metadata.Created = DateTime.Now;
            item.Metadata.LastModified = DateTime.Now;
            _context.Groups.Add(item);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            Response.ContentType = "application/scim+json";
            return CreatedAtAction(nameof(Get), new { id = item.DisplayName }, item);
        }

        /// <summary>
        /// PUT: api/Groups/5
        /// Replace all values for given Group, if it exists.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<Group>> Put(string id, Group item)
        {
            if (id != item.Identifier)
            {
                ErrorResponse BadRequestError = new ErrorResponse("Attribute 'id' is read only", "400");
                BadRequestError.AddSchema(ProtocolSchemaIdentifiers.Version2Error);
                return NotFound(BadRequestError);
            }
            Group group = _context.CompleteGroups().FirstOrDefault(g => g.Identifier.Equals(id, StringComparison.CurrentCulture));
            group.DisplayName = item.DisplayName;
            group.Members = item.Members;
            group.Metadata.LastModified = DateTime.Now;
            group.ExternalIdentifier = item.ExternalIdentifier;
            await _context.SaveChangesAsync().ConfigureAwait(false);
            Response.ContentType = "application/scim+json";
            return Ok(group);
        }

        /// <summary>
        /// DELETE: api/Groups/5
        /// Remove the given Group from persistent storage, if it exists.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var Group = await _context.Groups.FindAsync(id).ConfigureAwait(false);

            if (Group == null)
            {
                ErrorResponse notFoundError = new ErrorResponse("Resource " + id + " not found", "404");
                notFoundError.AddSchema(ProtocolSchemaIdentifiers.Version2Error);
                return NotFound(notFoundError);
            }

            _context.Groups.Remove(Group);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            Response.ContentType = "application/scim+json";
            return NoContent();
        }

        /// <summary>
        /// Method For PATCH Group.
        /// </summary>
        [HttpPatch("{id}")]
        public IActionResult Patch(string id, [FromBody]PatchRequest2Compliant patchRequest)
        {

            if (null == patchRequest)
            {
                string unsupportedPatchTypeName = patchRequest.GetType().FullName;
                throw new NotSupportedException(unsupportedPatchTypeName);
            }

            var groupToModify = _context.CompleteGroups().FirstOrDefault((group) => group.Identifier.Equals(id, StringComparison.Ordinal));

            if (groupToModify != null)
            {
                foreach (var op in patchRequest.Operations)
                {
                    if (op is PatchOperation2SingleValued singleValued)
                    {
                        var patchOp = PatchOperation.Create(getOperationName(singleValued.OperationName), singleValued.Path.ToString(), singleValued.Value);
                        groupToModify.Apply(patchOp);
                        groupToModify.Metadata.LastModified = DateTime.Now;
                    }
                }
            }
            _context.SaveChanges();

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
