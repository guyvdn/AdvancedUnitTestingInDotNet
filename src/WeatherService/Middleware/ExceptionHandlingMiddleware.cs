using System.Text.Json;

namespace WeatherService.Api.Middleware;

internal sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (FluentValidation.ValidationException e)
        {
            var problemDetails = new HttpValidationProblemDetails();
            foreach (var error in e.Errors.GroupBy(error => error.PropertyName))
            {
                problemDetails.Errors.Add(error.Key, error.Select(x => x.ErrorMessage).ToArray());
            }

            await SetResponse(context, problemDetails, Status400BadRequest);
        }
    }

    private static async Task SetResponse(HttpContext context, HttpValidationProblemDetails problemDetails, int statusCode)
    {
        var result = JsonSerializer.Serialize(problemDetails);

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsync(result, context.RequestAborted);
    }
}