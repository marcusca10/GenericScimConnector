//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
    using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
    using Newtonsoft.Json.Linq;

    public static class ColumnsUtility
    {
        public static T FilterAttributes<T>(IEnumerable<string> requestedAttributes, IEnumerable<string> excludedAttributes, T projectedResouce, string[] allwaysRetuned) where T : Resource
        {

            return ColumnsUtility.BuildResouce(requestedAttributes, excludedAttributes, allwaysRetuned, projectedResouce);
        }
        private static void AddArray(JObject finalObject, IPath attributePath, JToken itemValue)
        {
            finalObject.Add(attributePath.AttributePath, new JArray());
            JArray jarray = (JArray)finalObject[attributePath.AttributePath];
            List<JToken> children = itemValue.Children().ToList();
            foreach (JToken child in children)
            {
                if (MatchArrayItem(child, attributePath))
                {
                    jarray.Add(child);
                }
            }
        }

        private static void AddObject(JObject finalObject, IPath attributePath, JToken itemValue)
        {
            JObject jobject = (JObject)finalObject[attributePath.AttributePath];

            IPath nextlevelAttribe = attributePath.ValuePath;

            if (nextlevelAttribe == null)
            {
                finalObject.Add(attributePath.AttributePath, jobject);
            }
            else
            {
                JObject emptyObject = new JObject();
                finalObject.Add(attributePath.AttributePath, emptyObject);
                JToken item = itemValue[nextlevelAttribe.AttributePath];
                ColumnsUtility.AddObject(emptyObject, nextlevelAttribe, item);
            }
        }

        private static T BuildResouce<T>(IEnumerable<string> requestedAttributes, IEnumerable<string> excludedAttributes, string[] allwaysRetuned, T resource)
        {
            JObject result = new JObject();
            Type objectType = resource.GetType();
            JObject projectedResrouce = JObject.FromObject(resource);
            if (!requestedAttributes.Any() && !excludedAttributes.Any())
            {
                return resource;
            }
            foreach (string requestedAtt in requestedAttributes)
            {
                if (Path.TryParse(requestedAtt, out IPath attributePath))
                {
                    JToken itemValue = projectedResrouce[attributePath.AttributePath];
                    if (itemValue != null)
                    {
                        switch (itemValue.Type)
                        {
                            case JTokenType.Array:
                                if (!attributePath.SubAttributes.Any())
                                {
                                    result.Add(attributePath.AttributePath, itemValue);
                                }
                                else
                                {
                                    AddArray(result, attributePath, itemValue);
                                }
                                break;
                            case JTokenType.Object:
                                ColumnsUtility.AddObject(result, attributePath, itemValue);
                                break;
                            default:
                                result.Add(attributePath.AttributePath, itemValue);
                                break;
                        }
                    }
                }
            }

            foreach (string requestedAtt in allwaysRetuned)
            {
                if (Path.TryParse(requestedAtt, out IPath attributePath))
                {
                    JToken itemValue = projectedResrouce[attributePath.AttributePath];
                    if (itemValue != null)
                    {
                        switch (itemValue.Type)
                        {
                            case JTokenType.Array:
                                if (!attributePath.SubAttributes.Any())
                                {
                                    result.Add(attributePath.AttributePath, itemValue);
                                }
                                else
                                {
                                    ColumnsUtility.AddArray(result, attributePath, itemValue);
                                }
                                break;
                            case JTokenType.Object:
                                ColumnsUtility.AddObject(result, attributePath, itemValue);
                                break;
                            default:
                                result.Add(attributePath.AttributePath, itemValue);
                                break;
                        }
                    }
                }
            }

            if (!requestedAttributes.Any() && excludedAttributes.Any())
            {
                result = projectedResrouce;
            }

            foreach (string excluded in excludedAttributes)
            {
                if (Path.TryParse(excluded, out IPath attributePath))
                {
                    JToken itemValue = projectedResrouce[attributePath.AttributePath];
                    if (itemValue != null)
                    {
                        switch (itemValue.Type)
                        {
                            case JTokenType.Array:
                                if (!attributePath.SubAttributes.Any())
                                {
                                    result[attributePath.AttributePath].Remove();
                                }
                                else
                                {
                                    ColumnsUtility.ClearArray(result, attributePath);
                                }
                                break;
                            case JTokenType.Object:
                                if (attributePath.ValuePath == null)
                                {
                                    result[attributePath.AttributePath].Remove();
                                }
                                else
                                {
                                    ColumnsUtility.ClearObject(result, attributePath);
                                }
                                break;
                            default:
                                result[attributePath.AttributePath].Remove();
                                break;
                        }
                    }
                }
            }
            return (T)result.ToObject(objectType);
        }

        private static void ClearArray(JObject finalObject, IPath attributePath)
        {
            JToken array = (JArray)finalObject[attributePath.AttributePath];
            List<JToken> arrayItems = array.Children().ToList();

            foreach (JToken arItem in arrayItems)
            {
                if (ColumnsUtility.MatchArrayItem(arItem, attributePath))
                {
                    arItem.Remove();
                }
            }
        }

        private static void ClearArray(JToken child, IReadOnlyCollection<string> requestedAttributes, IReadOnlyCollection<string> excludedAttributes, string[] allwaysRetuned)
        {
            //Get Requested and excluded attributes that include child

            string path = child.Path;
            IEnumerable<IPath> directRequested = requestedAttributes.Select(re =>
                 {
                     return Path.TryParse(re, out IPath reqPath) ? reqPath : null;
                 }).
                 Where(req =>
                 {
                     return req?.AttributePath?.Equals(path, StringComparison.InvariantCultureIgnoreCase) ?? false;
                 });
            IEnumerable<IPath> directExclude = excludedAttributes.Select(re =>
                {
                    return Path.TryParse(re, out IPath reqPath) ? reqPath : null;
                }).
                Where(req =>
                {
                    return req?.AttributePath?.Equals(path, StringComparison.InvariantCultureIgnoreCase) ?? false;
                });

            if (allwaysRetuned.Contains(path))
            {
                return;
            }

            if (!directRequested.Any() && !directExclude.Any())//If the array is not mentioned then we can assume that the array can be removed //Assumed called after checking for requeted
            {
                child.Remove();
                return;
            }
            JArray jArray = (JArray)(child.Children().First());
            if (directRequested.Any(testing => !testing.SubAttributes.Any()))
            {
                return;//Requested just the entire array do not remve elemnts
            }
            if (directExclude.Any(testing => !testing.SubAttributes.Any()))
            {
                child.Remove();//Entire array Excluded
                return;
            }
            List<JToken> children = jArray.Children().ToList();
            foreach (JToken arrayItem in children)
            {
                bool shouldClearItem = true;
                foreach (IPath internalyRequested in directRequested)
                {
                    bool match = MatchArrayItem(arrayItem, internalyRequested);
                    if (match)
                    {
                        shouldClearItem = !match;
                        break;
                    }
                }

                foreach (IPath internalyExcluded in directExclude)
                {
                    bool match = MatchArrayItem(arrayItem, internalyExcluded);
                    if (match)
                    {
                        shouldClearItem = match;
                        break;
                    }
                }
                if (shouldClearItem)
                {
                    arrayItem.Remove();
                }
            }
            if (!jArray.Children().Any())
            {
                child.Remove();
            }
        }

        private static void ClearObject(JToken finalObject, IPath attributePath)
        {
            //Assume that object allreay exists
            JToken token = finalObject[attributePath.AttributePath];
            IPath nextLevelAttribute = attributePath.ValuePath;
            JToken internalProperty = token[nextLevelAttribute.AttributePath];
            if (nextLevelAttribute.ValuePath != null)
            {
                internalProperty.Remove();
            }
            else
            {
                ColumnsUtility.ClearObject(token, nextLevelAttribute);
            }
        }

        private static bool MatchArrayItem(JToken arrayItem, IPath internalyRequested)
        {
            IFilter filter = internalyRequested.SubAttributes.FirstOrDefault();//Cuts support for deeper attribute filters, very very rare use case
            string atribute = filter.AttributePath;
            JToken selectedAttribue = null;

            if (atribute.Equals("value", StringComparison.InvariantCultureIgnoreCase) && arrayItem.Type == JTokenType.String)
            {
                selectedAttribue = arrayItem;
            }
            else
            {
                selectedAttribue = arrayItem[atribute];
            }

            if (selectedAttribue != null && selectedAttribue.Type != JTokenType.Null)
            {
                string value = selectedAttribue.ToString();
                bool match = false;
                switch (filter.FilterOperator)
                {
                    case ComparisonOperator.Equals:
                        match = value.Equals(filter.ComparisonValue, StringComparison.InvariantCultureIgnoreCase);
                        break;
                    default:
                        throw new NotImplementedException("Does not suppport comparision operator " + filter.FilterOperator.ToString());
                }
                return match;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
