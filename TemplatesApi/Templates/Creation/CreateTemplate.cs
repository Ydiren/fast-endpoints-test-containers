using FastEndpoints;
using TemplatesApi.Data;
using TemplatesApi.Data.Entities;

namespace TemplatesApi.Templates.Creation;

public class CreateTemplateRequest
{
    public string Name { get; set; } = default!;
}

public record CreateTemplateResponse(Guid Id);

internal class CreateTemplateEndpoint : Endpoint<CreateTemplateRequest, CreateTemplateResponse, CreateTemplateMapper>
{
    private readonly TemplatesDbContext _dbContext;

    public CreateTemplateEndpoint(TemplatesDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public override void Configure()
    {
        Post("/api/templates");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateTemplateRequest req, CancellationToken ct)
    {
        var entity = Map.ToEntity(req);
        
        _dbContext.Templates.Add(entity);
        await _dbContext.SaveChangesAsync(ct);
        
        await SendAsync(Map.FromEntity(entity));
    }
}

internal sealed class CreateTemplateMapper : Mapper<CreateTemplateRequest, CreateTemplateResponse, Template>
{
    public override Template ToEntity(CreateTemplateRequest r) => new()
    {
        Name = r.Name
    };

    public override CreateTemplateResponse FromEntity(Template e) => new(
        e.Id
    );
}