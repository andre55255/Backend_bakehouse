using Bakehouse.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bakehouse.Infrastructure.Data.EntitiesConfiguration
{
    public class OrderPadItemConfiguration : IEntityTypeConfiguration<OrderPadItem>
    {
        public void Configure(EntityTypeBuilder<OrderPadItem> builder)
        {
            builder.Property(x => x.Quantity).HasPrecision(10, 2).IsRequired();
            builder.Property(x => x.ValueUnitary).HasPrecision(10, 2).IsRequired();
        }
    }
}