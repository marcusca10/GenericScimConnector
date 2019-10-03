using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{

	/// <summary>
	/// Class for tracking User Role by extedning TypedItem.
	/// </summary>
	[DataContract]
	public sealed class Role : TypedItem
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
        /// Get or set Display.
        /// </summary>
        [DataMember(Name = AttributeNames.Display, IsRequired = false, EmitDefaultValue = false)]
		public string Display { get; set; }

		/// <summary>
		/// Get or set Value.
		/// </summary>
		[DataMember(Name = AttributeNames.Value, IsRequired = false, EmitDefaultValue = false)]
		public string Value { get; set; }
	}
}
