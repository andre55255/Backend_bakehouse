using Bakehouse.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bakehouse.Infrastructure.Data.EntitiesConfiguration
{
    public class OrderPadConfiguration : IEntityTypeConfiguration<OrderPad>
    {
        public void Configure(EntityTypeBuilder<OrderPad> builder)
        {
            builder.Property(x => x.DateHour).IsRequired();
        }
    }
}