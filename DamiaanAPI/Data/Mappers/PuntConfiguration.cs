using DamiaanAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DamiaanAPI.Data.Mappers
{
    public class PuntConfiguration : IEntityTypeConfiguration<Punt>
    {
        private static readonly string FACILITEITEN_SEPARATOR = "|";

        public void Configure(EntityTypeBuilder<Punt> builder)
        {
            builder.ToTable("Punt");
            builder.HasKey(l => l.ID);
            builder.Property(l => l.ID).HasColumnName("PuntID");
            builder.Property(l => l.Naam).HasMaxLength(128);
            builder.Property(l => l.Lon).IsRequired().HasMaxLength(64);
            builder.Property(l => l.Lat).IsRequired().HasMaxLength(64);
            builder.Property(l => l.Faciliteiten)
                .HasConversion(f => string.Join(FACILITEITEN_SEPARATOR, f),//convert list to single string
                f => f.Split(FACILITEITEN_SEPARATOR, StringSplitOptions.RemoveEmptyEntries)//get individual strings back
                .ToHashSet());
        }
    }
}