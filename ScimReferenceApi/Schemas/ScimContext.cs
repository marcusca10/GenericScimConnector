using Microsoft.EntityFrameworkCore;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    /// <summary>
    /// Persistent storage context.
    /// </summary>
    public class ScimContext : DbContext
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ScimContext(DbContextOptions<ScimContext> options) : base(options) { }

        /// <summary>
        /// Get or set Users.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Get or set Groups.
        /// </summary>
        public DbSet<Group> Groups { get; set; }
    }
}
