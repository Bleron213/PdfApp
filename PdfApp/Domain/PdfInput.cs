using System;
namespace PdfApp.Domain
{
    public class PdfInput
    {
        public string? HtmlString { get; }
        public PdfOptions? Options { get; }

        public PdfInput(string htmlString)
        {
            HtmlString = htmlString;
        }
    }
}

