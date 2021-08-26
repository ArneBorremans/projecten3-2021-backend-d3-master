using DamiaanAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DamiaanAPI.Data.Mappers
{
    public class LoperConfiguration : IEntityTypeConfiguration<Loper>
    {
        public void Configure(EntityTypeBuilder<Loper> builder)
        {
            builder.ToTable("Loper");
            builder.HasKey(l => l.ID);
            builder.Property(l => l.ID).HasColumnName("LoperID");
            builder.Property(l => l.FirstName).IsRequired().HasMaxLength(32);
            builder.Property(l => l.LastName).IsRequired().HasMaxLength(64);
            builder.Property(l => l.Gemeente).IsRequired().HasMaxLength(64);
            builder.Property(l => l.Email).IsRequired().HasMaxLength(100);
        }
    }
}