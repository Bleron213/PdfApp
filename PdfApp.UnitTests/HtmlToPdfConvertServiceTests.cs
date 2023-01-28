using AngleSharp;
using Ganss.Xss;
using Moq;
using PdfApp.Application.Services;
using PdfApp.Contracts.Request;
using System;
using WkHtmlToPdfDotNet.Contracts;
using Xunit;

namespace PdfApp.UnitTests
{
    public class HtmlToPdfConvertServiceTests
    {
        private readonly Mock<IHtmlSanitizer> _htmlSanitizerStub;
        private readonly Mock<IConverter> _pdfConverterStub;
        public HtmlToPdfConvertServiceTests()
        {
            _htmlSanitizerStub = new Mock<IHtmlSanitizer>();
            _pdfConverterStub = new Mock<IConverter>();
        }

        [Fact]
        public void ConvertToPdf_NullHtmlInput_ShouldThrow_ArgumentNullException()
        {
            #region Arrange
            _htmlSanitizerStub
                .Setup(x => x.Sanitize(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IMarkupFormatter?>()))
                .Returns("");

            _pdfConverterStub
                .Setup(x => x.Convert(It.IsAny<IDocument>()))
                .Returns(System.Array.Empty<byte>());
            #endregion

            #region Act
            var htmlToPdfService = new HtmlToPdfConvertService(_htmlSanitizerStub.Object, _pdfConverterStub.Object);
            #endregion

            #region Assert

            Assert.Throws<ArgumentNullException>(() => htmlToPdfService.ConvertToPdf(new PdfInput(null)));

            #endregion

        }

        [Fact]
        public void ConvertToPdf_NullInput_ShouldThrow_ArgumentNullException()
        {
            #region Arrange
            _htmlSanitizerStub
                .Setup(x => x.Sanitize(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IMarkupFormatter?>()))
                .Returns("");

            _pdfConverterStub
                .Setup(x => x.Convert(It.IsAny<IDocument>()))
                .Returns(System.Array.Empty<byte>());
            #endregion

            #region Act
            var htmlToPdfService = new HtmlToPdfConvertService(_htmlSanitizerStub.Object, _pdfConverterStub.Object);
            #endregion

            #region Assert

            Assert.Throws<ArgumentNullException>(() => htmlToPdfService.ConvertToPdf(null));

            #endregion

        }


        [Theory]
        [InlineData("PGgxPk15IEZpcnN0IEhlYWRpbmc8L2gxPg==")]
        public void ConvertToPdf_ShouldReturn_ByteArray(string base64Html)
        {
            #region Arrange

            _htmlSanitizerStub
                .Setup(x => x.Sanitize(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IMarkupFormatter?>()))
                .Returns("");

            _pdfConverterStub
                .Setup(x => x.Convert(It.IsAny<IDocument>()))
                .Returns(System.Array.Empty<byte>());

            #endregion

            #region Act

            var htmlToPdfService = new HtmlToPdfConvertService(_htmlSanitizerStub.Object, _pdfConverterStub.Object);
            var result = htmlToPdfService.ConvertToPdf(new PdfInput(base64Html));

            #endregion

            #region Assert

            _htmlSanitizerStub.Verify(mock => mock.Sanitize(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IMarkupFormatter?>()), Times.Once);
            _pdfConverterStub.Verify(mock => mock.Convert(It.IsAny<IDocument>()), Times.Once);

            #endregion

        }
    }
}