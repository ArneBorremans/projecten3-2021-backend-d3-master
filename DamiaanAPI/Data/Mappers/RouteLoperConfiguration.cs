using DamiaanAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DamiaanAPI.Data.Mappers
{
    public class RouteLoperConfiguration : IEntityTypeConfiguration<RouteLoper>
    {
        public void Configure(EntityTypeBuilder<RouteLoper> builder)
        {
            builder.ToTable("RouteLoper");
            builder.HasKey(rl => new { rl.RouteID, rl.LoperID});
            builder.Property(rl => rl.GeregistreerdOp).IsRequired().HasMaxLength(100);
            builder.Property(rl => rl.StartDatumEnUur);
            builder.Property(rl => rl.EindDatumEnUur);
            builder.Property(rl => rl.LaatsteLocatieLon);
            builder.Property(rl => rl.LaatsteLocatieLat);
            builder.Property(rl => rl.TshirtMaat);
            builder.Property(rl => rl.Zichtbaarheid);
            builder.Property(rl => rl.LinkCode);
            builder.Property(rl => rl.OrderId);
            builder.Property(rl => rl.OrderStatus);

            builder.Property(rl => rl.RouteID).HasColumnName("ROUTE_ID");
            builder.Property(rl => rl.LoperID).HasColumnName("LOPER_ID");

            builder.HasOne(rl => rl.Loper)
                .WithMany(l => l.GeregistreerdeRoutes)
                .HasForeignKey(rl => rl.LoperID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(rl => rl.Route)
                .WithMany(r => r.Lopers)
                .HasForeignKey(rl => rl.RouteID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
