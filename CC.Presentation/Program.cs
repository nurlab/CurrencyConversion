using AspNetCoreRateLimit;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using CC.Application;
using CC.Application.Configrations;
using CC.Infrastructure;
using CC.Infrastructure.DatabaseContext;
using CC.Presentation;
using CC.Presentation.Helper;
using CC.Presentation.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Security.Cryptography.X509Certificates;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#region Swagger Configration
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Currency Conversion App",
        Version = "v1",
        Description = "Currency Conversion"
    });

    options.UseInlineDefinitionsForEnums(); // optional
    options.SchemaFilter<EnumSchemaFilter>();
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Put **_only_** your JWT Bearer token below.",

        Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition("Bearer", jwtSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            jwtSecurityScheme,
            Array.Empty<string>()
        }
    });
});
#endregion

#region App Settings configration
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Default settings
.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true) // Environment-specific settings
    .AddEnvironmentVariables()
    .AddCommandLine(args);
IConfiguration configuration = configurationBuilder.Build();
builder.Configuration.AddConfiguration(configuration);
builder.Services.Configure<ExchangeProviderSettings>(configuration.GetSection("ExchangeProviderSettings"));
builder.Services.Configure<SecuritySettings>(configuration.GetSection("SecuritySettings"));
#endregion

#region API throttling configration
builder.Services.AddInMemoryRateLimiting(); // Use in-memory storage for rate limits
builder.Services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
builder.Services.AddHttpContextAccessor();

#endregion

#region Caching configration

builder.Services.AddMemoryCache();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
#endregion

#region Database configration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={Path.Combine(Directory.GetCurrentDirectory(), "AppDb.db")}"));
#endregion

#region Auth configration

var cert = new X509Certificate2(
    builder.Configuration["SecuritySettings:CertificatePath"],
    builder.Configuration["SecuritySettings:CertificatePassword"]
);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "CurrencyConvetion",
        ValidAudience = "CurrencyConvetion",
        IssuerSigningKey = new X509SecurityKey(cert),
    };
});
#endregion
#region Mapper configration

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new ApplicationMappingProfile());
});
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    // Register modules from each layer
    containerBuilder.RegisterModule(new ApplicationModule());
    containerBuilder.RegisterModule(new InfrastructureModule());
    containerBuilder.RegisterModule(new PresentationModule());
    containerBuilder.RegisterInstance(mapperConfig.CreateMapper()).As<IMapper>().SingleInstance();
    containerBuilder.RegisterInstance(mapperConfig).As<AutoMapper.IConfigurationProvider>().SingleInstance();
});

IMapper mapper = mapperConfig.CreateMapper();
#endregion

#region Serilog Configuration

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.Seq(configuration.GetValue<string>("Serilog:WriteTo:1:Args:serverUrl"))
        .WriteTo.File(
            path: configuration.GetValue<string>("Serilog:WriteTo:2:Args:path"),
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 7)
    .CreateLogger();
builder.Host.UseSerilog((context, services, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));
#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseIpRateLimiting();

app.UseMiddleware<ThrottlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
