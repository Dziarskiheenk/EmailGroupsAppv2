using EmailGroupsAppv2.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailGroupsAppv2.Data
{
  public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
  {
    public ApplicationDbContext(
        DbContextOptions options,
        IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
    {
    }
    public virtual DbSet<MailGroup> MailGroups { get; set; }
    public virtual DbSet<MailAddress> MailAddresses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.Entity<MailGroup>()
          .HasIndex(u => new { u.Name, u.OwnerId })
          .IsUnique();

      builder.Entity<MailGroup>()
          .HasMany(x => x.Addresses)
          .WithOne(x => x.MailGroup)
          .HasForeignKey(x => x.GroupId)
          .OnDelete(DeleteBehavior.Cascade);

      builder.Entity<ApplicationUser>()
          .HasMany(x => x.MailGroups)
          .WithOne(x => x.Owner)
          .HasForeignKey(x => x.OwnerId)
          .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
