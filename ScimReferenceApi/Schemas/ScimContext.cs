using Microsoft.EntityFrameworkCore;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    /// <summary>
    /// Persistent storage context.
    /// </summary>
    public class ScimContext : DbContext
    {
		/// <summary>
		/// Add enterpirse user to system
		/// </summary>
		/// <param name="modelBuilder"></param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<EnterpriseUser>().HasBaseType<User>();
		}
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
