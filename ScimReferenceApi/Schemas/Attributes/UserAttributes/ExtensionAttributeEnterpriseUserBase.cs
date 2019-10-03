using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes.UserAttributes
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public abstract class ExtensionAttributeEnterpriseUserBase
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = AttributeNames.CostCenter, IsRequired = false, EmitDefaultValue = false)]
        public string CostCenter
        {
            get;
            set;
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
        [DataMember(Name = AttributeNames.Division, IsRequired = false, EmitDefaultValue = false)]
        public string Division
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = AttributeNames.EmployeeNumber, IsRequired = false, EmitDefaultValue = false)]
        public string EmployeeNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = AttributeNames.Organization, IsRequired = false, EmitDefaultValue = false)]
        public string Organization
        {
            get;
            set;
        }
    }
}