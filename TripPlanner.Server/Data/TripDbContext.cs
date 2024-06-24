using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TripPlanner.Server.Models.Auth;
using TripPlanner.Server.Models.Database;

namespace TripPlanner.Server.Data
{
    public class TripDbContext : IdentityDbContext<User>
    {
        public DbSet<Attraction> Attractions { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Configuration> Configurations { get; set; }

        public TripDbContext(DbContextOptions<TripDbContext> options) : base(options) {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Region>()
                .HasIndex(r => r.Name)
                .IsUnique();

            modelBuilder.Entity<City>()
                .HasIndex(r => r.Name)
                .IsUnique();
        }
    }
}
