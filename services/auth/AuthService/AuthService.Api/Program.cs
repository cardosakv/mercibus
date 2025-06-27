using Auth.Application.Interfaces;
using Auth.Application.Services;
using Auth.Application.Validators;
using Auth.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IAuthService, AuthService>();

// Add validators.
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
builder.Services.AddFluentValidationAutoValidation(config => config.OverrideDefaultResultFactoryWith<ValidationResultFactory>());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
