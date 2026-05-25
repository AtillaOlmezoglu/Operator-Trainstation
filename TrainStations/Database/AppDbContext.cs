using Features.Stations.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrainStations.Features.Users.Models;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    // Tar emot inställningar från Program.cs
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<Station> Stations => Set<Station>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}