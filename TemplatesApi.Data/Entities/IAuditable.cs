namespace TemplatesApi.Data.Entities;

public interface IAuditable
{
    DateTimeOffset CreatedAt { get; set; }
    DateTimeOffset? ModifiedAt { get; set; }
}