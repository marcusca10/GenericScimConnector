﻿//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;

    public class AttributeObject
    {
        public object this[string propertyName]
        {
            get
            {
                //occasionally scim attribute names do not equal propertyNames use proprty
                System.Reflection.PropertyInfo property = this.GetProperty(propertyName);
                return property.GetValue(this, null);

            }
            set { this.GetProperty(propertyName).SetValue(this, value, null); }
        }
        private System.Reflection.PropertyInfo GetProperty(string propertyName)
        {
            return this.GetType().GetProperties().First(prop =>
            {
                Attribute dataMemberAttribue = prop.GetCustomAttribute(typeof(DataMemberAttribute), true);
                if (dataMemberAttribue != null)
                {
                    DataMemberAttribute attribue = (DataMemberAttribute)dataMemberAttribue;
                    return attribue.Name.Equals(propertyName, System.StringComparison.InvariantCultureIgnoreCase);
                }
                return false;
            });
        }
    }
}
