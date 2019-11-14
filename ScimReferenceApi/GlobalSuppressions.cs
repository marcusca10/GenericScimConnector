
// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "ASP.NET core already checks JSON for null")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1724:TypeNamesShouldNotMatchNamespaces", Justification = "Metadata is an attribute defined in SCIM")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "Enum of the acceptable data types will include data type names", Scope = "type", Target = "~T:Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.AttributeDataType")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1307:Specify StringComparison", Justification = "<Pending>", Scope = "member", Target = "~M:Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol.Path.TryParse(System.String,Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol.IPath@)~System.Boolean")]
