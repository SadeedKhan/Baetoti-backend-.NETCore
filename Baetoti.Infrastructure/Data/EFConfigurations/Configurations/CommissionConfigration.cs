using Baetoti.Core.Entites;
using Baetoti.Shared.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Baetoti.Infrastructure.Data.EFConfigurations.Configurations
{
    public class CommissionConfigration : IEntityTypeConfiguration<Commissions>
    {
        public void Configure(EntityTypeBuilder<Commissions> builder)
        {
            builder.ToTable(nameof(Commissions), DBSchema.baetoti.ToString());
        }
    }
}
