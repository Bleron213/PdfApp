using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PdfApp.Application.Abstractions.Application;
using PdfApp.Application.Errors;
using PdfApp.Application.Models;
using PdfApp.Contracts.Request;
using PdfApp.Contracts.Response;
using PdfApp.Infrastructure.Identity.Constants;
using System.Net;
using Swashbuckle.AspNetCore.Annotations;

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
                logger.LogInformation("Entered {method}. POST: Params {input}", nameof(RegisterPdfModule), JsonConvert.SerializeObject(input));

                ValidationResult validationResult = validator.Validate(input);
                if (!validationResult.IsValid)
                    throw new RequestValidationError(validationResult.Errors);
                
                var pdfByteArray = converterService.ConvertToPdf(input);

                logger.LogInformation("Exiting {method}. POST", nameof(RegisterPdfModule));

                return Results.Ok(new Response<PdfOutput>
                {
                    Data = new PdfOutput(Convert.ToBase64String(pdfByteArray), pdfByteArray.Length),
                    Succeeded = true,
                    StatusCode = (int)HttpStatusCode.OK
                });
            })
                .WithMetadata(new SwaggerOperationAttribute("This endpoint will convert any base64 encoded HTML string to base64 encoded PDF bytearray and return it back to the caller",""))
                .WithMetadata(new ProducesResponseTypeAttribute(typeof(Response<PdfOutput>), 200))
                .WithMetadata(new ProducesResponseTypeAttribute(typeof(Response<string>), 400))
                .WithMetadata(new ProducesResponseTypeAttribute(typeof(Response<string>), 401))
                .WithMetadata(new ProducesResponseTypeAttribute(typeof(Response<string>), 500))
                .RequireAuthorization(PolicyConstants.HeaderXApiKeySchemePolicy);

        }
    }
}