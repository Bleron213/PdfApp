using System;
using System.Collections.Generic;
using System.Text;
using PdfApp.Contracts.Enums;

namespace PdfApp.Contracts.Request
{
    public class PdfInput
    {
        public string? HtmlString { get; }
        public PdfOptions? Options { get; } = new PdfOptions
        {
            PageColorMode = PageColorMode.Grayscale,
            PageOrientation = PageOrientation.Portrait,
            PagePaperSize = PagePaperSize.A4,
            PageMargins = new PageMargins
            {
                Top = 10,
                Bottom = 10,
                Right = 10,
                Left = 10
            }
        };

        public PdfInput(string htmlString)
        {
            HtmlString = htmlString;
        }
    }

    /// <summary>
    /// TODO To be implemented
    /// </summary>
    public class PdfOptions
    {
        public PageColorMode PageColorMode { get; set; }
        public PageOrientation PageOrientation { get; set; }
        public PagePaperSize PagePaperSize { get; set; }
        public PageMargins? PageMargins { get; set; } 
    }

    public class PageMargins
    {
        public int Top { get; set; }
        public int Bottom { get; set; }
        public int Right { get; set; }
        public int Left { get; set; }
    }
}
