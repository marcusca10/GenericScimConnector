//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AzureAD.Provisioning.ScimReference.Api.Controllers;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Patch;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.GroupAttributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    public class GroupProvider : IProvider
    {
        private readonly ScimContext _context;
        private readonly ILogger<UsersController> _log;
        private string[] allwaysRetuned = ControllerConstants.AlwaysRetunedAttributes;
        private int DefaultStartIndex = 1;

        public GroupProvider(ScimContext context, ILogger<UsersController> log)
        {
            this._context = context;
            this._log = log;
        }


        public async Task<ListResponse<Resource>> Query(string query, IEnumerable<string> requested, IEnumerable<string> exculted)
        {
            IEnumerable<Core2Group> groups;
            if (!string.IsNullOrWhiteSpace(query))
            {
                groups = new FilterGroups(this._context).FilterGen(query);
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
                startIndex = ControllerConstants.DefaultStartIndexString;
            }

            int start = int.Parse(startIndex, CultureInfo.InvariantCulture);

            if (start < this.DefaultStartIndex)
            {
                start = this.DefaultStartIndex;
            }

            int? count = null;
            int total = groups.Count();

            groups = groups.OrderBy(d => d.DisplayName).Skip(start - 1);

            if (countString != null)
            {
                count = int.Parse(countString, CultureInfo.InvariantCulture);
                groups = groups.Take(count.Value);
            }

            groups = groups.Select(u =>
                ColumnsUtility.FilterAttributes(requested, exculted, u, this.allwaysRetuned)).ToList();



            ListResponse<Resource> list = new ListResponse<Resource>()
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

        public async Task<Resource> GetById(string id)
        {
            Core2Group group = await this._context.CompleteGroups().FirstOrDefaultAsync(i => i.Identifier.Equals(id, StringComparison.Ordinal)).ConfigureAwait(false);
            return group;
        }

        public async Task Add(Resource item)
        {
            Core2Group group = (Core2Group)item;
            group.Metadata.Created = DateTime.Now;
            group.Metadata.LastModified = DateTime.Now;
           // group.Members = new List<Member>();
            this._context.Groups.Add(group);
            await this._context.SaveChangesAsync().ConfigureAwait(false);
            this._log.LogInformation(group.Identifier);
        }

        public async Task Replace(Resource old, Resource newresorce)
        {
            Core2Group item = (Core2Group)old;
            Core2Group group = (Core2Group)newresorce;
            group.DisplayName = item.DisplayName;
            group.Members = item.Members;
            group.Metadata.LastModified = DateTime.Now;
            group.ExternalIdentifier = item.ExternalIdentifier;
            await this._context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Delete(Resource resource)
        {
            Core2Group Group = (Core2Group)resource;
            this._context.Groups.Remove(Group);
            await this._context.SaveChangesAsync().ConfigureAwait(false);
            this._log.LogInformation(Group.Identifier);
        }

        public void Update(string id, JObject body)
        {
            PatchRequest2Compliant patchRequest = null;
            PatchRequestSimple patchSimple = null;
            try
            {
                patchRequest = body.ToObject<PatchRequest2Compliant>();
            }
            catch (Newtonsoft.Json.JsonException) { }
            if (patchRequest == null)
            {
                patchSimple = body.ToObject<PatchRequestSimple>();
            }

            if (null == patchRequest && null == patchSimple)
            {
                string unsupportedPatchTypeName = patchRequest.GetType().FullName;
                throw new NotSupportedException(unsupportedPatchTypeName);
            }

            Core2Group groupToModify = this._context.CompleteGroups().FirstOrDefault((group) => group.Identifier.Equals(id, StringComparison.Ordinal));

            if (groupToModify != null)
            {
                if (patchRequest != null)
                {
                    foreach (var op in patchRequest.Operations)
                    {

                        PatchOperation patchOp = PatchOperation.Create(OperationValue.getOperationName(op.OperationName), op.Path.ToString(), op.Value);
                        groupToModify.Apply(patchOp);
                        groupToModify.Metadata.LastModified = DateTime.Now;

                    }
                }
                if (patchSimple != null)
                {
                    foreach (var op in patchSimple.Operations)
                    {
                        groupToModify.Apply(op);
                        groupToModify.Metadata.LastModified = DateTime.Now;
                    }
                }
            }
            this._context.SaveChanges();
        }
    }
}
