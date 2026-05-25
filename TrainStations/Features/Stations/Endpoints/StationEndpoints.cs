using Microsoft.EntityFrameworkCore;

namespace TrainStations.Features.Stations.Endpoints;

public static class StationEndpoints
{
    public static IEndpointRouteBuilder MapStationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/stations", async (AppDbContext db) =>
        {
            var stations = await db.Stations.ToListAsync();

            return Results.Ok(stations);
        })
        .RequireAuthorization();

        return app;
    }

}
