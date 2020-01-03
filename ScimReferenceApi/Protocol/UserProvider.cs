﻿//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AzureAD.Provisioning.ScimReference.Api.Patch;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
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

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Controllers
{
    public class UserProvider : IProvider
    {
        private string[] alwaysRetuned = ControllerConstants.AlwaysRetunedAttributes;
        private readonly ScimContext context;
        private int defaultStartIndex = 1;
        private readonly ILogger<UsersController> logger;

        public UserProvider(ScimContext context, ILogger<UsersController> log)
        {
            this.context = context;
            this.logger = log;
        }

        public async Task<ListResponse<Resource>> Query(string query, IEnumerable<string> requested, IEnumerable<string> exculted)
        {

            IEnumerable<Core2User> users;

            if (!string.IsNullOrWhiteSpace(query))
            {
                users = new FilterUsers(this.context).FilterGen(query);
            }
            else
            {
                users = await this.context.CompleteUsers().ToListAsync().ConfigureAwait(false);
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

            if (start < this.defaultStartIndex)
            {
                start = this.defaultStartIndex;
            }

            int total = users.Count();
            int? count = null;

            users = users.OrderBy(d => d.UserName).Skip(start - 1);
            if (countString != null)
            {
                count = int.Parse(countString, CultureInfo.CurrentCulture);
                users = users.Take(count.Value);
            }

            users = users.Select(u =>
                ColumnsUtility.FilterAttributes(requested, exculted, u, this.alwaysRetuned)).ToList();
            
            ListResponse<Resource> list = new ListResponse<Resource>()
            {
                TotalResults = total,
                StartIndex = users.Any() ? start : (int?)null,
                Resources = users,
            };
            if (count.HasValue)
            {
                list.ItemsPerPage = count.Value;
            }

            return list;
        }

        public async Task<Resource> GetById(string id)
        {
            Core2User User = await this.context.CompleteUsers().FirstOrDefaultAsync(i => i.Identifier.Equals(id, StringComparison.Ordinal)).ConfigureAwait(false);
            return User;
        }

        public async Task Add(Resource item)
        {
            Core2User user = (Core2User)item;
            user.Metadata.Created = DateTime.Now;
            user.Metadata.LastModified = DateTime.Now;
            user.Identifier = Guid.NewGuid().ToString();
            this.context.Users.Add(user);
            await this.context.SaveChangesAsync().ConfigureAwait(false);
            this.logger.LogInformation(user.UserName);
        }

        public async Task Replace(Resource old, Resource next)
        {
            Core2User item = (Core2User)old;
            Core2User user = (Core2User)next;
            item.Metadata.LastModified = DateTime.Now;
            user.Metadata = item.Metadata;
            user.Name = item.Name;
            user.ElectronicMailAddresses = item.ElectronicMailAddresses;
            user.PhoneNumbers = item.PhoneNumbers;
            user.Roles = item.Roles;
            user.Addresses = item.Addresses;
            this.context.Entry(user).CurrentValues.SetValues(item);
            await this.context.SaveChangesAsync().ConfigureAwait(false);
            this.logger.LogInformation(item.UserName);
        }

        public async Task Delete(Resource User)
        {
            this.context.Users.Remove((Core2User)User);
            await this.context.SaveChangesAsync().ConfigureAwait(false);
            this.logger.LogInformation(User.Identifier);
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

            var usertoModify = this.context.CompleteUsers().FirstOrDefault((user) => user.Identifier.Equals(id, StringComparison.Ordinal));
            if (patchRequest != null)
            {
                if (usertoModify != null)
                {
                    foreach (var op in patchRequest.Operations)
                    {
                        if (op is PatchOperation2SingleValued singleValued)
                        {

                            PatchOperation patchOp = PatchOperation.Create(OperationValue.getOperationName(singleValued.OperationName), singleValued.Path.ToString(), singleValued.Value);
                            usertoModify.Apply(patchOp);
                            usertoModify.Metadata.LastModified = DateTime.Now;
                        }
                    }
                }
            }
            else if (patchSimple != null)
            {
                if (usertoModify != null)
                {
                    foreach (var op in patchSimple.Operations)
                    {
                        usertoModify.Apply(op);
                        usertoModify.Metadata.LastModified = DateTime.Now;
                    }
                }
            }
            this.context.SaveChanges();
        }
    }
}
