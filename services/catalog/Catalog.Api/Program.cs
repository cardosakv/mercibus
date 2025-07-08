using Catalog.Application.Interfaces;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using Catalog.Application.Mappers;
using Catalog.Application.Services;
using Catalog.Infrastructure;
using Catalog.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services.
    builder.Services.AddScoped<IProductService, ProductService>();

    // Add repositories.
    builder.Services.AddScoped<IProductRepository, ProductRepository>();

    // Add auto mapper.
    builder.Services.AddAutoMapper(config => config.AddProfile<MappingProfile>());

    // Add database context.
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddScoped<IAppDbContext>(provider => 
        provider.GetRequiredService<AppDbContext>());

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseAuthorization();
    app.MapControllers();

    await app.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}