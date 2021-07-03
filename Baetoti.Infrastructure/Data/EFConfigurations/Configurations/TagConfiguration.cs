using Baetoti.Core.Entites;
using Baetoti.Shared.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Baetoti.Infrastructure.Data.EFConfigurations.Configurations
{
    class TagConfiguration : IEntityTypeConfiguration<Tags>
    {
        public void Configure(EntityTypeBuilder<Tags> builder)
        {
            builder.ToTable(nameof(Tags), DBSchema.baetoti.ToString());
        }

    }
}
