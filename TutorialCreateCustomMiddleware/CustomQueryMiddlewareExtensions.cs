using Microsoft.AspNetCore.Builder;

namespace CustomMiddleware
{
    /// <summary>
    /// An extension method to expose the middleware through IApplicationBuilder.
    /// </summary>
    public static class CustomQueryMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomQueryMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomQueryMiddleware>();
        }
    }
}
