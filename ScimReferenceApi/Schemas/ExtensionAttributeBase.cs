namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class ExtensionAttributeBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public abstract string ExtensionSchemaName
        {
            get;
        }
    }
}
