using FluentValidation;
using Mapster;
using Mercibus.Common.Validations;
using Orders.Api.Extensions;
using Orders.Application.Interfaces.Repositories;
using Orders.Application.Interfaces.Services;
using Orders.Application.Mappings;
using Orders.Application.Services;
using Orders.Application.Validations;
using Orders.Infrastructure.Repositories;
using Orders.Infrastructure.Services;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductReadService, ProductReadService>();
builder.Services.AddDatabase(builder.Configuration);

// Add validation.
builder.Services.AddValidatorsFromAssemblyContaining<OrderRequestValidator>();
builder.Services.AddFluentValidationAutoValidation(options => options.OverrideDefaultResultFactoryWith<ValidationResultFactory>());

// Add mapping.
builder.Services.AddMapster();
OrderMapping.Configure();

// Add messaging.
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

app.MapHealthChecks("/health");
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
