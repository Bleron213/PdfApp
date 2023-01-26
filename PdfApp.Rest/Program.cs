using FluentValidation;
using FluentValidation.Results;
using PdfApp.Application.Abstractions.Application;
using PdfApp.Application.Errors;
using PdfApp.Application.Models;
using PdfApp.Contracts.Request;
using PdfApp.Contracts.Response;
using PdfApp.Infrastructure.Extensions;
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

    app.MapPost("/", async (HttpRequest request, PdfInput input,
        IValidator<PdfInput> validator,
        ILoggerFactory loggerFactory,
        IHtmlToPdfConvertService converterService
        ) =>
    {
        var logger = loggerFactory.CreateLogger("home");

        ValidationResult validationResult = validator.Validate(input);
        if (!validationResult.IsValid)
        {
            throw new RequestValidationError(validationResult.Errors);
        }

        var pdfByteArray = converterService.ConvertToPdf(input);

        return Results.Ok(new Response<PdfOutput>
        {
            Data = new PdfOutput(Convert.ToBase64String(pdfByteArray), pdfByteArray.Length)
        });
    });

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


