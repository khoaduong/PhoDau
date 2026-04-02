using Backend.Features.Status.Models;
using Microsoft.AspNetCore.Builder;

namespace Backend.Features.Status;

public static class StatusEndpoint
{
    public static void MapStatusEndpoint(this WebApplication app)
    {
        app.MapGet("/api/status", GetStatus)
            .WithName("GetStatus")
            .WithOpenApi();
    }

    private static IResult GetStatus()
    {
        var response = new StatusResponse("running", DateTime.UtcNow);
        return Results.Ok(response);
    }
}
