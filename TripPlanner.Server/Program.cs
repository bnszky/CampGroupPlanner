
using TripPlanner.Server.Data;
using Microsoft.EntityFrameworkCore;
using TripPlanner.Server.Services.Abstractions;
using TripPlanner.Server.Services.Implementations;

namespace TripPlanner.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IImageService, ImageService>();

            builder.Services.AddDbContext<TripDbContext>(options => options.UseSqlServer("Data Source=DESKTOP-FQ3UJS6;Initial Catalog=TripAppDatabase;Integrated Security=True;Trust Server Certificate=True"));

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
