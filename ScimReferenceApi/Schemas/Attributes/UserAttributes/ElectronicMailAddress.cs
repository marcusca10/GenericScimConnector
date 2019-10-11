using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    /// <summary>
    /// Email, of a User.
    /// </summary>
    [DataContract]
    public sealed class ElectronicMailAddress : TypedValue
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ElectronicMailAddress()
        {
        }

        /// <summary>
        /// Reflection.
        /// </summary>
        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }


        /// <summary>
        /// home email.
        /// </summary>
        public const string Home = "home";

        /// <summary>
        /// other email.
        /// </summary>
        public const string Other = "other";

        /// <summary>
        /// work email.
        /// </summary>
        public const string Work = "work";
    }
}
