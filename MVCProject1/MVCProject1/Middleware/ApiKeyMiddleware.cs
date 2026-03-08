using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
 
namespace MVCProject1.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string APIKEYNAME = "ApiKey";
        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
			// Inject configuration inside the Invoke method
			var config = context.RequestServices.GetRequiredService<IConfiguration>();
			var validKey = config["ApiKey"];

			if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey) || validKey != extractedApiKey)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized Client.");
                return;
            }
            await _next(context);
        }
    }
}