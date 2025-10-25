using Mapster;
using Payments.Api.Extensions;
using Payments.Application.Interfaces.Repositories;
using Payments.Application.Interfaces.Services;
using Payments.Application.Mappings;
using Payments.Application.Services;
using Payments.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddDatabase(builder.Configuration);

// Add mapping.
builder.Services.AddMapster();
PaymentMapping.Configure();

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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();