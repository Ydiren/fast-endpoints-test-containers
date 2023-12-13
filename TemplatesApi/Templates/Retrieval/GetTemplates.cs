using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TemplatesApi.Data;
using TemplatesApi.Data.Entities;

namespace TemplatesApi.Templates.Retrieval;

sealed class GetAllTemplatesEndpoint : EndpointWithoutRequest<GetAllTemplatesResponse, GetAllTemplatesMapper>
{
    private readonly TemplatesDbContext _dbContext;

    public GetAllTemplatesEndpoint(TemplatesDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public override void Configure()
    {
        Get("/api/templates");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        var entities = await _dbContext.Templates.ToListAsync(c);
        var response = new GetAllTemplatesResponse()
        {
            Templates = entities.Select(Map.FromEntity).ToList()
        };
        await SendOkAsync(response, c);    
    }
}

sealed class GetAllTemplatesResponse
{
    public List<TemplateSummary> Templates { get; set; } = default!;
}

sealed class TemplateSummary
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
}

sealed class GetAllTemplatesMapper : ResponseMapper<TemplateSummary, Template>
{
    public override TemplateSummary FromEntity(Template e) => new()
    {
        Id = e.Id,
        Name = e.Name,
        CreatedAt = e.CreatedAt,
        ModifiedAt = e.ModifiedAt
    };
}