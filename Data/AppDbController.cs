using CampGroupPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace CampGroupPlanner.Data
{
    public class AppDbController : DbContext
    {
        public DbSet<User> Users { get; set; }
        public AppDbController(DbContextOptions<AppDbController> options) : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Email = "michal@onet.eu", FirstName = "Michal", LastName = "Kowalski" },
                new User { Id = 2, Email = "adam@gmail.com", FirstName = "Adam", LastName = "Dobrek" },
                new User { Id = 3, Email = "karolina@gmail.com", FirstName = "Caroline", LastName = "Koralińska" }
            );
        }
    }
}
