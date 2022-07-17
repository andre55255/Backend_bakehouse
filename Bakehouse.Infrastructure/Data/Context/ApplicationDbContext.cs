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
        public DbSet<Category> Categories { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<GenericType> GenericTypes { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Movement> Movements { get; set; }
        public DbSet<OrderPad> OrderPads { get; set; }
        public DbSet<OrderPadItem> OrderPadItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<UnitOfMeasurement> UnitOfMeasurements { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opt) : base(opt)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Add mappings foreign keys
            // Product - Category
            builder.Entity<Product>()
                   .HasOne(x => x.Category)
                   .WithMany(x => x.Products)
                   .HasForeignKey(fk => fk.CategoryId);

            // Product - UnitOfMeasurement
            builder.Entity<Product>()
                   .HasOne(x => x.UnitOfMeasurement)
                   .WithMany(x => x.Products)
                   .HasForeignKey(fk => fk.UnitOfMeasurementId);

            // OrderPadItem - Product
            builder.Entity<OrderPadItem>()
                   .HasOne(x => x.Product)
                   .WithMany(x => x.OrderPadItems)
                   .HasForeignKey(fk => fk.ProductId);

            // OrderPadItem - OrderPad
            builder.Entity<OrderPadItem>()
                   .HasOne(x => x.OrderPad)
                   .WithMany(x => x.OrderPadItems)
                   .HasForeignKey(fk => fk.OrderPadId);

            // Movement - GenericType
            builder.Entity<Movement>()
                   .HasOne(x => x.Type)
                   .WithMany(x => x.Movements)
                   .HasForeignKey(fk => fk.TypeId);

            // Movement - ApplicationUser
            builder.Entity<Movement>()
                   .HasOne(x => x.User)
                   .WithMany(x => x.Movements)
                   .HasForeignKey(fk => fk.UserId);

            // Add data default (seeding)
            builder.ApplyConfiguration(new UserSeed());
            builder.ApplyConfiguration(new RoleSeed());
            builder.ApplyConfiguration(new UserRoleSeed());

            // Config entities/tables
            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new ConfigurationConfiguration());
            builder.ApplyConfiguration(new GenericTypeConfiguration());
            builder.ApplyConfiguration(new LogConfiguration());
            builder.ApplyConfiguration(new MovementConfiguration());
            builder.ApplyConfiguration(new OrderPadConfiguration());
            builder.ApplyConfiguration(new OrderPadItemConfiguration());
            builder.ApplyConfiguration(new ProductConfiguration());
            builder.ApplyConfiguration(new UnitOfMeasurementConfiguration());
        }
    }
}