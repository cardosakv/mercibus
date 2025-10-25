using FluentValidation;
using Mapster;
using Mercibus.Common.Validations;
using Payments.Api.Extensions;
using Payments.Application.Interfaces.Repositories;
using Payments.Application.Interfaces.Services;
using Payments.Application.Mappings;
using Payments.Application.Services;
using Payments.Application.Validations;
using Payments.Infrastructure.Repositories;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddDatabase(builder.Configuration);

// Add validation.
builder.Services.AddValidatorsFromAssemblyContaining<PaymentRequestValidator>();
builder.Services.AddFluentValidationAutoValidation(options => options.OverrideDefaultResultFactoryWith<ValidationResultFactory>());

// Add mapping.
builder.Services.AddMapster();
PaymentMapping.Configure();

// Add messaging.
builder.Services.AddMessaging(builder.Configuration);

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