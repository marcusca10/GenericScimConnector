using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    /// <summary>
    /// Value of a TypedItem
    /// </summary>
    [DataContract]
    public abstract class TypedValue : TypedItem
    {
        /// <summary>
        /// Get or set Value.
        /// </summary>
        [DataMember(Name = AttributeNames.Value, Order = 0)]
        public string Value { get; set; }
    }
}
