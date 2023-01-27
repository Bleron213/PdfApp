using FluentValidation;
using FluentValidation.Results;
using PdfApp.Application.Abstractions.Application;
using PdfApp.Application.Errors;
using PdfApp.Application.Models;
using PdfApp.Contracts.Request;
using PdfApp.Contracts.Response;
using PdfApp.Infrastructure.Identity.Constants;
using System.Net;

namespace PdfApp.Rest.Modules
{
    public static class PdfModule
    {
        public static void RegisterPdfModule(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/",
                (
                    HttpRequest request,
                    PdfInput input,
                    IValidator<PdfInput> validator,
                    ILoggerFactory loggerFactory,
                    IHtmlToPdfConvertService converterService
                ) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(PdfModule));

                ValidationResult validationResult = validator.Validate(input);
                if (!validationResult.IsValid)
                {
                    throw new RequestValidationError(validationResult.Errors);
                }

                var pdfByteArray = converterService.ConvertToPdf(input);

                return Task.FromResult(Results.Ok(new Response<PdfOutput>
                {
                    Data = new PdfOutput(Convert.ToBase64String(pdfByteArray), pdfByteArray.Length),
                    Succeeded = true,
                    StatusCode = HttpStatusCode.OK
                }));
            }).RequireAuthorization(PolicyConstants.HeaderXApiKeySchemePolicy);
        }
    }
}
