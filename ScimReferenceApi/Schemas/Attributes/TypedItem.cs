using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    /// <summary>
    /// TypedItem e.g. an email
    /// </summary>
    [DataContract]
    public abstract class TypedItem
    {
        /// <summary>
        /// Get or set Id.
        /// </summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// Get or set ItemTye.
        /// </summary>
        [DataMember(Name = AttributeNames.Type)]
        public string ItemType { get; set; }

        /// <summary>
        /// Get or set Primary.
        /// </summary>
        [DataMember(Name = AttributeNames.Primary, IsRequired = false)]
        public bool Primary { get; set; }
    }
}
