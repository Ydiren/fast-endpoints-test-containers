using FastEndpoints;
using FluentValidation;
using TemplatesApi.Data;
using TemplatesApi.Data.Entities;

namespace TemplatesApi.Templates.Retrieval;

sealed class GetTemplateEndpoint : Endpoint<GetTemplateRequest, GetTemplateResponse, GetTemplateMapper>
{
    private readonly TemplatesDbContext _dbContext;

    public GetTemplateEndpoint(TemplatesDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public override void Configure()
    {
        Get("route-pattern");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetTemplateRequest r, CancellationToken c)
    {
        var entity = await _dbContext.Templates.FindAsync(r.Id);
        if (entity == null)
        {
            await SendNotFoundAsync(c);
            return;
        }

        await SendOkAsync(Map.FromEntity(entity), c);
    }
}

sealed class GetTemplateRequest
{
    public Guid Id { get; set; }
}

sealed class GetTemplateResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
}

sealed class GetTemplateValidator : Validator<GetTemplateRequest>
{
    public GetTemplateValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

sealed class GetTemplateMapper : ResponseMapper<GetTemplateResponse, Template>
{
    public override GetTemplateResponse FromEntity(Template e) => new()
    {
        Id = e.Id,
        Name = e.Name,
        CreatedAt = e.CreatedAt,
        ModifiedAt = e.ModifiedAt
    };
}