using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CustomMiddleware
{
    public class CustomQueryMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomQueryMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // STEP 1.
            // Add your middleware's logic here (BEFORE the next delegate/middleware in the pipeline).        
            // Note: You can perform changes to the "Response" but the values may be replaced
            //  from the next delegate/middleware or from the application code.
            // Note: We have access to the HttpContext, thus, to the following:
            // - context.User
            // - context.Session
            // - context.Request.Query
            // - context.Request.Header
            // - context.Request.Form
            // - etc.


            // For example, the following code read the culture from the URL and set it to the current request thread.
            // Reference: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-3.1#middleware-class
            var cultureQuery = context.Request.Query["culture"];

            if (string.IsNullOrWhiteSpace(cultureQuery))
            {
                throw new CultureNotFoundException("The query parameter 'culture' has no value.");
            }
            else
            {
                var culture = new CultureInfo(cultureQuery);
                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;
            }


            // STEP 2.
            // Call the next delegate/middleware in the pipeline
            await _next(context);


            // STEP 3. (if needed)
            // Add your middleware's logic here (AFTER the next delegate/middleware in the pipeline),
            // that doesn't write to the Response (e.g. logging).
        }
    }
}
