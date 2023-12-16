using Serilog;
using Serilog.Context;

namespace TemplatesApi.EndpointFilters;

public class RequestLoggingFilter(ILogger<RequestLoggingFilter> logger) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var endpoint = context.HttpContext.GetEndpoint();
        var endpointName = endpoint?.DisplayName ?? string.Empty;
        
        var result = await next(context);
        
        if (context.HttpContext.Response.StatusCode < 400)
        {
            logger.LogInformation("Completed request {EndpointName} returned {StatusCode}", endpointName, context.HttpContext.Response.StatusCode);
        }
        else
        {
            using (LogContext.PushProperty("Error", context.HttpContext.Response.StatusCode))
            {
                logger.LogError("Request {EndpointName} returned {StatusCode}", endpointName, context.HttpContext.Response.StatusCode);
            }
        }

        return result;
    }
}