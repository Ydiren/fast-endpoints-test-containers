using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using TemplatesApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddFastEndpoints()
    .SwaggerDocument();

builder.Services.AddDbContext<TemplatesDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("Templates")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}

app.UseHttpsRedirection();

app.UseFastEndpoints();

app.Run();

public partial class Program {}