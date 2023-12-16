using Serilog;
using Serilog.Context;

namespace TemplatesApi.EndpointFilters;

public class CorrelationIdLoggingFilter : IEndpointFilter
{
    private readonly ILogger<CorrelationIdLoggingFilter> _logger;
    private const string CorrelationIdHeader = "X-Correlation-Id";

    public CorrelationIdLoggingFilter(ILogger<CorrelationIdLoggingFilter> logger)
    {
        _logger = logger;
    }
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var correlationId = GetCorrelationId(context.HttpContext);
        _logger.LogInformation("CorrelationId: {CorrelationId}", correlationId);
        Log.Logger.Information("Serilog CorrelationId: {CorrelationId}", correlationId);

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            return await next(context);
        }
    }

    private string GetCorrelationId(HttpContext httpContext)
    {
        httpContext.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationId);
        
        return correlationId.FirstOrDefault() ?? Guid.NewGuid().ToString();
    }
}