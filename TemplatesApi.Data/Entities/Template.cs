namespace TemplatesApi.Data.Entities;

public class Template : IAuditable
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
}