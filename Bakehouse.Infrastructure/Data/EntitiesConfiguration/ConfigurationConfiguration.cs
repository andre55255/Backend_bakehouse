using Bakehouse.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bakehouse.Infrastructure.Data.EntitiesConfiguration
{
    public class ConfigurationConfiguration : IEntityTypeConfiguration<Configuration>
    {
        public void Configure(EntityTypeBuilder<Configuration> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(256).IsRequired();
            builder.Property(x => x.Token).HasMaxLength(256).IsRequired();
            builder.Property(x => x.Value).HasMaxLength(256).IsRequired(false);
            builder.Property(x => x.Extra1).HasMaxLength(256).IsRequired(false);
            builder.Property(x => x.Extra2).HasMaxLength(256).IsRequired(false);
        }
    }
}