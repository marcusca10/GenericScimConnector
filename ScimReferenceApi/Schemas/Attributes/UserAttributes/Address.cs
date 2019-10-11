using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    /// <summary>
    /// Address, of a User.
    /// </summary>
    [DataContract]
    public sealed class Address : TypedItem
    {
        /// <summary>
        /// 
        /// </summary>
        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }
        /// <summary>
        /// Adress location home.
        /// </summary>
        public const string Home = "home";

        /// <summary>
        /// Adress location other.
        /// </summary>
        public const string Other = "other";

        /// <summary>
        ///Address location untyped.
        /// </summary>
        public const string Untyped = "untyped";

        /// <summary>
        /// Address location work.
        /// </summary>
        public const string Work = "work";

        /// <summary>
        /// Constructor.
        /// </summary>
        public Address()
        {
        }

        /// <summary>
        /// Get or set country.
        /// </summary>
        [DataMember(Name = AttributeNames.Country)]
        public string Country { get; set; }

        /// <summary>
        /// Get or set Formatted Address.
        /// </summary>
        [DataMember(Name = AttributeNames.Formatted)]
        public string Formatted { get; set; }

        /// <summary>
        /// Get or set Locality (for localizing lang, currency etc.)
        /// </summary>
        [DataMember(Name = AttributeNames.Locality, IsRequired = false)]
        public string Locality { get; set; }

        /// <summary>
        /// Get or set PostalCode.
        /// </summary>
        [DataMember(Name = AttributeNames.PostalCode)]
        public string PostalCode { get; set; }

        /// <summary>
        /// Get or set Region.
        /// </summary>
        [DataMember(Name = AttributeNames.Region)]
        public string Region { get; set; }

        /// <summary>
        /// Get or set StreetAddress.
        /// </summary>
        [DataMember(Name = AttributeNames.StreetAddress)]
        public string StreetAddress { get; set; }
    }
}
