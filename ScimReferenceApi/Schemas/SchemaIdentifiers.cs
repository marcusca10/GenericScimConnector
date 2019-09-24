namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
	/// <summary>
	/// Class for building Schema strings.
	/// Should we take away 1.0 strings?
	/// </summary>
	public static class SchemaIdentifiers
	{
		private const string VersionSchemasCore2 = "core:2.0:";
		private const string ExtensionEnterprise2 = SchemaIdentifiers.Extension + "enterprise:2.0:";
		private const string Polling1 =
			SchemaIdentifiers.PrefixTypes2 +
			SchemaIdentifiers.Extension +
			"polling:1.0:";

		/// <summary>
		/// For adding extension to schema id.
		/// </summary>
		public const string Extension = "extension:";

		/// <summary>
		/// For adding none to schema id.
		/// </summary>
		public const string None = "/";

		/// <summary>
		/// Prefix for scim.
		/// </summary>
		public const string PrefixTypes2 = "urn:ietf:params:scim:schemas:";

		/// <summary>
		/// Active Directory prefix types 2.0
		/// </summary>
		public const string PrefixTypesActiveDirectory2 = "http://schemas.microsoft.com/2006/11/ResourceManagement/ADSCIM/2.0/";



		/// <summary>
		/// Schema Id for changed resource.
		/// </summary>
		public const string Changed =
			SchemaIdentifiers.Polling1 +
			"Changed";

		/// <summary>
		/// Creates Schema Id string for core2 enterprise user.
		/// </summary>
		public const string Core2EnterpriseUser =
			SchemaIdentifiers.PrefixTypes2 +
			SchemaIdentifiers.ExtensionEnterprise2 +
			Types.User;

		/// <summary>
		/// Schema Id for Core 2 enterprise user.
		/// </summary>
		public const string Core2EnterpriseUserDomainControllerServices =
			"urn:ietf:params:scim:schemas:extension:enterprise:2.0User";

		/// <summary>
		/// Creates Schema Id for core 2 group.
		/// </summary>
		public const string Core2Group =
			SchemaIdentifiers.PrefixTypes2 +
			SchemaIdentifiers.VersionSchemasCore2 +
			Types.Group;

		/// <summary>
		/// Creates Id for Enterprise Resource.
		/// </summary>
		public const string Core2ResourceType =
			SchemaIdentifiers.PrefixTypes2 +
			SchemaIdentifiers.ExtensionEnterprise2 +
			Types.ResourceType;

		/// <summary>
		/// Creates Id for core 2 service provider configuration.
		/// </summary>
		public const string Core2ServiceConfiguration =
			SchemaIdentifiers.PrefixTypes2 +
			SchemaIdentifiers.VersionSchemasCore2 +
			Types.ServiceProviderConfiguration;

		/// <summary>
		/// Creates Scema Id for Core 2 User.
		/// </summary>
		public const string Core2User =
			SchemaIdentifiers.PrefixTypes2 +
			SchemaIdentifiers.VersionSchemasCore2 +
			Types.User;

		/// <summary>
		/// For Atribute names.
		/// </summary>
		public const string WindowsAzureActiveDirectory2Group =
			SchemaIdentifiers.PrefixTypesActiveDirectory2 +
			Types.Group;

		/// <summary>
		/// For Atribute names.
		/// </summary>
		public const string WindowsAzureActiveDirectory2User =
			SchemaIdentifiers.PrefixTypesActiveDirectory2 +
			Types.User;

		/// <summary>
		/// Schema Id for List Response for query resources.
		/// </summary>
		public const string ListResponse = "urn:ietf:params:scim:api:messages:2.0:ListResponse";
	}
}
