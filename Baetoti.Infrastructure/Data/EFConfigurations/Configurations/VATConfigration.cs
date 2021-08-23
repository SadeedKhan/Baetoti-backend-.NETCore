using Baetoti.Core.Entites;
using Baetoti.Shared.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Baetoti.Infrastructure.Data.EFConfigurations.Configurations
{
    public class VATConfigration : IEntityTypeConfiguration<VAT>
    {
        public void Configure(EntityTypeBuilder<VAT> builder)
        {
            builder.ToTable(nameof(VAT), DBSchema.baetoti.ToString());
        }
    }
}
