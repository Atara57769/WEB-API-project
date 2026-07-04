using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace WebApiShop.Middleware
{
    public class ContentSecurityPolicyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _cspHeaderValue;

        public ContentSecurityPolicyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            var configValue = configuration["Security:ContentSecurityPolicy"];
            _cspHeaderValue = string.IsNullOrEmpty(configValue)
                ? "default-src 'self'; script-src 'self' 'unsafe-inline' 'unsafe-eval'; style-src 'self' 'unsafe-inline'; img-src 'self' data:; frame-ancestors 'none'; form-action 'self';"
                : configValue;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.Headers.Append("Content-Security-Policy", _cspHeaderValue);
            await _next(httpContext);
        }
    }

    public static class ContentSecurityPolicyExtensions
    {
        public static IApplicationBuilder UseContentSecurityPolicy(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ContentSecurityPolicyMiddleware>();
        }
    }
}
