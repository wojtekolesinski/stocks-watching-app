﻿using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Duende.IdentityServer.EntityFramework.Options;
using Stocks.Server.Models;

namespace Stocks.Server.Data;

public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<Article> Articles { get; set; }
    
    public ApplicationDbContext(
        DbContextOptions options,
        IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Company>()
            .HasMany(c => c.Users)
            .WithMany(u => u.Companies)
            .UsingEntity("Users_Companies");

        builder.Entity<Article>()
            .HasOne(a => a.Company)
            .WithMany(c => c.Articles);
    }
}