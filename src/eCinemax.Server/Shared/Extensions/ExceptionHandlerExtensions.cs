using System.Net;
using Microsoft.AspNetCore.Diagnostics;

namespace eCinemax.Server.Shared.Extensions;

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
                                await WriteResponse(context, (int)HttpStatusCode.Unauthorized, ["Unauthorized"]);
                                break;
                            case BadRequestException:
                                await WriteResponse(context, (int)HttpStatusCode.BadRequest, GetErrors(exception));
                                break;
                            case ValidationException:
                                await WriteResponse(context, (int)HttpStatusCode.BadRequest, GetErrors(exception));
                                break;
                            default:
                                await WriteResponse(context, (int)HttpStatusCode.InternalServerError, GetErrors(exception));
                                break;
                        }
                    }
                });
            });
        
        return app;
    }
    
    private static async Task WriteResponse(HttpContext context, int statusCode, string[]? errors)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(
            new APIResponse
            {
                Success = false,
                Code = statusCode,
                Errors = errors
            });
    }

    private static string[] GetErrors(Exception ex)
    {
        var messages = new List<string>();

        if (ex is ValidationException vEx)
        {
            messages.AddRange(vEx.Errors.Select(x => x.ErrorMessage).Distinct());
            return messages.ToArray();
        }
        
        messages.Add(ex.Message);
        var inner = ex.InnerException;
        var count = 5;
        while (inner is not null && count-- > 0)
        {
            messages.Add(inner.Message);
            inner = inner.InnerException;
        }

        return messages.Distinct().ToArray();
    }
}