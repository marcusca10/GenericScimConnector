using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes.UserAttributes
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public sealed class ExtensionAttributeEnterpriseUser : ExtensionAttributeEnterpriseUserBase
    {
		/// <summary>
		/// Invidiable indentifier for relatoinal object
		/// </summary>
		[Key]
		[DataMember]
		[ScaffoldColumn(false)]
		public string Id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[DataMember(Name = AttributeNames.Manager, IsRequired = false, EmitDefaultValue = false)]
        public Manager Manager
        {
            get;
            set;
        }
    }
}