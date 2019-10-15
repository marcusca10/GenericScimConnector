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
    public sealed class Manager
    {
		/// <summary>
		/// Identiier for simple type
		/// </summary>
		[Key]
		[DataMember]
		[ScaffoldColumn(false)]
		public string Id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[DataMember(Name = AttributeNames.Value)]
        public string Value
        {
            get;
            set;
        }
    }
}