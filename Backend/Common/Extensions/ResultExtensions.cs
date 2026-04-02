namespace Backend.Common.Extensions;

public static class ResultExtensions
{
    public static IResult AsJson<T>(this T data, int statusCode = StatusCodes.Status200OK) where T : class
    {
        return Results.Json(data, statusCode: statusCode);
    }

    public static IResult AsCreated<T>(this T data, string location) where T : class
    {
        return Results.Created(location, data);
    }
}
