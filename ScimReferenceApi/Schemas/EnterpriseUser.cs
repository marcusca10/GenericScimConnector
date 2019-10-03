using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes.UserAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public abstract class EnterpriseUser : User
    {
        /// <summary>
        /// 
        /// </summary>
        protected EnterpriseUser()
            : base()
        {
            this.AddSchema(SchemaIdentifiers.Core2EnterpriseUser);
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = AttributeNames.Department, IsRequired = false, EmitDefaultValue = false)]
        public string Department
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = AttributeNames.Manager, IsRequired = false, EmitDefaultValue = false)]
        public Manager Manager
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = AttributeNames.ExtensionEnterpriseUser2)]
        public ExtensionAttributeEnterpriseUser EnterpriseExtension
        {
            get;
            set;
        }
    }
}