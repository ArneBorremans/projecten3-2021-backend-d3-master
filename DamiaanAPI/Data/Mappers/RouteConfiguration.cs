using DamiaanAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DamiaanAPI.Data.Mappers
{
    public class RouteConfiguration : IEntityTypeConfiguration<Route>
    {
        public void Configure(EntityTypeBuilder<Route> builder)
        {
            builder.ToTable("Route");
            builder.HasKey(r => r.ID);
            builder.Property(r => r.ID).HasColumnName("RouteID");
            builder.Property(r => r.Naam).IsRequired()
                .HasConversion(b => JsonConvert.SerializeObject(b), b => JsonConvert.DeserializeObject<Dictionary<string, string>>(b));
            builder.Property(r => r.Lengte).IsRequired();
            builder.Property(r => r.Openbaar).IsRequired();
            builder.Property(r => r.Start).IsRequired();
            builder.Property(r => r.Einde).IsRequired();
            builder.Property(r => r.Beschrijving)
                .HasConversion(b => JsonConvert.SerializeObject(b), b => JsonConvert.DeserializeObject<Dictionary<string, string>>(b));
            builder.Property(r => r.GeoJson);
            builder.Property(r => r.Prijs);
            builder.Property(r => r.Afbeeldingen)
                .HasConversion(a => JsonConvert.SerializeObject(a), json => JsonConvert.DeserializeObject<HashSet<string>>(json));
        }
    }
}
