
using TripPlanner.Server.Data;
using Microsoft.EntityFrameworkCore;
using TripPlanner.Server.Services.Abstractions;
using TripPlanner.Server.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using TripPlanner.Server.Middlewares;
using Serilog;
using Serilog.Events;
using TripPlanner.Server.Models.Auth;

namespace TripPlanner.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseSerilog();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "TripApp Api", Version = "v1" });
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

            builder.Services.AddHttpClient();

            // Services
            builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddScoped<IRegionFetchService, RegionFetchService>();
            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddScoped<IRegionService, RegionService>();
            builder.Services.AddScoped<ICityService, CityService>();
            builder.Services.AddScoped<IErrorService, ErrorService>();
            builder.Services.AddScoped<IResponseService, ResponseService>();
            builder.Services.AddScoped<IAttractionService, AttractionService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddTransient<ISeedingService, SeedingService>();
            builder.Services.AddSingleton<ITokenBlacklistService, TokenBlacklistService>();
            builder.Services.AddTransient<IEmailService, EmailService>();

            // Services for fetching articles
            builder.Services.AddTransient<IArticleFetchService, ArticleFetchService>();

            builder.Services.AddTransient<IArticleSourceService, ArticleSourceDemoService>();
            builder.Services.AddTransient<IArticleSourceService, ArticleSourceRSS>();

            builder.Services.AddTransient<IArticleRatingService, ArticleRatingService>();

            // Services for fetching attractions
            builder.Services.AddTransient<IAttractionFetchService, AttractionFetchService>();

            builder.Services.AddDbContext<TripDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("TripAppDb")));

            builder.Services.AddIdentity<User, IdentityRole>(options => {
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
            })
            .AddEntityFrameworkStores<TripDbContext>()
            .AddDefaultTokenProviders();

            // AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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

            // Add Middlewares
            // Middleware to ensure that logout disactivate token
            app.UseMiddleware<TokenBlacklistMiddleware>();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            //app.MapIdentityApi<User>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Authentication and Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            try
            {
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
