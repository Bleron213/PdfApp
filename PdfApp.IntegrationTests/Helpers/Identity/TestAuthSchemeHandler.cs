using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace PdfApp.IntegrationTests.Helpers.Identity
{
    public class TestAuthSchemeHandler : AuthenticationHandler<TestAuthSchemeOptions>
    {
        public TestAuthSchemeHandler(
            IOptionsMonitor<TestAuthSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new List<Claim>();
            var identity = new ClaimsIdentity(claims, "Placeholder");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }

    public class TestAuthSchemeOptions : AuthenticationSchemeOptions { }
}
