using Bakehouse.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bakehouse.Infrastructure.Data.EntitiesConfiguration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.BarCode).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Quantity).HasPrecision(10, 2).IsRequired();
            builder.Property(x => x.ValueUnitary).HasPrecision(10, 2).IsRequired();
            builder.Property(x => x.MinQuantity).HasPrecision(10, 2).IsRequired();
        }
    }
}
