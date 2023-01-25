using System;
namespace PdfApp.Domain
{
    public class PdfOutput
    {
        public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
        public string? ErrorMessage { get; private set; }
        public string? PdfDocument { get; private set; }
        public int? PdfDocumentSize { get; private set; }

        public PdfOutput(string errorMessage) => ErrorMessage = errorMessage;

        public PdfOutput(string pdfDocument, int pdfDocumentSize)
        {
            PdfDocument = pdfDocument;
            PdfDocumentSize = pdfDocumentSize;
        }
    }
}

