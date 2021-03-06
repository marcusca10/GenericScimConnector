﻿//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    using Microsoft.EntityFrameworkCore;

    public class ScimContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Resource>().Property(e => e.Identifier).ValueGeneratedOnAdd();

            modelBuilder.Entity<Core2EnterpriseUser>().HasBaseType<Core2User>();
        }
        public ScimContext(DbContextOptions<ScimContext> options) : base(options) { }

        public DbSet<Core2User> Users { get; set; }

        public DbSet<Core2Group> Groups { get; set; }
    }
}
