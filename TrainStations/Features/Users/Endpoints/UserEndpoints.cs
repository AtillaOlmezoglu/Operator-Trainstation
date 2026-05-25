namespace TrainStations;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/users", () =>
        {
            return Results.Ok("Users endpoint works");
        });
    }
}
