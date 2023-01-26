using WkHtmlToPdfDotNet.Contracts;
using WkHtmlToPdfDotNet;
using FluentValidation;
using PdfApp.Contracts.Request;
using PdfApp.Core.Validators;

namespace PdfApp.Rest.ServiceCollection
{
    public static class ServiceCollectionExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            #region Services
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            #endregion

            #region Validations
            services.AddScoped<IValidator<PdfInput>, PdfInputValidator>();
            #endregion
        }
    }
}
