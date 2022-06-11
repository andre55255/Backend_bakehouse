using Bakehouse.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bakehouse.Infrastructure.Data.EntitiesConfiguration
{
    public class LogConfiguration : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.Property(l => l.Object).HasMaxLength(255).IsRequired(false);
            builder.Property(l => l.Exception).HasMaxLength(255).IsRequired(false);
            builder.Property(l => l.Description).HasMaxLength(255).IsRequired(false);
            builder.Property(l => l.ResponseJson).HasMaxLength(2056).IsRequired(false);
        }
    }
}
