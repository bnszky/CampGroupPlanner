using CampGroupPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace CampGroupPlanner.Data
{
    public class AppDbController : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Attraction> Attractions { get; set; }
        public DbSet<Localization> Localization { get; set; }
        public DbSet<AttractionType> AttractionTypes { get; set; }
        public DbSet<AttractionImage> AttractionImages { get; set; }
        public AppDbController(DbContextOptions<AppDbController> options) : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attraction>()
                .HasOne(a => a.Author)
                .WithMany(u => u.Attractions)
                .HasForeignKey(a => a.AuthorId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AttractionImage>().
                HasOne(a => a.Author).
                WithMany(u => u.AttractionImages).
                HasForeignKey(a => a.AuthorId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>().
                HasOne(a => a.Author).
                WithMany(u => u.Reviews).
                HasForeignKey(a => a.AuthorId).OnDelete(DeleteBehavior.Restrict);

            /*modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Email = "michal@onet.eu", FirstName = "Michal", LastName = "Kowalski" },
                new User { Id = 2, Email = "adam@gmail.com", FirstName = "Adam", LastName = "Dobrek" },
                new User { Id = 3, Email = "karolina@gmail.com", FirstName = "Caroline", LastName = "Koralińska" },
                new User { Id = 4, Email = "karolek@gmail.com", FirstName = "Karol", LastName = "Korzen" }
            );

            modelBuilder.Entity<AttractionType>().HasData(
                new AttractionType { Id = 1, Name = "Castle" },
                new AttractionType { Id = 2, Name = "Temple" },
                new AttractionType { Id = 3, Name = "Lake" }
            );*/

            /*modelBuilder.Entity<AttractionImage>().HasData(
                new AttractionImage { Author = (User)Users.Where(user => user.FirstName == "Michal"), Likes = Users.ToList() },
                new AttractionImage { Author = (User)Users.Where(user => user.FirstName == "Adam"), Likes = Users.Take(3).ToList() }
            );

            modelBuilder.Entity<Attraction>().HasData(
                new Attraction
                {
                    Author = (User)Users.Where(user => user.FirstName == "Michal"),
                    CreationDate = DateTime.Now,
                    Name = "Czocha Castle",
                    Description = "This is Czocha Castle",
                    Images = AttractionImages.ToList(),
                    Localization = null,
                    Reviews = null,
                    Type = (AttractionType)AttractionTypes.Where(attraction => attraction.Name == "Castle"),
                    Visitors = Users.Take(3).ToList(),
                    PositionRatio = 1
                }
            );*/
        }
    }
}
