namespace PdfApp.Contracts.Request
{
    public class PdfInput
    {
        public string? HtmlString { get; }
        public PdfOptions? Options { get; } 

        public PdfInput(string htmlString, PdfOptions? options = null)
        {
            HtmlString = htmlString;
            Options = options ?? new PdfOptions
            {
                PageColorMode = "Grayscale",
                PageOrientation = "Portrait",
                PagePaperSize = "A4",
            };

            Options.PageMargins = options?.PageMargins ?? new PageMargins
            {
                Top = 0,
                Bottom = 0,
                Right = 0,
                Left = 0
            };
        }
    }

    /// <summary>
    /// TODO To be implemented
    /// </summary>
    public class PdfOptions
    {
        public string PageColorMode { get; set; }
        public string PageOrientation { get; set; }
        public string PagePaperSize { get; set; }
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
