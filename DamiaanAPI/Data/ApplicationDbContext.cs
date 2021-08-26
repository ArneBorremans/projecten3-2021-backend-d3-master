using System;
using DamiaanAPI.Data.Mappers;
using DamiaanAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DamiaanAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Route> Routes { get; set; }
        public DbSet<Loper> Lopers { get; set; }
        public DbSet<Punt> Punten { get; set; }
        public DbSet<Message> Messages { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RouteConfiguration());
            builder.ApplyConfiguration(new LoperConfiguration());
            builder.ApplyConfiguration(new PuntConfiguration());
            builder.ApplyConfiguration(new RouteLoperConfiguration());
            builder.ApplyConfiguration(new MessageConfiguration());
        }

    }

}
