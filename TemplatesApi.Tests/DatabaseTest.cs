using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using TemplatesApi.Templates.Creation;
using Xunit.Abstractions;

namespace TemplatesApi.Tests;

public class DatabaseTest : TestClass<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public DatabaseTest(DatabaseFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Test1()
    {
        var (response, result) = await _fixture.Client.POSTAsync<CreateTemplateEndpoint, CreateTemplateRequest, CreateTemplateResponse>(
            new CreateTemplateRequest
            {
                Name = "Test"
            });
        response.IsSuccessStatusCode.Should().BeTrue();
        result.Id.Should().NotBeEmpty();
        
        _fixture.DbContext.Templates.Should().HaveCount(1);
        _fixture.DbContext.Templates.First().Name.Should().Be("Test");
    }
}