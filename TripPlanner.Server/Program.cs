
using TripPlanner.Server.Data;
using Microsoft.EntityFrameworkCore;
using TripPlanner.Server.Services.Abstractions;
using TripPlanner.Server.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TripPlanner.Server.Models;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;

namespace TripPlanner.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            // Services
            builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddScoped<IRegionFetchService, RegionFetchService>();
            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddScoped<IRegionService, RegionService>();
            builder.Services.AddScoped<ICityService, CityService>();
            builder.Services.AddScoped<IErrorService, ErrorService>();
            builder.Services.AddScoped<IAttractionService, AttractionService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddTransient<ISeedingService, SeedingService>();

            builder.Services.AddDbContext<TripDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("TripAppDb")));

            builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<TripDbContext>()
            .AddDefaultTokenProviders();

            //Jwt tokens
            var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
            var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
             {
                 options.RequireHttpsMetadata = false;
                 options.SaveToken = true;
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = jwtIssuer,
                     ValidAudience = jwtIssuer,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                 };
             });

            // EF Identity
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
            });
            //builder.Services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<TripDbContext>();

            var app = builder.Build();

            // Register admin if doesn't exist
            var seedingService = app.Services.GetRequiredService<ISeedingService>();
            seedingService.SeedAsync().Wait();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            //app.MapIdentityApi<User>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Logging
            builder.Services.AddLogging(logging =>
            {
                logging.AddConsole();
                logging.AddDebug();
            });

            app.UseHttpsRedirection();

            // Authentication and Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
