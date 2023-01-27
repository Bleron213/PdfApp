using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Infrastructure.Identity.Requirements
{
    public class XKeyHeaderRequirement : IAuthorizationRequirement
    {
        public const string API_KEY_HEADER = "X-API-KEY";
    }
    public class XKeyHeaderRequirementHandler : AuthorizationHandler<XKeyHeaderRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public XKeyHeaderRequirementHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, XKeyHeaderRequirement requirement)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
