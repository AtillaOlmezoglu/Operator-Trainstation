using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrainStations.Features.Stations.Endpoints;
using TrainStations.Features.Users.Models;

namespace TrainStations
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services
               .AddIdentityApiEndpoints<ApplicationUser>()
               .AddEntityFrameworkStores<AppDbContext>()
               .AddDefaultTokenProviders();

            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Frontend", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            builder.Services.AddOpenApi();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseCors("Frontend");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapIdentityApi<ApplicationUser>();
            app.MapStationEndpoints();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var logger = scope.ServiceProvider
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger("AppDbContextSeed");

                await AppDbContextSeed.SeedAsync(logger, dbContext, userManager);
            }

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.Run();
        }
    }
}