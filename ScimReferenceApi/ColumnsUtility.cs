using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api
{
	/// <summary>
	/// Static tools class 
	/// </summary>
	public static class ColumnsUtility
	{
		/// <summary>
		/// From retieved listed object select requested columns 
		/// </summary>
		/// <param name="requestedAttributes"></param>
		/// <param name="excludedAttributes"></param>
		/// <param name="projectedResouce"></param>
		/// <param name="allwaysRetuned"></param>
		/// <returns></returns>
		public static T SelectColumns<T>(IEnumerable<string> requestedAttributes, IEnumerable<string> excludedAttributes, T projectedResouce, string[] allwaysRetuned) where T: Resource
		{

			return BuildResouce(requestedAttributes,excludedAttributes, allwaysRetuned, projectedResouce);

			//if (!requestedAttributes.Any() && !excludedAttributes.Any())
			//{
			//	return projectedResouce;
			//}
			//var type = projectedResouce.GetType();
			//var jsonObject = JObject.FromObject(projectedResouce);
			////TODO: Replace with Build Object instead of clear object 
			//if (requestedAttributes.Any() || excludedAttributes.Any())//If not requested attributes or excluded attributes then remove only what is nessecary.
			//{

			//	var children = jsonObject.Children().ToList();
			//	foreach (var child in children)
			//	{
			//		string path = child.Path;
			//		if (child.Children().First().Type == JTokenType.Array)
			//		{
			//			clearArray(child, requestedAttributes, excludedAttributes,allwaysRetuned);
			//		}
			//		else
			//		{
			//			if (
			//			(
			//				((!requestedAttributes.Any((requested) => requested.Equals(path, StringComparison.InvariantCultureIgnoreCase))) && requestedAttributes.Any()) //if there are requested attributes and it does not match remove
			//					|| excludedAttributes.Any((excluded) => excluded.Equals(path, StringComparison.InvariantCultureIgnoreCase)))//Or if matches requested attributs
			//			&& !allwaysRetuned.Contains(path))//And not marked as allways
			//			{
			//				child.Remove();
			//			}
			//		}
			//	}
			//}
			
			//var tobject = jsonObject.ToObject(type);

			//return (Resource)tobject;
		}

		private static void ClearArray(JToken child, IReadOnlyCollection<string> requestedAttributes, IReadOnlyCollection<string> excludedAttributes, string[] allwaysRetuned)
		{
			//Get Requested and excluded attributes that include child

			string path = child.Path;
			var directRequested = requestedAttributes.Select(re =>
				 {
					 return Path.TryParse(re, out IPath reqPath) ? reqPath : null;
				 }).
				 Where(req =>
				 {
					 return req?.AttributePath?.Equals(path, StringComparison.InvariantCultureIgnoreCase) ?? false;
				 });
			var directExclude = excludedAttributes.Select(re =>
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
			var children = jArray.Children().ToList();
			foreach(var arrayItem in children)
			{
				bool shouldClearItem = true;
				foreach(var internalyRequested in directRequested)
				{
					bool match = MatchArrayItem(arrayItem, internalyRequested);
					if (match)
					{
						shouldClearItem = !match;
						break;
					}
				}
				
				foreach(var internalyExcluded in directExclude)
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
			var atribute = filter.AttributePath;
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="requestedAttributes"></param>
		/// <param name="excludedAttributes"></param>
		/// <param name="allwaysRetuned"></param>
		/// <param name="resource"></param>
		/// <returns></returns>
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
					var itemValue = projectedResrouce[attributePath.AttributePath];
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
					var itemValue = projectedResrouce[attributePath.AttributePath];
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
					var itemValue = projectedResrouce[attributePath.AttributePath];
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
			var nextLevelAttribute = attributePath.ValuePath;
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
			var arrayItems = array.Children().ToList();
			var filter = attributePath.SubAttributes.First();

			foreach(var arItem in arrayItems)
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
			var jarray = (JArray) finalObject[attributePath.AttributePath];
			var children = itemValue.Children().ToList();
			foreach(var child in children)
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
			
			var nextlevelAttribe = attributePath.ValuePath;
			
			if(nextlevelAttribe == null)
			{
                finalObject.Add(attributePath.AttributePath, jobject);
			}
			else
			{
                var emptyObject = new JObject();
                finalObject.Add(attributePath.AttributePath, emptyObject);
                JToken item = itemValue[nextlevelAttribe.AttributePath];
                AddObject(emptyObject, nextlevelAttribe, item);
			}
		}
	}
}
