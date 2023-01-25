using PdfApp.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var app = builder.Build();

app.Run();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.MapPost("/", (HttpRequest request, PdfInput? input) => {
    return Results.Ok(new PdfOutput(errorMessage: "To be implemented"));
});