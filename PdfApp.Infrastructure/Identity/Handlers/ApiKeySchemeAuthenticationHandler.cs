using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PdfApp.Application.Errors;
using PdfApp.Infrastructure.Identity.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace PdfApp.Infrastructure.Identity.Handlers
{
    public class ApiKeySchemeAuthenticationHandler : AuthenticationHandler<ApiKeySchemeOptions>
    {
        public const string API_KEY_HEADER = "X-API-KEY";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiKeySchemeAuthenticationHandler(
            IHttpContextAccessor httpContextAccessor,
            IOptionsMonitor<ApiKeySchemeOptions> options,
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Read the Header
            var config = _httpContextAccessor.HttpContext.RequestServices.GetService<IConfiguration>();
            var apiKey = config.GetValue<string>(API_KEY_HEADER);

            if(string.IsNullOrEmpty(apiKey))
                throw new NotImplementedException("No Api Key was found. This route cannot be accessed");

            var retrievedHeaderValue = Request.Headers.TryGetValue(API_KEY_HEADER, out var submittedApiKey);
            if(!retrievedHeaderValue)
                throw new NotAuthorizedException("Submitted key is invalid");

            if (string.IsNullOrEmpty(submittedApiKey))
                throw new NotAuthorizedException("Submitted key is invalid");

            if (submittedApiKey != apiKey)
                throw new NotAuthorizedException("Submitted key is invalid");

            var claims = new Claim[] { };
            var identity = new ClaimsIdentity(claims, "XApiKeyAuthentication");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            AuthenticationTicket ticket = new(claimsPrincipal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
