using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Attribute = System.Attribute;

namespace Catalog.Infrastructure;

/// <summary>
/// Represents the application database context for catalog management.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Attribute> Attributes { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<ProductReview> Reviews { get; set; }
}