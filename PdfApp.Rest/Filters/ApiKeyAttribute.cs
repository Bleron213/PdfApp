using Microsoft.AspNetCore.Mvc.Filters;
using PdfApp.Application.Errors;

namespace PdfApp.Rest.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ApiKeyAttribute : Attribute, IAuthorizationFilter
    {
        public const string API_KEY_HEADER = "X-API-KEY";
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = config.GetValue<string>($"ApiKey");

            if (string.IsNullOrEmpty(apiKey))
                throw new NotImplementedException("No Api Key was found. This route cannot be accessed");

            var submittedApikey = context.HttpContext.Request.Headers[API_KEY_HEADER];

            if (string.IsNullOrEmpty(submittedApikey))
                throw new NotAuthorizedException("Submitted key is invalid");

            if(submittedApikey != apiKey)
                throw new NotAuthorizedException("Submitted key is invalid");
        }
    }
}


