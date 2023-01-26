using WkHtmlToPdfDotNet.Contracts;
using WkHtmlToPdfDotNet;
using FluentValidation;
using PdfApp.Contracts.Request;
using Ganss.Xss;
using PdfApp.Application.Services;
using PdfApp.Application.Abstractions.Application;
using PdfApp.Application.Validators;

namespace PdfApp.Rest.ServiceCollection
{
    public static class ServiceCollectionExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            #region Integration Services
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddScoped<IHtmlSanitizer>(x => new HtmlSanitizer());
            #endregion

            #region Services
            services.AddScoped<IHtmlToPdfConvertService, HtmlToPdfConvertService>();
            #endregion

            #region Validations
            services.AddScoped<IValidator<PdfInput>, PdfInputValidator>();
            #endregion
        }
    }
}
