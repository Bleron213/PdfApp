using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using PdfApp.Contracts.Request;
using PdfApp.IntegrationTests.Core.Builders;
using PdfApp.IntegrationTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WkHtmlToPdfDotNet;
using Xunit;

namespace PdfApp.IntegrationTests
{
    public class PdfModuleIntegrationTests : TestBase
    {
        private readonly PdfModuleBuilder _pdfModuleBuilder;
        public PdfModuleIntegrationTests()
        {
            _pdfModuleBuilder = new PdfModuleBuilder(pdfAppRestFactory);
        }

        [Fact]
        public async Task ConvertHtmlToPdfRequest_NullRequest_ShouldFail()
        {
            #region Act
            await _pdfModuleBuilder.ConvertHtmlToPdfRequest(null);
            #endregion

            #region Assert
            var response = _pdfModuleBuilder.PdfOutputResponse;
            Assert.False(response.Succeeded);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.NotNull(response.Message);
            Assert.NotEmpty(response.Message);
            Assert.Equal(400, (int) response.StatusCode);
            #endregion
        }

        [Theory]
        [InlineData("InvalidColor", "Landscape", "A4", false)]
        [InlineData("InvalidColor", "Portrait", "", false)]
        [InlineData(null, "InvalidLandscape", "InvalidSize", false)]
        [InlineData(null, "", "A4", true)]
        public async Task ConvertHtmlToPdfRequest_InvalidParameters_ShouldFail(string pageColorMode, string pageOrientation, string pagePaperSize, bool sendValidHtml)
        {
            #region Arrange

            bool parsedColorMode = Enum.TryParse<ColorMode>(pageColorMode, out _);
            bool parsedOrientation = Enum.TryParse<Orientation>(pageOrientation, out _);
            bool parsedPaperKind = Enum.TryParse<PaperKind>(pagePaperSize, out _);

            var html = "<h1> Hello </h1>";
            var pageMargins = new PageMargins
            {
                Bottom = 10,
                Top = 10,
                Left = 10,
                Right = 10
            };
            
            if (!sendValidHtml)
                html = Convert.ToBase64String(Encoding.UTF8.GetBytes(html));

            var pdfInput = new PdfInput(html, new PdfOptions
            {
                PageColorMode = pageColorMode,
                PageOrientation = pageOrientation,
                PagePaperSize = pagePaperSize,
                PageMargins = pageMargins
            });

            #endregion

            #region Act

            await _pdfModuleBuilder.ConvertHtmlToPdfRequest(pdfInput);

            #endregion

            #region Assert

            var response = _pdfModuleBuilder.PdfOutputResponse;
            Assert.False(response.Succeeded);
            Assert.Null(response.Data);
            Assert.NotNull(response.Errors);
            Assert.Equal(400, (int)response.StatusCode);

            if (!parsedColorMode)
                response.Errors.SingleOrDefault(x => x.Key == nameof(pdfInput.Options.PageColorMode));

            if (!parsedOrientation)
                response.Errors.SingleOrDefault(x => x.Key == nameof(pdfInput.Options.PageOrientation));

            if (!parsedPaperKind)
                response.Errors.SingleOrDefault(x => x.Key == nameof(pdfInput.Options.PagePaperSize));

            if (!sendValidHtml)
                response.Errors.SingleOrDefault(x => x.Key == nameof(pdfInput.HtmlString));

            #endregion
        }

        [Fact]
        public async Task ConvertHtmlToPdfRequest_CorrectParameters_ShouldBeSuccessful()
        {
            #region Arrange

            var html = "<h1> Hello </h1>";
            var htmlBase64 = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(html));

            var pdfInput = new PdfInput(htmlBase64, new PdfOptions
            {
                PageColorMode = "Color",
                PageOrientation = "Landscape",
                PagePaperSize  = "A4",
                PageMargins = new PageMargins
                {
                    Bottom = 10,
                    Top = 10,
                    Left = 10,
                    Right = 10
                }
            });

            #endregion

            #region Act

            await _pdfModuleBuilder.ConvertHtmlToPdfRequest(pdfInput);

            #endregion

            #region Assert

            var response = _pdfModuleBuilder.PdfOutputResponse;

            Assert.True(response.Succeeded);
            Assert.NotNull(response.Data);
            Assert.NotNull(response.Data.PdfDocument);
            Assert.NotEmpty(response.Data.PdfDocument);
            Assert.True(Base64Helper.IsBase64String(response.Data.PdfDocument));
            Assert.True(response.Data.PdfDocumentSize > 0);
            Assert.Equal(200, (int)response.StatusCode);

            #endregion
        }
    }
}
