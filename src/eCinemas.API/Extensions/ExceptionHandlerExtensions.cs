namespace eCinemas.API.Extensions;

public static class ExceptionHandlerExtensions
{
    public static IApplicationBuilder UseDefaultExceptionHandler(this IApplicationBuilder app)
    {
        return app;
    }
}