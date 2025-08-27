using Catalog.Api.Extensions;
using Catalog.Application.Interfaces;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using Catalog.Application.Services;
using Catalog.Application.Validations;
using Catalog.Infrastructure;
using Catalog.Infrastructure.Repositories;
using Catalog.Infrastructure.Services;
using FluentValidation;
using Mercibus.Common.Middlewares;
using Mercibus.Common.Validations;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using StackExchange.Redis;
using static Npgsql.NpgsqlConnection;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services.
    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddScoped<ICategoryService, CategoryService>();
    builder.Services.AddScoped<IBrandService, BrandService>();
    builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
    builder.Services.AddScoped<IProductImageService, ProductImageService>();
    builder.Services.AddScoped<IProductReviewService, ProductReviewService>();
    builder.Services.AddScoped<ICacheService, RedisCacheService>();

    // Add repositories.
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
    builder.Services.AddScoped<IBrandRepository, BrandRepository>();
    builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
    builder.Services.AddScoped<IProductReviewRepository, ProductReviewRepository>();

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
    GlobalTypeMapper.EnableDynamicJson();
    
    // Add authentication.
    builder.Services.AddAuthentication()
        .AddJwtBearer(options =>
        {
            options.Authority = builder.Configuration["Jwt:Authority"];
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["Jwt:Audience"],
                ValidateLifetime = true
            };
        });
        

    // Add caching.
    builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    {
        var connectionString = builder.Configuration.GetConnectionString("RedisConnection");
        return ConnectionMultiplexer.Connect(connectionString ?? string.Empty);
    });

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
    app.UseCustomAuthMiddleware();
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