using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Revisio.API.Middlewares;
using Revisio.Application.Behaviors;
using Revisio.Application.Common.Interfaces;
using Revisio.Domain.Entities;
using Revisio.Infrastructure.Data;
using Revisio.Infrastructure.Services;
using Revisio.Infrastructure.Settings;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.Configure<JwtSetting>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSetting"));
builder.Services.Configure<AppConfig>(builder.Configuration.GetSection("AppUrl"));
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
//Add mediatR
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Revisio.Application.IAssemblyMarker).Assembly));
//Add fluent validation
builder.Services.AddValidatorsFromAssembly(typeof(Revisio.Application.IAssemblyMarker).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(builder.Configuration["cs"]));
builder.Services.AddScoped<IAppDbContext>(x => x.GetRequiredService<AppDbContext>());
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option =>
{
    option.Password.RequiredLength = 6;
    option.Password.RequireDigit = true;
    option.Password.RequireUppercase = true;
    option.Password.RequiredUniqueChars = 1;
    option.SignIn.RequireConfirmedEmail = true;
}).AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
