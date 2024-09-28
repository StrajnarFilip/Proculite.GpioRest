using System.Text;

namespace Proculite.GpioRest.Middlewares
{
    public class SecurityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SecurityMiddleware> _logger;

        public SecurityMiddleware(
            RequestDelegate next,
            IConfiguration configuration,
            ILogger<SecurityMiddleware> logger
        )
        {
            _logger = logger;
            _configuration = configuration;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            IConfigurationSection keySection = _configuration.GetSection("Key");
            if (!keySection.Exists())
            {
                _logger.LogWarning(
                    "Configuration does not contain a key. Running in insecure mode."
                );
                await _next(context);
                return;
            }

            string? accessKey = keySection.Get<string>();
            if (accessKey is null)
            {
                _logger.LogError("Key is expected but is not set.");
                return;
            }

            string? requestKey = context.Request.Headers.Authorization.FirstOrDefault();

            if (requestKey == accessKey)
            {
                await _next(context);
                return;
            }

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "text/plain; charset=utf-8";
            byte[] messageBytes = Encoding.UTF8.GetBytes("Invalid key is used. Access denied.");
            context.Response.ContentLength = messageBytes.Length;
            await context.Response.Body.WriteAsync(messageBytes);
        }
    }

    public static class SecurityMiddlewareExtensions
    {
        public static IApplicationBuilder UseSecurity(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SecurityMiddleware>();
        }
    }
}
