using Bakehouse.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bakehouse.Infrastructure.Data.EntitiesConfiguration
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
            builder.Property(x => x.LastName).HasMaxLength(255).IsRequired();
        }
    }
}