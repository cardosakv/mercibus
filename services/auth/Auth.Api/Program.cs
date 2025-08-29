using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using Auth.Api.Extensions;
using Auth.Application.Interfaces.Repositories;
using Auth.Application.Interfaces.Services;
using Auth.Application.Mappings;
using Auth.Application.Services;
using Auth.Application.Validators;
using Auth.Domain.Entities;
using Auth.Infrastructure;
using Auth.Infrastructure.Repositories;
using Auth.Infrastructure.Services;
using FluentValidation;
using Mapster;
using Mercibus.Common.Middlewares;
using Mercibus.Common.Validations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

// Add validators.
builder.Services.AddValidatorsFromAssembly(typeof(RegisterRequestValidator).Assembly);
builder.Services.AddFluentValidationAutoValidation(config =>
    config.OverrideDefaultResultFactoryWith<ValidationResultFactory>());

// Add mapping.
builder.Services.AddMapster();
MappingConfig.Configure();

// Add identity services.
builder.Services.AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddTokenProvider<DataProtectorTokenProvider<User>>("Default")
    .AddDefaultTokenProviders();

// Add database context.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add email provider.
builder.Services.AddFluentEmail(builder.Configuration["Email:Sender"])
    .AddRazorRenderer()
    .AddSmtpSender(
        new SmtpClient(builder.Configuration["Email:Server"])
        {
            EnableSsl = true,
            Port = Convert.ToInt32(builder.Configuration["Email:Port"]),
            Credentials = new NetworkCredential(
                builder.Configuration["Email:Username"],
                builder.Configuration["Email:Password"])
        });

// Add authentication.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var rsa = RSA.Create();
        rsa.ImportFromPem(File.ReadAllText(builder.Configuration["Jwt:PublicKeyPath"] ?? "jwt_priv_key.pem"));

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new RsaSecurityKey(rsa)
        };
    });

// Add health check.
builder.Services.AddCustomHealthChecks(builder);

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.MapCustomHealthChecks();
app.UseExceptionMiddleware();
app.UseLoggingMiddleware();
app.UseCustomAuthMiddleware();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();

public partial class Program
{
}