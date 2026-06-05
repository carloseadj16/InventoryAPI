using FluentValidation;
using InventoryAPI.Application.Common;
using InventoryAPI.Application.Products.Commands;
using InventoryAPI.Extensions;
using InventoryAPI.Infraestructure.Extensions;
using InventoryAPI.Infraestructure.Persistence.Context;
using InventoryAPI.Middleware;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddIfraestructure(builder.Configuration);
builder.Services.AddHealthChecks().AddDbContextCheck<InventoryDbContext>("database");
builder.Services.AddValidatorsFromAssembly(typeof(CreateProductValidator).Assembly);
builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(typeof(CreateProductHandler).Assembly);
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(IdempotencyBehavior<,>));
    }
);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSwaggerWithAuth();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/healt").AllowAnonymous();
app.MapHealthChecks("/healt/detail", new HealthCheckOptions
{
    ResponseWriter = WriteDetailedResponse
}).AllowAnonymous();

app.Run();

static async Task WriteDetailedResponse(HttpContext context, HealthReport report)
{
    context.Response.ContentType = "application/json";
    var response = new
    {
        status = report.Status.ToString(),
        duration = report.TotalDuration.TotalMilliseconds,
        checks = report.Entries.Select(e => new
        {
            name = e.Key,
            status = e.Value.Status.ToString(),
            duration = e.Value.Duration.TotalMilliseconds,
            error = e.Value.Exception?.Message
        })
    };

    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
}