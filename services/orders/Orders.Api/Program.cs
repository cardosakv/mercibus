using Mapster;
using Orders.Api.Extensions;
using Orders.Application.Interfaces.Repositories;
using Orders.Application.Interfaces.Services;
using Orders.Application.Services;
using Orders.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddMapster();

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

app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
