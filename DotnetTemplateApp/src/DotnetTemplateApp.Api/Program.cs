using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Azure.Identity;
using DotnetTemplateApp.Api.Cors;
using DotnetTemplateApp.Api.Filters;
using DotnetTemplateApp.Api.Middleware;
using DotnetTemplateApp.Api.RateLimiting;
using DotnetTemplateApp.Api.Security.Attributes;
using DotnetTemplateApp.Api.Security.Jwt;
using DotnetTemplateApp.Api.Security.Jwt.Interfaces;
using DotnetTemplateApp.Api.Utils.Extensions;
using DotnetTemplateApp.Core;
using DotnetTemplateApp.Core.Services.Account.MapperProfiles;
using DotnetTemplateApp.Shared;
using DotnetTemplateApp.Shared.Converters;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Core;
using Serilog.Events;

[assembly: ExcludeFromCodeCoverage]

Log.Logger = new LoggerConfiguration()
.WriteTo.Console()
.CreateBootstrapLogger();

var loggingLevelSwitch = new LoggingLevelSwitch(LogEventLevel.Information);
var builder = WebApplication.CreateBuilder(args);

builder.WebHost
    .CaptureStartupErrors(true)
        .UseSetting(WebHostDefaults.DetailedErrorsKey, true.ToString());
builder.Host
    .UseSerilog((hostingContext, services, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(hostingContext.Configuration)
        .Enrich.FromLogContext()
        .MinimumLevel.ControlledBy(loggingLevelSwitch)
        .WriteTo.Logger(x => x
        .WriteTo.Console()
        .WriteTo.ApplicationInsights(
            services.GetRequiredService<TelemetryConfiguration>(),
            TelemetryConverter.Traces,
            levelSwitch: loggingLevelSwitch))
       );

ConfigureServices();

var app = builder.Build();

Configure();

await app.Services.ExecuteDatabaseMigrations();

await app.RunAsync();

#region Configure services
void ConfigureServices()
{
    builder.Configuration
        .SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddAzureKeyVault(
            new Uri(builder.Configuration.GetSection("KeyVaultOptions:Uri").Value!),
            new DefaultAzureCredential(new DefaultAzureCredentialOptions()
            {
                ExcludeVisualStudioCredential = true,
                ExcludeVisualStudioCodeCredential = true
            }))
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
        .AddEnvironmentVariables();

    builder.Services.AddApplicationInsightsTelemetry(opt =>
    {
        opt.EnableActiveTelemetryConfigurationSetup = true;
        opt.EnableAdaptiveSampling = false;
    });

    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    });
    builder.Services.AddHttpContextAccessor();
    
    builder.Services.AddApplicationCore(builder.Configuration, builder.Environment.EnvironmentName);
    builder.Services.AddShared(builder.Configuration);

    AddServices();
    AddValidators();
    AddRateLimitersDI();
    AddAutoMapperProfiles();

    builder.Services.AddScoped<ValidationFilter>();
    builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.Converters.Add(new GuidStringConverter());
        })
        .ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

    builder.Services.AddCors(cors =>
    {
        cors.AddPolicy(CorsPolicyNames.DevCors, options =>
        {
            options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithOrigins("http://localhost:5002");
        });
    });

    builder.Services.AddHealthChecks();
}

#endregion

#region  Configure app
void Configure()
{
    app.UseAppExceptionHandler();
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "v1");
        });
    }

    AddMiddleware();

    app.UseHttpsRedirection();
    app.UseHsts();
    app.UseCors(CorsPolicyNames.DevCors);
    app.UseCookiePolicy();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseRateLimiter();

    app.MapHealthChecks("/healthcheck").WithMetadata(new AllowAnonymousAttribute(full: true));

    app.MapControllers();
}

void AddServices()
{
    builder.Services.AddTransient<IJwtUtils, JwtUtils>();
}

void AddRateLimitersDI()
{
    builder.Services.AddRateLimiter(options => options
        .AddPolicy<string, DefaultRateLimiterPolicy>(DefaultRateLimiterPolicy.PolicyName)
        .AddPolicy<string, SecurityRateLimiterPolicy>(SecurityRateLimiterPolicy.PolicyName));
    builder.Services.AddTransient<DefaultRateLimiterPolicy>();
    builder.Services.AddTransient<SecurityRateLimiterPolicy>();
}

void AddValidators()
{
    builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssemblyContaining<Program>();
}

void AddAutoMapperProfiles()
{
    builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(UserProfile).Assembly));
}

void AddMiddleware()
{
    app.UseMiddleware<AccessMiddleware>();
    app.UseMiddleware<JwtMiddleware>();
    app.UseMiddleware<HttpSecurityMiddleware>();
}

#endregion