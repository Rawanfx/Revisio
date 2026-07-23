using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
builder.Services.Configure<AppConfig>(builder.Configuration.GetSection("AppConfig"));
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
JwtSetting jwtSetting = builder.Configuration.GetSection("Jwt").Get<JwtSetting>();
//jwt
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>{
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience=true,
        ValidAudience = jwtSetting.Audience,

        ValidateIssuer = true,
        ValidIssuer=jwtSetting.Issuer,

        ValidateIssuerSigningKey=true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSetting.Key))
    };
});

var keysFolder = Path.Combine(builder.Environment.ContentRootPath, "App_Data", "DataProtectionKeys");
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
    .SetApplicationName("Revisio");

builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow",policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseCors("Allow");
app.UseAuthorization();

app.MapControllers();

app.Run();
