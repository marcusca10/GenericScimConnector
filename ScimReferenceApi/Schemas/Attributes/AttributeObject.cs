using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Reflection;
namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes
{
    /// <summary>
    /// Base Class for attribute objects for ease of items
    /// </summary>
    public class AttributeObject
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public object this[string propertyName]
        {
            get
            {
                //occasionally scim attribute names do not equal propertyNames use proprty
                System.Reflection.PropertyInfo property = GetProperty(propertyName);
                return property.GetValue(this, null);

            }
            set { GetProperty(propertyName).SetValue(this, value, null); }
        }
        private System.Reflection.PropertyInfo GetProperty(string propertyName)
        {
            return this.GetType().GetProperties().First(prop =>
            {
                var dataMemberAttribue = prop.GetCustomAttribute(typeof(DataMemberAttribute), true);
                if (dataMemberAttribue != null)
                {
                    var attribue = (DataMemberAttribute)dataMemberAttribue;
                    return attribue.Name.Equals(propertyName, System.StringComparison.InvariantCultureIgnoreCase);
                }
                return false;
            });
        }
    }
}
