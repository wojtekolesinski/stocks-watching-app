using Stocks.Server.Middlawares;

namespace Stocks.Server.Extensions
{

    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionLoggingMiddleware>();
        }

    }
}