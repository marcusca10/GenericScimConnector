﻿//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;

    public class FilterGroups
    {

        private readonly ScimContext _context;

        public FilterGroups(ScimContext context)
        {
            this._context = context;
        }

        public IEnumerable<Core2Group> FilterGen(string query)
        {
            IEnumerable<Core2Group> AllGroups = new List<Core2Group>();
            NameValueCollection keyedValues = HttpUtility.ParseQueryString(query);
            IEnumerable<string> keys = keyedValues.AllKeys;
            foreach (string key in keys)
            {
                if (string.Equals(key, QueryKeys.Filter, StringComparison.OrdinalIgnoreCase))
                {
                    string filterExpression = keyedValues[key];
                    AllGroups = this.GetGroups(filterExpression);

                }
            }
            return AllGroups;
        }

        public IEnumerable<Core2Group> GetGroups(string filterExpression)
        {
            List<Core2Group> AllGroups = new List<Core2Group>();
            if (Filter.TryParse(filterExpression, out IReadOnlyCollection<IFilter> results))
            {
                for (int i = 0; i < results.Count; i++)
                {
                    IEnumerable<Core2Group> groups = this._context.CompleteGroups();
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
                                    groups = groups.Where(p => p.Schemas.Any(s => s.Equals(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))).ToList();
                                    break;
                                case AttributeNames.DisplayName:
                                    groups = groups.Where(p => p.DisplayName.Equals(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList(); ;
                                    break;
                                case AttributeNames.Members:
                                    groups = groups.Where(p => p.Members.Any(m => m[propName].Equals(value))).ToList();
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
                                    groups = groups.Where(p => !p.Schemas.Any(s => s.Equals(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))).ToList();
                                    break;
                                case AttributeNames.DisplayName:
                                    groups = groups.Where(p => !p.DisplayName.Equals(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList(); ;
                                    break;
                                case AttributeNames.Members:
                                    groups = groups.Where(p => !p.Members.Any(m => m[propName].Equals(value))).ToList();
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
                                    groups = groups.Where(p => p.Schemas.Any(s => s.Contains(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))).ToList();
                                    break;
                                case AttributeNames.DisplayName:
                                    groups = groups.Where(p => p.DisplayName.Contains(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList(); ;
                                    break;
                                case AttributeNames.Members:
                                    groups = groups.Where(p => p.Members.Any(m => m[propName].ToString().Contains(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))).ToList();
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
                                    groups = groups.Where(p => p.Schemas.Any(s => s.StartsWith(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))).ToList();
                                    break;
                                case AttributeNames.DisplayName:
                                    groups = groups.Where(p => p.DisplayName.StartsWith(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList(); ;
                                    break;
                                case AttributeNames.Members:
                                    groups = groups.Where(p => p.Members.Any(m => m[propName].ToString().StartsWith(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))).ToList();
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
                                    groups = groups.Where(p => p.Schemas.Any(s => s.EndsWith(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))).ToList();
                                    break;
                                case AttributeNames.DisplayName:
                                    groups = groups.Where(p => p.DisplayName.EndsWith(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase)).ToList(); ;
                                    break;
                                case AttributeNames.Members:
                                    groups = groups.Where(p => p.Members.Any(m => m[propName].ToString().EndsWith(value ?? String.Empty, StringComparison.InvariantCultureIgnoreCase))).ToList();
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
                                    groups = groups.Where(p => p.Schemas.Any(s => !string.IsNullOrWhiteSpace(s))).ToList();
                                    break;
                                case AttributeNames.DisplayName:
                                    groups = groups.Where(p => !string.IsNullOrWhiteSpace(p.DisplayName)).ToList(); ;
                                    break;
                                case AttributeNames.Members:
                                    groups = groups.Where(p => p.Members.Any(m => !string.IsNullOrWhiteSpace(m[propName].ToString()))).ToList();
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
                                    groups = groups.Where(p => (DateTime)p.Metadata[propName] > DateTime.Parse(value, CultureInfo.CurrentCulture)).ToList();
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
                                    groups = groups.Where(p => (DateTime)p.Metadata[propName] >= DateTime.Parse(value, CultureInfo.CurrentCulture)).ToList();
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
                                    groups = groups.Where(p => (DateTime)p.Metadata[propName] < DateTime.Parse(value, CultureInfo.CurrentCulture)).ToList();
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
                                    groups = groups.Where(p => (DateTime)p.Metadata[propName] <= DateTime.Parse(value, CultureInfo.CurrentCulture)).ToList();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    AllGroups.AddRange(groups);
                }
            }
            AllGroups = AllGroups.GroupBy(p => new { p.Identifier }).Select(o => o.FirstOrDefault()).ToList();
            return AllGroups;
        }
    }
}
