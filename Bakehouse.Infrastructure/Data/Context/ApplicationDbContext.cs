using Bakehouse.Core.Entities;
using Bakehouse.Infrastructure.Data.EntitiesConfiguration;
using Bakehouse.Infrastructure.Data.Seeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bakehouse.Infrastructure.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<GenericType> GenericTypes { get; set; }
        public DbSet<Log> Logs { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opt) : base(opt)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Add mappings foreign keys

            // Add data default (seeding)
            builder.ApplyConfiguration(new UserSeed());
            builder.ApplyConfiguration(new RoleSeed());
            builder.ApplyConfiguration(new UserRoleSeed());

            // Config entities/tables
            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new ConfigurationConfiguration());
            builder.ApplyConfiguration(new GenericTypeConfiguration());
            builder.ApplyConfiguration(new LogConfiguration());
        }
    }
}