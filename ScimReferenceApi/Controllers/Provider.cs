//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
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
    public class Provider
    {
        private readonly ScimContext _context;
        private readonly ILogger<UsersController> _log;

        public Provider(ScimContext context, ILogger<UsersController> log)
        {
            this._context = context;
            this._log = log;
        }

        public async Task<ListResponse<User>> GetUsers(string query, IEnumerable<string> requested, IEnumerable<string> exculted)
        {

            IEnumerable<User> users;

            if (!string.IsNullOrWhiteSpace(query))
            {
                users = new FilterUsers(_context).FilterGen(query);
            }
            else
            {
                users = await this._context.CompleteUsers().ToListAsync().ConfigureAwait(false);
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

            if (start < 1)
            {
                start = 1;
            }

            int total = users.Count();
            int? count = null;

            users = users.OrderBy(d => d.UserName).Skip(start - 1);
            if (countString != null)
            {
                count = int.Parse(countString, CultureInfo.CurrentCulture);
                users = users.Take(count.Value);
            }


            string[] allwaysRetuned = new string[] { AttributeNames.Identifier, AttributeNames.Schemas, AttributeNames.Active, AttributeNames.Metadata };//TODO Read from schema 
            users = users.Select(u =>
                ColumnsUtility.SelectColumns(requested, exculted, u, allwaysRetuned)).ToList();


            ListResponse<User> list = new ListResponse<User>()
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

        public async Task<User> GetUserByID(string id)
        {
            User User = await this._context.CompleteUsers().FirstOrDefaultAsync(i => i.Identifier.Equals(id, StringComparison.Ordinal)).ConfigureAwait(false);
            return User;
        }

        public async Task AddUser(User item)
        {
            item.meta.Created = DateTime.Now;
            item.meta.LastModified = DateTime.Now;
            this._context.Users.Add(item);
            await this._context.SaveChangesAsync().ConfigureAwait(false);
            this._log.LogInformation(item.UserName);
        }

        public async Task ReplaceUser(User item, User User)
        {
            item.meta.LastModified = DateTime.Now;
            User.meta = item.meta;
            User.Name = item.Name;
            User.ElectronicMailAddresses = item.ElectronicMailAddresses;
            User.PhoneNumbers = item.PhoneNumbers;
            User.Roles = item.Roles;
            User.Addresses = item.Addresses;
            this._context.Entry(User).CurrentValues.SetValues(item);
            await this._context.SaveChangesAsync().ConfigureAwait(false);
            this._log.LogInformation(item.UserName);
        }

        public async Task DeleteUser(User User)
        {
            this._context.Users.Remove(User);
            await this._context.SaveChangesAsync().ConfigureAwait(false);
            this._log.LogInformation(User.Identifier);
        }

        public void PatchUser(string id, JObject body)
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

            var usertoModify = this._context.CompleteUsers().FirstOrDefault((user) => user.Identifier.Equals(id, StringComparison.Ordinal));
            if (patchRequest != null)
            {
                if (usertoModify != null)
                {
                    foreach (var op in patchRequest.Operations)
                    {
                        if (op is PatchOperation2SingleValued singleValued)
                        {

                            PatchOperation patchOp = PatchOperation.Create(getOperationName(singleValued.OperationName), singleValued.Path.ToString(), singleValued.Value);
                            usertoModify.Apply(patchOp);
                            usertoModify.meta.LastModified = DateTime.Now;
                        }
                    }
                }
            }
            else if (patchLegacy != null)
            {
                if (usertoModify != null)
                {
                    foreach (var op in patchLegacy.Operations)
                    {
                        usertoModify.Apply(op);
                        usertoModify.meta.LastModified = DateTime.Now;
                    }
                }
            }
            this._context.SaveChanges();
        }

        public static User BuildUser(JObject body)
        {
            if (body["schemas"] == null)
            {
                throw new ArgumentException("schemas");
            }
            JEnumerable<JToken> shemas = body["schemas"].Children();
            User item;
            if (shemas.Contains(SchemaIdentifiers.Core2EnterpriseUser))
            {
                item = body.ToObject<EnterpriseUser>();
            }
            else
            {
                item = body.ToObject<User>();
            }

            return item;
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
