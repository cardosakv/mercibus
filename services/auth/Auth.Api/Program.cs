using System.Net;
using System.Net.Mail;
using System.Text;
using Auth.Api.Filters;
using Auth.Api.Middlewares;
using Auth.Application.Interfaces;
using Auth.Application.Services;
using Auth.Application.Validators;
using Auth.Domain.Entities;
using Auth.Infrastructure;
using Auth.Infrastructure.Repositories;
using Auth.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add logging.
    builder.Services.AddTransient<LoggingMiddleware>();

    // Add services to the container.
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ITokenService, JwtTokenService>();
    builder.Services.AddScoped<IEmailService, EmailService>();
    builder.Services.AddScoped<ITransactionService, TransactionService>();
    builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

    // Add validators.
    builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
    builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
    builder.Services.AddValidatorsFromAssemblyContaining<SendConfirmationEmailRequestValidator>();
    builder.Services.AddValidatorsFromAssemblyContaining<ForgotPasswordRequestValidator>();
    builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserInfoRequestValidator>();
    builder.Services.AddFluentValidationAutoValidation(config =>
        config.OverrideDefaultResultFactoryWith<ValidationResultFactory>());

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

    // Add swagger.
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc(name: "Auth API", new OpenApiInfo { Title = "Auth API", Version = "v1" });

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description = "JWT Authorization header using the Bearer scheme.",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new List<string>()
            }
        });
    });

    builder.Services.AddAuthorization();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddHttpContextAccessor();

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.UseMiddleware<LoggingMiddleware>();

    await app.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}