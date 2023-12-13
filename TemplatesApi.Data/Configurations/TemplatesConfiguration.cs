using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplatesApi.Data.Entities;

namespace TemplatesApi.Data.Configurations;

public class TemplatesConfiguration : IEntityTypeConfiguration<Template>
{
    public void Configure(EntityTypeBuilder<Template> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property<string>(x => x.Name)
            .IsRequired();
    }
}