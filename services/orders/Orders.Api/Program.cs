using FluentValidation;
using Mapster;
using Mercibus.Common.Middlewares;
using Mercibus.Common.Validations;
using Orders.Api.Extensions;
using Orders.Api.Hubs;
using Orders.Application.Common;
using Orders.Application.Interfaces.Messaging;
using Orders.Application.Interfaces.Repositories;
using Orders.Application.Interfaces.Services;
using Orders.Application.Mappings;
using Orders.Application.Services;
using Orders.Application.Validations;
using Orders.Infrastructure.Messaging;
using Orders.Infrastructure.Repositories;
using Orders.Infrastructure.Services;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductReadService, ProductReadService>();
builder.Services.AddScoped<IEventPublisher, MassTransitEventPublisher>();
builder.Services.AddScoped<IOrderNotifier, OrderNotifier>();
builder.Services.AddDatabase(builder.Configuration);

// Add validation.
builder.Services.AddValidatorsFromAssemblyContaining<OrderRequestValidator>();
builder.Services.AddFluentValidationAutoValidation(options => options.OverrideDefaultResultFactoryWith<ValidationResultFactory>());

// Add mapping.
builder.Services.AddMapster();
OrderMapping.Configure();

// Add messaging.
builder.Services.AddMessaging(builder.Configuration);

builder.Services.AddJwtAuthentication(builder);
builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.MapHealthChecks("/health");
app.UseCustomAuthMiddleware();
app.UseAuthorization();
app.MapControllers();
app.MapHub<OrderHub>(Constants.Hub.OrderHub);

await app.RunAsync();

public partial class Program
{
}