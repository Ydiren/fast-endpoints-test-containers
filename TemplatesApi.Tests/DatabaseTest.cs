using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using TemplatesApi.Templates.Creation;
using TemplatesApi.Templates.Retrieval;
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
    public async Task TemplateIsCreated()
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
    
    [Fact]
    public async Task TemplateIsRetrieved()
    {
        var (response, result) = await _fixture.Client.POSTAsync<CreateTemplateEndpoint, CreateTemplateRequest, CreateTemplateResponse>(
            new CreateTemplateRequest
            {
                Name = "Test"
            });
        response.IsSuccessStatusCode.Should().BeTrue();
        result.Id.Should().NotBeEmpty();
        
        var (getResponse, getResult) = await _fixture.Client.GETAsync<GetTemplateEndpoint, GetTemplateRequest, GetTemplateResponse>(
            new GetTemplateRequest
            {
                Id = result.Id
            });
        getResponse.IsSuccessStatusCode.Should().BeTrue();
        getResult.Id.Should().Be(result.Id);
        getResult.Name.Should().Be("Test");
        getResult.CreatedAt.Should().NotBe(DateTimeOffset.UnixEpoch);
    }
    
    [Fact]
    public async Task TemplateIsRetrievedWithAll()
    {
        var templateCount = 5;
        await AddTemplates(templateCount);
        
        var (getResponse, getResult) = await _fixture.Client.GETAsync<GetAllTemplatesEndpoint, GetAllTemplatesResponse>();
        getResponse.IsSuccessStatusCode.Should().BeTrue();
        getResult.Templates.Should().HaveCount(templateCount);
        getResult.Templates.First().Id.Should().NotBeEmpty();
        getResult.Templates.First().Name.Should().NotBeNullOrWhiteSpace();
        getResult.Templates.First().CreatedAt.Should().NotBeOnOrBefore(DateTimeOffset.UnixEpoch);
    }

    private async Task AddTemplates(int templateCount)
    {
        for (var i = 0; i < templateCount; i++)
        {
            var (response, result) = await _fixture.Client.POSTAsync<CreateTemplateEndpoint, CreateTemplateRequest, CreateTemplateResponse>(
                new CreateTemplateRequest
                {
                    Name = _fixture.Fake.Name.FullName()
                });
            response.IsSuccessStatusCode.Should().BeTrue();
        }
    }
}