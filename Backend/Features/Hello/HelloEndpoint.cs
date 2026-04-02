using Backend.Features.Hello.Models;
using Microsoft.AspNetCore.Builder;

namespace Backend.Features.Hello;

public static class HelloEndpoint
{
    public static void MapHelloEndpoint(this WebApplication app)
    {
        app.MapGet("/api/hello", GetHello)
            .WithName("GetHello")
            .WithOpenApi();
    }

    private static IResult GetHello()
    {
        var response = new HelloResponse("Hello from ASP.NET Core", DateTime.UtcNow);
        return Results.Ok(response);
    }
}
