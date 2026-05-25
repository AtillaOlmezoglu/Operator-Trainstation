using Features.Stations.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TrainStations.Features.Users.Models;

public static class AppDbContextSeed
{
    public static async Task SeedAsync(
        ILogger logger,
        AppDbContext dbContext,
        UserManager<ApplicationUser> userManager)
    {
        try
        {
            await dbContext.Database.MigrateAsync();

            await SeedStationsAsync(logger, dbContext);
            await SeedUserAsync(userManager);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while seeding database");
            throw;
        }
    }

    private static async Task SeedStationsAsync(
    ILogger logger,
    AppDbContext dbContext)
    {
        var stationCountInDb = await dbContext.Stations.CountAsync();

        logger.LogInformation("Stations currently in database: {Count}", stationCountInDb);

        if (stationCountInDb > 0)
        {
            logger.LogInformation("Skipping station seed because stations already exist.");
            return;
        }

        var path = "Database/train-station.json";
        var fullPath = Path.GetFullPath(path);

        logger.LogInformation("Reading station JSON from: {Path}", fullPath);

        if (!File.Exists(path))
        {
            logger.LogError("Station JSON file was not found at: {Path}", fullPath);
            throw new FileNotFoundException("Station JSON file was not found.", fullPath);
        }

        var json = await File.ReadAllTextAsync(path);

        logger.LogInformation("JSON length: {Length}", json.Length);

        var response = JsonSerializer.Deserialize<StationApiResponse>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        var apiStations = response?
            .RESPONSE?
            .RESULT
            .SelectMany(result => result.TrainStation)
            .ToList();

        logger.LogInformation("Stations found in JSON: {Count}", apiStations?.Count ?? 0);

        if (apiStations is null || apiStations.Count == 0)
        {
            logger.LogWarning("No stations found in JSON. Station seed stopped.");
            return;
        }

        var stations = apiStations.Select(x => new Station
        {
            Advertised = x.Advertised,
            AdvertisedLocationName = x.AdvertisedLocationName ?? string.Empty,
            AdvertisedShortLocationName = x.AdvertisedShortLocationName ?? string.Empty,
            PrimaryLocationCode = x.PrimaryLocationCode ?? string.Empty,
            CountryCode = x.CountryCode ?? string.Empty,
            Deleted = x.Deleted,
            LocationInformationText = x.LocationInformationText,
            LocationSignature = x.LocationSignature ?? string.Empty,
            Prognosticated = x.Prognosticated,
            OfficialLocationName = x.OfficialLocationName ?? string.Empty,
            ModifiedTime = x.ModifiedTime,
            Sweref99Tm = x.Geometry?.SWEREF99TM,
            Wgs84 = x.Geometry?.WGS84,
            CountyNumbers = string.Join(",", x.CountyNo),
            PlatformLines = string.Join(",", x.PlatformLine)
        }).ToList();

        dbContext.Stations.AddRange(stations);

        var saved = await dbContext.SaveChangesAsync();

        logger.LogInformation("Saved station changes: {Saved}", saved);
    }

    private static async Task SeedUserAsync(UserManager<ApplicationUser> userManager)
    {
        var existingUser = await userManager.FindByEmailAsync("admin@test.com");

        if (existingUser is not null)
        {
            return;
        }

        var admin = new ApplicationUser
        {
            UserName = "admin@test.com",
            Email = "admin@test.com",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(admin, "Admin123!");

        if (!result.Succeeded)
        {
            throw new Exception(
                string.Join(", ", result.Errors.Select(x => x.Description)));
        }
    }
}