using Baetoti.Core.Entites;
using Baetoti.Shared.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Baetoti.Infrastructure.Data.EFConfigurations.Configurations
{
    public class TempItemConfiguration : IEntityTypeConfiguration<TempItem>
    {
        public void Configure(EntityTypeBuilder<TempItem> builder)
        {
            builder.ToTable(nameof(TempItem), DBSchema.baetoti.ToString());
        }
    }
}
