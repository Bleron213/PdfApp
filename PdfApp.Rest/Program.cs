using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using PdfApp.Infrastructure.Extensions;
using PdfApp.Infrastructure.Identity;
using PdfApp.Rest.Modules;
using PdfApp.Rest.ServiceCollection;
using Serilog;

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

    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddAuthorization(o =>
    {
        o.AddPolicy("ApiKeyPolicy", p => p.AddRequirements(new ApiKeyRequirement()));
    });

    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    builder.Services.AddSingleton<IAuthorizationHandler, ApiKeyRequirementHandler>();

    builder.Services.RegisterServices();

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

    app.RegisterPdfModule();

    app.MapGet("/security/getMessage", () => "Hello World!").RequireAuthorization("ApiKeyPolicy");

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


