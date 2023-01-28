using Microsoft.AspNetCore.Authorization;
using PdfApp.Infrastructure.Configurations;
using PdfApp.Infrastructure.Extensions;
using PdfApp.Infrastructure.Identity.Constants;
using PdfApp.Infrastructure.Identity.Handlers;
using PdfApp.Infrastructure.Identity.Options;
using PdfApp.Infrastructure.Identity.Requirements;
using PdfApp.Rest.Modules;
using PdfApp.Rest.ServiceCollection;
using Serilog;
using MvcJsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));

    var configuration = new Configuration();
    builder.Configuration.Bind("Configurations", configuration);
    builder.Services.AddSingleton(configuration);
   
    // Need to inject both for this to work
    builder.Services.Configure<JsonOptions>(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
    builder.Services.Configure<MvcJsonOptions>(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    builder.Services.AddSingleton<IAuthorizationHandler, XKeyHeaderRequirementHandler>();

    builder.Services
        .AddAuthentication()
        .AddScheme<ApiKeySchemeOptions, ApiKeySchemeAuthenticationHandler>(SchemeConstants.XKeyHeaderSchemeName, options => { });

    builder.Services
        .AddAuthorization(options =>
        {
            var headerXApiKeySchemePolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(SchemeConstants.XKeyHeaderSchemeName)
                .RequireAuthenticatedUser()
                .Build();

            options.AddPolicy(PolicyConstants.HeaderXApiKeySchemePolicy, headerXApiKeySchemePolicy);
        });

    builder.Services.RegisterServices(configuration);

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.ConfigureCustomExceptionMiddleware();

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    // ENDPOINTS

    app.RegisterPdfModule();

    app.MapGet("/hello", () => "Hello world");

    // --- ///
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception while bootstrapping application");
}
finally
{
    Log.Information("Shutting down...");
    Log.CloseAndFlush();
}


public partial class Program { }