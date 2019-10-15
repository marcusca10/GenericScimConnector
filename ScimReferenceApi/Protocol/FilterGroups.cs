using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    /// <summary>
    /// Class for generating alist of Groups based on the filter expression in a GET.
    /// </summary>
    public class FilterGroups
    {

        private readonly ScimContext _context;

        /// <summary>
        /// Constructor.
        /// </summary>
        public FilterGroups(ScimContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Method for filtering retruned results based on URI input.
        /// </summary>
        /// <param name="query">Query portion of URI</param>
        /// <returns>AllGroups a list of the slected groups</returns>
        public IEnumerable<Group> FilterGen(string query)
        {
            IEnumerable<Group> AllUsers = new List<Group>();
            NameValueCollection keyedValues = HttpUtility.ParseQueryString(query);
            IEnumerable<string> keys = keyedValues.AllKeys;
            foreach (string key in keys)
            {
                if (string.Equals(key, QueryKeys.Filter, StringComparison.OrdinalIgnoreCase))
                {
                    string filterExpression = keyedValues[key];
                    AllUsers = GetGroups(filterExpression);

                }
            }
            return AllUsers;
        }

        /// <summary>
        /// Method for apply the logic of the genearted collection of filter lists.
        /// </summary>
        public IEnumerable<Group> GetGroups(string filterExpression)
        {
            List<Group> AllGroups = new List<Group>();
            if (Filter.TryParse(filterExpression, out IReadOnlyCollection<IFilter> results))
            {
                for (int i = 0; i < results.Count; i++)
                {
                    IEnumerable<Group> groups = _context.Groups;
                    Filter currentFilter = (Filter)results.ElementAt(i);
                    while (currentFilter != null)
                    {
                        var attribute = currentFilter.AttributePath;
                        var fullAttribute = attribute;
                        int charLocation = attribute.IndexOf(".", StringComparison.Ordinal);
                        string propName = "";
                        if (charLocation > 0)
                        {
                            attribute = attribute.Substring(0, charLocation);
                            propName = fullAttribute.Substring(charLocation + 1, fullAttribute.Length - (charLocation + 1));
                        }

                        var value = currentFilter.ComparisonValue;
                        var filterOp = currentFilter.FilterOperator;
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
