using Amazon.S3;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Revisio.API.Middlewares;
using Revisio.API.Service;
using Revisio.Application.Behaviors;
using Revisio.Application.Common.Interfaces;
using Revisio.Domain.Entities;
using Revisio.Infrastructure.Consumers;
using Revisio.Infrastructure.Data;
using Revisio.Infrastructure.Services;
using Revisio.Infrastructure.Services.TextExtractor;
using Revisio.Infrastructure.Settings;
using Serilog;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(x=>x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
//Log.Logger = new LoggerConfiguration()
//    .ReadFrom.Configuration(builder.Configuration)
//    .CreateLogger();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.Configure<JwtSetting>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSetting"));
builder.Services.Configure<AppConfig>(builder.Configuration.GetSection("AppConfig"));
builder.Services.Configure<B2Setting>(builder.Configuration.GetSection("B2"));
builder.Services.AddScoped<IUploadToCloud, UploadToBackBlaze>();
builder.Services.AddScoped<IExamAIGenerator, ExamAIGenerator>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<ITopicPerformanceService, PerformanceService>();
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
builder.Services.AddGrpcClient<ExamAIService.ExamAIServiceClient>(o =>
{
    o.Address = new Uri(builder.Configuration["AIService:Address"]!);
});
//add rabbitMq
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<QuestionGenerationConsumer>();
    x.AddConsumer<UploadLectureConsumer>();
    x.AddEntityFrameworkOutbox<AppDbContext>(x =>
    {
        x.UseSqlServer();
        //  x.UseBusOutbox();
        x.QueryDelay = TimeSpan.FromSeconds(5);
    });
    x.UsingRabbitMq ((context, cfg) =>
    {
        cfg.Host(new Uri(builder.Configuration["RabbitMQ:CS"]));
        cfg.ConfigureEndpoints(context);
    });
});
//pdfExtractor
builder.Services.AddScoped<ITextExtractor, PdfExtractor>();
builder.Services.AddScoped<ITextExtractorFactory, TextExtractorFactory>();
var keysFolder = Path.Combine(builder.Environment.ContentRootPath, "App_Data", "DataProtectionKeys");
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
    .SetApplicationName("Revisio");

builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var b2Setting = sp.GetRequiredService<IOptions<B2Setting>>().Value;

    var s3Config = new AmazonS3Config
    {
        ServiceURL = $"https://{b2Setting.Endpoint}",
        ForcePathStyle = true
    };

    return new AmazonS3Client(b2Setting.AccessKey, b2Setting.SecretKey, s3Config);
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUser>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow",policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});
builder.Services.AddRateLimiter(options => {
    options.AddFixedWindowLimiter("fixed", w =>
    {
        w.QueueLimit = 0;
        w.Window = TimeSpan.FromMinutes(1);
        w.PermitLimit = 1;
    });
    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            success = false,
            message = "Too many requests. Please wait a moment before trying again."
        }, cancellationToken);
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("Allow");
app.UseRateLimiter();
app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllers();
app.Run();