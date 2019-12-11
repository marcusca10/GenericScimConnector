//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    public class FilterUsers
    {

        private readonly ScimContext _context;

        public FilterUsers(ScimContext context)
        {
            this._context = context;
        }

        public IEnumerable<User> FilterGen(string query)
        {
            IEnumerable<User> AllUsers = new List<User>();
            NameValueCollection keyedValues = HttpUtility.ParseQueryString(query);
            IEnumerable<string> keys = keyedValues.AllKeys;

            string filterExpression = keyedValues[QueryKeys.Filter];

            if (filterExpression != null)
            {
                AllUsers = this.GetUsers(filterExpression);

            }
            else
            {
                AllUsers = this._context.CompleteUsers().AsEnumerable();
            }

            return AllUsers;
        }

        public IEnumerable<User> GetUsers(string filterExpression)
        {
            List<User> AllUsers = new List<User>();
            if (Filter.TryParse(filterExpression, out IReadOnlyCollection<IFilter> results))
            {
                for (int i = 0; i < results.Count; i++)
                {
                    IEnumerable<User> users = this._context.CompleteUsers();
                    Filter currentFilter = (Filter)results.ElementAt(i);
                    while (currentFilter != null)
                    {
                        string attribute = currentFilter.AttributePath;
                        string fullAttribute = attribute;
                        int charLocation = attribute.IndexOf(".", StringComparison.Ordinal);
                        string propName = "";
                        if (charLocation > 0)
                        {
                            attribute = attribute.Substring(0, charLocation);
                            propName = fullAttribute.Substring(charLocation + 1, fullAttribute.Length - (charLocation + 1));
                        }

                        string value = currentFilter.ComparisonValue;
                        ComparisonOperator filterOp = currentFilter.FilterOperator;
                        currentFilter = (Filter)currentFilter.AdditionalFilter;
                        if (filterOp == ComparisonOperator.Equals)
                        {
                            switch (attribute)
                            {
                                case AttributeNames.Schema:
                                    users = users.Where(p => p.Schemas.Any(s => s.Equals(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))).ToList();
                                    break;
                                case AttributeNames.UserName:
                                    users = users.Where(p => p.UserName.Equals(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;
                                case AttributeNames.Active:
                                    users = users.Where(p => p.Active.ToString(CultureInfo.CurrentCulture).Equals(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;
                                case AttributeNames.Addresses:
                                    users = users.Where(p => p.Addresses.Any(a => a[propName].Equals(value))).ToList();
                                    break;
                                case AttributeNames.DisplayName:
                                    users = users.Where(p => p.DisplayName.Equals(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;
                                case AttributeNames.ElectronicMailAddresses:
                                    users = users.Where(p => p.ElectronicMailAddresses.Any(e => e[propName].ToString().Equals(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))).ToList();
                                    break;
                                case AttributeNames.Name:
                                    users = users.Where(p => p.Name[propName].Equals(value ?? String.Empty)).ToList();
                                    break;
                                case AttributeNames.PhoneNumbers:
                                    users = users.Where(p => p.PhoneNumbers.Any(n => n[propName].ToString().Equals(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))).ToList();
                                    break;
                                case AttributeNames.PreferredLanguage:
                                    users = users.Where(p => p.PreferredLanguage.Equals(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;
                                case AttributeNames.Roles:
                                    users = users.Where(p => p.Roles.Any(r => r[propName].Equals(value))).ToList();
                                    break;
                                case AttributeNames.Title:
                                    users = users.Where(p => p.Title.Equals(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;

                                default:
                                    break;
                            }
                        }
                        if (filterOp == ComparisonOperator.NotEquals)
                        {
                            switch (attribute)
                            {
                                case AttributeNames.Schema:
                                    users = users.Where(p => !p.Schemas.Any(s => s.Equals(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))).ToList();
                                    break;
                                case AttributeNames.UserName:
                                    users = users.Where(p => !p.UserName.Equals(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;
                                case AttributeNames.Active:
                                    users = users.Where(p => !p.Active.ToString(CultureInfo.CurrentCulture).Equals(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;
                                case AttributeNames.Addresses:
                                    users = users.Where(p => !p.Addresses.Any(a => a[propName].Equals(value))).ToList();
                                    break;
                                case AttributeNames.DisplayName:
                                    users = users.Where(p => !p.DisplayName.Equals(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;
                                case AttributeNames.ElectronicMailAddresses:
                                    users = this._context.Users.Where(p => p.ElectronicMailAddresses.Any(e => e[propName].ToString().Equals(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))).ToList();
                                    break;
                                case AttributeNames.Name:
                                    users = users.Where(p => !p.Name[propName].Equals(value ?? String.Empty)).ToList();
                                    break;
                                case AttributeNames.PhoneNumbers:
                                    users = this._context.Users.Where(p => !p.PhoneNumbers.Any(n => n[propName].ToString().Equals(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))).ToList();
                                    break;
                                case AttributeNames.PreferredLanguage:
                                    users = users.Where(p => !p.PreferredLanguage.Equals(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;
                                case AttributeNames.Roles:
                                    users = users.Where(p => !p.Roles.Any(r => r[propName].Equals(value))).ToList();
                                    break;
                                case AttributeNames.Title:
                                    users = users.Where(p => !p.Title.Equals(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;

                                default:
                                    break;
                            }
                        }
                        if (filterOp == ComparisonOperator.Includes)
                        {
                            switch (attribute)
                            {
                                case AttributeNames.Schema:
                                    users = users.Where(p => p.Schemas.Any(s => s.Contains(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))).ToList();
                                    break;
                                case AttributeNames.UserName:
                                    users = users.Where(p => p.UserName.Contains(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;
                                case AttributeNames.Active:
                                    users = users.Where(p => p.Active.ToString(CultureInfo.CurrentCulture).Equals(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;
                                case AttributeNames.Addresses:
                                    users = users.Where(p => p.Addresses.Any(a => a[propName].ToString().Contains(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))).ToList();
                                    break;
                                case AttributeNames.DisplayName:
                                    users = users.Where(p => p.DisplayName.Contains(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;
                                case AttributeNames.ElectronicMailAddresses:
                                    users = users.Where(p => p.ElectronicMailAddresses.Any(e => e[propName].ToString().Contains(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))).ToList();
                                    break;
                                case AttributeNames.Name:
                                    users = users.Where(p => p.Name[propName].ToString().Contains(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;
                                case AttributeNames.PhoneNumbers:
                                    users = users.Where(p => p.PhoneNumbers.Any(n => n[propName].ToString().Contains(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))).ToList();
                                    break;
                                case AttributeNames.PreferredLanguage:
                                    users = users.Where(p => p.PreferredLanguage.Contains(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;
                                case AttributeNames.Roles:
                                    users = users.Where(p => p.Roles.Any(r => r[propName].ToString().Contains(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))).ToList();
                                    break;
                                case AttributeNames.Title:
                                    users = users.Where(p => p.Title.Contains(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;

                                default:
                                    break;
                            }
                        }
                        if (filterOp == ComparisonOperator.StartsWith)
                        {
                            switch (attribute)
                            {
                                case AttributeNames.Schema:
                                    users = users.Where(p => p.Schemas.Any(s => s.StartsWith(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))).ToList();
                                    break;
                                case AttributeNames.UserName:
                                    users = users.Where(p => p.UserName.StartsWith(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;
                                case AttributeNames.Active:
                                    users = users.Where(p => p.Active.ToString(CultureInfo.CurrentCulture).Equals(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;
                                case AttributeNames.Addresses:
                                    users = users.Where(p => p.Addresses.Any(a => a[propName].ToString().StartsWith(value, StringComparison.InvariantCultureIgnoreCase))).ToList();
                                    break;
                                case AttributeNames.DisplayName:
                                    users = users.Where(p => p.DisplayName.StartsWith(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;
                                case AttributeNames.ElectronicMailAddresses:
                                    users = users.Where(p => p.ElectronicMailAddresses.Any(e => e[propName].ToString().StartsWith(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))).ToList();
                                    break;
                                case AttributeNames.Name:
                                    users = users.Where(p => p.Name[propName].ToString().StartsWith(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;
                                case AttributeNames.PhoneNumbers:
                                    users = users.Where(p => p.PhoneNumbers.Any(n => n[propName].ToString().StartsWith(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))).ToList();
                                    break;
                                case AttributeNames.PreferredLanguage:
                                    users = users.Where(p => p.PreferredLanguage.StartsWith(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;
                                case AttributeNames.Roles:
                                    users = users.Where(p => p.Roles.Any(r => r[propName].ToString().StartsWith(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))).ToList();
                                    break;
                                case AttributeNames.Title:
                                    users = users.Where(p => p.Title.StartsWith(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;

                                default:
                                    break;
                            }
                        }
                        if (filterOp == ComparisonOperator.EndsWith)
                        {
                            switch (attribute)
                            {
                                case AttributeNames.Schema:
                                    users = users.Where(p => p.Schemas.Any(s => s.EndsWith(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))).ToList();
                                    break;
                                case AttributeNames.UserName:
                                    users = users.Where(p => p.UserName.EndsWith(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;
                                case AttributeNames.Active:
                                    users = users.Where(p => p.Active.ToString(CultureInfo.CurrentCulture).Equals(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;
                                case AttributeNames.Addresses:
                                    users = users.Where(p => p.Addresses.Any(a => a[propName].ToString().EndsWith(value, StringComparison.InvariantCultureIgnoreCase))).ToList();
                                    break;
                                case AttributeNames.DisplayName:
                                    users = users.Where(p => p.DisplayName.EndsWith(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;
                                case AttributeNames.ElectronicMailAddresses:
                                    users = users.Where(p => p.ElectronicMailAddresses.Any(e => e[propName].ToString().EndsWith(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))).ToList();
                                    break;
                                case AttributeNames.Name:
                                    users = users.Where(p => p.Name[propName].ToString().EndsWith(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;
                                case AttributeNames.PhoneNumbers:
                                    users = users.Where(p => p.PhoneNumbers.Any(n => n[propName].ToString().EndsWith(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))).ToList();
                                    break;
                                case AttributeNames.PreferredLanguage:
                                    users = users.Where(p => p.PreferredLanguage.EndsWith(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;
                                case AttributeNames.Roles:
                                    users = users.Where(p => p.Roles.Any(r => r[propName].ToString().EndsWith(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))).ToList();
                                    break;
                                case AttributeNames.Title:
                                    users = users.Where(p => p.Title.EndsWith(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;

                                default:
                                    break;
                            }
                        }
                        if (filterOp == ComparisonOperator.Exists)
                        {
                            switch (attribute)
                            {
                                case AttributeNames.Schema:
                                    users = users.Where(p => p.Schemas.Any(s => !string.IsNullOrWhiteSpace(s))).ToList();
                                    break;
                                case AttributeNames.UserName:
                                    users = users.Where(p => !string.IsNullOrWhiteSpace(p.UserName)).ToList();
                                    break;
                                case AttributeNames.Active:
                                    users = users.Where(p => p.Active.ToString(CultureInfo.CurrentCulture).Equals(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                    break;
                                case AttributeNames.Addresses:
                                    users = users.Where(p => p.Addresses.Any(a => !string.IsNullOrWhiteSpace(a[propName].ToString()))).ToList();
                                    break;
                                case AttributeNames.DisplayName:
                                    users = users.Where(p => !string.IsNullOrWhiteSpace(p.DisplayName)).ToList();
                                    break;
                                case AttributeNames.ElectronicMailAddresses:
                                    users = users.Where(p => p.ElectronicMailAddresses.Any(e => !string.IsNullOrWhiteSpace(e[propName].ToString()))).ToList();
                                    break;
                                case AttributeNames.Name:
                                    users = users.Where(p => !string.IsNullOrWhiteSpace(p.Name[propName].ToString())).ToList();
                                    break;
                                case AttributeNames.PhoneNumbers:
                                    users = users.Where(p => p.PhoneNumbers.Any(n => !string.IsNullOrWhiteSpace(n[propName].ToString()))).ToList();
                                    break;
                                case AttributeNames.PreferredLanguage:
                                    users = users.Where(p => !string.IsNullOrWhiteSpace(p.PreferredLanguage)).ToList();
                                    break;
                                case AttributeNames.Roles:
                                    users = this._context.Users.Where(p => p.Roles.Any(r => !string.IsNullOrWhiteSpace(r[propName].ToString()))).ToList();
                                    break;
                                case AttributeNames.Title:
                                    users = users.Where(p => !string.IsNullOrWhiteSpace(p.Title)).ToList();
                                    break;

                                default:
                                    break;
                            }
                        }
                        if (filterOp == ComparisonOperator.GreaterThan)
                        {
                            switch (attribute)
                            {
                                case AttributeNames.Metadata:
                                    users = users.Where(p => (DateTime)p.meta[propName] > DateTime.Parse(value, CultureInfo.CurrentCulture)).ToList();
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (filterOp == ComparisonOperator.EqualOrGreaterThan)
                        {
                            switch (attribute)
                            {
                                case AttributeNames.Metadata:
                                    users = users.Where(p => (DateTime)p.meta[propName] >= DateTime.Parse(value, CultureInfo.CurrentCulture)).ToList();
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (filterOp == ComparisonOperator.LessThan)
                        {
                            switch (attribute)
                            {
                                case AttributeNames.Metadata:
                                    users = users.Where(p => (DateTime)p.meta[propName] < DateTime.Parse(value, CultureInfo.CurrentCulture)).ToList();
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (filterOp == ComparisonOperator.EqualOrLessThan)
                        {
                            switch (attribute)
                            {
                                case AttributeNames.Metadata:
                                    users = users.Where(p => (DateTime)p.meta[propName] <= DateTime.Parse(value, CultureInfo.CurrentCulture)).ToList();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    AllUsers.AddRange(users);
                }
            }
            AllUsers = AllUsers.GroupBy(p => new { p.Identifier }).Select(o => o.FirstOrDefault()).ToList();
            return AllUsers;
        }
    }
}
