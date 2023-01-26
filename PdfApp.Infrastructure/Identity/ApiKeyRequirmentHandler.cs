using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PdfApp.Application.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Infrastructure.Identity
{
    public class ApiKeyRequirementHandler : AuthorizationHandler<ApiKeyRequirement>
    {
        public const string API_KEY_HEADER = "X-API-KEY";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiKeyRequirementHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiKeyRequirement requirement)
        {
            var config = _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = config.GetValue<string>($"ApiKey");

            if (string.IsNullOrEmpty(apiKey))
                throw new NotImplementedException("No Api Key was found. This route cannot be accessed");

            var submittedApikey = _httpContextAccessor.HttpContext.Request.Headers[API_KEY_HEADER];

            if (string.IsNullOrEmpty(submittedApikey))
                throw new NotAuthorizedException("Submitted key is invalid");

            if (submittedApikey != apiKey)
                throw new NotAuthorizedException("Submitted key is invalid");
        }
    }

    public class ApiKeyRequirement : IAuthorizationRequirement
    {
    }
}
