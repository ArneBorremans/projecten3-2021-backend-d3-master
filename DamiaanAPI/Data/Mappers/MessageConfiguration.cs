using DamiaanAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DamiaanAPI.Data.Mappers
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {

        public void Configure(EntityTypeBuilder<Message> builder)
        { 
            builder.ToTable("Message");
            builder.HasKey(l => l.ID);
            builder.Property(l => l.ID).HasColumnName("MessageID");
            builder.Property(l => l.Text).IsRequired().HasMaxLength(256);
            builder.Property(l => l.Zender).IsRequired().HasMaxLength(32);
            builder.Property(l => l.Date).IsRequired();
        }
    }
}
