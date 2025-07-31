using Catalog.Api.Extensions;
using Catalog.Application.Interfaces;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using Catalog.Application.Services;
using Catalog.Application.Validations;
using Catalog.Infrastructure;
using Catalog.Infrastructure.Repositories;
using FluentValidation;
using Mercibus.Common.Middlewares;
using Mercibus.Common.Validations;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services.
    builder.Services.AddScoped<IProductService, ProductService>();

    // Add repositories.
    builder.Services.AddScoped<IProductRepository, ProductRepository>();

    // Add validators.
    builder.Services.AddValidatorsFromAssembly(typeof(ProductRequestValidator).Assembly);
    builder.Services.AddFluentValidationAutoValidation(options =>
        options.OverrideDefaultResultFactoryWith<ValidationResultFactory>());

    // Add mapping.
    builder.Services.AddMapping();

    // Add database context.
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddScoped<IAppDbContext>(provider =>
        provider.GetRequiredService<AppDbContext>());

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

    app.UseExceptionMiddleware();
    app.UseLoggingMiddleware();
    app.UseAuthorization();
    app.MapControllers();

    await app.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

public partial class Program
{
}