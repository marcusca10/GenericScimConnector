using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <param name="projectedUser"></param>
        /// <param name="allwaysRetuned"></param>
        /// <returns></returns>
        public static Resource SelectColumns(IReadOnlyCollection<string> requestedAttributes, IReadOnlyCollection<string> excludedAttributes, Resource projectedUser, string[] allwaysRetuned)
        {
            //TODO: always includes meta but of default values, likely from user constructor?
            if (!requestedAttributes.Any() && !excludedAttributes.Any())
            {
                return projectedUser;
            }
            JObject jObject = JObject.FromObject(projectedUser);
            if (requestedAttributes.Any())
            {
                JObject retunedObject = new JObject();
                //TODO: Better approuch build object instead of remove object
            }



            var type = projectedUser.GetType();
            var jsonObject = JObject.FromObject(projectedUser);
            if (requestedAttributes.Any() || excludedAttributes.Any())//If not requested attributes or excluded attributes then remove only what is nessecary.
            {

                var children = jsonObject.Children().ToList();
                foreach (var child in children)
                {
                    string path = child.Path;
                    if (
                    (
                        ((!requestedAttributes.Any((requested) => requested.Equals(path, StringComparison.CurrentCultureIgnoreCase))) && requestedAttributes.Any()) //if there are requested attributes and it does not match remove
                        || excludedAttributes.Any((excluded) => excluded.Equals(path, StringComparison.CurrentCultureIgnoreCase)))//Or if matches requested attributs
                    && !allwaysRetuned.Contains(path))//And not marked as allways
                    {
                        child.Remove();
                    }
                }
            }
            var tobject = jsonObject.ToObject(type);

            return (Resource)tobject;
        }
    }
}
