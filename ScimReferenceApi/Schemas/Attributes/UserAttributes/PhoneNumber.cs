using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
	/// <summary>
	/// Phone number, of a User.
	/// </summary>
	[DataContract]
	public sealed class PhoneNumber : TypedValue
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
        /// Phone type fax.
        /// </summary>
        public const string Fax = "fax";

		/// <summary>
		/// Phone type home.
		/// </summary>
		public const string Home = "home";

		/// <summary>
		/// Phone type mobile.
		/// </summary>
		public const string Mobile = "mobile";

		/// <summary>
		/// Phone type other.
		/// </summary>
		public const string Other = "other";

		/// <summary>
		/// Phone type pager.
		/// </summary>
		public const string Pager = "pager";

		/// <summary>
		/// Phone type work.
		/// </summary>
		public const string Work = "work";

		/// <summary>
		/// Constructor.
		/// </summary>
		public PhoneNumber()
		{
		}
	}
}
