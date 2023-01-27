using WkHtmlToPdfDotNet.Contracts;
using WkHtmlToPdfDotNet;
using FluentValidation;
using PdfApp.Contracts.Request;
using Ganss.Xss;
using PdfApp.Application.Services;
using PdfApp.Application.Abstractions.Application;
using PdfApp.Application.Validators;
using Microsoft.AspNetCore.Authorization;
using PdfApp.Infrastructure.Identity.Constants;
using PdfApp.Infrastructure.Identity.Handlers;
using PdfApp.Infrastructure.Identity.Options;
using PdfApp.Infrastructure.Identity.Requirements;
using PdfApp.Infrastructure.Configurations;

namespace PdfApp.Rest.ServiceCollection
{
    public static class ServiceCollectionExtension
    {
        public static void RegisterServices(this IServiceCollection services, Configuration configuration)
        {
            #region Integration Services
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddScoped<IHtmlSanitizer>(x =>
            {
                var htmlSanitizer = new HtmlSanitizer();

                if (configuration.SanitizerOptions.AllowedHtmlTags.Any())
                {
                    htmlSanitizer.AllowedTags.UnionWith(configuration.SanitizerOptions.AllowedHtmlTags);
                }

                if (configuration.SanitizerOptions.AllowedHtmlAttributes.Any())
                {
                    htmlSanitizer.AllowedAttributes.UnionWith(configuration.SanitizerOptions.AllowedHtmlAttributes);
                }

                return htmlSanitizer;
            });
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
