using Catalog.Api.Filters;
using Catalog.Api.Middlewares;
using Catalog.Application.Interfaces;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using Catalog.Application.Mappers;
using Catalog.Application.Services;
using Catalog.Application.Validations;
using Catalog.Infrastructure;
using Catalog.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services.
    builder.Services.AddScoped<IProductService, ProductService>();

    // Add repositories.
    builder.Services.AddScoped<IProductRepository, ProductRepository>();

    // Add validators.
    builder.Services.AddFluentValidationAutoValidation(options =>
        options.OverrideDefaultResultFactoryWith<ValidationResultFactory>());
    builder.Services.AddValidatorsFromAssemblyContaining<AddProductRequestValidator>();
    builder.Services.AddValidatorsFromAssemblyContaining<UpdateProductRequestValidator>();

    // Add auto mapper.
    builder.Services.AddAutoMapper(config => config.AddProfile<MappingProfile>());

    // Add database context.
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddScoped<IAppDbContext>(provider =>
        provider.GetRequiredService<AppDbContext>());

    // Add custom middlewares.
    builder.Services.AddTransient<ExceptionMiddleware>();

    builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new StringEnumConverter()));
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.UseMiddleware<ExceptionMiddleware>();
    app.UseAuthorization();
    app.MapControllers();

    await app.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}