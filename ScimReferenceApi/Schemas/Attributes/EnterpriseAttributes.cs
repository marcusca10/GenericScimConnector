using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes.UserAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes
{
    public class EnterpriseAttributes
    {
        public EnterpriseAttributes()
        {
        }

        [Key]
        public string Id { get; set; }

        [DataMember(Name = AttributeNames.Department, IsRequired = false, EmitDefaultValue = false)]
        public string Department
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Manager, IsRequired = false, EmitDefaultValue = false)]
        public Manager Manager
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.CostCenter, IsRequired = false, EmitDefaultValue = false)]
        public string CostCenter
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Division, IsRequired = false, EmitDefaultValue = false)]
        public string Division
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.EmployeeNumber, IsRequired = false, EmitDefaultValue = false)]
        public string EmployeeNumber
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Organization, IsRequired = false, EmitDefaultValue = false)]
        public string Organization
        {
            get;
            set;
        }
    }
}
