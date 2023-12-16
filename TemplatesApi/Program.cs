using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TemplatesApi.Data;
using TemplatesApi.EndpointFilters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog((context, loggerConfiguration) => 
    loggerConfiguration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.DocumentSettings = s =>
        {
            s.Title = "Templates API";
            s.Version = "v1";
        };
    });

builder.Services.AddDbContext<TemplatesDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("Templates")));

var app = builder.Build();

app.UseDefaultExceptionHandler()
    .UseFastEndpoints(config =>
{
    config.Endpoints.Configurator = endpoints =>
    {
        endpoints.Options(b =>
        {
            // Filters are executed in the order they are added!
            b.AddEndpointFilter<CorrelationIdLoggingFilter>();
            b.AddEndpointFilter<RequestLoggingFilter>();
        });
    };
});

app.UseSerilogRequestLogging();


// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}

app.UseHttpsRedirection();


app.Run();

// For the benefit of xUnit which doesn't allow anything but public classes
public partial class Program {}