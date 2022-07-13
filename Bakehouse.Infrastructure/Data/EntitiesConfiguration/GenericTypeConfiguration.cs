using Bakehouse.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bakehouse.Infrastructure.Data.EntitiesConfiguration
{
    public class GenericTypeConfiguration : IEntityTypeConfiguration<GenericType>
    {
        public void Configure(EntityTypeBuilder<GenericType> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(255).IsRequired(false);
            builder.Property(x => x.Token).HasMaxLength(255).IsRequired(false);
            builder.Property(x => x.Description).HasMaxLength(255).IsRequired(false);
            builder.Property(x => x.Value).HasPrecision(18, 6).IsRequired(false);
        }
    }
}