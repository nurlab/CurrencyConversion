using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using CC.Application;
using CC.Application.Configrations;
using CC.Presentation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

//services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlite($"Data Source={Path.Combine(Directory.GetCurrentDirectory(), "AppDb.db")}"));

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
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new X509SecurityKey(cert)
    };
});

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    // Register modules from each layer
    containerBuilder.RegisterModule(new ApplicationModule());
    containerBuilder.RegisterAssemblyModules(Assembly.Load("CC.Infrastructure"));
    containerBuilder.RegisterModule(new PresentationModule());
});

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new ApplicationMappingProfile());
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
