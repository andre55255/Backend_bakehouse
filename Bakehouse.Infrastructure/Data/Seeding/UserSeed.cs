using Bakehouse.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Bakehouse.Infrastructure.Data.Seeding
{
    public class UserSeed : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = 1,
                UserName = "admin",
                Email = "admin@uorak.com.br",
                NormalizedEmail = "ADMIN@UORAK.COM.BR",
                EmailConfirmed = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                DisabledAt = null,
                SecurityStamp = Guid.NewGuid().ToString(),
                Name = "Admin",
                LastName = "Souza"
            };

            PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = hasher.HashPassword(user, "Admin132!");

            builder.HasData(user);
        }
    }

    public class RoleSeed : IEntityTypeConfiguration<IdentityRole<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<int>> builder)
        {
            IdentityRole<int> role = new IdentityRole<int>
            {
                Id = 1,
                Name = "Admin",
                NormalizedName = "ADMIN"
            };

            builder.HasData(role);
        }
    }

    public class UserRoleSeed : IEntityTypeConfiguration<IdentityUserRole<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<int>> builder)
        {
            IdentityUserRole<int> relational = new IdentityUserRole<int>
            {
                RoleId = 1,
                UserId = 1
            };

            builder.HasData(relational);
        }
    }
}