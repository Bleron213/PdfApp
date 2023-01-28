using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PdfApp.Application.Models;
using PdfApp.Contracts.Request;
using PdfApp.Contracts.Response;
using PdfApp.Infrastructure.Identity.Constants;
using PdfApp.IntegrationTests.Helpers.Factories;
using PdfApp.IntegrationTests.Helpers.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.IntegrationTests.Core.Builders
{
    public class PdfModuleBuilder 
    {
        private readonly PdfAppRestFactory _pdfAppRestFactory;
        public string CurrentUser { get; set; } = "PdfApp.IntegrationTests";
        public PdfModuleBuilder(PdfAppRestFactory pdfAppRestFactory)
        {
            _pdfAppRestFactory = pdfAppRestFactory;
        }

        private HttpClient CreatedAuthenticatedPdfRestClient()
        {
            var client = _pdfAppRestFactory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    #region Mocking Authentication and Authorization

                    services.AddAuthentication()
                    .AddScheme<TestAuthSchemeOptions, TestAuthSchemeHandler>("TestScheme", opts => { });

                    services.AddAuthorization(options =>
                    {
                        var policy = new AuthorizationPolicyBuilder()
                        .AddAuthenticationSchemes("TestScheme")
                        .RequireAuthenticatedUser()
                        .Build();

                        options.AddPolicy(PolicyConstants.HeaderXApiKeySchemePolicy, policy);
                    });

                    #endregion
                });
            })
            .CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            return client;
        }

        #region Response Objects

        public Response<PdfOutput> PdfOutputResponse { get; private set; }

        #endregion

        #region Requests

        public async Task<PdfModuleBuilder> ConvertHtmlToPdfRequest(PdfInput pdfInput)
        {
            using var client = CreatedAuthenticatedPdfRestClient();
            var jsonRequest = JsonConvert.SerializeObject(pdfInput);
            using var httpContent = new StringContent(jsonRequest, Encoding.UTF8, System.Net.Mime.MediaTypeNames.Application.Json);
            var response = await client.PostAsync($"/", httpContent);
            var content = await response.Content.ReadAsStringAsync();

            PdfOutputResponse = JsonConvert.DeserializeObject<Response<PdfOutput>>(content);                                                                                                                               

            return this;
        }
        #endregion
    }

}
