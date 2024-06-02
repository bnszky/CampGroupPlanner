using Microsoft.EntityFrameworkCore;
using TripPlanner.Server.Models;

namespace TripPlanner.Server.Data
{
    public class TripDbContext : DbContext
    {
        public DbSet<Attraction> Attractions { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Image> ImageURLs { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Region> Regions { get; set; }

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

            modelBuilder.Entity<Region>().HasData(
                new Region
                {
                    Id = 1,
                    Name = "Catalonia",
                    Country = "Spain",
                    Description = "Boasting a reputation as one of the most attractive cities in Europe, Barcelona celebrates its role as the capital of Catalonia. The city’s cosmopolitan and international vibe makes it a favorite city for many people around the world. The city is especially known for its architecture and art—travelers flock from around the world to see the iconic Sagrada Família church and other modernist landmarks designed by Gaudí. These Barcelona travel tips just scrape the surface of what can be found in the vibrant city!"
                }
            );

            modelBuilder.Entity<Image>().HasData(
                new Image { Id = 1, RegionId = 1,
                    Link = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRnwf8dsSKIsCsVbwXlpQEuvEP6q70MdNVjdQ&s" },
                new Image
                {
                    Id = 2,
                    RegionId = 1,
                    Link = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRaRfTP8AW7Od72m4IRi4LPRt9xNqPYfYlPrg&s"
                }
            );

            modelBuilder.Entity<City>().HasData(
                new City { Id = 1, Name = "Barcelona", Country="Spain", RegionId = 1},
                new City { Id = 2, Name = "Tarragona", Country = "Spain", RegionId = 1 },
                new City { Id = 3, Name = "Girona", Country = "Spain", RegionId = 1 }
            );

            modelBuilder.Entity<Attraction>().HasData(

                new Attraction { Id = 1, Name = "Basílica de la Sagrada Familia", Description = """
                    Basilica de la Sagrada Familia is a church devoted to the Holy Family. One of Antoni Gaudis most famous works, the church is perhaps best known for still being under construction since 1882—with works funded purely by donations. Take a lift to the top of the towers for a panoramic view of the city
                """, ImageURL = "https://dynamic-media-cdn.tripadvisor.com/media/photo-o/08/10/a7/d6/basilica-de-la-sagrada.jpg?w=1200&h=-1&s=1", 
                Longitude = 41.40377892106611,
                Latitude = 2.174366495306742,
                RegionId = 1,
                },

                new Attraction
                {
                    Id = 2,
                    Name = "Parc Guell",
                    Description = """
                    Basilica de la Sagrada Familia is a church devoted to the Holy Family. One of Antoni Gaudis most famous works, the church is perhaps best known for still being under construction since 1882—with works funded purely by donations. Take a lift to the top of the towers for a panoramic view of the city
                """,
                    ImageURL = "https://lh5.googleusercontent.com/p/AF1QipNgwQHFyIjmdNz9RYHLND4_2hXzrBmqObHjBIfR=w408-h305-k-no",
                    Longitude = 41.4146798295698,
                    Latitude = 2.152780327073272,
                    RegionId = 1,
                },

                new Attraction
                {
                    Id = 3,
                    Name = "Casa Batlló",
                    Description = """
                    Welcome to Barcelona's magical house. A Gaudí masterpiece. A unique immersive experience. International Exhibition of the Year 2022. Children free up to 12 years old.
                """,
                    ImageURL = "https://dynamic-media-cdn.tripadvisor.com/media/daodao/photo-o/19/ac/b2/a5/caption.jpg?w=1200&h=-1&s=1",
                    Longitude = 41.39187830789514,
                    Latitude = 2.164871022478367,
                    RegionId = 1,
                }
            );

            modelBuilder.Entity<Article>().HasData(
            new Article
            {
                Id = 1,
                Title = "Beatiful World!",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque eu vestibulum turpis. Nullam id ipsum at ipsum blandit porttitor sed ut odio. Sed vulputate justo est. Sed tristique, libero eu luctus pellentesque, sem justo luctus nulla, euismod semper quam dui eget mi. Sed at pretium arcu, at gravida nulla. Ut sagittis lacinia ex ut venenatis. Aenean.",
                CreatedAt = new DateTime(2020, 5, 4),
                RegionId = 1,
                ImageURL = "https://rodzinanomadow.pl/wp-content/uploads/2018/06/image-10-1024x683.jpeg",
                SourceLink = "https://www.niagarafallsstatepark.com/",
                RegionName="Catalonia",
                IsVisible= true,
                PositioningRate=4
            },
            new Article
            {
                Id = 2,
                Title = "Barcelona",
                Description = "Boasting a reputation as one of the most attractive cities in Europe, Barcelona celebrates its role as the capital of Catalonia.",
                CreatedAt = new DateTime(2022, 11, 25),
                RegionId = 1,
                ImageURL = "https://www.theblondeabroad.com/wp-content/uploads/2022/02/theodor-vasile-LSscVPEyQpI-unsplash.jpg",
                SourceLink = "https://www.theblondeabroad.com/ultimate-barcelona-travel-guide/",
                RegionName = "Catalonia",
                IsVisible = true,
                PositioningRate = 1
            },
            new Article
            {
                Id = 3,
                Title = "Rome",
                Description = "Rome is one of the most iconic and most-traveled cities in Europe, with a long history to match. With a mixture of cultures from around the world",
                CreatedAt = new DateTime(2024, 4, 14, 15, 41, 0),
                RegionId = 1,
                ImageURL = "https://www.theblondeabroad.com/wp-content/uploads/2022/02/david-edkins-grlIoctRp1o-unsplash.jpg",
                SourceLink = "https://www.theblondeabroad.com/ultimate-rome-travel-guide/",
                RegionName = "Catalonia",
                IsVisible = true,
                PositioningRate = 3
            }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "Michali",
                    LastName = "LastName1",
                    Email = "michali@example.com",
                    Nick = "michali",
                    ProfileImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSGDohX4qAelLzi3t8vCfqccDFxifY-huxkmRrgnSRoig&s"
                },
                new User
                {
                    Id = 2,
                    FirstName = "Gombalo",
                    LastName = "LastName2",
                    Email = "gombalo@example.com",
                    Nick = "gombalo",
                    ProfileImageUrl = "https://static.vecteezy.com/system/resources/thumbnails/002/002/403/small/man-with-beard-avatar-character-isolated-icon-free-vector.jpg"
                }
            );

            modelBuilder.Entity<Review>().HasData(
            new Review
            {
                Id = 1,
                Title = "Opinion 1",
                Text = "Beautiful place! I would like to be there again",
                Rate = 4.5,
                CreatedAt = new DateTime(2024, 4, 14, 15, 41, 0),
                AuthorId = 1,
                RegionId = 1
            },
            new Review
            {
                Id = 2,
                Title = "Opinion 2",
                Text = "I don't like spanish people, Ughh...",
                Rate = 2,
                CreatedAt = new DateTime(2023, 7, 18, 15, 41, 0),
                AuthorId = 2,
                RegionId = 1
            }
            );
        }
    }
}
