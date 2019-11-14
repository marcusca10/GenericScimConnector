//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.EntityFrameworkCore;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    public class ScimContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EnterpriseUser>().HasBaseType<User>();
        }
        public ScimContext(DbContextOptions<ScimContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Group> Groups { get; set; }
    }
}
