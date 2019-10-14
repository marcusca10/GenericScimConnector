using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.GroupAttributes
{
    /// <summary>
    /// Members, of a Group.
    /// </summary>
    [DataContract]
    public class Member : TypedItem
    {
        /// <summary>
        /// Reflection.
        /// </summary>
        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        internal Member()
        {
        }

        /// <summary>
        /// Get or set TypeName.
        /// </summary>
        [DataMember(Name = AttributeNames.DisplayName, IsRequired = false)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Get or set Value.
        /// </summary>
        [DataMember(Name = AttributeNames.Value)]
        public string Value { get; set; }
    }
}
