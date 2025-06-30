using Auth.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure;

/// <summary>
/// Represents the application database context for identity management.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity properties 
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.Name);
            entity.Property(u => u.Street);
            entity.Property(u => u.City);
            entity.Property(u => u.State);
            entity.Property(u => u.Country);
            entity.Property(u => u.PostalCode);
            entity.Property(u => u.CreatedAt);
        });

        // Rename default identity tables
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<IdentityRole>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

        // Seed roles
        var roles = new List<IdentityRole>
        {
            new()
            {
                Id = "1",
                Name = Domain.Common.Roles.Guest,
                NormalizedName = Domain.Common.Roles.Guest.ToUpper()
            },
            new()
            {
                Id = "2",
                Name = Domain.Common.Roles.Customer,
                NormalizedName = Domain.Common.Roles.Customer.ToUpper()
            },
            new()
            {
                Id = "3",
                Name = Domain.Common.Roles.Admin,
                NormalizedName = Domain.Common.Roles.Admin.ToUpper()
            }
        };

        modelBuilder.Entity<IdentityRole>().HasData(roles);

        // Configure Refresh Token entity properties
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.Property(r => r.Id).ValueGeneratedOnAdd();
            entity.Property(r => r.TokenHash);
            entity.Property(r => r.UserId);
            entity.Property(r => r.ExpiresAt);
            entity.Property(r => r.IsRevoked).HasDefaultValue(false);

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}