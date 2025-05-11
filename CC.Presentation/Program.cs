using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using CC.Application;
using CC.Application.Configrations;
using CC.Infrastructure.DatabaseContext;
using CC.Presentation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Conversion API", Version = "v1" });

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

    c.AddSecurityDefinition("Bearer", jwtSecurityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            jwtSecurityScheme,
            Array.Empty<string>()
        }
    });
});

builder.Services.AddMemoryCache();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={Path.Combine(Directory.GetCurrentDirectory(), "AppDb.db")}"));

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

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new ApplicationMappingProfile());
});
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    // Register modules from each layer
    containerBuilder.RegisterModule(new ApplicationModule());
    containerBuilder.RegisterAssemblyModules(Assembly.Load("CC.Infrastructure"));
    containerBuilder.RegisterModule(new PresentationModule());
    containerBuilder.RegisterInstance(mapperConfig.CreateMapper()).As<IMapper>().SingleInstance();
    containerBuilder.RegisterInstance(mapperConfig).As<AutoMapper.IConfigurationProvider>().SingleInstance();
});

IMapper mapper = mapperConfig.CreateMapper();

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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
