using System.Net;
using System.Net.Mail;
using System.Text;
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

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ITokenService, JwtTokenService>();
    builder.Services.AddScoped<IEmailService, EmailService>();
    builder.Services.AddScoped<ITransactionService, TransactionService>();
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
        .AddSmtpSender(new SmtpClient(builder.Configuration["Email:Server"])
        {
            EnableSsl = true,
            Port = Convert.ToInt32(builder.Configuration["Email:Port"]),
            Credentials = new NetworkCredential(builder.Configuration["Email:Username"],
                builder.Configuration["Email:Password"])
        });

    // Add authentication.
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
            };
        });

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
    }

    app.UseExceptionMiddleware();
    app.UseLoggingMiddleware();
    app.UseCustomAuthMiddleware();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    await app.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}