using Ganss.Xss;
using PdfApp.Application.Abstractions.Application;
using PdfApp.Contracts.Request;
using System.Text;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace PdfApp.Application.Services
{
    public class HtmlToPdfConvertService : IHtmlToPdfConvertService
    {
        private readonly IHtmlSanitizer _htmlSanitizer;
        private readonly IConverter _converter;

        public HtmlToPdfConvertService(IHtmlSanitizer htmlSanitizer, IConverter converter)
        {
            _htmlSanitizer = htmlSanitizer;
            _converter = converter;
        }

        public byte[] ConvertToPdf(PdfInput input)
        {
            // Decode html from base64
            var html = Encoding.UTF8.GetString(Convert.FromBase64String(input.HtmlString));

            // Sanitize HTML
            html = _htmlSanitizer.Sanitize(html);

            // Prepare Document
            var document = new HtmlToPdfDocument
            {
                GlobalSettings =
                {
                    ColorMode = (ColorMode) Enum.Parse(typeof(ColorMode), input.Options.PageColorMode.ToString()),
                    Orientation = (Orientation) Enum.Parse(typeof(Orientation), input.Options.PageOrientation.ToString()),
                    PaperSize = (PaperKind) Enum.Parse(typeof(PaperKind), input.Options.PagePaperSize.ToString()),
                    Margins = new MarginSettings
                    {
                        Bottom = input.Options.PageMargins.Bottom,
                        Top = input.Options.PageMargins.Top,
                        Left = input.Options.PageMargins.Left,
                        Right = input.Options.PageMargins.Right
                    }
                    //Out = @"C:\Users\bbler\Desktop\test\test.pdf"
                }, 
                Objects =
                {
                    new ObjectSettings
                    {
                        PagesCount = true,
                        HtmlContent = html,
                        WebSettings = {
                            DefaultEncoding = "utf-8",
                            EnableJavascript = false
                        },
                        HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 },
                        Encoding = Encoding.UTF8,
                        
                    }
                },
            };

            return _converter.Convert(document);
        }
    }

}
