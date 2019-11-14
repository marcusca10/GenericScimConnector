//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api
{
    public static class ColumnsUtility
	{
		public static T SelectColumns<T>(IEnumerable<string> requestedAttributes, IEnumerable<string> excludedAttributes, T projectedResouce, string[] allwaysRetuned) where T: Resource
		{

			return BuildResouce(requestedAttributes,excludedAttributes, allwaysRetuned, projectedResouce);
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
			if(directRequested.Any(testing => !testing.SubAttributes.Any()))
			{
				return;//Requested just the entire array do not remve elemnts 
			}
			if(directExclude.Any(testing => !testing.SubAttributes.Any()))
			{
				child.Remove();//Entire array Excluded
				return;
			}
			List<JToken> children = jArray.Children().ToList();
			foreach(JToken arrayItem in children)
			{
				bool shouldClearItem = true;
				foreach(IPath internalyRequested in directRequested)
				{
					bool match = MatchArrayItem(arrayItem, internalyRequested);
					if (match)
					{
						shouldClearItem = !match;
						break;
					}
				}
				
				foreach(IPath internalyExcluded in directExclude)
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

		private static bool MatchArrayItem(JToken arrayItem, IPath internalyRequested)
		{
			IFilter filter = internalyRequested.SubAttributes.FirstOrDefault();//Cuts support for deeper attribute filters, very very rare use case
			string atribute = filter.AttributePath;
			JToken selectedAttribue = null ;
            
			if (atribute.Equals ("value",StringComparison.InvariantCultureIgnoreCase) && arrayItem.Type == JTokenType.String)
			{
				selectedAttribue = arrayItem;
			}
			else {
				selectedAttribue = arrayItem[atribute];
			}
			
			if (selectedAttribue != null && selectedAttribue.Type != JTokenType.Null)
			{
				string value = selectedAttribue.ToString();
				bool match = false;
				switch (filter.FilterOperator)
				{
					case ComparisonOperator.Equals:
						match = value.Equals(filter.ComparisonValue,StringComparison.InvariantCultureIgnoreCase);
						break;
					//TODO: Other comparison operators, very rare use case
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

		private static T BuildResouce<T>(IEnumerable<string> requestedAttributes, IEnumerable<string> excludedAttributes, string[] allwaysRetuned, T resource)
		{
			JObject FinalObject = new JObject();
			Type objectType = resource.GetType();
			JObject projectedResrouce = JObject.FromObject(resource);
			if(!requestedAttributes.Any() && !excludedAttributes.Any())
			{
				return resource;
			}
			foreach(string requestedAtt in requestedAttributes)
			{
				if(Path.TryParse(requestedAtt,out IPath attributePath))
				{
					JToken itemValue = projectedResrouce[attributePath.AttributePath];
                    if (itemValue != null)
                    {
                        switch (itemValue.Type)
                        {
                            case JTokenType.Array:
                                if (!attributePath.SubAttributes.Any())
                                {
                                    FinalObject.Add(attributePath.AttributePath, itemValue);
                                }
                                else
                                {
                                    AddArray(FinalObject, attributePath, itemValue);
                                }
                                break;
                            case JTokenType.Object:
                                AddObject(FinalObject, attributePath, itemValue);
                                break;
                            default:
                                FinalObject.Add(attributePath.AttributePath, itemValue);
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
                                    FinalObject.Add(attributePath.AttributePath, itemValue);
                                }
                                else
                                {
                                    AddArray(FinalObject, attributePath, itemValue);
                                }
                                break;
                            case JTokenType.Object:
                                AddObject(FinalObject, attributePath, itemValue);
                                break;
                            default:
                                FinalObject.Add(attributePath.AttributePath, itemValue);
                                break;
                        }
                    }
				}
			}

			if (!requestedAttributes.Any() && excludedAttributes.Any())
			{
				FinalObject = projectedResrouce;
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
                                    FinalObject[attributePath.AttributePath].Remove();

                                }
                                else
                                {
                                    ClearArray(FinalObject, attributePath);
                                }
                                break;
                            case JTokenType.Object:
                                if (attributePath.ValuePath == null)
                                {
                                    FinalObject[attributePath.AttributePath].Remove();
                                }
                                else
                                {
                                    ClearObject(FinalObject, attributePath);
                                }
                                break;
                            default:
                                FinalObject[attributePath.AttributePath].Remove();
                                break;
                        }
                    }
				}
			}
			return (T) FinalObject.ToObject(objectType);
		}

		private static void ClearObject(JToken finalObject, IPath attributePath)
		{
            //TODO: Support Notation ??.??[?? eq ??] but does not appear to exist in defined scim objects
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
				ClearObject(token, nextLevelAttribute);
			}
		}

		private static void ClearArray(JObject finalObject, IPath attributePath)
		{
			JToken array = (JArray) finalObject[attributePath.AttributePath];
			List<JToken> arrayItems = array.Children().ToList();

			foreach(JToken arItem in arrayItems)
			{
				if(MatchArrayItem(arItem, attributePath))
				{
					arItem.Remove();
				}
			}
		}

		private static void AddArray(JObject finalObject, IPath attributePath, JToken itemValue)
		{
			finalObject.Add(attributePath.AttributePath, new JArray());
			JArray jarray = (JArray) finalObject[attributePath.AttributePath];
			List<JToken> children = itemValue.Children().ToList();
			foreach(JToken child in children)
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
			
			if(nextlevelAttribe == null)
			{
                finalObject.Add(attributePath.AttributePath, jobject);
			}
			else
			{
                JObject emptyObject = new JObject();
                finalObject.Add(attributePath.AttributePath, emptyObject);
                JToken item = itemValue[nextlevelAttribe.AttributePath];
                AddObject(emptyObject, nextlevelAttribe, item);
			}
		}
	}
}
