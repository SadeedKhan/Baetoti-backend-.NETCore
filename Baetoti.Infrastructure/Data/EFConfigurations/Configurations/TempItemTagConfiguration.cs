using Baetoti.Core.Entites;
using Baetoti.Shared.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Baetoti.Infrastructure.Data.EFConfigurations.Configurations
{
    class TempItemTagConfiguration : IEntityTypeConfiguration<TempItemTag>
    {
        public void Configure(EntityTypeBuilder<TempItemTag> builder)
        {
            builder.ToTable(nameof(TempItemTag), DBSchema.baetoti.ToString());
        }
    }
}
