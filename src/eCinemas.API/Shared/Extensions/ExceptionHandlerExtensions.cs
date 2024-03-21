using System.Net;
using eCinemas.API.Shared.Exceptions;
using eCinemas.API.Shared.ValueObjects;
using Microsoft.AspNetCore.Diagnostics;

namespace eCinemas.API.Shared.Extensions;

public static class ExceptionHandlerExtensions
{
    public static IApplicationBuilder UseDefaultExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(
            appErr =>
            {
                appErr.Run(async context =>
                {
                    var feature = context.Features.Get<IExceptionHandlerFeature>();

                    if (feature is not null)
                    {
                        var exception = feature.Error;

                        switch (exception)
                        {
                            case UnauthorizedAccessException:
                                await WriteResponse(context, (int)HttpStatusCode.Unauthorized, "Unauthorized");
                                break;
                            case BadRequestException:
                                await WriteResponse(context, (int)HttpStatusCode.BadRequest, GetErrorMessage(exception));
                                break;
                            default:
                                await WriteResponse(context, (int)HttpStatusCode.InternalServerError, GetErrorMessage(exception));
                                break;
                        }
                    }
                });
            });
        
        return app;
    }
    
    private static async Task WriteResponse(HttpContext context, int statusCode, string? errorMessage)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(
            new APIResponse
            {
                Success = false,
                Code = statusCode,
                Message = errorMessage
            });
    }

    private static string GetErrorMessage(Exception ex)
    {
        var messages = new List<string> { ex.Message };

        var inner = ex.InnerException;
        var count = 5;
        while (inner is not null && count-- > 0)
        {
            messages.Add(inner.Message);
            inner = inner.InnerException;
        }

        return string.Join(";", messages.Distinct().ToList());
    }
}