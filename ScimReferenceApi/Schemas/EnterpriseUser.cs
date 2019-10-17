using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes.UserAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    /// <summary>
    /// Class for extending a user to add enterprise attributes.
    /// </summary>
    [DataContract]
    public class EnterpriseUser : User
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        protected EnterpriseUser()
            : base()
        {
            this.AddSchema(SchemaIdentifiers.Core2EnterpriseUser);
        }

        /// <summary>
        /// Get or set the user department.
        /// </summary>
        [DataMember(Name = AttributeNames.Department, IsRequired = false, EmitDefaultValue = false)]
        public string Department
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the user manager.
        /// </summary>
        [DataMember(Name = AttributeNames.Manager, IsRequired = false, EmitDefaultValue = false)]
        public Manager Manager
        {
            get;
            set;
        }

    }
}