using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NConnect.Shared.Api.Exceptions.Mappers;

namespace NConnect.Shared.Api.Exceptions.Middlewares;

internal sealed class ExceptionHandlerMiddleware(
    IExceptionToResponseMapper mapper,
    ILogger<ExceptionHandlerMiddleware> logger)
    : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, exception.Message);
            await HandleExceptionAsync(exception, context);
        }
    }

    private async Task HandleExceptionAsync(Exception exception, HttpContext context)
    {
        var response = mapper.Map(exception);

        context.Response.StatusCode = (int)response.StatusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}