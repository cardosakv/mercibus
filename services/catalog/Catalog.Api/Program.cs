using Catalog.Api.Extensions;
using Catalog.Application.Validations;
using FluentValidation;
using Mercibus.Common.Middlewares;
using Mercibus.Common.Validations;
using Prometheus;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices();
builder.Services.AddRepositories();
builder.Services.AddMapping();
builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddValidatorsFromAssembly(typeof(ProductRequestValidator).Assembly);
builder.Services.AddFluentValidationAutoValidation(options =>
    options.OverrideDefaultResultFactoryWith<ValidationResultFactory>());

builder.Services.AddJwtAuthentication(builder);
builder.Services.AddCaching(builder.Configuration);
builder.Services.AddMessaging(builder.Configuration);
builder.Services.AddHealthChecks();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseHttpMetrics();
app.MapMetrics("/metrics");
app.MapHealthChecks("/health");
app.UseExceptionMiddleware();
app.UseLoggingMiddleware();
app.UseCustomAuthMiddleware();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();

public partial class Program
{
}