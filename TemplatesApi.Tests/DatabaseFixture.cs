using FastEndpoints.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TemplatesApi.Data;
using TemplatesApi.Tests.Extensions;
using Testcontainers.PostgreSql;
using Xunit.Abstractions;

namespace TemplatesApi.Tests;

public class DatabaseFixture : TestFixture<Program>
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("templates")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithCleanUp(true)
        .Build();

    public DatabaseFixture(IMessageSink s)
        : base(s)
    {
    }

    public TemplatesDbContext DbContext => Services.GetRequiredService<TemplatesDbContext>();
    
    protected override Task SetupAsync()
    {
        return Task.CompletedTask;
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        _container.StartAsync().Wait();
        
        // Remove existing DB context
        services.RemoveDbContext<TemplatesDbContext>();

        // Add DB context pointing to the test container
        services.AddDbContext<TemplatesDbContext>(options => 
            options.UseNpgsql(_container.GetConnectionString()));

        // Ensure schema gets created
        services.EnsureDbCreated<TemplatesDbContext>();
    }

    protected override Task TearDownAsync()
    {
        return _container.DisposeAsync().AsTask();
    }
}